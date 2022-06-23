using AulpagCollecte.Data;
using AulpagCollecte.Models;
using AulpagCollecte.Tools;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;

namespace AulpagCollecte.Services
{
    class GestionDates
    {
              
        public static void Traitement()
        {
            
           // RemplirCalendrier();
            Collecte_jours_passage();
            ExtraireCalendrier_tad();
        }

        //---->  Rempli le table Calendier    (Toutes les dates de la période)
        public static void RemplirCalendrier()
        {
            Feed_info ReadInfo = new Feed_info();
            ReadInfo = Database.GetFeed_info[0]; // Base de traitement de la période à traiter     
            DateTime start = ReadInfo.date_d;
            DateTime end = ReadInfo.date_f;

            // Liste des dates de la période
            var calculatedDates = new List<string>(AllDatesBetween(start, end).Select(d => d.ToString("yyyyMMdd")));

            using (var context = new BaseContext())
            {
                try
                {
                    foreach (var m in calculatedDates)
                    {
                        Calendriers calendrier = new Calendriers() { date_calendrier = m, jour_semaine = (int)DateTime.ParseExact(m, "yyyyMMdd", null).DayOfWeek, ferie = Jours_feries.bolIsWorkingDay(DateTime.ParseExact(m, "yyyyMMdd", null)) };
                        if ((int)DateTime.ParseExact(m, "yyyyMMdd", null).DayOfWeek == 0) calendrier.jour_semaine = 7;
                        context.calendrier.Add(calendrier);
                        context.SaveChanges();
                    }

                }
                catch (Exception ex) { string message = ex.Message; }
            }

        }            
        static IEnumerable<DateTime> AllDatesBetween(DateTime start, DateTime end)
        {
            for (var day = start.Date; day <= end; day = day.AddDays(1))
                yield return day;
        }

