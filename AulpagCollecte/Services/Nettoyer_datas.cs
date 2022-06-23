using AulpagCollecte.Data;
using System.Collections.Generic;
using System.Linq;

namespace AulpagCollecte.Services
{
    class Nettoyer
    {
        class Temp
        {
            public int trip_headsign { get; set; }
            public string trip1 { get; set; }
            public string trip2 { get; set; }
            public int service1 { get; set; }
            public int service2 { get; set; }
        }


        public static void Services()
        {

            string SQL = "WITH rql1 AS( SELECT   \"Trips\".trip_headsign, \"Trips\".service_id,\"Trips\".trip_id, " +
               " sum(departure::time without time zone::interval) AS hh " +
               "FROM " +
               "  public.\"Stop_times\", " +
               "  public.\"Trips\" " +
               "WHERE " +
               "  \"Trips\".trip_id = \"Stop_times\".trip_id and departure<> 'ö' " +
               "group by  \"Trips\".trip_headsign,   \"Trips\".service_id,  \"Trips\".trip_id " +
               "       )," +
               " rql2 AS( " +
               "  SELECT rql1_1.hh, " +
               "  count(rql1_1.hh) AS count " +
               "  FROM rql1 rql1_1 " +
               "  GROUP BY rql1_1.hh " +
               "  HAVING count(rql1_1.hh) > 1 " +
               "        ), " +
               "rql3 As (SELECT" +
               "   CASE " +
                    "   WHEN(row_number() OVER(ORDER BY rql1.trip_headsign, rql1.service_id) % 2::bigint) = 0 THEN 2 " +
                    "   ELSE 1 " +
                    " END AS rownum," +
               "  rql1.trip_headsign, rql1.trip_id,rql1.service_id,rql1.hh FROM rql1  JOIN rql2 ON rql1.hh = rql2.hh order by 1,2,3 ) ";
            SQL = SQL + "SELECT a.trip_headsign, a.trip_id AS trip1, b.trip_id AS trip2, a.service_id AS service1,b.service_id AS service2 FROM rql3 a JOIN rql3 b ON a.trip_headsign = b.trip_headsign and a.hh=b.hh   WHERE a.rownum = 1 AND b.rownum = 2 ";

            using (var context = new BaseContext())
            {
                string SQL2 = null;
                List<Temp> liste = context.Database.SqlQuery<Temp>(SQL).ToList<Temp>();  // liste des min stop times Argentan dans stop_times
                foreach (var v in liste)
                {
                    int index = 10000 + v.service1 + v.service2;
                    SQL2 = "Insert Into \"Calendar_dates\"  select " + index + ", date from \"Calendar_dates\" Where  service_id =  " + v.service1;
                    try { context.Database.ExecuteSqlCommand(SQL2); } catch { }
                    SQL2 = "Insert Into \"Calendar_dates\"  select " + index + ", date from \"Calendar_dates\" Where  service_id =  " + v.service2;
                    try { context.Database.ExecuteSqlCommand(SQL2); } catch { }
                    context.Database.ExecuteSqlCommand("Delete from \"Trips\"  Where trip_id = '" + v.trip1 + "'");
                    context.Database.ExecuteSqlCommand("Delete from \"Stop_times\"  Where trip_id = '" + v.trip1 + "'");
                    context.Database.ExecuteSqlCommand("Update  \"Trips\" set service_id = " + index + " Where trip_id = '" + v.trip2 + "'");
                }
            }

        }

        class Temp2
        {
            public int trip_headsign { get; set; }
            public int service_id { get; set; }
            public string trip_id { get; set; }
            public string hh { get; set; }
            public string h1 { get; set; }

        }
        static int drapeau { get; set; }
        public static void Services2()
        {

            string SQL = "SELECT   \"Trips\".trip_headsign, \"Trips\".service_id,\"Trips\".trip_id, " +
               "  sum(departure::time without time zone::interval)::text AS hh ," +
               "  date_trunc('hour', sum(departure::time without time zone::interval))::text as h1 " +
               "  FROM " +
               "  public.\"Stop_times\", " +
               "  public.\"Trips\" " +
               "  WHERE " +
               "  \"Trips\".trip_id = \"Stop_times\".trip_id and departure<> 'ö' " +
               "  group by  \"Trips\".trip_headsign,   \"Trips\".service_id,  \"Trips\".trip_id " +
               "  order by 1,4     ";

            using (var context = new BaseContext())
            {
                string SQL2 = null;
                List<Temp2> liste = context.Database.SqlQuery<Temp2>(SQL).ToList<Temp2>(); // .Where(x=> x.trip_headsign==3464).ToList();  // liste des min stop times Argentan dans stop_times
                Temp2 temp = new Temp2();
               
                foreach (var v in liste)
                {

                    List<Temp2> t = liste.Where(x => x.trip_headsign == v.trip_headsign && x.h1 == v.h1).ToList();
                    int nb = t.Count();
                    if(nb>1)
                    {                      
                       int index = 10000;for (int i = 0; i < nb; i++) {index = index + t[i].service_id;}
                       for (int i=0; i<nb; i++)
                        {
                            SQL2 = "Insert Into \"Calendar_dates\"  select " + index + ", date from \"Calendar_dates\" Where  service_id =  " + t[i].service_id  ;
                            try { context.Database.ExecuteSqlCommand(SQL2); } catch { }
                            if (i != 0)
                            {
                                context.Database.ExecuteSqlCommand("Delete from \"Trips\"  Where trip_id = '" + t[i].trip_id + "'");
                                context.Database.ExecuteSqlCommand("Delete from \"Stop_times\"  Where trip_id = '" + t[i].trip_id + "'");
                            }
                            else if (i == 0)
                            context.Database.ExecuteSqlCommand("Update  \"Trips\" set service_id = " + index + " Where trip_id = '" + t[i].trip_id + "'");

                        }

                    }

                }
            }

        }
    }
}

