using AulpagCollecte.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;


namespace AulpagCollecte.Data
{
    public  class Database
    {
        private static readonly object locker = new object();
     
        public static bool Update_histo_trips()
        {
            bool test = false;

            var ReadInfo = Database.GetFeed_info[0].feed_start_date;
            DateTime date_info = DateTime.ParseExact(ReadInfo.ToString(), "yyyyMMdd", null);

            string SQL = " SELECT   a.trip_id, a.trip_headsign,  a.passage,  a.exempt,  a.remarques,  date_api,  date_fin FROM " +
                "public.\"Histo_trips\" a left join public.\"Trips\" b " +
                " on SPLIT_PART(a.trip_id, ':', 1) = SPLIT_PART(b.trip_id, ':', 1) " +
                " Where a.date_fin is null and b.trip_id is null ";

            lock (locker)
            {
                using (BaseContext context = new BaseContext())
                {

                    List<Histo_trips> histo = context.Database.SqlQuery<Histo_trips>(SQL).ToList<Histo_trips>();

                    foreach (var v in histo)
                    {                        
                        v.date_fin = date_info;
                        context.histo_trips.AddOrUpdate(v);
                        test = true;
                    }
                    try
                    { context.SaveChanges(); }
                    catch { }

                    return test;
                }
            }
        }

        public static bool Insert_histo_trips()
        {
            bool test = false;
            var ReadInfo = Database.GetFeed_info[0].feed_start_date;
            DateTime date_info = DateTime.ParseExact(ReadInfo.ToString(), "yyyyMMdd", null);

            string SQL =
                        " SELECT  " +
                        " \"Trips\".trip_id,  " +
                        " \"Trips\".trip_headsign::text,  " +
                        " \"Calendar_list\".passage,   " +
                        " \"Calendar_list\".exempt,  " +
                        " \"Calendar_list\".remarques, '" + date_info +
                        "'::date as date_api, null as date_fin" +
                        " FROM " +
                        "  public.\"Trips\"  " +
                        "  join public.\"Calendar_list\"   on   \"Trips\".service_id = \"Calendar_list\".service_id::integer " +
                        "  Left join \"Histo_trips\" on  SPLIT_PART(\"Trips\".trip_id, ':', 1)=SPLIT_PART(\"Histo_trips\".trip_id, ':', 1) where \"Histo_trips\".trip_id is null  ";

            lock (locker)
            {
                using (BaseContext context = new BaseContext())
                {                    
                    List<Histo_trips> histo = context.Database.SqlQuery<Histo_trips>(SQL).ToList<Histo_trips>();

                    foreach (var v in histo)
                    {
                        context.histo_trips.Add(v);
                        try
                        {
                            context.SaveChanges();
                            test = true;
                            Insert_histo_stop_times(v.trip_id);
                        }
                        catch {}
                    }

                    return test;
                }
            }
        }

        public static void Insert_histo_stop_times(string trip_id)
        {
           
            string SQL = "SELECT trip_id, station_id::integer, substr(departure, 0, 6) as departure FROM \"Stop_times\" Where trip_id='" + trip_id + "'";

            lock (locker)
            {
                using (BaseContext context = new BaseContext())
                {
                    List<Histo_stop_times> histo = context.Database.SqlQuery<Histo_stop_times>(SQL).ToList<Histo_stop_times>();

                    foreach (var v in histo)
                    {
                        context.histo_stop_times.Add(v);
                        try
                        { context.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }
                    }
            
                }
            }
        }

        public static List<Headers> GetHeaders
        {
            get
            {

                lock (locker)
                {
                    using (BaseContext context = new BaseContext())
                    {
                        IQueryable<Headers> fic = from item in context.headers select item;
                        return fic.ToList();
                    }
                }

            }
        }

        public static List<int> GetListCalendar_Dates
        {
            get
            {

                lock (locker)
                {
                    string SQL = "Select Distinct service_id from \"Calendar_dates\"  order by 1";

                    using (BaseContext context = new BaseContext())
                    {                        
                           return context.Database.SqlQuery<int>(SQL).ToList<int>();                    
                    }

                }

            }
        }

        public static IEnumerable<Calendar_dates> GetCalendar_Dates
        {
            get
            {
                string SQL = " SELECT a.date_calendrier::integer AS date, a.jour_semaine, Case When a.ferie Then 1 Else 0 End as exception_type, b.service_id::integer AS service_id , " +
                    " 0 as rang FROM   \"Calendriers\"  a Left join \"Calendar_dates\" b On  a.date_calendrier::integer = b.date   where service_id is not null  order by b.service_id ";

                lock (locker)
                {
                    using (BaseContext context = new BaseContext())
                    {
                        List<Calendar_dates> fic = context.Database.SqlQuery<Calendar_dates>(SQL).ToList<Calendar_dates>();                     
                        return fic;
                    }
                }

            }
        }

