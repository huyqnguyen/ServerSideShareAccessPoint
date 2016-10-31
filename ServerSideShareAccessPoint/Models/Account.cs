using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSideShareAccessPoint.Models
{
    public class Account : IdentityUser
    {
        public virtual AccountInfo AccountInfo { get; set; }
        public virtual AccountBalance AccountBalance { get; set; }
    }
}