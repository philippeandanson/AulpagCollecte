using AulpagCollecte.Data;
using AulpagCollecte.Models;
using AulpagCollecte.Parameters;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;

namespace AulpagCollecte.Services
{
    class CollecteZip
    {

        public static List<Trips> ListTrips { get; set; }
        public static List<Stop_times> ListStopsTimes { get; set; }

        public static void DownLoadApiSncf()
        {
            WebClient Client = new WebClient();
            Client.DownloadFile(@"https://eu.ftp.opendatasoft.com/sncf/gtfs/export-ter-gtfs-last.zip",
                @"C:\Users\phili\source\repos\AulpagCollecte\AulpagCollecte\AulpagCollecte\AllFiles.zip");
        }
    

        public static void ExtractZip()
        {
            string zipPath = @"C:\Users\phili\source\repos\AulpagCollecte\AulpagCollecte\AulpagCollecte\AllFiles.zip";
            string extractPath = @"D:\Z";
            string[] txtList = Directory.GetFiles(extractPath, "*.txt");
            // Delete source files that were copied.
            foreach (string f in txtList) { File.Delete(f);  }

            System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);
        }

        public static void Import_Donnees()
        {

           // ExtraireStop_times("Tad\\stop_times_tad.txt");
            Database.Clear();
            // Mise à jour de la table Feed_info
            
            var info = Feed_info();
         //   Update_periode_parametree(DateTime.ParseExact(info.feed_start_date.ToString(), "yyyyMMdd", null), DateTime.ParseExact(info.feed_end_date.ToString(), "yyyyMMdd", null));
            Update_periode_parametree(periodes.Debut, periodes.Fin);
            ExtraireTrips("trips.txt");
            ExtraireTrips("Tad\\trips_tad.txt");

            ExtraireStop_times("stop_times.txt");
            ExtraireStop_times("Tad\\stop_times_tad.txt");
            Extrairestop_times_complement();
          
            ExtraireStop("stops.txt");            
            ExtraireStop_Tad();

            Extraire_Tad_to_stop_times();

            ExtraireCalendar();

            GestionDates.RemplirCalendrier();

            Nettoyer.Services2();

        }

