//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Auction_BD.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        public user()
        {
            this.products = new HashSet<product>();
            this.products1 = new HashSet<product>();
        }
    
        public int u_id { get; set; }
        public string u_name { get; set; }
        public string u_email { get; set; }
        public string u_phone { get; set; }
        public string u_password { get; set; }
        public string u_image { get; set; }
    
        public virtual ICollection<product> products { get; set; }
        public virtual ICollection<product> products1 { get; set; }
    }
}
