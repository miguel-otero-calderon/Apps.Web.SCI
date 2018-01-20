using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apps.Web.SCI.Models
{
    public class AuthorizeNet
    {
        public string Date_Ordered { get; set; }
        public string Order_Number { get; set; }
        public string Source_DNIS { get; set; }
        public string KEYCODE { get; set; }
        public string BILL_TO_First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string City { get; set; }
        public string State_Province { get; set; }
        public string Zip_Code { get; set; }
        public string Country { get; set; }
        public string Telephone_Number { get; set; }
        public string E_mail_Address { get; set; }
        public string Payment_Method { get; set; }
        public string Credit_Card_Number { get; set; }
        public string Expiry_Date { get; set; }
        public string Auth_Code { get; set; }
        public string Transaction_ID { get; set; }
        public string Transaction_Date { get; set; }
        public string Check_Routing_Number { get; set; }
        public string Check_Account_Number { get; set; }
        public string Check_Number { get; set; }
        public string Amount_Paid_Check_Only { get; set; }
        public string Ship_To_First_Name { get; set; }
        public string Ship_To_Last_Name { get; set; }
        public string Ship_To_Add1 { get; set; }
        public string Ship_To_Add2 { get; set; }
        public string Ship_To_City { get; set; }
        public string Ship_To_State { get; set; }
        public string Ship_To_Zip { get; set; }
        public string Ship_To_Country { get; set; }
        public string Order_Level_Tax { get; set; }
        public string Order_Level_SH { get; set; }
        public string Order_Level_Total_Amount { get; set; }
        public string SKU_1 { get; set; }
        public string DESCRIPTION_1 { get; set; }
        public string QTY_1 { get; set; }
        public string PRICE_1 { get; set; }
        public string TAX_1 { get; set; }
        public string S_H_1 { get; set; }
        public string SKU_2 { get; set; }
        public string DESCRIPTION_2 { get; set; }
        public string QTY_2 { get; set; }
        public string PRICE_2 { get; set; }
        public string TAX_2 { get; set; }
        public string S_H_2 { get; set; }
        public string SKU_3 { get; set; }
        public string DESCRIPTION_3 { get; set; }
        public string QTY_3 { get; set; }
        public string PRICE_3 { get; set; }
        public string TAX_3 { get; set; }
        public string S_H_3 { get; set; }
        public string SKU_4 { get; set; }
        public string DESCRIPTION_4 { get; set; }
        public string QTY_4 { get; set; }
        public string PRICE_4 { get; set; }
        public string TAX_4 { get; set; }
        public string S_H_4 { get; set; }
        public string SKU_5 { get; set; }
        public string DESCRIPTION_5 { get; set; }
        public string QTY_5 { get; set; }
        public string PRICE_5 { get; set; }
        public string TAX_5 { get; set; }
        public string S_H_5 { get; set; }
        public string RESERVED_1 { get; set; }
        public string RESERVED_2 { get; set; }
        public string RESERVED_3 { get; set; }
        public string RESERVED_4 { get; set; }
        public string RESERVED_5 { get; set; }
        public string Error { get; set; }
        public bool Status { get; set; }
        public int Index { get; set; }
        public int IndexShoppingCart { get; set; }
        public string Key
        {
            get
            {                
                string key = string.Empty;
                if(!string.IsNullOrEmpty(Transaction_Date) && Transaction_Date != "??" &&
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
                    int month = GetMonth(month_name: values[1]);
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
                    if (value.ToLower().Trim().Contains("American Express".ToLower()) || value.ToLower().Trim() == "A".ToLower())
                        Payment_Method = "A";
                    else if (value.ToLower().Trim().Contains("Visa".ToLower()) || value.ToLower().Trim() == "V".ToLower())
                        Payment_Method = "V";
                    else if (value.ToLower().Trim().Contains("Master Card".ToLower()) || value.ToLower().Trim() == "M".ToLower())
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
            if (!string.IsNullOrEmpty(value))
                Credit_Card_Number = value.Trim();
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

            if (Payment_Method == "??")
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

        public void Load_Fields(string line)
        {
            try
            {
                char[] delimiters = new char[] { ';' };
                string[] values = line.Split(delimiters);

                Date_Ordered = values[0];
                Order_Number = values[1];
                Source_DNIS = values[2];
                KEYCODE = values[3];
                BILL_TO_First_Name = values[4];
                Last_Name = values[5];
                Address_1 = values[6];
                Address_2 = values[7];
                City = values[8];
                State_Province = values[9];
                Zip_Code = values[10];
                Country = values[11];
                Telephone_Number = values[12];
                E_mail_Address = values[13];
                Payment_Method = values[14];
                Credit_Card_Number = values[15];
                Expiry_Date = values[16];
                Auth_Code = values[17];
                Transaction_ID = values[18];
                Transaction_Date = values[19];
                Check_Routing_Number = values[20];
                Check_Account_Number = values[21];
                Check_Number = values[22];
                Amount_Paid_Check_Only = values[23];
                Ship_To_First_Name = values[24];
                Ship_To_Last_Name = values[25];
                Ship_To_Add1 = values[26];
                Ship_To_Add2 = values[27];
                Ship_To_City = values[28];
                Ship_To_State = values[29];
                Ship_To_Zip = values[30];
                Ship_To_Country = values[31];
                Order_Level_Tax = values[32];
                Order_Level_SH = values[33];
                Order_Level_Total_Amount = values[34];
                SKU_1 = values[35];
                DESCRIPTION_1 = values[36];
                QTY_1 = values[37];
                PRICE_1 = values[38];
                TAX_1 = values[39];
                S_H_1 = values[40];
                SKU_2 = values[41];
                DESCRIPTION_2 = values[42];
                QTY_2 = values[43];
                PRICE_2 = values[44];
                TAX_2 = values[45];
                S_H_2 = values[46];
                SKU_3 = values[47];
                DESCRIPTION_3 = values[48];
                QTY_3 = values[49];
                PRICE_3 = values[50];
                TAX_3 = values[51];
                S_H_3 = values[52];
                SKU_4 = values[53];
                DESCRIPTION_4 = values[54];
                QTY_4 = values[55];
                PRICE_4 = values[56];
                TAX_4 = values[57];
                S_H_4 = values[58];
                SKU_5 = values[59];
                DESCRIPTION_5 = values[60];
                QTY_5 = values[61];
                PRICE_5 = values[62];
                TAX_5 = values[63];
                S_H_5 = values[64];
                RESERVED_1 = values[65];
                RESERVED_2 = values[66];
                RESERVED_3 = values[67];
                RESERVED_4 = values[68];
                RESERVED_5 = values[69];
            }
            catch
            {

            }        
        }        


    }
}