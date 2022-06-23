using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AulpagCollecte.Services
{
    class CollecteWeb
    {

        public static bool GetDepatures()
        {

            string DateQuery = DateTime.Now.ToString("yyyMMdd") + "T000000";
            string fichier = "D:\\Z\\Api_jour\\" + DateTime.Now.ToString("MMdd_HHmm") + ".json";

            string Line = "line:SNCF:FR:Line::6155CD5F-AB2D-4274-A052-8C7462540578:/";
            string Query = "departures?&data_freshness=realtime&count=200&from_datetime=" + DateQuery;

            string Serveur = "https://" + "api.sncf.com/v1/coverage/sncf/lines/" + Line + Query + "&;";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Serveur);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var byteArray = System.Text.Encoding.ASCII.GetBytes($"8a443eff-3e05-49ff-b077-d6e18a20f5d6");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            var task = client.GetStringAsync(Serveur);
            task.Wait();
            JObject Json = JObject.Parse(task.Result);

            StreamWriter sw = new StreamWriter(fichier);           
            sw.WriteLine(Json.ToString());                    
            sw.Close();

            return true;
        }
    }
}
