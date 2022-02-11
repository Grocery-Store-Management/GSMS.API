using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class ProductDetail
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public decimal? Price { get; set; }
        public Status Status { get; set; }
        public int? StoredQuantity { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
