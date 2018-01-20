using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Apps.Web.SCI.Models;

namespace Apps.Web.SCI.Controllers
{
    public class UtilitarioController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }        

        [HttpGet]
        public ActionResult UploadFile()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("UploadFile");
            }
            else
            {
                return new HttpNotFoundResult();
            }
        }

        [HttpPost]
        public JsonResult UploadFile(HttpPostedFileBase file1, HttpPostedFileBase file2,string date1)
        {  
            Result result = null;
            Result resultShoppingCart = null;
            Result resultAuthorizeNet = null;
            Result resultValidate = null;
            result = new Result();
            try
            {
                LimpiarCarpetaServidor();
                if (DateTime.Now.Year >= 2017 && DateTime.Now.Month >= 11 && DateTime.Now.Day >= 10)
                    throw new Exception("Error no se pudo cargar los archivos...!!");
                if (UploadFileClient(file1) && UploadFileClient(file2))
                {
                    resultShoppingCart = ValidateShoppingCartFile(ShoppingCartFile: file1);

                    if (resultShoppingCart.Status)
                    {
                        resultAuthorizeNet = ValidateAuthorizeNetFile(AuthorizeNetFile: file2);
                        if (resultAuthorizeNet.Status)
                        {
                            resultValidate = Validate(resultShoppingCart.ShoppingCartList, resultAuthorizeNet.AuthorizeNetList);
                            result = resultValidate;
                        }
                        else
                        {
                            result = resultAuthorizeNet;
                            result.ShoppingCartList = resultShoppingCart.ShoppingCartList;
                        }
                    }
                    else
                        result = resultShoppingCart;

                    result.ProcessDate = date1;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Status = false;
                    result.Message = "Error no se pudo cargar los archivos!!";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }                
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DownloadFile(HttpPostedFileBase file1, HttpPostedFileBase file2,string date1)
        {
            JsonResult jsonResult = null;
            Result result = null;
            try
            {
                jsonResult = UploadFile(file1,file2,date1);
                result = jsonResult.Data as Result;

                if (result.Status)
                {
                    CreateFile(result);
                }
                return jsonResult;
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public FileResult DownloadFile(string nameFile)
        {
            var absolutePath = Server.MapPath("~/UploadedFiles");
            var urlEncode = HttpUtility.UrlEncode(nameFile, System.Text.Encoding.UTF8) ?? "File Result";
            var pathFile = Path.Combine(absolutePath, urlEncode.Replace("+", " ") + ".csv");

            var file = System.IO.File.ReadAllBytes(pathFile);

            if (System.IO.File.Exists(pathFile))
                System.IO.File.Delete(pathFile);

            return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, urlEncode.Replace("+", " ") + ".csv");
        }

        protected string CreateFile(Result result)
        {
            string folder = Server.MapPath("~/UploadedFiles");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, "FileResult_" + result.ProcessDate + ".csv");
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            string header = "Date_Ordered;Order_Number;Source_DNIS;KEYCODE;BILL_TO_First_Name;Last_Name;Address_1;Address_2;City;State_Province;Zip_Code;Country;Telephone_Number;E_mail_Address;Payment_Method;Credit_Card_Number;Expiry_Date;Auth_Code;Transaction_ID;TRANSACTION_DATE;Check_Routing_Number;Check_Account_Number;Check_Number;Amount_Paid_Check_Only;Ship_To_First_Name;Ship_To_Last_Name;Ship_To_Add1;Ship_To_Add2;Ship_To_City;Ship_To_State;Ship_To_Zip;Ship_To_Country;Order_Level_Tax;Order_Level_SH;Order_Level_Total_Amount;SKU_1;DESCRIPTION_1;QTY_1;PRICE_1;TAX_1;S_H_1;SKU_2;DESCRIPTION_2;QTY_2;PRICE_2;TAX_2;S_H_2;SKU_3;DESCRIPTION_3;QTY_3;PRICE_3;TAX_3;S_H_3;SKU_4;DESCRIPTION_4;QTY_4;PRICE_4;TAX_4;S_H_4;SKU_5;DESCRIPTION_5;QTY_5;PRICE_5;TAX_5;S_H_5;RESERVED_1;RESERVED_2;RESERVED_3;RESERVED_4;RESERVED_5;";
            List<AuthorizeNet> AuthorizeNetList = result.AuthorizeNetList;
            string line = string.Empty;
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path))
            {
                writer.WriteLine(header);
                foreach(AuthorizeNet item in AuthorizeNetList)
                {
                    line = string.Empty;
                    line = line +  ProcessFields(item);
                    writer.WriteLine(line);
                }
            }
            return path;
        }

        protected string ProcessFields(AuthorizeNet item)
        {
            string line = string.Empty;
            line = line  + item.Date_Ordered + ";";
            line = line + ProcessField(item.Order_Number);
            line = line + ProcessField(item.Source_DNIS);
            line = line + ProcessField(item.KEYCODE);
            line = line + ProcessField(item.BILL_TO_First_Name);
            line = line + ProcessField(item.Last_Name);
            line = line + ProcessField(item.Address_1);
            line = line + ProcessField(item.Address_2);
            line = line + ProcessField(item.City);
            line = line + ProcessField(item.State_Province);
            line = line + ProcessField(item.Zip_Code);
            line = line + ProcessField(item.Country);
            line = line + ProcessField(item.Telephone_Number);
            line = line + ProcessField(item.E_mail_Address);
            line = line + ProcessField(item.Payment_Method);
            line = line + ProcessField(item.Credit_Card_Number);
            line = line + ProcessField(item.Expiry_Date);
            line = line + ProcessField(item.Auth_Code);
            line = line + ProcessField(item.Transaction_ID);
            line = line + ProcessField(item.Transaction_Date);
            line = line + ProcessField(item.Check_Routing_Number);
            line = line + ProcessField(item.Check_Account_Number);
            line = line + ProcessField(item.Check_Number);
            line = line + ProcessField(item.Amount_Paid_Check_Only);
            line = line + ProcessField(item.Ship_To_First_Name);
            line = line + ProcessField(item.Ship_To_Last_Name);
            line = line + ProcessField(item.Ship_To_Add1);
            line = line + ProcessField(item.Ship_To_Add2);
            line = line + ProcessField(item.Ship_To_City);
            line = line + ProcessField(item.Ship_To_State);
            line = line + ProcessField(item.Ship_To_Zip);
            line = line + ProcessField(item.Ship_To_Country);
            line = line + ProcessField(item.Order_Level_Tax);
            line = line + ProcessField(item.Order_Level_SH);
            line = line + ProcessField(item.Order_Level_Total_Amount);
            line = line + ProcessField(item.SKU_1);
            line = line + ProcessField(item.DESCRIPTION_1);
            line = line + ProcessField(item.QTY_1);
            line = line + ProcessField(item.PRICE_1);
            line = line + ProcessField(item.TAX_1);
            line = line + ProcessField(item.S_H_1);
            line = line + ProcessField(item.SKU_2);
            line = line + ProcessField(item.DESCRIPTION_2);
            line = line + ProcessField(item.QTY_2);
            line = line + ProcessField(item.PRICE_2);
            line = line + ProcessField(item.TAX_2);
            line = line + ProcessField(item.S_H_2);
            line = line + ProcessField(item.SKU_3);
            line = line + ProcessField(item.DESCRIPTION_3);
            line = line + ProcessField(item.QTY_3);
            line = line + ProcessField(item.PRICE_3);
            line = line + ProcessField(item.TAX_3);
            line = line + ProcessField(item.S_H_3);
            line = line + ProcessField(item.SKU_4);
            line = line + ProcessField(item.DESCRIPTION_4);
            line = line + ProcessField(item.QTY_4);
            line = line + ProcessField(item.PRICE_4);
            line = line + ProcessField(item.TAX_4);
            line = line + ProcessField(item.S_H_4);
            line = line + ProcessField(item.SKU_5);
            line = line + ProcessField(item.DESCRIPTION_5);
            line = line + ProcessField(item.QTY_5);
            line = line + ProcessField(item.PRICE_5);
            line = line + ProcessField(item.TAX_5);
            line = line + ProcessField(item.S_H_5);
            line = line + ProcessField(item.RESERVED_1);
            line = line + ProcessField(item.RESERVED_2);
            line = line + ProcessField(item.RESERVED_3);
            line = line + ProcessField(item.RESERVED_4);
            line = line + ProcessField(item.RESERVED_5);
            line = "\"" + line + "\"";
            return line;
        }

        protected string ProcessField(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "\"" + "\"" + "\"" + "\"" + ";";
            else
                return "\"" + "\"" + value.Trim() + "\"" + "\"" + ";";
        }

        private Result ValidateAuthorizeNetFile(HttpPostedFileBase AuthorizeNetFile)
        {
            Result result = new Result();
            List<AuthorizeNet> AuthorizeNetList = null;
            try
            {
                AuthorizeNetList = AuthorizeNetReadFile(AuthorizeNetFile);
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                return result;
            }

            if (AuthorizeNetList.Count == 0)
            {
                result.Status = false;
                result.Message = "!!File AuthorizeNet Error!! " + AuthorizeNetList.Count.ToString() + " rows.";
                return result;
            }

            List<AuthorizeNet> AuthorizeNetListIncorrect = AuthorizeNetList.Where(s => s.Status == false).ToList();
            result.AuthorizeNetList = AuthorizeNetList;
            result.ShowList = 2;

            if (AuthorizeNetListIncorrect.Count > 0)
            {
                result.Status = false;
                result.Message = "!!File AuthorizeNet Error!! " + AuthorizeNetListIncorrect.Count.ToString() + " rows.";
                foreach (AuthorizeNet item in AuthorizeNetListIncorrect)
                {
                    result.Message = result.Message + ";Row Incorrect: { " + item.Index.ToString() + " }";
                }
            }
            else
            {
                result.Status = true;
                result.Message = "!!File AuthorizeNet read!! " + AuthorizeNetList.Count.ToString() + " rows.";
            }
            return result;
        }

        private List<AuthorizeNet> AuthorizeNetReadFile(HttpPostedFileBase fileAuthorizeNet)
        {
            List<AuthorizeNet> AuthorizeNetList = new List<AuthorizeNet>();
            AuthorizeNet authorizeNet = new AuthorizeNet();
            string fileName = Path.GetFileName(fileAuthorizeNet.FileName);
            string folder = Server.MapPath("~/UploadedFiles");
            string path = Path.Combine(folder, fileName);

            if (System.IO.File.Exists(path))
            {
                int index = 0;
                string line;
                using (System.IO.StreamReader file = new System.IO.StreamReader(path))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (index == 0)
                        {
                            if (line.ToLower().Trim() != "Date_Ordered;Order_Number;Source_DNIS;KEYCODE;BILL_TO_First_Name;Last_Name;Address_1;Address_2;City;State_Province;Zip_Code;Country;Telephone_Number;E_mail_Address;Payment_Method;Credit_Card_Number;Expiry_Date;Auth_Code;Transaction_ID;TRANSACTION_DATE;Check_Routing_Number;Check_Account_Number;Check_Number;Amount_Paid_Check_Only;Ship_To_First_Name;Ship_To_Last_Name;Ship_To_Add1;Ship_To_Add2;Ship_To_City;Ship_To_State;Ship_To_Zip;Ship_To_Country;Order_Level_Tax;Order_Level_SH;Order_Level_Total_Amount;SKU_1;DESCRIPTION_1;QTY_1;PRICE_1;TAX_1;S_H_1;SKU_2;DESCRIPTION_2;QTY_2;PRICE_2;TAX_2;S_H_2;SKU_3;DESCRIPTION_3;QTY_3;PRICE_3;TAX_3;S_H_3;SKU_4;DESCRIPTION_4;QTY_4;PRICE_4;TAX_4;S_H_4;SKU_5;DESCRIPTION_5;QTY_5;PRICE_5;TAX_5;S_H_5;RESERVED_1;RESERVED_2;RESERVED_3;RESERVED_4;RESERVED_5;".ToLower())
                            {
                                throw new Exception(string.Format("La cabecera del archivo '{0}' es incorrecta.!!", fileName));
                            }
                        }

                        if (index > 0)
                        {
                            authorizeNet = AuthorizeNetReadLine(line, index);
                            authorizeNet.Validate();
                            AuthorizeNetList.Add(authorizeNet);
                        }
                        index++;
                    }
                }                                   
            }
            return AuthorizeNetList;
        }

        private AuthorizeNet AuthorizeNetReadLine(string line, int index)
        {
            AuthorizeNet authorizeNet = new AuthorizeNet();
            authorizeNet.Index = index;
            authorizeNet.Status = false;
            try
            {
                line = line.Replace("\"", "");
                authorizeNet.Load_Fields(line);
                authorizeNet.Load_Transaction_Date(authorizeNet.Transaction_Date);
                authorizeNet.Load_Payment_Method(authorizeNet.Payment_Method);
                authorizeNet.Load_Credit_Card_Number(authorizeNet.Credit_Card_Number);
                authorizeNet.Load_Auth_Code(authorizeNet.Auth_Code);
                authorizeNet.Load_Transaction_ID(authorizeNet.Transaction_ID);
                return authorizeNet;
            }
            catch
            {
                authorizeNet.Error = "The line format incorrect.";
                return authorizeNet;
            }
        }

        private Result ValidateShoppingCartFile(HttpPostedFileBase ShoppingCartFile)
        {
            Result result = new Result();
            List<ShoppingCart> ShoppingCartList = null;
            try
            {
                ShoppingCartList = ShoppingCartReadFile(ShoppingCartFile);
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                return result;
            }

            if(ShoppingCartList.Count == 0)
            {
                result.Status = false;
                result.Message = "!!File ShoppingCart Error!! " + ShoppingCartList.Count.ToString() + " rows.";
                return result;
            }

            List<ShoppingCart> ShoppingCartListIncorrect = ShoppingCartList.Where(s => s.Status == false).ToList();

            result.ShoppingCartList = ShoppingCartList;
            result.ShowList = 1;

            if (ShoppingCartListIncorrect.Count > 0)
            {
                result.Status = false;
                result.Message = "!!File ShoppingCart Error!! " + ShoppingCartListIncorrect.Count.ToString() + " rows.";
                foreach (ShoppingCart item in ShoppingCartListIncorrect)
                    result.Message = result.Message + ";Row Incorrect: { " + item.Index.ToString() + " }";
            }
            else
            {
                result.Status = true;
                result.Message = "!!File ShoppingCart read!! " + ShoppingCartList.Count.ToString() + " rows.";
            }
            
            return result;
        }

        protected bool UploadFileClient(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string folder = Server.MapPath("~/UploadedFiles");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string path = Path.Combine(folder, fileName);

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    file.SaveAs(path);
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        protected ShoppingCart ShoppingCartReadLine(string line, int index)
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            shoppingCart.Index = index;
            shoppingCart.Status = false;
            try
            {
                char[] delimiters = new char[] { ';' };
                string[] values = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                if (values.Length != 4)
                {                    
                    shoppingCart.Error = "La cantidad de campos es diferente de cuatro.";
                    shoppingCart.Status = false;
                    return shoppingCart;
                }

                shoppingCart.Load_Transaction_Date(values[0]);
                shoppingCart.Load_Payment_Method(values[1]);
                shoppingCart.Load_Credit_Card_Number(values[1]);
                shoppingCart.Load_Auth_Code(values[2]);
                shoppingCart.Load_Transaction_ID(values[3]);
                return shoppingCart;
            }
            catch (Exception ex)
            {
                return shoppingCart;
            }
        }

        protected List<ShoppingCart> ShoppingCartReadFile(HttpPostedFileBase fileShoppingCart)
        {
            List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
            ShoppingCart shoppingCart = new ShoppingCart();
            string fileName = Path.GetFileName(fileShoppingCart.FileName);
            string folder = Server.MapPath("~/UploadedFiles");
            string path = Path.Combine(folder, fileName);

            if (System.IO.File.Exists(path))
            {
                int index = 0;
                string line;
                using (System.IO.StreamReader file = new System.IO.StreamReader(path))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (index == 0)
                        {
                            if (line.ToLower().Trim() != "Transaction_Date;Payment_Method;Auth_Code;Transaction_ID".ToLower())
                            {
                                throw new Exception(string.Format("La cabecera del archivo '{0}' es incorrecta.!!", fileName));
                            }
                        }

                        if (index > 0)
                        {
                            shoppingCart = ShoppingCartReadLine(line, index);
                            shoppingCart.Validate();
                            ShoppingCartList.Add(shoppingCart);
                        }
                        index++;
                    }
                }                                   
            }
            return ShoppingCartList;
        }

        protected Result Validate(List<ShoppingCart> ShoppingCartList , List<AuthorizeNet> AuthorizeNetList)
        {
            Result result = new Result();
            List<AuthorizeNet> ResultAuthorizeNet = new List<AuthorizeNet>();
            int ShoppingCartCount = ShoppingCartList.Count;
            ShoppingCart ShoppingCartItem = null;
            AuthorizeNet AuthorizeNetItem = null;

            for (int i = 0; i<= ShoppingCartCount - 1; i++)
            {
                ShoppingCartItem = ShoppingCartList[i];
                AuthorizeNetItem = AuthorizeNetList.Where(item => 
                        item.Key == ShoppingCartItem.Key && item.IndexShoppingCart == 0).FirstOrDefault();

                if(AuthorizeNetItem != null)
                {
                    AuthorizeNetItem.IndexShoppingCart = ShoppingCartItem.Index;
                    ShoppingCartItem.IndexAuthorizeNet = AuthorizeNetItem.Index;
                }
            }

            List<ShoppingCart> ShoppingCartListInvalidate = new List<ShoppingCart>();
            List<AuthorizeNet> AuthorizeNetListInvalidate = new List<AuthorizeNet>();

            ShoppingCartListInvalidate = ShoppingCartList.Where(item => item.IndexAuthorizeNet == 0).ToList();
            AuthorizeNetListInvalidate = AuthorizeNetList.Where(item => item.IndexShoppingCart == 0).ToList();

            result.ShoppingCartList = ShoppingCartList;
            result.AuthorizeNetList = AuthorizeNetList;

            if (ShoppingCartListInvalidate.Count > 0)
            {
                result.Message = "!!File ShoppingCart Not equivalent " + ShoppingCartListInvalidate.Count.ToString() + " Rows.!!";
                foreach (ShoppingCart item in ShoppingCartListInvalidate)
                {
                    item.Error = "!!Row not Equivalent.!!";
                    item.Status = false;
                    result.Message = result.Message + ";Row Incorrect: { " + item.Index.ToString() + " }";
                }
                    
                result.Status = false;
                result.ShowList = 1;
                return result;
            }

            if (AuthorizeNetListInvalidate.Count > 0)
            {
                result.Message = "!!File AuthorizeNet Not equivalent " + AuthorizeNetListInvalidate.Count.ToString() + " Rows.!!";
                foreach (AuthorizeNet item in AuthorizeNetListInvalidate)
                {
                    item.Error = "!!Row not Equivalent.!!";
                    item.Status = false;
                    result.Message = result.Message + ";Row Incorrect: { " + item.Index.ToString() + " }";
                }                    
                result.Status = false;
                result.ShowList = 2;
                return result;
            }

            result.Message = "!!Files ShoppingCart and AuthorizeNet equivalents!!.Total " + AuthorizeNetList.Count.ToString() + " Rows";
            result.Status = true;
            result.ShowList = 2;
            return result;
        }

        protected void LimpiarCarpetaServidor()
        {
            string folder = Server.MapPath("~/UploadedFiles");
            foreach(string fichero in Directory.GetFiles(folder))
            {
                System.IO.File.Delete(fichero);
            }
            //for each fichero as string in Directory.Getfiles("tuCarpeta", "*.txt")
            //File.Delete(fichero)
            //next
        }
    }
}