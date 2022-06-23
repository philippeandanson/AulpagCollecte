using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AulpagCollecte.Models
{

    public class Calendriers
    {
        [Key]
        public string date_calendrier { get; set; }
        public int jour_semaine { get; set; }
        public bool ferie { get; set; }   

       public  Calendriers() { }
        
        public   Calendriers(string odate, int ojour, bool oferie)
        {
            date_calendrier = odate;
            jour_semaine = ojour;
            ferie = oferie;
        }
    }

    public class Feed_info
    {
        [Key, Column(Order = 0)]
        public DateTime    feed_version { get; set; }
        [Key, Column(Order = 1)]
        public string     conv_rev { get; set; }
        public int        feed_start_date { get; set; }
        public int        feed_end_date { get; set; }
        public DateTime   date_d { get; set; }
        public DateTime   date_f { get; set; }

    }

    public class Tads
    {
      [Key, Column(Order = 0)]
      public string id_station { get; set; }
      [Key, Column(Order = 1)]
      public int stop_headsign { get; set; }
      public string arrival_time { get; set; }
      public string departure_time { get; set; }

        public Tads() { }
        public Tads(string oid_station,int ostop_headsign,string oarrival,string odeparture)
        {
            id_station = oid_station;
            stop_headsign = ostop_headsign;
            arrival_time = oarrival;
            departure_time = odeparture;
        }
    }
    
    public class Stops
    {
        [Key, Column(Order = 0)]
        public string stop_id { get; set; }
        public string stop_name { get; set; }
        public string stop_lat { get; set; }
        public string stop_lon { get; set; }
        [Key, Column(Order = 1)]
        public string station_id { get; set; }

        public Stops() { }

        public Stops(string ostop_id,string ostop_name,string ostop_lat,string ostop_lon,string ostation_id)
        {
            stop_id = ostop_id;
            stop_name = ostop_name;
            stop_lat = ostop_lat;
            stop_lon = ostop_lon;
            station_id = ostation_id;
        }

    }

    public class Stations
    {
        [Key]
        public string station_id { get; set; }
        public string station_name { get; set; }
        public int tri { get; set; }
    }

    public class Calendar_dates
    {
       [Key, Column(Order = 0)]
       public int service_id { get; set; }
       [Key, Column(Order = 1)]
       public int  date { get; set; }
       public int  exception_type { get; set; }
       public int  rang { get; set; }
       public int  jour_semaine { get; set; }

        public Calendar_dates() { }

        public Calendar_dates(int oservice_id, int odate)
        {
            service_id = oservice_id;
            date = odate;
        }

    }

    public class Calendar_list
    {
        [Key]
        public string service_id { get; set; }
        public DateTime du { get; set; }
        public DateTime au { get; set; }
        public string passage { get; set; }
        public string exempt { get; set; }
        public string remarques { get; set; }
        public string test { get; set; }
        public int week_d { get; set; }
        public int week_f { get; set; }

        public Calendar_list() { }

        public Calendar_list(string oservice_id,DateTime odu, DateTime oau, string opassage,string oexempt,string oremarque,string otest,int oweek_d,int oweek_end)
        {
            service_id = oservice_id;
            du = odu;
            au = oau;
            passage = opassage;
            exempt = oexempt;
            remarques = oremarque;
            test = otest;
            week_d = oweek_d;
            week_f = oweek_end;
        }
    }

    public class Stop_times
    {
        [Key, Column(Order = 0)]
        public string trip_id { get; set; }
        public string arrival { get; set; }
        public string departure { get; set; }
        [Key, Column(Order = 1)]
        public string stop_id { get; set; }
        public int stop_sequence { get; set; }
        public string station_id { get; set; }

        public Stop_times() { }

        public Stop_times(string oTrip_id, string oArrival,string oDeparture,string oStop_id,int oStop_sequence,string ostation_id)
        {
            trip_id = oTrip_id;
            arrival = oArrival;
            departure = oDeparture;
            stop_id = oStop_id;
            stop_sequence = oStop_sequence;
            station_id = ostation_id;
        }   

    }

    public class Trips
    {

        [Key]
        public string trip_id { get; set; }
        public string route_id { get; set; }
        public int service_id { get; set; }
        public int trip_headsign { get; set; }
        public string direction_id { get; set; }
        public int block_id { get; set; }
        public string tri { get; set; }

        public Trips()  { }

        public Trips (string oroute_id , int oservice_id, string otrip_id, int otrip_headeign, string odisrection_id, int oblock_id)
        {
            route_id = oroute_id;
            service_id = oservice_id;
            trip_id = otrip_id;
            trip_headsign = otrip_headeign;
            direction_id = odisrection_id;
            block_id = oblock_id;
        }

    }

    public class Temp
    {
        public string Contenu { get; set; }

    }
}
