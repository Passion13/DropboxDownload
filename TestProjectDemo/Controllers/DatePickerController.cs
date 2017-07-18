using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestProjectDemo.Models;

namespace TestProjectDemo.Controllers
{
    public class DatePickerController : Controller
    {
        //
        // GET: /DatePicker/

        public ActionResult Index()
        {
            int i = 15;
          var item = factorial(i);

            return View();
        }

        public ActionResult List(int? Id)
        {
            Id = 0;
            List<MyImage> all = new List<MyImage>();


            //var date = FirstDateOfWeekISO8601(DateTime.Now.Year, 22);
            //using (AnantTestEntities dc = new AnantTestEntities())
            //{
            //    all = dc.Usp_GetAll_UserSmileMaster(0).Select(a => new MyImage { FileData = a.FileContent,Id=a.UserSmileId}).ToList();
            //}

            return View(all);
        }

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
        public int factorial(int i)
        {
            List<int> test = new List<int>(); 
            if (i <= 1)
            {
                return 1;
            }
            var ttt = i * factorial(i - 1);
            test.Add(ttt);

            return ttt;
        }
        public ActionResult ViewAttachment()
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:58542/"); 
                request.Method = "PUT";

                using (Stream requestStream = request.GetRequestStream())
                using (Stream video = System.IO.File.OpenRead(@"D:\D\Anant\All Video types\3gp_big_buck_bunny_144p_1mb.3gp"))
                {
                    byte[] buffer = new byte[4096];

                    while (true)
                    {
                        int bytesRead = video.Read(buffer, 0, buffer.Length);

                        if (bytesRead == 0) break;
                        requestStream.Write(buffer, 0, bytesRead);
                    }
                    return File(requestStream, "3gp");
                }


                
                //return View("Detail", model);
            }
            catch (Exception ex)
            {
              
                throw ex;
            }
        }


    }
}