        public static List<Services_in_calendar> GetCalendar_Dates2(int service)
        {

                string SQL = " SELECT a.date_calendrier::integer AS date_calendrier,date,a.jour_semaine ,service_id::integer, " +
                "  ferie " +
                   " FROM   \"Calendriers\"  a Left join \"Calendar_dates\" b On  a.date_calendrier::integer = b.date  And service_id =" +  service  + " order by a.date_calendrier, b.service_id ";

                lock (locker)
                {
                    using (BaseContext context = new BaseContext())
                    {
                        List<Services_in_calendar> fic = context.Database.SqlQuery<Services_in_calendar>(SQL).ToList<Services_in_calendar>();
                        return fic;
                    }
                }           
        }

        public static List<Calendar_list> GetCalendar_List
        {
            get
            {

                lock (locker)
                {
                    using (BaseContext context = new BaseContext())
                    {
                        IQueryable<Calendar_list> fic = from item in context.calendar_list select item;
                        return fic.ToList();
                    }
                }

            }
        }

        public static Calendar_list GetCalendar_From_List(string service)
        {                      
           using (BaseContext context = new BaseContext())
            {
                Calendar_list fic = context.calendar_list.First(x => x.service_id == service);
              return fic;
            }                      
        }

        public static List<Services_in_calendar> Services_in_calendar
        {
            get
            {

                lock (locker)
                {
                    using (BaseContext context = new BaseContext())
                    {                      
                        return context.Database.SqlQuery<Services_in_calendar>("select * from \"rServices_in_calendar\" ").ToList<Services_in_calendar>();
                    }
                }

            }
        }

        public static List<Feed_info> GetFeed_info
        {
            get
            {

                lock (locker)
                {
                    using (BaseContext context = new BaseContext())
                    {
                        IQueryable<Feed_info> fic = from item in context.feed_info select item;
                        return fic.ToList();
                    }
                }

            }
        }

        public static List<Stops> GetReadStops
        {
            get
            {

                lock (locker)
                {
                    using (BaseContext context = new BaseContext())
                    {                     
                        IQueryable<Stops> fic = from item in context.stops  select item;
                        return fic.ToList();
                    }
                }

            }
        }
   
        public static List<Stop_times> GetReadStop_time
        {
            get
            {

                lock (locker)
                {
                    using (BaseContext context = new BaseContext())
                    {
                        IQueryable<Stop_times> mobileQuery = from item in context.stop_times orderby item.trip_id select item;
                        return mobileQuery.ToList();
                    }
                }

            }
        }
    
        public static List<Trips> GetReadTrip
        {
            get
            {
                lock (locker)
                {
                    using (BaseContext context = new BaseContext())
                    {
                        IQueryable<Trips> tripsQuery = from item in context.trips orderby item.trip_id select item;
                        return tripsQuery.ToList();
                    }
                }

            }
        }

        public static void UpdateStop_timesAsync(Stop_times item)
        {
            lock (locker)
            {
                string id = item.trip_id;
                using (var db = new BaseContext())
                {

                    if (id == "")
                    {
                        var t = db.stop_times.Add(new Stop_times());
                        t.trip_id = item.trip_id;
                        t.stop_id = item.stop_id;
                     
                    }
                    else
                    {

                        var entity = db.stop_times.First(a => a.trip_id == item.trip_id);
                        entity.trip_id = item.trip_id;
                        entity.stop_id = item.stop_id;
                      
                    }

                    db.SaveChanges();
                }
            }
        }
      
        public static void DeleteMobileAsync(Stop_times item)
        {
            lock (locker)
            {

                using (var db = new BaseContext())
                {
                    var entity = db.stop_times.First(a => a.trip_id == item.trip_id);
                    db.stop_times.Remove(entity);
                    db.SaveChanges();
                }

            }
        }

        public static void Clear()
        {
            using (var db = new BaseContext())
            {
                db.feed_info.RemoveRange(db.feed_info);
                db.tads.RemoveRange(db.tads);
                db.calendar_dates.RemoveRange(db.calendar_dates);
                db.calendar_list.RemoveRange(db.calendar_list);
                db.calendrier.RemoveRange(db.calendrier);
                db.stops.RemoveRange(db.stops);
                db.stop_times.RemoveRange(db.stop_times);               
                db.trips.RemoveRange(db.trips);
                db.headers.RemoveRange(db.headers);

                //    db.stations.RemoveRange(db.stations);


                db.SaveChanges();
            }
        }
  
    }
  
}



