using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ServerSideShareAccessPoint.Models
{
    public class AccountBalance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Balance { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public byte[] GuidVersion { get; set; }
    }
}