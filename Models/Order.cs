using System;
using System.Collections.Generic;

namespace Souq.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? UserId { get; set; }
        public string? Mobile { get; set; }
        public bool? IsOnlinePaid { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
