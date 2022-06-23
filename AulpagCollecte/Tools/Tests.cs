using AulpagCollecte.Data;
using System.Linq;

namespace AulpagCollecte.Tools
{
    class Tests
    {
        public static void Getjours_de_passage()
        {
            int service = 12422;

            using (BaseContext context = new BaseContext())
            {
                var calendrier = Database.Services_in_calendar.Where(x => x.service_id == service);
                int[,] jours = new int[2,8];
                foreach (var o in calendrier)
                {
                   
                    int t = o.jour_semaine;
                    if (t == 0) t = 7;
                    jours[0, t]++;

                    if(o.date!=null) jours[1, t]++;
                }
          
            }
        }
    }
}
