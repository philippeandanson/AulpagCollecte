using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AulpagCollecte.Models
{   
    public class Histo_trips
    {
        [Key]
        public string trip_id { get; set; }
        public string trip_headsign { get; set; }
        public string passage { get; set; }
        public string exempt { get; set; }
        public string remarques { get; set; }
        public DateTime date_api { get; set; }    
        public DateTime? date_fin { get; set; }
    }

    public class Histo_stop_times
    {
        [Key, Column(Order = 0)]
        public string trip_id { get; set; }
        [Key, Column(Order = 1)]      
        public int station_id { get; set; }
        [Key, Column(Order = 2)]
        public string departure { get; set; }
    }

}