        //---->  Remplir la table Calendar_list
        static int GetWeekNumber(DateTime Dt)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(Dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        static void Collecte_jours_passage()
        {
            var lists = Database.GetListCalendar_Dates;
            using (var context = new BaseContext())
            {
                context.calendar_list.RemoveRange(context.calendar_list);
                context.SaveChanges();
            }
            foreach (var service in lists)
            {
                Getjours_de_passage(service);
            }


        }           
        public static void Getjours_de_passage(int service)
        {
          
            using (BaseContext context = new BaseContext())
            {

                var calendrier = Database.GetCalendar_Dates.Where(x => x.service_id == service).OrderBy(y=>y.date);
                var first = calendrier.FirstOrDefault();    var last = calendrier.Last();
                int[] jours = new int[8]; string Passage = ""; string Exempt = "";
                DateTime Du = DateTime.ParseExact(first.date.ToString(), "yyyyMMdd", null);
                DateTime Au = DateTime.ParseExact(last.date.ToString(), "yyyyMMdd", null);
                int wDu = Du.Year*100+ GetWeekNumber(Du);
                int wAu = Au.Year*100+ GetWeekNumber(Au);

                //-->                                              Calcul Passages et Exceptions
                foreach (var o in calendrier)
                {
                    int t = (int)DateTime.ParseExact(o.date.ToString(), "yyyyMMdd", null).DayOfWeek; 
                    if (t == 0) t = 7;
                    jours[t]++; 
                }

                for (int j = 1; j <= 7; j++)
                {
                    string d = jours[j] > 2  ? "1" : "0"; Passage = Passage + d;
                    string e = jours[j] ==1 || jours[j]==2 ? "1" : "0"; Exempt  = Exempt + e;
                }

            //    if (Passage == "0000000") { Passage = Exempt; Exempt = "0000000"; } // Correction s'il y a qu'une semaine impactée

                //-->  Enregistrements dans la table Calendar_list
                Calendar_list list = new Calendar_list() 
                { service_id = service.ToString(), du = Du, au = Au, passage = Passage, exempt = Exempt, week_d=wDu,week_f=wAu };
                context.calendar_list.Add(list);
                context.SaveChanges();

            }
        }
        static void ExtraireCalendrier_tad()
        {
            List<Temp> fic = CollecteZip.ExtraireFromTxt(@"D:\Z\Tad\Calendar_tad.txt");
         
            DateTime dd = DateTime.ParseExact(Database.GetFeed_info[0].feed_start_date.ToString(), "yyyyMMdd", null);
            DateTime df = DateTime.ParseExact(Database.GetFeed_info[0].feed_end_date.ToString(), "yyyyMMdd", null);

            foreach (var f in fic)
            {
                using (var context = new BaseContext())
                {
                    try
                    {
                        string t = f.Contenu;
                        if (t != null)
                        {
                            string[] subs = t.Split(',');
                            context.calendar_list.Add(new Calendar_list() { service_id = subs[0], passage = subs[1],du= dd,au = df});
                            context.SaveChanges();
                        }

                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                }

            }

        }

    }
    class Commentaires
    {
        static int GetWeekNumber(DateTime Dt)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(Dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        static List<Services_in_calendar> date_service = Database.Services_in_calendar; // Dates Services inclus dans Période globales
        static List<Calendar_list> listcalendar_list = Database.GetCalendar_List;       // Services documentés avec les périodes
        static Feed_info Periode = Database.GetFeed_info.FirstOrDefault();              // Extraire les données de la période globale
        static DateTime Start_from=Periode.date_d.AddDays(7);                           // Date début de période globale
        static DateTime End_to =Periode.date_f.AddDays(-7);                             // Date fin de période globale
       
        static string FiltreService(string periode)
        {
            string FiltreJours = "";
            for (int i = 1; i <= 6; i++)
            {
                if (periode.Substring(i - 1, 1) == "1") FiltreJours = FiltreJours + i + ",";
            }
            return FiltreJours;
        }                                  // Formatage du filtre période pour l'utlisation
        public static void Get_commentaires()
        {
            List<Calendar_list> liste = Database.GetCalendar_List;
            var ListCalendar_Dates = Database.GetListCalendar_Dates;

              foreach (var item in ListCalendar_Dates)
               {
             
               string remarques= ToolDates.GetComment(item);
               
                Calendar_list Cl = Database.GetCalendar_From_List(item.ToString());
                Cl.remarques = remarques;
         
                using (var context = new BaseContext())
                {
                    context.calendar_list.AddOrUpdate(Cl);
                    context.SaveChanges();
                }
           }

        }
        static string Sauf(int service)
        {          
            string resultat = null;
         
            var From_calendar_list = listcalendar_list.Where(x => x.service_id == service.ToString());
            string filtre = ""; int du = 0; int au = 0;
            foreach (var t in From_calendar_list)
            {
                filtre = FiltreService(t.passage);
                du = int.Parse(t.du.ToString("yyyyMMdd"));
                au = int.Parse(t.au.ToString("yyyyMMdd"));
            }
            var calend_dates = date_service.Where(x => x.service_id == service && x.date_calendrier >= du && x.date_calendrier <= au);
            var result = calend_dates.Where(p => filtre.Any(t => p.jour_semaine.ToString().Contains(t)) && p.date == null);
            int i = 0;
            foreach (var m in result) 
            {
                var t  = DateTime.ParseExact(m.date_calendrier.ToString(), "yyyyMMdd", null).ToString("ddd dd/MM").ToString();                
                resultat = resultat + t + ", ";
                i++;
            }
            if (resultat != null)
            {
                if (i==1)
                    resultat = "sauf " + resultat.Substring(0, resultat.Length - 2);
                else
                    resultat = "sauf " + resultat.Substring(0, resultat.Length - 2);
            }         
            return  resultat; 
        }
        static string Et(int service)
        {
            string resultat = null;

            var From_calendar_list = listcalendar_list.Where(x => x.service_id == service.ToString());
            string filtre = "";  int du = 0; int au = 0;
            foreach (var t in From_calendar_list)
            {
                filtre = FiltreService(t.exempt);
                du = int.Parse(t.du.ToString("yyyyMMdd"));
                au = int.Parse(t.au.ToString("yyyyMMdd"));
               
            }

            DateTime dd = DateTime.ParseExact(du.ToString(), "yyyyMMdd", null).AddDays(1);
            DateTime df = DateTime.ParseExact(au.ToString(), "yyyyMMdd", null);

            if (dd == df) return null;  // Pas de Et si  la date de fin  = date début + 1  (cas d'un we seul)

            var calend_dates = date_service.Where(x => x.service_id == service && x.date_calendrier >= du && x.date_calendrier <= au);
            var result = calend_dates.Where(p => filtre.Any(t => p.jour_semaine.ToString().Contains(t)) && p.date != null);
            int i = 0;
            int y = 1;
            foreach (var m in result)
            {
                var t = DateTime.ParseExact(m.date_calendrier.ToString(), "yyyyMMdd", null).ToString("ddd dd/MM").ToString();
                resultat = resultat + t + "(" +  y  +  ")" + ", ";
                i++;
                y++;
            }
           
            if (resultat != null)
            {
                if (i == 1)
                    resultat = "et " + resultat.Substring(0, resultat.Length - 2);
                else
                    resultat = "et " + resultat.Substring(0, resultat.Length - 2);
            }

            if (du == au) resultat = null;

            return resultat;
        }
        static string Date_periode(int service)
        {      
            string Date_periode = null;
            string Apartir_du = null;   

            int m = Database.GetCalendar_Dates.Where(x=> x.service_id==service).First().date;                              // (2) Première date du calendrier pour le calendrier particulier
            int n =  int.Parse(Start_from.ToString("yyyyMMdd"));                                                           // (1) Date pour libelle   ->   A partir de 
            if (m > n)   Apartir_du =  DateTime.ParseExact(m.ToString(), "yyyyMMdd", null).ToString("dd/MM").ToString() ;  // A partir de   si (2) > (1) 

            string Date_end_to = null;
            int o= Database.GetCalendar_Dates.Where(x => x.service_id == service).Last().date;                             // (3) Dernière date du calendrier pour le calendrier particulier
            int p = int.Parse(End_to.ToString("yyyyMMdd"));                                                                // (4) Date générale fin période
            if (o < p) Date_end_to =  DateTime.ParseExact(o.ToString(), "yyyyMMdd", null).ToString("dd/MM").ToString();

            DateTime dd = DateTime.ParseExact(m.ToString(), "yyyyMMdd", null);
            DateTime df = DateTime.ParseExact(o.ToString(), "yyyyMMdd", null);
            DateTime compare = dd.AddDays(1);
            if (m == o)               
                Date_periode = "Circule seulement le (1) " + DateTime.ParseExact(m.ToString(), "yyyyMMdd", null).ToString("dd/MM").ToString();
            else if ( Apartir_du == null && Date_end_to !=null  && compare!=df)
                Date_periode = "Jusqu'au " + Date_end_to;
            else if (Date_end_to==null && Apartir_du!=null)
                Date_periode = "A partir du " + Apartir_du;
            else if (compare==df)
                Date_periode = "Seulement du (1) " + dd.ToString("dd/MM") + " au (2) " + df.ToString("dd/MM");
            else if(Apartir_du != null && Date_end_to != null )
                Date_periode = "du " + Apartir_du + " au "+  Date_end_to;                     
            return Date_periode;
        }  

    }
    class Mise_a_jour_tables
    {
        class Temp2
        {
            public int trip_headsign { get; set; }
            public string trip_id    { get; set; }
            public string departure  { get; set; }
        }
        class Temp3
        {
            public int trip_headsign { get; set; }
            public string sens { get; set; }
        }

        public static void  Traitement()
        {
            Insert_to_headertrips();
            Insert_into_header_tri();         
            Correction_Sens();
        } 
        static void Insert_to_headertrips()
        {
            //---->   Rempli la table Headers
            /// Ajoute les propriétés IconHeader et   OrderLine       
            /// <summary>
            /// Insertion dans la colonne order_line des critères de tri 
            /// </summary>
            var LGroupTrips = Database.GetReadTrip.GroupBy(x => x.trip_headsign).Select(y => y.Key).ToList();


            foreach (int header in LGroupTrips)
            {
                string logo_hearder = null; string logo_service = null;
                if (header < 1000) { logo_hearder = "Ä"; logo_service = "ö"; }
                if (header > 30000) { logo_hearder = "Ë"; }
                if (header > 1000 && header < 20000) { logo_hearder = "ô"; logo_service = "Ü"; }
                Headers trips = new Headers() { trip_headsign = header.ToString(), Icon_header = logo_hearder, Icon_service = logo_service };

                using ( BaseContext context = new BaseContext())
                {
                    context.headers.Add(trips);
                    try {context.SaveChanges();}catch { }
                }

            }
        }       
        public static void Insert_into_header_tri()       // trier les lignes des tableaux horaires 
        {
            List<Trips> LTrips = Database.GetReadTrip;    //    Collecte données table Headers


            string SQL = "SELECT \"Trips\".trip_headsign, \"Trips\".trip_id, \"Stop_times\".departure " +
                " FROM  public.\"Stop_times\", public.\"Trips\" " +
                " WHERE   \"Trips\".trip_id = \"Stop_times\".trip_id and station_id='87444539' ";  
                    
            using (BaseContext context = new BaseContext())
            {
                List<Temp2> argentan =  context.Database.SqlQuery<Temp2>(SQL).ToList<Temp2>();  // liste des min stop times Argentan dans stop_times

                foreach (var item in LTrips)
                {
                    item.tri = null;
                    context.trips.AddOrUpdate(item);
                }
                context.SaveChanges();

                foreach (var item in argentan)
                {                   
                    Trips trip = LTrips.FirstOrDefault(x => x.trip_headsign == item.trip_headsign && x.trip_id==item.trip_id);
                    trip.tri = item.departure;
                    context.trips.AddOrUpdate(trip);      
                }

                context.SaveChanges();

                // De Villedieu à Argentan
                string SQL2 = "SELECT a.trip_headsign,a.trip_id,departure FROM  \"Trips\" a ,\"Stop_times\" b WHERE  tri is null  And a.trip_id = b.trip_id " +
                    "and station_id = '87447698' ";
                List<Temp2> autres = context.Database.SqlQuery<Temp2>(SQL2).ToList<Temp2>();    // liste des min stop times ne passant pas par Argentan               
               foreach (var item in autres)
               {
                 Trips trip = LTrips.FirstOrDefault(x => x.trip_headsign == item.trip_headsign && x.trip_id == item.trip_id);
                    DateTime Dt = DateTime.Parse(item.departure);
                    if (trip.direction_id == "0")
                        trip.tri = Dt.AddHours(1).ToString("HH:mm:ss");
                    else
                        trip.tri = Dt.AddHours(-1).ToString("HH:mm:ss");

                    context.trips.AddOrUpdate(trip);
               }              
                context.SaveChanges();

                // de l'Aigle à Argentan
                string SQL3 = "SELECT a.trip_headsign,a.trip_id,departure FROM  \"Trips\" a ,\"Stop_times\" b WHERE  tri is null  And a.trip_id = b.trip_id " +
                 "and station_id = '87444638' ";
                List<Temp2> autre3s = context.Database.SqlQuery<Temp2>(SQL3).ToList<Temp2>();    // liste des min stop times ne passant pas par Argentan               
                foreach (var item in autre3s)
                {
                    Trips trip = LTrips.FirstOrDefault(x => x.trip_headsign == item.trip_headsign && x.trip_id == item.trip_id);
                    DateTime Dt = DateTime.Parse(item.departure);
                    if (trip.direction_id == "0")
                        trip.tri = Dt.AddMinutes(-30).ToString("HH:mm:ss");
                    else
                        trip.tri = Dt.AddMinutes(30).ToString("HH:mm:ss");

                    context.trips.AddOrUpdate(trip);
                }
                context.SaveChanges();

                string SQL4 = "SELECT a.trip_headsign,a.trip_id,min(departure) As departure FROM  \"Trips\" a ,\"Stop_times\" b   " +
                    " WHERE  tri is null  And a.trip_id = b.trip_id Group by a.trip_headsign,a.trip_id ";
                List<Temp2> autre4s = context.Database.SqlQuery<Temp2>(SQL4).ToList<Temp2>();    // liste des min stop times ne passant pas par Argentan               
                foreach (var item in autre4s)
                {
                    Trips trip = LTrips.FirstOrDefault(x => x.trip_headsign == item.trip_headsign && x.trip_id == item.trip_id);
                    trip.tri = item.departure;
                    context.trips.AddOrUpdate(trip);
                }
                context.SaveChanges();
            }
        }
        static void Correction_Sens()
        {
            /// Calcul la direction en fonction de la valeur de la longitude à partir de la posotion de départ
            /// Si la postition longitude gps est négative alors sens 0 sinon 1
            /// 

            string SQL = "	with sql1 as (		" +
            "	SELECT   \"Trips\".trip_headsign,\"Trips\".trip_id, min(departure) as db,max(departure) as df		" +
            "	   FROM   		" +
            "	      public.\"Stop_times\" join   public.\"Trips\" on \"Stop_times\".trip_id = \"Trips\".trip_id		" +
            "	      where departure <> 'ö'		" +
            "	      group by \"Trips\".trip_headsign, \"Trips\".trip_id		" +
            "	      order by 1),		" +
            "	  sql2 as (		" +
            "	  select distinct  trip_headsign, b.trip_id,c.tri  from sql1 a join  public.\"Stop_times\" b  on a.trip_id = b.trip_id and a.db = b.departure		" +
            "	                                                           join  \"Stations\" c  on b.station_id = c.station_id  order by 3),		" +
            "	 sql3 as (                                                          		" +
            "	 select   trip_headsign, b.trip_id, c.tri  from sql1 a join  public.\"Stop_times\" b  on a.trip_id = b.trip_id and  a.df =  b.departure		" +
            "	                                                           join  \"Stations\" c  on b.station_id = c.station_id order by 3 ) " +
            "	select distinct a.trip_headsign,case when a.tri<b.tri then '0' else '1' end  as sens  from sql2 a join  sql3 b on a.trip_headsign =  b.trip_headsign and a.trip_id=b.trip_id  "; 


            using (BaseContext context = new BaseContext())
            {
                var ReadHeader = context.Database.SqlQuery<Temp3>(SQL).ToList<Temp3>();
                List<Trips> LTrips = Database.GetReadTrip;
                foreach (var f in LTrips)
                {

                    if (f.trip_headsign > 1000)
                    {
                        var trouve_sens = ReadHeader.FirstOrDefault(a => a.trip_headsign == f.trip_headsign).sens;
                        f.direction_id = trouve_sens;
                        context.trips.AddOrUpdate(f);

                        context.SaveChanges();
                    }
             
                }
            }
        }
    }
}
   