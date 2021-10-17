using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auction_BD.Models
{
    public class viewadmodel
    {

        public int pro_id { get; set; }
        public string pro_name { get; set; }
        public string pro_iamge { get; set; }
        public int pro_price { get; set; }
        public string pro_des { get; set; }
        public Nullable<int> pro_fk_cat { get; set; }
        public Nullable<int> pro_fk_user { get; set; }
        public Nullable<int> bid_price { get; set; }
        public Nullable<int> u_id { get; set; }

        public string u_name { get; set; }
        public string u_email { get; set; }
        public string u_phone { get; set; }

        public string u_image { get; set; }
        public int cat_id { get; set; }
        public string cat_name { get; set; }

        public string bider_name { get; set; }

        public Nullable<int> appv { get; set; }
         
        public TimeSpan time { get; set; }


    }
}