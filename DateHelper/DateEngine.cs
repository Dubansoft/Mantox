using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateHelper
{
    public class DateEngine
    {
        private string error;
        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        private object date;
        public object Date
        {
            set { date = value; }
        }

        /// <summary>
        /// Example: domingo, 10 de enero de 2016 06:08:02 p.m.
        /// </summary>
        public static string CurrentDateTime
        {
            get
            {
                return DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

            }
        }

        /// <summary>
        /// Example: 42367,456444
        /// </summary>
        public static string CurrentDateTimeDouble
        {
            get { return "" + DateTime.Now.ToOADate().ToString() + ""; }
        }

        /// <summary>
        /// Example: 42768
        /// </summary>
        public static string CurrentDateInteger
        {
            get { return "" + DateTime.Today.ToOADate().ToString() + ""; }
        }

        /// <summary>
        /// Example: 01/01/1990 11:00:15 a.m.
        /// </summary>
        public static string CurrentDateTimeShort
        {

            get { return DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(); }

        }

        /// <summary>
        /// Example: 01/01/1990
        /// </summary>
        public static string CurrentDateShort
        {

            get { return DateTime.Today.ToShortDateString(); }

        }

        /// <summary>
        /// Example: 11:00:15 a.m.
        /// </summary>
        public static string CurrentTimeShort
        {

            get { return DateTime.Now.ToShortTimeString(); }
        }

        public DateEngine(object myDate)
        {
            Date = myDate;
        }

        public DateEngine() { }

        public string FromDoubleToStringDate(object date)
        {
            this.date = date;
            return FromDoubleToStringDate();
        }

        public string FromDoubleToStringDate()
        {
            try
            {
                double doubleDate = Convert.ToDouble(date);
                DateTime baseDate = DateTime.FromOADate(doubleDate);
                return baseDate.ToShortDateString() + " " + baseDate.ToShortTimeString();
            }
            catch (Exception ex)
            {
                error = ex.Message.ToString();
                return "";
            }

        }

        public string FromDoubleToShortStringDate(object date)
        {
            this.date = date;
            return FromDoubleToShortStringDate();
        }

        public string FromDoubleToShortStringDate()
        {
            try
            {
                double doubleDate = Convert.ToDouble(date);
                DateTime baseDate = DateTime.FromOADate(doubleDate);
                return baseDate.ToShortDateString();
            }
            catch (Exception ex)
            {
                error = ex.Message.ToString();
                return "";
            }

        }

        public int FromValueDateToInteger(DateTime dateValue)
        {
            return (int)dateValue.ToOADate();
        }

        public static long GetDateInMilliseconds(string dateNumeric)
        {
            try
            {
                DateTime myDate = DateTime.FromOADate(Convert.ToDouble(dateNumeric));
                return GetDateInMilliseconds(myDate);
            }
            catch (Exception ee)
            {
                //EventLogger.LogEvent(null, ee.Message.ToString(), ee);
                return 0;
            }

        }

        public static long GetDateInMilliseconds(DateTime date)
        {
            DateEngine myDateEngine = new DateEngine();

            //Console.WriteLine("Received date is " + myDateEngine.FromDoubleToShortStringDate(date.ToOADate()));

            try
            {
                long startTick = (new DateTime(1970, 1, 1)).Ticks;
                long endTick = date.Ticks;
                long tick = endTick - startTick;

                long milliseconds = tick / TimeSpan.TicksPerMillisecond;

                Console.WriteLine(milliseconds.ToString());

                return milliseconds;
            }
            catch (Exception ee)
            {
                //EventLogger.LogEvent(null, ee.Message.ToString(), ee);
                return 0;
            }
        }
    }
}
