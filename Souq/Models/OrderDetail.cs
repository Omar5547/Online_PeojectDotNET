using System;
using System.Collections.Generic;

namespace Souq.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public decimal? Price { get; set; }
        public int? Qty { get; set; }
        public decimal? Totalprice { get; set; }
        public int? Orderid { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
