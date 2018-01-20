using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apps.Web.SCI.Models
{
    public class Result
    {        
        public string Message { get; set; }        
        public bool Status { get; set; }
        public List<ShoppingCart> ShoppingCartList { get; set; }
        public List<AuthorizeNet> AuthorizeNetList { get; set; }    
        public int ShowList { get; set; }
        public string ProcessDate { get; set;}
        public Result()
        {
            ShoppingCartList = new List<ShoppingCart>();
            AuthorizeNetList = new List<AuthorizeNet>();
        }
    }
}