        static Feed_info Feed_info()
        {
            List<Temp> fic = ExtraireFromTxt(@"D:\Z\feed_info.txt");
            Feed_info item = new Feed_info();

            using (var context = new BaseContext())
            {
                foreach (var f in fic)
                {
                    try
                    {
                        string t = f.Contenu;
                        if (t != null)
                        {
                            string[] subs = t.Split(',');
                          //   Feed_info item = new Feed_info(int.Parse(subs[4]), int.Parse(subs[5]),DateTime.Parse(subs[6]),subs[7]);
                            item = new Feed_info() { feed_version = DateTime.Parse(subs[6]), feed_start_date = int.Parse(subs[4]), feed_end_date = int.Parse(subs[5]), conv_rev = subs[7] };
                            context.feed_info.Add(item);
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return item;

            }
        }

        static void Update_periode_parametree(DateTime date_debut,DateTime date_fin)
        {
            Feed_info ReadInfo = new Feed_info();
            ReadInfo = Database.GetFeed_info[0]; // Base de traitement de la période à traiter 
            ReadInfo.date_d = date_debut;
            ReadInfo.date_f = date_fin; // new DateTime(2022, 03, 27);  // date_fin;
            using (var context = new BaseContext())
            {
                context.feed_info.AddOrUpdate(ReadInfo);
                context.SaveChanges();
            }

        }

        static void Extraire_Tad_to_stop_times()
        {
           
                string SQL = "SELECT b.trip_id,'StopPoint:OCETrain TER-' || a.id_station as stop_id, 0 as stop_sequence,a.arrival_time as arrival,a.departure_time as departure, a.id_station as station_id FROM  \"Tads\" a, \"Trips\" b WHERE  a.stop_headsign = b.trip_headsign ";
              
                    using (BaseContext context = new BaseContext())
                   {
                      var ReadTad= context.stop_times.SqlQuery(SQL).ToList<Stop_times>();
                      foreach (var t in ReadTad)
                      {
                         context.stop_times.Add(t);                      
                      }
                      context.SaveChanges();

                string SQL2 = "select distinct stop_id,b.station_name as stop_name,'0' as stop_lat, '0' as stop_lon,a.station_id from \"rStations_manquantes\" a join \"Stations\" b on  a.station_id = b.station_id  ";
                var fic = context.stops.SqlQuery(SQL2).ToList<Stops>();
                foreach (var f in fic)   {context.stops.Add(f); }
                context.SaveChanges();
            }                      
        }

        static void Extrairestop_times_complement()
        {
            List<Temp> fic = ExtraireFromTxt(@"D:\Z\Tad\stop_times_complement.txt");    
            using (var context = new BaseContext())
            {               
                foreach (var f in fic)
                {
                    try
                    {
                        string t = f.Contenu;
                        if (t != null)
                        {
                            string[] subs = t.Split(',');
                            Tads item = new Tads(subs[0], int.Parse(subs[1]), subs[2], subs[3]);
                            context.tads.Add(item);
                            context.SaveChanges();
                        }
                    } 
                    catch (Exception ex) 
                    {
                    }
               }
                               
            }
        }

        static void ExtraireStation()
        {           
         List<Stops> ListStops = Database.GetReadStops;
          var d = ListStops.GroupBy(x => x.station_id).Select(y => y.First());         

            using (var context = new BaseContext())
           {
            
                foreach (var m in d)
               {
                    Stations st = new Stations() {  station_id =  m.station_id, station_name= m.stop_name };
                    context.stations.Add(st) ;
                    try  {context.SaveChanges(); }catch { }
                }
            }

        }

        static void ExtraireStop_Tad()
        {
            List<Temp> fic = ExtraireFromTxt(@"D:\Z\Tad\stops_tad.txt");
            using (var context = new BaseContext())
            {
                foreach (var f in fic)
                {
                    try
                    {
                        string t = f.Contenu;
                        if (t != null)
                        {
                            string[] subs = t.Split(',');
                            Stops item = new Stops(subs[0], subs[1], subs[3], subs[4], subs[0].Substring(subs[0].Length - 8));
                            context.stops.Add(item);
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

            }
        }

        static void ExtraireStop_times(string fichier)
        {
            List<Temp> fic = ExtraireFromTxt(@"D:\Z\" + fichier);
            ListTrips = Database.GetReadTrip;

            using (var context = new BaseContext())
            {
                Stop_times item = new Stop_times();
                foreach (var f in fic)
                {
                    try
                    {
                        string t = f.Contenu;
                        if (t != null)
                        {
                            string[] subs = t.Split(',');



                            var monAssiete = ListTrips.FirstOrDefault(a => a.trip_id == subs[0]);
                            if (monAssiete != null)
                            {
                                string oStation = subs[3].Substring(subs[3].Length - 8);
                                item = new Stop_times(subs[0], subs[1], subs[2], subs[3], int.Parse(subs[4]), oStation);
                                context.stop_times.Add(item);
                                context.SaveChanges();
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var resume = ex.Message;
                    }

                }
            }
        }

        static void ExtraireStop(string fichier)
        {

            List<Temp> fic = ExtraireFromTxt(@"D:\Z\" + fichier);
            List<Stops> stops = new List<Stops>();
            foreach (var t in fic)
            {
                if (t.Contenu != null)
                {
                    string[] subs = t.Contenu.Split(',');
                 
                    stops.Add(new Stops(subs[0], subs[1], subs[3], subs[4], subs[0].Substring(subs[0].Length - 8)));
                }            
            }

            var DistinctStop_times = Database.GetReadStop_time.GroupBy(x => x.stop_id).Select(y => y.First());

            using (var context = new BaseContext())
            {
                
                foreach (var f in DistinctStop_times)
                {
                    try
                    {
                       string oRecherche = f.stop_id.Substring(f.stop_id.Length - 8);
                       Stops  recherche = stops.FirstOrDefault(a => a.stop_id == f.stop_id);
                      // Stops recherche = stops.FirstOrDefault(a => a.station_id == oRecherche);
                      
                        context.stops.Add(recherche);
                        context.SaveChanges();
                   }
                    catch (Exception ex)
                    {
                        var resume = ex.Message;
                    }

                }

            }
        }      

        static void ExtraireTrips(string fichier)
        {
            //.trip_headsign Not In(36600,36702,36703,36704,36706,36707,36708,36709,36711,36713,36700) And(Trips.trip_headsign) < 77000));
            //  route_id) In("OCESN-2002149", "OCESN-2002152", "OCESN-2002154")));
            string[] stringArray = {"OCESN-2002149","OCESN-2002152","OCESN-2002154" };
            string[] intArray = { "36600","36702","36703","36704","36706","36707","36708","36709","36711","36713","36700" };

            List<Temp> fic = ExtraireFromTxt(@"D:\Z\" + fichier );

            using (var context = new BaseContext())
            {
               
                foreach (var f in fic)
                {
                    try
                    {

                        string t = f.Contenu;
                        if (t != null)
                        {
                            string[] subs = t.Split(',');
                         
                            if (stringArray.Any(subs[0].Contains)  && !intArray.Any(subs[3].ToString().Contains))
                            {
                                context.trips.Add(new Trips(subs[0], int.Parse(subs[1]), subs[2], int.Parse(subs[3]), subs[4], int.Parse(subs[4])));
                                context.SaveChanges();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var resume = ex.Message;
                    }

                }
            }
        }

        static void ExtraireCalendar()
        {

            List<Temp> fic = ExtraireFromTxt(@"D:\Z\Calendar_dates.txt");
            ListTrips = Database.GetReadTrip;

            using (var context = new BaseContext())
            {
                Calendar_dates item = new Calendar_dates();
                int Db = int.Parse(periodes.Debut.ToString("yyyyMMdd"));
                int  Df = int.Parse(periodes.Fin.ToString("yyyyMMdd"));
                foreach (var f in fic)
                {
                    try
                    {
                        string t = f.Contenu;
                        if (t != null)
                        {
                            string[] subs = t.Split(',');
                            var monAssiete = ListTrips.FirstOrDefault(a => a.service_id.ToString("D" + 6) == subs[0]);
                            if (monAssiete != null && int.Parse(subs[1]) >=Db   && int.Parse(subs[1])<=Df)
                            {
                                item = new Calendar_dates(int.Parse(subs[0]), int.Parse(subs[1]));
                                context.calendar_dates.Add(item);
                                context.SaveChanges();
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var resume = ex.Message;
                    }

                }
            }
        }
   
        public static List<Temp> ExtraireFromTxt(string file)
        {
            List<Temp> tp = new List<Temp>();
            try
            {
                string line;
                StreamReader sr = new StreamReader(file);

                //Read the first line of text
                line = sr.ReadLine();
                string[] entete = line.Split(',');
                //Continue to read until you reach end of file
              
                while (line != null)
                {                   
                    line = sr.ReadLine();
                    tp.Add(new Temp() { Contenu = line });                     
                }
                //close the file
                sr.Close();
                Console.ReadLine();
            }
            catch {}
     
            return tp;
        }
    }
}
