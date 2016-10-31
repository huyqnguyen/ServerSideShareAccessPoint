using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSideShareAccessPoint.Models
{
    public class RegisterModel
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}