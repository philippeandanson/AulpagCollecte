using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AulpagCollecte.Models
{


    public class Headers
    {
        [Key]
        public string trip_headsign { get; set; }
        public string Order_line { get; set; }
        public string Icon_header { get; set; }
        public string Icon_service { get; set; }
        public string sens { get; set; }

        public Headers() { }
        public Headers(string otrip_headers,string osens)
        {
            trip_headsign = otrip_headers;
            sens = osens;
        }

    }

    public class Services_in_calendar
    {
        [Key, Column(Order = 0)]
        public int date_calendrier { get; set; }
        public int jour_semaine { get; set; }
        public bool ferie { get; set; }
        [Key, Column(Order = 1)]
        public int? service_id { get; set; }        
        public int? date { get; set; }
        public double semaine { get; set; }
     
    }

}
