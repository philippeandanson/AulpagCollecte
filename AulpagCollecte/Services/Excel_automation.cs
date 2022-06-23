using ExcelApp = Microsoft.Office.Interop.Excel;
using System;


namespace AulpagCollecte.Services
{
    class Excel_automation
    {

        public static void ExecuteExcelMacro()
        {

            string sourceFile = @"D:\BddWindev\Edition planning\Horaires Sncf Paris-Granville.xls";

            ExcelApp.Application ExcelApp = new ExcelApp.Application();
            ExcelApp.DisplayAlerts = false;
            ExcelApp.Visible = false;
          //  ExcelApp.AutomationSecurity = Microsoft.Office.Core.MsoAutomationSecurity.msoAutomationSecurityLow;
            ExcelApp.Workbook ExcelWorkBook = ExcelApp.Workbooks.Open(sourceFile);

            string macro = "ThisWorkbook.Maj";

            try
            {
                ExcelApp.Run(macro);
                Console.WriteLine("Macro: " + macro + " exceuted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to Run Macro: " + macro + " Exception: " + ex.Message);
            }

            ExcelWorkBook.Close(true);
            ExcelApp.Quit();
            if (ExcelWorkBook != null) { System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelWorkBook); }
            if (ExcelApp != null) { System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApp); }
        }
    }

    
}
