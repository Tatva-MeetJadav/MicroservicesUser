using OfficeOpenXml;
using System.Drawing;
using MicroservicesUser.Models.ViewModels.History;
using MicroservicesUser.BusinessLogic.Interfaces;
using OfficeOpenXml.Style;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class ExcelExportService : IExcelExportServices
    {
        public byte[] GenerateEmailVerificationExcel(EmailVerificationDetailVM detailVM)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Verification History");

            // Set default alignment for all cells
            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            worksheet.Cells["A1"].Value = "General Details";
            worksheet.Cells["A1:B1"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            worksheet.Cells["A2"].Value = "Email";
            worksheet.Cells["B2"].Value = detailVM.RequestVM?.Email;
            worksheet.Cells["A3"].Value = "Verification Date";
            worksheet.Cells["B3"].Value = detailVM.CreatedAt.ToString("dd/MM/yyyy hh:mm tt");

            // Add general fields
            worksheet.Cells["A4"].Value = "Is Valid Email?";
            worksheet.Cells["B4"].Value = StatusIcon(detailVM.ResponseVM?.Valid);
            worksheet.Cells["A5"].Value = "Has Timed Out (Can't be verify)?";
            worksheet.Cells["B5"].Value = StatusIcon(detailVM.ResponseVM?.TimedOut);
            worksheet.Cells["A6"].Value = "Is Temporary Email?";
            worksheet.Cells["B6"].Value = StatusIcon(detailVM.ResponseVM?.Disposable);
            worksheet.Cells["A7"].Value = "Is Non-personal?";
            worksheet.Cells["B7"].Value = StatusIcon(detailVM.ResponseVM?.Generic);
            worksheet.Cells["A8"].Value = "Has Common Domain?";
            worksheet.Cells["B8"].Value = StatusIcon(detailVM.ResponseVM?.Common);
            worksheet.Cells["A9"].Value = "Has Catch-all Domain?";
            worksheet.Cells["B9"].Value = StatusIcon(detailVM.ResponseVM?.CatchAll);
            worksheet.Cells["A10"].Value = "SMTP Score";
            worksheet.Cells["B10"].Value = detailVM.ResponseVM?.SmtpScore;
            worksheet.Cells["A11"].Value = "Overall Score";
            worksheet.Cells["B11"].Value = detailVM.ResponseVM?.OverallScore;

            // Email Deliverability
            worksheet.Cells["A12"].Value = "Email Deliverability";
            worksheet.Cells["B12"].Value = detailVM.ResponseVM?.Deliverability;
            worksheet.Cells["A13"].Value = "Fraud Score";
            worksheet.Cells["B13"].Value = detailVM.ResponseVM?.FraudScore;
            worksheet.Cells["A14"].Value = "Spam Trap Score";
            worksheet.Cells["B14"].Value = detailVM.ResponseVM?.SpamTrapScore;

            // Security & Risk Section
            worksheet.Cells["A16"].Value = "Security & Risk";
            worksheet.Cells["A16:B16"].Merge = true;
            worksheet.Cells["A16"].Style.Font.Bold = true;
            worksheet.Cells["A16"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A16"].Style.Fill.BackgroundColor.SetColor(Color.LightCoral);

            worksheet.Cells["A17"].Value = "Has Valid DNS (Domain Name System)";
            worksheet.Cells["B17"].Value = StatusIcon(detailVM.ResponseVM?.DnsValid);
            worksheet.Cells["A18"].Value = "Honeypot";
            worksheet.Cells["B18"].Value = StatusIcon(detailVM.ResponseVM?.Honeypot);
            worksheet.Cells["A19"].Value = "Frequent Complainer";
            worksheet.Cells["B19"].Value = StatusIcon(detailVM.ResponseVM?.FrequentComplainer);
            worksheet.Cells["A20"].Value = "Suspect";
            worksheet.Cells["B20"].Value = StatusIcon(detailVM.ResponseVM?.Suspect);
            worksheet.Cells["A21"].Value = "Recent Abuse (Blacklist)";
            worksheet.Cells["B21"].Value = StatusIcon(detailVM.ResponseVM?.RecentAbuse);
            worksheet.Cells["A22"].Value = "Is Leaked?";
            worksheet.Cells["B22"].Value = StatusIcon(detailVM.ResponseVM?.Leaked);
            worksheet.Cells["A23"].Value = "Risky TLD (Top Level Domain)";
            worksheet.Cells["B23"].Value = StatusIcon(detailVM.ResponseVM?.RiskyTld);

            // Email Details Section
            worksheet.Cells["A25"].Value = "Email Details";
            worksheet.Cells["A25:B25"].Merge = true;
            worksheet.Cells["A25"].Style.Font.Bold = true;
            worksheet.Cells["A25"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A25"].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);

            worksheet.Cells["A26"].Value = "First Seen";
            worksheet.Cells["B26"].Value = detailVM.ResponseVM?.FirstSeen?.Iso.ToString("dd/MM/yyyy hh:mm tt");
            worksheet.Cells["A27"].Value = "Sanitized Email";
            worksheet.Cells["B27"].Value = detailVM.ResponseVM?.SanitizedEmail;
            worksheet.Cells["A28"].Value = "Suggested Domain";
            worksheet.Cells["B28"].Value = detailVM.ResponseVM?.SuggestedDomain;
            worksheet.Cells["A29"].Value = "First Name";
            worksheet.Cells["B29"].Value = detailVM.ResponseVM?.FirstName;
            worksheet.Cells["A30"].Value = "Has SPF Record?";
            worksheet.Cells["B30"].Value = StatusIcon(detailVM.ResponseVM?.SpfRecord);
            worksheet.Cells["A31"].Value = "Has DMARC Record?";
            worksheet.Cells["B31"].Value = StatusIcon(detailVM.ResponseVM?.DmarcRecord);

            int currentRow = 32;
            worksheet.Cells[$"A{currentRow}"].Value = "MX Records";
            worksheet.Cells[$"A{currentRow}:A{currentRow + detailVM.ResponseVM!.MxRecords!.Count - 1}"].Merge = true;
            worksheet.Cells[$"A{currentRow}:A{currentRow + detailVM.ResponseVM.MxRecords.Count - 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[$"A{currentRow}:A{currentRow + detailVM.ResponseVM.MxRecords.Count - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            foreach (var mxRecord in detailVM.ResponseVM.MxRecords)
            {
                worksheet.Cells[$"B{currentRow}"].Value = mxRecord;
                currentRow++;
            }

            worksheet.Cells[$"A{currentRow}"].Value = "A Records";
            worksheet.Cells[$"A{currentRow}:A{currentRow + detailVM.ResponseVM.ARecords!.Count - 1}"].Merge = true; // 
            worksheet.Cells[$"A{currentRow}:A{currentRow + detailVM.ResponseVM.ARecords.Count - 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[$"A{currentRow}:A{currentRow + detailVM.ResponseVM.ARecords.Count - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            foreach (var aRecord in detailVM.ResponseVM.ARecords)
            {
                worksheet.Cells[$"B{currentRow}"].Value = aRecord;
                currentRow++;
            }

            worksheet.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }

        private static string StatusIcon(bool? status)
        {
            return status switch
            {
                true => "Yes",
                false => "No",
                _ => "Unknown"
            };
        }
    }
}
