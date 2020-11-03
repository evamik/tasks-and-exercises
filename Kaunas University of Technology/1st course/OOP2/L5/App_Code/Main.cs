using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace L4.App_Code
{
    /// <summary>
    /// class that stores all execution methods
    /// </summary>
    public class Main
    {
        public const string SubDataDir = "~/App_Data/Subscribtions/";
        public const string IssueData = "~/App_Data/IssueData.txt";
        public const string Results = "~/App_Data/Results.txt";

        const string SubHeaderLine = "-----------------------------------------------------------------------";
        public const string SubHeader = "| Pavardė      | Adresas      | Pradžia | Ilgis | Leid. Kod. | Kiekis |";
        const string IssueHeaderLine = "--------------------------------------";
        public const string IssueHeader =     "| Kodas      | Pavadinimas  | €/mėn. |";
        const string SubscriberHeaderLine = "-----------------------------------------";
        public const string SubscriberHeader =     "| Pavardė      | Adresas      | Suma, € |";

        public static List<SubscribtionsData> DataList;
        public static List<Subscriber> SubscriberList;
        public static List<Issue> IssueList;
        public static string DatesHeader;

        /// <summary>
        /// Reads data from App_Data/Subscribtion/ directory and stores it in
        /// a list of SubscribtionData classes.
        /// </summary>
        public static void ReadSubData()
        {
            DataList = new List<SubscribtionsData>();
            string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath(SubDataDir));
            foreach (string file in files)
            {
                string[] lines = File.ReadAllLines(file);
                DateTime date = DateTime.Parse(lines[0]);
                List<Subscribtion> subList = new List<Subscribtion>();
                foreach (string line in lines.Skip(1)) {
                    string[] data = line.Split(',');
                    string surname = data[0].Trim();
                    string address = data[1].Trim();
                    int periodStart = int.Parse(data[2]);
                    int periodLength = int.Parse(data[3]);
                    string issueCode = data[4].Trim();
                    int amount = int.Parse(data[5]);
                    Subscribtion sub = new Subscribtion(surname, address, periodStart, periodLength, issueCode, amount);
                    subList.Add(sub);
                }
                SubscribtionsData subData = new SubscribtionsData(subList, date);
                DataList.Add(subData);
            }
        }

        /// <summary>
        /// Saves the dataList list to the results file.
        /// </summary>
        /// <param name="dataList"></param>
        public static void SaveSubData(List<SubscribtionsData> dataList)
        {
            using (var fr = File.AppendText(HttpContext.Current.Server.MapPath(Results)))
            {
                foreach (SubscribtionsData subData in dataList)
                {
                    fr.WriteLine(subData.Date.ToString("yyy/MM/dd"));
                    fr.WriteLine(SubHeaderLine);
                    fr.WriteLine(SubHeader);
                    fr.WriteLine(SubHeaderLine);

                    foreach(Subscribtion sub in subData.Subscribtions)
                    {
                        fr.WriteLine(sub.ToString());
                    }

                    fr.WriteLine(SubHeaderLine);
                    fr.WriteLine();
                }
            }
        }

        /// <summary>
        /// Reads issues data from App_Data/IssueData.txt and stores it in a
        /// list of Issue classes.
        /// </summary>
        public static void ReadIssueData()
        {
            IssueList = new List<Issue>();
            string[] lines = File.ReadAllLines(HttpContext.Current.Server.MapPath(IssueData));

            foreach(string line in lines)
            {
                string[] data = line.Split(',');
                string code = data[0].Trim();
                string title = data[1].Trim();
                double monthlyCost = double.Parse(data[2]);
                Issue issue = new Issue(code, title, monthlyCost);
                IssueList.Add(issue);

            }
        }

        /// <summary>
        /// Saves the IssueList list to the results file.
        /// </summary>
        public static void SaveIssueData()
        {
            using (var fr = File.AppendText(HttpContext.Current.Server.MapPath(Results)))
            {
                fr.WriteLine(IssueHeaderLine);
                fr.WriteLine(IssueHeader);
                fr.WriteLine(IssueHeaderLine);

                foreach (Issue issue in IssueList)
                {
                    fr.WriteLine(issue.ToString());
                }

                fr.WriteLine(IssueHeaderLine);
                fr.WriteLine();
            }
        }

        /// <summary>
        /// Saves the SubscriberList list to the results file.
        /// </summary>
        public static void SaveSubscriberData()
        {
            using (var fr = File.AppendText(HttpContext.Current.Server.MapPath(Results)))
            {
                fr.WriteLine(SubscriberHeaderLine);
                fr.WriteLine(SubscriberHeader);
                fr.WriteLine(SubscriberHeaderLine);

                foreach (Subscriber subscriber in SubscriberList)
                {
                    fr.WriteLine(subscriber.ToString());
                }

                fr.WriteLine(SubscriberHeaderLine);
                fr.WriteLine();
            }
        }

        /// <summary>
        /// Saves the SubscriberList list along with months that were ordered in the timeframe
        /// </summary>
        /// <param name="startDate"> starting date</param>
        /// <param name="endDate"> ending date</param>
        /// <param name="month"> month </param>
        public static void SaveSubscriberDataWithMonths(int startYear, int endYear, int month)
        {
            string headerLine = SubscriberHeaderLine + "".PadRight(DatesHeader.Length, '-');
            string header = SubscriberHeader + DatesHeader;

            using (var fr = File.AppendText(HttpContext.Current.Server.MapPath(Results)))
            {
                fr.WriteLine(headerLine);
                fr.WriteLine(header);
                fr.WriteLine(headerLine);

                foreach (Subscriber subscriber in SubscriberList)
                {
                    fr.Write(subscriber.ToString());
                    for(int i = 0; i < endYear-startYear+1; i++)
                    {
                        if (subscriber.OrderedDates.Contains(new DateTime(startYear + i, month, 1)))
                            fr.Write(" ******* |");
                        else fr.Write(" ....... |");
                    }
                    fr.WriteLine();
                }

                fr.WriteLine(headerLine);
                fr.WriteLine();
            }
        }

        /// <summary>
        /// Saves a message with exception message to the results file.
        /// </summary>
        /// <param name="message"> message </param>
        /// <param name="exception"> exception </param>
        public static void SaveExceptionMessage(string message, Exception exception)
        {
            using (var fr = File.AppendText(HttpContext.Current.Server.MapPath(Results)))
            {
                fr.WriteLine(message);
                fr.WriteLine(exception.Message);
                fr.WriteLine();
            }
        }

        /// <summary>
        /// Calculates the total cost for each subscriber based on their subscribtions
        /// and stores it in a list of Subscriber classes
        /// </summary>
        public static void CalculateCosts()
        {
            SubscriberList = new List<Subscriber>();

            var query = from subData in DataList
                        from sub in subData.Subscribtions
                        select new { sub, cost = sub.PeriodLength * sub.Amount * SubToIssue(sub).MonthlyCost };

            foreach (var pair in query) {
                var subscriber = SubToSubscriber(pair.sub);
                if (subscriber == null)
                {
                    subscriber = new Subscriber(pair.sub.Surname, pair.sub.Address);
                    subscriber.AddCost(pair.cost);
                    SubscriberList.Add(subscriber);
                }
                else subscriber.AddCost(pair.cost);
            }
        }

        /// <summary>
        /// Gets all dates for subscribers that ordered from startDate to endDate on "month" month.
        /// </summary>
        /// <param name="startDate"> starting date </param>
        /// <param name="endDate"> ending date </param>
        /// <param name="month"> month </param>
        public static void GetOrderDates(DateTime startDate, DateTime endDate, int month)
        {
            DatesForHeader(startDate, endDate, month);

            var query = from subData in DataList
                        where subData.Date >= startDate && subData.Date < endDate
                        from sub in subData.Subscribtions
                        where month >= sub.PeriodStart && month <= sub.PeriodStart + sub.PeriodLength
                        select new { date = new DateTime(subData.Date.Year, month, 1), subscriber = SubToSubscriber(sub) };

            foreach (var pair in query)
                if (!pair.subscriber.OrderedDates.Contains(pair.date))
                    pair.subscriber.AddDate(pair.date);
        }

        /// <summary>
        /// Sorts SubscriberList list in ascending order by address then by surname.
        /// </summary>
        public static void SortSubscribers()
        {
            SubscriberList = SubscriberList.OrderBy(a => a.Address).ThenBy(a => a.Surname).ToList();
        }

        /// <summary>
        /// Finds the Issue class in IssueList from sub
        /// </summary>
        /// <param name="sub"> Subscribtion class </param>
        /// <returns> null if doesn't exist </returns>
        public static Issue SubToIssue(Subscribtion sub)
        {
            return IssueList.SingleOrDefault(a => a.Code == sub.IssueCode);
        }

        /// <summary>
        /// Finds the Subscriber class in SubscriberList from sub
        /// </summary>
        /// <param name="sub"> Subscribtion class </param>
        /// <returns> null if doesn't exist </returns>
        public static Subscriber SubToSubscriber(Subscribtion sub)
        {
            return SubscriberList.SingleOrDefault(a => a.Surname == sub.Surname && a.Address == sub.Address);
        }

        /// <summary>
        /// Builds a dates string for header
        /// </summary>
        /// <param name="startDate"> starting date </param>
        /// <param name="endDate"> ending date </param>
        /// <param name="month"> month </param>
        public static void DatesForHeader(DateTime startDate, DateTime endDate, int month)
        {
            int years = endDate.Year - startDate.Year + 1;
            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < years; i++)
            {
                stringBuilder.Append($" {startDate.Year + i}/{month:D2} |");
            }
            DatesHeader = stringBuilder.ToString();
        }

        /// <summary>
        /// Converts string with '|' characters to list of strings
        /// </summary>
        /// <param name="str"> convertable string </param>
        /// <returns> string list </returns>
        public static List<string> StringToStringList(string str)
        {
            List<string> list = new List<string>();
            
            Regex r = new Regex(@"(?<=\|)(.+?)(?=\|)");

            MatchCollection mc = r.Matches(str);

            foreach(Match m in mc)
            {
                list.Add(m.Groups[1].Value.Trim());
            }

            return list;
        }

        /// <summary>
        /// Converts subscribtion list to string list
        /// </summary>
        /// <param name="subList"> subscribtion list </param>
        /// <returns> string list </returns>
        public static List<string> SubListToStringList(List<Subscribtion> subList)
        {
            return subList.Select(a => a.ToString()).ToList();
        }

        /// <summary>
        /// Converts IssueList list to string list
        /// </summary>
        /// <returns> string list </returns>
        public static List<string> IssueListToStringList()
        {
            return IssueList.Select(a => a.ToString()).ToList();
        }

        /// <summary>
        /// Converts SubscriberList list to string list
        /// </summary>
        /// <returns> string list </returns>
        public static List<string> SubscriberToStringList()
        {
            return SubscriberList.Select(a => a.ToString()).ToList();
        }

        /// <summary>
        /// Converts SubscriberList with ordered months to string list
        /// </summary>
        /// <param name="startYear"> starting year </param>
        /// <param name="endYear"> ending year </param>
        /// <param name="month"> month </param>
        /// <returns> string list </returns>
        public static List<string> OrdersToStringList(int startYear, int endYear, int month)
        {
            List<string> list = new List<string>();
            foreach(Subscriber subscriber in SubscriberList)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(subscriber.ToString());
                for (int i = 0; i < endYear - startYear + 1; i++)
                {
                    if (subscriber.OrderedDates.Any(a => a.Year == startYear + i && a.Month == month && a.Day == 1))
                        sb.Append(" ******* |");
                    else sb.Append(" ....... |");
                }
                list.Add(sb.ToString());
            }
            return list;
        }
    }
}