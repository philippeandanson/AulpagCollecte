using AulpagCollecte.Data;
using AulpagCollecte.Models;
using AulpagCollecte.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AulpagCollecte.Tools
{
    class ToolDates
    {

        static string FiltreService(string passage)
        {
            string FiltreJours = "";
            for (int i = 1; i <= 7; i++)
            {
                if (passage.Substring(i - 1, 1) == "1") FiltreJours = FiltreJours + i + ",";
            }
            return FiltreJours;
        }
        static Feed_info Periode = Database.GetFeed_info.FirstOrDefault();
        static List<Services_in_calendar> date_service = Database.Services_in_calendar;                                      // Dates Services inclus dans Période globales
        static DateTime Start_from = Periode.date_d;                                                                         // Date début de période globale
        static DateTime End_to     = Periode.date_f;                                                                         // Date fin de période globale      
        static List<Calendar_list> listcalendar_list = Database.GetCalendar_List;                                            // Services documentés avec les périodes
        public class Weeks
        {
            public int WeekFromPeriode { get; set; }
            public int WeekFromService { get; set; }
        }
        static List<Weeks> w = new List<Weeks>();
      
        public static string GetComment(int service)
        {

          //  service = 278;

            string sortie =null;
            DateTime Dt1 = new DateTime();
            DateTime Dt2 = new DateTime();
            DateTime Dt3 = new DateTime();
            DateTime Dt4 = new DateTime();

            Calendar_list fromlist = Database.GetCalendar_From_List(service.ToString());                                                   // Obtient la ligne service dans la liste des services
            var calend_dates = date_service.Where(x => x.service_id == service);                                                           // Liste service dans période totale obtien les trous dans la période
            string filtre = FiltreService(fromlist.passage);                                                                               // prépare la requête suivante
            int count = fromlist.passage.Count(f => f == '1');
            List<Services_in_calendar> 
            result = calend_dates.Where(p => filtre.Any(t => p.jour_semaine.ToString().Contains(t))).OrderBy(y=>y.date_calendrier).ToList();  // Filtre résultats sur les jours de passage

            // filtre s'il y a des exceptions 
            string filtre2 = FiltreService(fromlist.exempt);                                                 // prépare la requête suivante
            List<Services_in_calendar> result2 = calend_dates.Where(p => filtre2.Any(t => p.jour_semaine.ToString().Contains(t))).ToList();  // Filtre résultats sur les jours de passage
            //--------

            var oOut = result.Where(x => x.date==null).ToList();        // Pas de date dans la période
            var oIn  = result.Where(x => x.date != null).ToList();      // Date dans la période   
            var oIn2 = result2.Where(x => x.date != null).ToList();     // Cas des execptions

            string _in = "";  foreach (var item in oIn)  { _in = _in + item.date_calendrier + ", "; }   // liste des dates ouvertes sur la périodes
            string _out = ""; foreach (var item in oOut) { _out = _out + item.date_calendrier + ", "; } // liste des dates fermée sur la période
 
            if (result.Count != 0)
            {
                // Recherche s'il y a un interval sans passage dans le planning 
                bool periode_out = false;
                if (oOut.Count != 0) periode_out = interval_7j(oOut, fromlist.passage.Count(f => f == '1'));

                // Retirer les occurences dans l'interval hors période
                if (periode_out)
                {
                    DateTime Db = T1;
                    DateTime Df = T2;

                    for (DateTime dt = Db; dt <= Df; dt = dt.AddDays(1))
                    {
                        Services_in_calendar t = result.Find(x => DateTime.ParseExact(x.date_calendrier.ToString(), "yyyyMMdd", null) == dt);
                        if (t != null) result.Remove(t);
                    }
                }
                else
                {
                    Dt1 = DateTime.ParseExact(result.Min(x => x.date_calendrier).ToString(), "yyyyMMdd", null);    // Date de début
                    Dt2 = DateTime.ParseExact(result.Max(x => x.date_calendrier).ToString(), "yyyyMMdd", null);    // Date de fin   
                    DateTime Db = DateTime.ParseExact(oIn.Min(x => x.date.ToString()), "yyyyMMdd", null);
                    DateTime Df = DateTime.ParseExact(oIn.Max(x => x.date.ToString()), "yyyyMMdd", null);

                    for (DateTime dt = Dt1; dt <= Dt2; dt = dt.AddDays(1))
                    {
                        Services_in_calendar t = result.Find(x => DateTime.ParseExact(x.date_calendrier.ToString(), "yyyyMMdd", null) == dt);
                        if (dt < Db || dt > Df)
                            result.Remove(t);
                    }
                }

                // Extraire les dates sauf 
                string Sauf = null;
                int i = 0;
                foreach (var sauf in result)
                {
                    if (sauf.date == null) { Sauf = Sauf + DateTime.ParseExact((sauf.date_calendrier).ToString(), "yyyyMMdd", null).ToString("dd/MM") + ", "; i++; }
                }

                string d = i > 1 ? "les " : "le ";
                if (Sauf != null) { Sauf = "sauf " + d + Sauf.Substring(0, Sauf.Length - 2) + ", "; }

                if (!periode_out)      // Cas des dates ouvertes
                {
                    Dt1 = DateTime.ParseExact(result.Min(x => x.date_calendrier).ToString(), "yyyyMMdd", null);    // Date de début
                    Dt2 = DateTime.ParseExact(result.Max(x => x.date_calendrier).ToString(), "yyyyMMdd", null);    // Date de fin           

                    sortie = Borne_date(Dt1, Dt2);

                    if (oIn.Count() == 1) return "Le " + Dt1.ToString("dd/MM");  // Si touvé 1 occurence
                    if (oIn.Count() == 2) return "Le " + Dt1.ToString("dd/MM") + " et le " + Dt2.ToString("dd/MM");  // Pour 2 occurences

                }

                if (periode_out)         // Cas jours fermés
                {
                    Dt3 = DateTime.ParseExact(oOut.Min(x => x.date_calendrier).ToString(), "yyyyMMdd", null);
                    Dt4 = DateTime.ParseExact(oOut.Max(x => x.date_calendrier).ToString(), "yyyyMMdd", null);

                    //   string ferie_out = Out_Ferie(oOut);

                    if (periode_out)                                                 // S'il y  a + de date fermée que ouverte
                                                                                     //sortie = Sauf_entre(oOut, filtre);
                        sortie = "sauf du " + T1.ToString("dd/MM") + " au " + T2.ToString("dd/MM") + ", ";
                    else
                    if (filtre.Contains("6") || filtre.Contains("7") && filtre.Length > 3)  //  Traite les WE
                    {
                        if (!interval_7j(oOut, 1)) return Cas_paticulier(service);
                    }
                    else if (oOut.Count() == 1)
                        sortie = "sauf " + Dt3.ToString("dd/MM");
                }

                string oEt = null;
                if (sortie.Contains("sauf")  && Sauf!=null ) { oEt = " et "; sortie = sortie.Substring(0, sortie.Length - 2); }
                //     if (sortie != "") sortie = sortie + "\n" + oEt + Sauf;
                if (sortie != "") sortie = sortie + oEt + Sauf;
                else 
                    sortie = oEt + Sauf;

            }

            string oET = Et(service);         
        //    bool test = !String.IsNullOrEmpty(sortie) && sortie.Substring(sortie.Length-1,1)=="\n";
            
       //     if (!test) oET = "\n" + oET;
           
            sortie = sortie + oET; 
            sortie = sortie.Trim();

            if (String.IsNullOrEmpty(sortie)) return null;
            if (sortie.Substring(sortie.Length - 1, 1) == ",") sortie = sortie.Substring(0, sortie.Length - 1) + ".";  // se termine par un point

            return  ToolString.CapitalizeFirstLetter(sortie);         
        }


        static string Borne_date(DateTime Dt1, DateTime Dt2)
        {           
            string retour = null;

            if (Dt1 < Start_from.AddDays(7) &&  Dt2> End_to.AddDays(-7)) return "";  // Si la période est incluse dans la période générale alors pas de date séléectionnée 

            // Cas où les dates debut et fin sont interne à la période générale
            if (Dt1 > Start_from.AddDays(7) && Dt2 < End_to.AddDays(-7)) return  "du " + Dt1.ToString("dd/MM") + " au " + Dt2.ToString("dd/MM")+ ", ";

            // cas où les dates sont à l'extérieure  à gauche ou à droite de la période
            if (Dt1 > Start_from.AddDays(7)) retour = "du " + Dt1.ToString("dd/MM") + ", " ; else retour = "au " + Dt2.ToString("dd/MM") + ", ";

            return retour;
        }

        static string Sauf()
        {


            return "";
        }
 
        static string Sauf_entre(IEnumerable<Services_in_calendar> Out,string filtre)
        {    
            return  "sauf du " + DateTime.ParseExact(Out.First().date_calendrier.ToString(), "yyyyMMdd", null).ToString("dd/MM") + 
                        " au " +  DateTime.ParseExact(Out.Last().date_calendrier.ToString(), "yyyyMMdd", null).ToString("dd/MM");      
        }

        static string Out_Ferie(IEnumerable<Services_in_calendar> Ferie)
        {
            bool ferie = false;
            bool lendemain_ferie = false;
            string ldf=null;   // lendemain férié
            foreach (var o in Ferie)
            {             
                if (!o.ferie) if (Lendemain_feries(o.date_calendrier)) { ldf += ldf + " le " +  o.date_calendrier.ToString("dd/MM") ;  ferie = true; }
            }

            if (ferie && lendemain_ferie) return "Sauf fériés et lendemain fériés.";
            else if (ferie) return "Sauf fériés";

           
            return null;
            // return true is si fériés et veilles de fête

        }

        private static bool Lendemain_feries(int odate)
        {        
           return Jours_feries.bolIsWorkingDay(DateTime.ParseExact(odate.ToString(), "yyyyMMdd", null).AddDays(1));           
        }


            static string Out_Ferie1(IEnumerable<Services_in_calendar> We)
        {

            string filtre = ("6,7");                                               // prépare la requête suivante
            var result = We.Where(p => filtre.Any(t => p.jour_semaine.ToString().Contains(t)) && p.ferie == true);
            if (result.Count() == 0) return null;
            DateTime Dt1 = DateTime.ParseExact(result.Min(x => x.date_calendrier).ToString(), "yyyyMMdd", null);
            DateTime Dt2 = DateTime.ParseExact(result.Max(x => x.date_calendrier).ToString(), "yyyyMMdd", null);


            return "Sauf WE du " + Dt1.ToString("dd/MM") + " au " + Dt2.ToString("dd/MM");
        }

        static string Out_We (List<Services_in_calendar> We,string filtre)
        {       
            // prépare la requête suivante
            var result = We.Where(p=> filtre.Any(t => p.jour_semaine.ToString().Contains(t)) && p.ferie==false).ToList();
            if (result.Count() == 0) return null;
            DateTime Dt1 = DateTime.ParseExact(result.Min(x => x.date_calendrier).ToString(), "yyyyMMdd", null);
            DateTime Dt2 = DateTime.ParseExact(result.Max(x => x.date_calendrier).ToString(), "yyyyMMdd", null);

            int i = 0; int j = 0;
            foreach (var o in result)
            { 
                if (o.jour_semaine == 6) i = 1; else j = 1;  
            }

            if (i == 1 && j == 0 && filtre.Length >= 4)
            {
             
                if(interval_7j(result,1))
                return "Sauf sam. du " + Dt1.ToString("dd/MM") + " au " + Dt2.ToString("dd/MM");
            }
            if (i == 0 && j == 1 && filtre.Length >=4 )
                return "Sauf dim. du " + Dt1.ToString("dd/MM") + " au " + Dt2.ToString("dd/MM");
            if (filtre.Length <5) 
                return "Sauf du " + Dt1.ToString("dd/MM") + " au " + Dt2.ToString("dd/MM");

            return "Sauf WE du " + Dt1.ToString("dd/MM") + " au " + Dt2.ToString("dd/MM");
        }

        static DateTime T1 { get; set; } 
        static DateTime T2 { get; set; } 
        static bool interval_7j(List<Services_in_calendar> test,int passage)
        {
          
            int nb = test.Count() - 1;
            T2 = new DateTime();
 
            T1 = DateTime.ParseExact(test.First().date_calendrier.ToString(), "yyyyMMdd", null);
            for (int i = 1; i <= nb; i++)
            {
                DateTime Dt1 = DateTime.ParseExact(test[i].date_calendrier.ToString(), "yyyyMMdd", null).AddDays(7);
                try
                {
                    DateTime Dt2 = DateTime.ParseExact(test[i + passage].date_calendrier.ToString(), "yyyyMMdd", null);
                      if (Dt1 == Dt2)
                    T2 = Dt2;
                } catch { }
                
            }

            bool reponse1 = T1 < T2;
            if (T2 >= End_to.AddDays(-7)) return false; // Corrige le cas  Au si  test = vrai et toutes les occurences ne se limite pas avant la fin du planning
            if (T1 <= Start_from.AddDays(7)) return false ;   // Corrige le cas  Au si  test = vrai et la période commence avant le début Start_from
            return   T1 < T2;

      
        }
    
   
        public static string Cas_paticulier(int service)
        {
            var calendrier = Database.GetCalendar_Dates.Where(x => x.service_id == service);      // Tous les jours de la période
            int[] jours = new int[8]; string Passage = "";
            foreach (var o in calendrier)                             //Enregistre les jours de passages dans la variable jours[]
            {
                int t = (int)DateTime.ParseExact(o.date.ToString(), "yyyyMMdd", null).DayOfWeek;
                if (t == 0) t = 7;
                jours[t] = 1;
            }

            for (int j = 1; j <= 7; j++)
            {
                string d = jours[j] != 0 ? "1" : "0"; Passage = Passage + d;             // créé tableau  des jours de passage
            }

            var liste = date_service.Where(x => x.service_id == service).ToList();                          // Jours de passage pour le service sur vue rServices_in_calendar
            string filtre = FiltreService(Passage);                                                         // Filtre pour requête jour de passage  
            var analyse = liste.Where(p => filtre.Any(t => p.jour_semaine.ToString().Contains(t))).ToList();
            string Test = " "; string Temp;
           
            for (int i =1;i<analyse.Count()-1; i++)
            {
               string t = Test.Substring(Test.Length - 1,1);
               bool test1 = (analyse[i].date != null) == (analyse[i-1].date != null);
               if (test1) Temp = "1"; else Temp = "0";            
               if (Temp!=t) Test = Test + Temp;
              
            }

            if (Test.Length  <4 ) return "";

            var liste1 = date_service.Where(x => x.service_id == service).Where(x => x.date != null);
            string resultat=""; string resultat_in = ""; string resultat_out = "";
            // prépare la requête suivante
            var result1 = liste1.Where(p => filtre.Any(t => p.jour_semaine.ToString().Contains(t)));        // Filtre résultats sur les jours de passage
           
            foreach (var o in result1)
            {
                resultat_in = resultat_in + DateTime.ParseExact(o.date_calendrier.ToString(), "yyyyMMdd", null).ToString("d/M").ToString() + ", ";
            }
            var liste2 = date_service.Where(x => x.service_id == service).Where(x => x.date == null);
            var result2 = liste2.Where(p => filtre.Any(t => p.jour_semaine.ToString().Contains(t)));        // Filtre résultats sur les jours de passage

            bool test_ferie_out = false;
            foreach (var o in result2)
            {
                resultat_out = resultat_out+ DateTime.ParseExact(o.date_calendrier.ToString(), "yyyyMMdd", null).ToString("d/M").ToString() + ", ";
                test_ferie_out = o.ferie;
            }

            if (resultat_in!="" && (resultat_out!=""))
            {
                if (resultat_in.Length <= resultat_out.Length) resultat = resultat_in;
                else
                {
                    if (test_ferie_out) resultat = "sauf férié  ";  else   resultat = "sauf " + resultat_out;
                }
            }

            resultat = resultat.Substring(0,resultat.Length - 2) + ".";
    
            return resultat;
        }

        public static string Date_periode(int service)
        {
            string Date_periode = null;
            string Apartir_du = null;

            int n = int.Parse(Start_from.ToString("yyyyMMdd"));                                                        // (1) Première date aprèz 7 jours du début du calendrier
            int m = Database.GetCalendar_Dates.Where(x => x.service_id == service).OrderBy(y=>y.date).First().date;   //  (2) Première date du calendrier pour le calendrier particulier
           
            if (m > n) Apartir_du = DateTime.ParseExact(m.ToString(), "yyyyMMdd", null).ToString("dd/MM").ToString();  // A partir de   si (2) > (1) 

            string Date_end_to = null;
            int o = Database.GetCalendar_Dates.Where(x => x.service_id == service).Last().date;                             // (3) Dernière date du calendrier pour le calendrier particulier
            int p = int.Parse(End_to.ToString("yyyyMMdd"));                                                                // (4) Date générale fin période
            if (o < p) Date_end_to = DateTime.ParseExact(o.ToString(), "yyyyMMdd", null).ToString("dd/MM").ToString();

            DateTime dd = DateTime.ParseExact(m.ToString(), "yyyyMMdd", null);
            DateTime df = DateTime.ParseExact(o.ToString(), "yyyyMMdd", null);
            DateTime compare = dd.AddDays(1);
            if (m == o)
                Date_periode = "(1) Circule seulement le " + DateTime.ParseExact(m.ToString(), "yyyyMMdd", null).ToString("dd/MM").ToString();
            else if (Apartir_du == null && Date_end_to != null && compare != df)
                Date_periode = "Au " + Date_end_to;
            else if (Date_end_to == null && Apartir_du != null)
                Date_periode = "Du " + Apartir_du;
            else if (compare == df)
                Date_periode = "(1) Seulement du " + dd.ToString("dd/MM") + " au (2) " + df.ToString("dd/MM");
            else if (Apartir_du != null && Date_end_to != null)
                Date_periode = "du " + Apartir_du + " au " + Date_end_to;
            return Date_periode;
        }

        static string Et(int service)
        {
            string resultat = null;

            List<Calendar_list> From_calendar_list = listcalendar_list.Where(x => x.service_id == service.ToString()).ToList();

            string filtre = FiltreService(From_calendar_list[0].exempt);
            int du = int.Parse(From_calendar_list[0].du.ToString("yyyyMMdd"));
            int au = int.Parse(From_calendar_list[0].au.ToString("yyyyMMdd"));

            DateTime dd = DateTime.ParseExact(du.ToString(), "yyyyMMdd", null);
            DateTime df = DateTime.ParseExact(au.ToString(), "yyyyMMdd", null);

            // Pas de Et si  la date de fin  = date début + 1  (cas d'un we seul)
            if (dd.AddDays(1) == df)
            {
                return "seulement (1) " + dd.ToString("dd/MM") + " et (2) " + df.ToString("dd/MM")  + ".";
            }
                 

            List<Services_in_calendar> calend_dates = date_service.Where(x => x.service_id == service && x.date_calendrier >= du && x.date_calendrier <= au).ToList();
        
            var result = calend_dates.Where(p => filtre.Any(t => p.jour_semaine.ToString().Contains(t)) && p.date != null);
            int i = 0;
            int y = 1;
          
            foreach (var m in result)
            {
                var t = DateTime.ParseExact(m.date_calendrier.ToString(), "yyyyMMdd", null).ToString("dd/MM").ToString();
                resultat = resultat + t  + " et ";
                i++;
                y++;
            }

            if (resultat != null)
            {
                if (i == 1)
                    resultat = "(1) seulement le " + resultat.Substring(0, resultat.Length - 4) + ",";
                else
                    resultat = "(1) seulement les " + resultat.Substring(0, resultat.Length - 4) + ",";
            }

        //    if (du == au) resultat = null;

            return resultat;
        }      
    }
}

