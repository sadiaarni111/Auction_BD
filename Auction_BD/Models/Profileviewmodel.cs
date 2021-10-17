using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auction_BD.Models
{
    public class Profileviewmodel
    {
        public int u_id { get; set; }
        public string u_name { get; set; }
        public string u_email { get; set; }
        public string u_phone { get; set; }

        public string u_image { get; set; }

        public List<product> pro_list { get; set; }

        public List<product> won_list { get; set; }
        public List<product> pen_list { get; set; }

        public List<product> Sold_list { get; set; }
    }
}