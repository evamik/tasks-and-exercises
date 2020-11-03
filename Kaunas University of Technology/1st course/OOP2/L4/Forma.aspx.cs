using L4.App_Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web.UI.WebControls;

namespace L4
{
    public partial class Forma : System.Web.UI.Page
    {
        static int StartYear;
        static int EndYear;
        static int Month;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Table_Data.Rows.Count == 0 && Main.IssueList != null)
            {
                Table_Data.Visible = true;
                Button_Calculate.Visible = true;
                
                PopulateDataTable();
            }

            if (Table_Calculated.Rows.Count == 0 && Main.SubscriberList != null)
            {
                Table_Calculated.Visible = true;
                Label_From.Visible = true;
                Label_To.Visible = true;
                Label_Month.Visible = true;
                TextBox_start.Visible = true;
                TextBox_end.Visible = true;
                TextBox_month.Visible = true;
                Button_Search.Visible = true;

                PopulateCalculatedTable();
            }

            if (Table_Searched.Rows.Count == 0 && Main.DatesHeader != null)
            {
                Table_Searched.Visible = true;

                PopulateSearchTable();
            }
        }

        protected void Button_Read_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete(Server.MapPath(Main.Results));
                Main.ReadSubData();
                Main.SaveSubData(Main.DataList);
                Main.ReadIssueData();
                Main.SaveIssueData();
                
                Response.Redirect(Request.RawUrl);
            }
            catch(Exception exception)
            {
                HandleException(exception, "Įvyko klaida nuskaitant duomenis");
            }
        }

        protected void Button_Calculate_Click(object sender, EventArgs e)
        {
            try
            {
                Main.CalculateCosts();
                Main.SaveSubscriberData();

                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exception)
            {
                HandleException(exception, "Įvyko klaida skaičiuojant prenumeratorių sumas");
            }
        }

        protected void Button_Search_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = DateTime.Parse(TextBox_start.Text);
                DateTime endDate = DateTime.Parse(TextBox_end.Text);
                StartYear = startDate.Year;
                EndYear = endDate.Year;
                Month = int.Parse(TextBox_month.Text);
                Main.GetOrderDates(startDate, endDate, Month);
                Main.SaveSubscriberDataWithMonths(StartYear, EndYear, Month);

                Response.Redirect(Request.RawUrl);
            }
            catch(Exception exception)
            {
                HandleException(exception, "Įvyko klaida atrenkant užsakytus mėnesius");
            }
        }

        /// <summary>
        /// Handles the exception
        /// </summary>
        /// <param name="e"> exception </param>
        /// <param name="str"> message </param>
        protected void HandleException(Exception e, string str)
        {
            if (e is ThreadAbortException)
                return;
            ShowError(str);
            Main.SaveExceptionMessage(str+":", e);
        }

        /// <summary>
        /// Populates search table
        /// </summary>
        protected void PopulateSearchTable()
        {
            Table_Searched.CssClass = "table";
            CreateTableHeader(Table_Searched, Main.StringToStringList(Main.SubscriberHeader + Main.DatesHeader));
            PopulateTable(Table_Searched, Main.OrdersToStringList(StartYear, EndYear, Month));
        }

        /// <summary>
        /// Populates calculated table
        /// </summary>
        protected void PopulateCalculatedTable()
        {
            Table_Calculated.CssClass = "table";
            CreateTableHeader(Table_Calculated, Main.StringToStringList(Main.SubscriberHeader));
            PopulateTable(Table_Calculated, Main.SubscriberToStringList());
        }

        /// <summary>
        /// Populates data table
        /// </summary>
        protected void PopulateDataTable()
        {
            Table_Data.CssClass = "table";
            foreach (SubscribtionsData subData in Main.DataList)
            {
                AddTablePreHeader(Table_Data, subData.Date.ToString("yyyy/MM/dd"));
                CreateTableHeader(Table_Data, Main.StringToStringList(Main.SubHeader));
                PopulateTable(Table_Data, Main.SubListToStringList(subData.Subscribtions));
                TableRow row = new TableRow();
                row.Height = 30;
                Table_Data.Rows.Add(row);
            }

            CreateTableHeader(Table_Data, Main.StringToStringList(Main.IssueHeader));
            PopulateTable(Table_Data, Main.IssueListToStringList());
        }

        /// <summary>
        /// Adds table preheader
        /// </summary>
        /// <param name="table"> table </param>
        /// <param name="str"> preheader text </param>
        protected static void AddTablePreHeader(Table table, string str)
        {
            TableRow row = new TableRow();
            row.Attributes.Add("id", "preheader");
            TableCell cell = new TableCell();
            cell.Attributes.Add("id", "preheader");
            cell.Text = str;
            row.Cells.Add(cell);
            table.Rows.Add(row);
        }

        /// <summary>
        /// Creates a header for a table with list of strings as cells text
        /// </summary>
        /// <param name="table"> table </param>
        /// <param name="data"> list of cell strings </param>
        protected static void CreateTableHeader(Table table, List<string> data)
        {
            TableRow row = new TableRow();

            foreach(string cellText in data)
            {
                TableHeaderCell cell = new TableHeaderCell();
                cell.Text = cellText;
                row.Cells.Add(cell);
            }

            row.Height = 30;
            table.Rows.Add(row);
        }

        /// <summary>
        /// Populates the table with a 2D list of string data
        /// </summary>
        /// <param name="table"></param>
        /// <param name="data"></param>
        protected static void PopulateTable(Table table, List<string> data)
        {
            foreach(string str in data)
            {
                TableRow row = new TableRow();

                foreach(string cellText in Main.StringToStringList(str))
                {
                    TableCell cell = new TableCell();
                    cell.Text = cellText;
                    row.Cells.Add(cell);
                }

                table.Rows.Add(row);
            }
            table.Rows[table.Rows.Count - 1].Attributes.Add("id", "bottom");
        }

        /// <summary>
        /// Shows an error message on the Label_Error label
        /// </summary>
        /// <param name="message"> error message </param>
        protected void ShowError(string message)
        {
            Label_Error.Visible = true;
            Label_Error.Text = message;
        }
    }
}