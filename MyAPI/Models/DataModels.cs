using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAPI.Models
{
    public class PinDataClass
    {
        public string Name { get; set; }
        public string[] Function { get; set; }
    }
    public class UserClass
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}