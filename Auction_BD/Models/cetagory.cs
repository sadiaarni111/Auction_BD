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
    
    public partial class cetagory
    {
        public cetagory()
        {
            this.products = new HashSet<product>();
        }
    
        public int cat_id { get; set; }
        public string cat_name { get; set; }
        public string cat_iamge { get; set; }
        public Nullable<int> cat_fk_ad { get; set; }
        public Nullable<int> sts { get; set; }
    
        public virtual admin admin { get; set; }
        public virtual ICollection<product> products { get; set; }
    }
}