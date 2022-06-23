using AulpagCollecte.Data;

namespace AulpagCollecte.Services
{
    class Alerte_changement
    {
        public static void RecordHisto()
        {
            bool t1 = Database.Insert_histo_trips();
            bool t2 = Database.Update_histo_trips();
            Excel_automation.ExecuteExcelMacro();
            if (t1 || t2)
            {
                
                Envoi.SendMail();
            }
        }
    }

}
