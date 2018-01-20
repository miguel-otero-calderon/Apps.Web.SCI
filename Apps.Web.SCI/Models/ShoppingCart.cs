using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apps.Web.SCI.Models
{
    public class ShoppingCart
    {
        public int Index { get; set; }
        public string Transaction_Date { get; set; }
        public string Payment_Method { get; set; }
        public string Credit_Card_Number { get; set; }
        public string Auth_Code { get; set; }
        public string Transaction_ID { get; set; }
        public int IndexAuthorizeNet { get; set; }
        public string Error { get; set; }
        public bool Status { get; set; }
        public string Key
        {
            get
            {
                string key = string.Empty;
                if (!string.IsNullOrEmpty(Transaction_Date) && Transaction_Date != "??" &&
                    !string.IsNullOrEmpty(Payment_Method) && Payment_Method != "??" &&
                    !string.IsNullOrEmpty(Credit_Card_Number) && Credit_Card_Number != "??" &&
                    !string.IsNullOrEmpty(Auth_Code) && Auth_Code != "??" &&
                    !string.IsNullOrEmpty(Transaction_ID) && Transaction_ID != "??")
                {
                    char[] delimiters = new char[] { '/' };
                    string[] values = Transaction_Date.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    int month = Convert.ToInt16(values[0]);
                    int day = Convert.ToInt16(values[1]);
                    int year = Convert.ToInt32(values[2]);
                    if (year < 2000)
                        year = year + 2000;

                    key = year.ToString() + month.ToString("00") + day.ToString("00");
                    key = key + Payment_Method.Trim().ToLower();
                    key = key + Credit_Card_Number.Trim().ToLower();
                    key = key + Auth_Code.Trim().ToLower();
                    key = key + Transaction_ID.Trim().ToLower();
                }
                return key;
            }
        }

        protected void Load_Transaction_Date(DateTime value)
        {
            Transaction_Date = "??";
            try
            {
                Transaction_Date = string.Format("{0}/{1}/{2}", value.Month, value.Day, value.Year - 2000);
            }
            catch
            {
            }
        }

        public void Load_Transaction_Date(string value)
        {
            Transaction_Date = "??";
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Contains("-"))
                        Load_Transaction_Date_Format_One(value);
                    else
                        if (value.Contains("/"))
                            Load_Transaction_Date_Format_Two(value);
                }
            }
            catch
            {
            }
        }

        protected void Load_Transaction_Date_Format_One(string value)
        {
            /*example: 5-Sep-2017*/
            Transaction_Date = "??";
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    char[] delimiters = new char[] { '-' };
                    string[] values = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    int day = Convert.ToInt16(values[0]);
                    int month = GetMonth(month_name:values[1]);
                    int year = Convert.ToInt32(values[2]);
                    DateTime date = new DateTime(year, month, day);
                    Load_Transaction_Date(date);
                }
            }
            catch
            {
            }
        }

        private int GetMonth(string month_name)
        {
            int month = 0;
            switch (month_name.Trim().ToLower())
            {
                case "ene":
                    month = 1;
                    break;
                case "feb":
                    month = 2;
                    break;
                case "mar":
                    month = 3;
                    break;
                case "abr":
                    month = 4;
                    break;
                case "may":
                    month = 5;
                    break;
                case "jun":
                    month = 6;
                    break;
                case "jul":
                    month = 7;
                    break;
                case "ago":
                    month = 8;
                    break;
                case "set":
                    month = 9;
                    break;
                case "sep":
                    month = 9;
                    break;
                case "oct":
                    month = 10;
                    break;
                case "nov":
                    month = 11;
                    break;
                case "dic":
                    month = 12;
                    break;
            }
            return month;
        }

        public void Load_Transaction_ID(string value)
        {
            Transaction_ID = "??";
            if (!string.IsNullOrEmpty(value))
                Transaction_ID = value.Trim();
        }

        public void Load_Auth_Code(string value)
        {
            Auth_Code = "??";
            if (!string.IsNullOrEmpty(value))
                Auth_Code = value.Trim();
        }

        protected void Load_Transaction_Date_Format_Two(string value)
        {
            Transaction_Date = "??";
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    char[] delimiters = new char[] { '/' };
                    string[] values = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    int month = Convert.ToInt16(values[0]);
                    int day = Convert.ToInt16(values[1]);                    
                    int year = Convert.ToInt32(values[2]);
                    if (year < 2000)
                        year = year + 2000;
                    DateTime date = new DateTime(year, month, day);
                    Load_Transaction_Date(date);
                }
            }
            catch
            {
            }
        }

        public void Load_Payment_Method(string value)
        {
            Payment_Method = "??";
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.ToLower().Trim().Contains("American Express".ToLower()))
                        Payment_Method = "A";
                    else if (value.ToLower().Trim().Contains("Visa".ToLower()))
                        Payment_Method = "V";
                    else if (value.ToLower().Trim().Contains("Master Card".ToLower()))
                        Payment_Method = "M";
                }
            }
            catch
            {                
            }
        }

        public void Load_Credit_Card_Number(string value)
        {
            Credit_Card_Number = "??";
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    char[] delimiters = new char[] { ' ' };
                    string[] values = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    Credit_Card_Number = values[values.Length - 1];
                }
            }
            catch
            {                
            }
        }

        public void Validate()
        {
            if (!string.IsNullOrEmpty(Error))
            {
                Status = false;
                return;
            }
            if (Transaction_Date == "??")
            {
                Error = "!Transaction_Date Invalidate!";
                return;
            }
            else
            {
                try
                {
                    char[] delimiters = new char[] { '/' };
                    string[] values = Transaction_Date.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    int month = Convert.ToInt16(values[0]);
                    int day = Convert.ToInt16(values[1]);
                    int year = Convert.ToInt32(values[2]) + 2000;
                    DateTime date = new DateTime(year, month, day);
                }
                catch
                {
                    Error = "!Transaction_Date Invalidate!";
                    return;
                }                
            }
            
            if(Payment_Method == "??")
            {
                Error = "!Payment_Method Invalidate!";
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(Payment_Method))
                {
                    Error = "!Payment_Method Invalidate!";
                    return;
                }
            }

            if (Credit_Card_Number == "??")
            {
                Error = "!Credit_Card_Number Invalidate!";
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(Credit_Card_Number))
                {
                    Error = "!Credit_Card_Number Invalidate!";
                    return;
                }
            }

            if (Auth_Code == "??")
            {
                Error = "!Auth_Code Invalidate!";
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(Auth_Code))
                {
                    Error = "!Auth_Code Invalidate!";
                    return;
                }
            }

            if (Transaction_ID == "??")
            {
                Error = "!Transaction_ID Invalidate!";
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(Transaction_ID))
                {
                    Error = "!Transaction_ID Invalidate!";
                    return;
                }
            }
            Status = true;
        }
    }
}