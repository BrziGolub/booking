﻿using Booking.Commands;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace Booking.WPF.ViewModels.Guest2
{
    public class ShowVouchersViewModel
    {
        private readonly Window _window;
        private readonly IVoucherService _voucherService;

        public ObservableCollection<Voucher> Vouchers { get; set; }

        public RelayCommand Button_Click_Close { get; set; }
        public RelayCommand Button_Click_GenerateReport { get; set; }

        public ShowVouchersViewModel(Window window)
        {
            _window = window;
            _voucherService = InjectorService.CreateInstance<IVoucherService>();

            Vouchers = new ObservableCollection<Voucher>(_voucherService.GetVouchersByUserId(TourService.SignedGuideId));

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            Button_Click_Close = new RelayCommand(ButtonClose);
            Button_Click_GenerateReport = new RelayCommand(ButtonGenerateReport);
        }

        private void ButtonClose(object param)
        {
            _window.Close();
        }

        private void ButtonGenerateReport(object param)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PDF file|*.pdf", FileName = "Vouchers_Report", ValidateNames = true, InitialDirectory = GetDownloadsPath() })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Document doc = new Document(PageSize.A4);
                    try
                    {
                        PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                        doc.Open();

                        Paragraph para1 = new Paragraph("All currently valid vouchers", new Font(Font.FontFamily.HELVETICA, 20));
                        para1.Alignment = Element.ALIGN_CENTER;
                        para1.SpacingAfter = 10;
                        doc.Add(para1);

                        PdfPTable table = new PdfPTable(2);

                        PdfPCell cell1 = new PdfPCell(new Phrase("Voucher No.", new Font(Font.FontFamily.HELVETICA, 10)));
                        cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                        cell1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        cell1.BorderWidthBottom = 1f;
                        cell1.BorderWidthTop = 1f;
                        cell1.BorderWidthLeft = 1f;
                        cell1.BorderWidthRight = 1f;
                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell1.VerticalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell1);

                        PdfPCell cell2 = new PdfPCell(new Phrase("Expiration date", new Font(Font.FontFamily.HELVETICA, 10)));
                        cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                        cell2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        cell2.BorderWidthBottom = 1f;
                        cell2.BorderWidthTop = 1f;
                        cell2.BorderWidthLeft = 1f;
                        cell2.BorderWidthRight = 1f;
                        cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell2.VerticalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell2);

                        foreach (var v in Vouchers)
                        {
                            PdfPCell cellNo = new PdfPCell(new Phrase(v.Id.ToString(), new Font(Font.FontFamily.HELVETICA, 10)));
                            PdfPCell cellDate = new PdfPCell(new Phrase(v.ValidTime.ToString("dd.MM.yyyy."), new Font(Font.FontFamily.HELVETICA, 10)));

                            cellNo.HorizontalAlignment = Element.ALIGN_CENTER;
                            cellDate.HorizontalAlignment = Element.ALIGN_CENTER;

                            table.AddCell(cellNo);
                            table.AddCell(cellDate);
                        }

                        doc.Add(table);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Error);
                    }
                    finally
                    {
                        doc.Close();
                    }
                }
            }
        }

        public static string GetDownloadsPath()
        {
            string path = null;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                IntPtr pathPtr;
                int hr = SHGetKnownFolderPath(ref FolderDownloads, 0, IntPtr.Zero, out pathPtr);
                if (hr == 0)
                {
                    path = Marshal.PtrToStringUni(pathPtr);
                    Marshal.FreeCoTaskMem(pathPtr);
                    return path;
                }
            }
            path = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            path = Path.Combine(path, "Downloads");
            return path;
        }

        private static Guid FolderDownloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetKnownFolderPath(ref Guid id, int flags, IntPtr token, out IntPtr path);
    }
}
