using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class ReceiptDetail
    {
        public string Id { get; set; }
        public string ReceiptId { get; set; }
        public string ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }

        public virtual Product Product { get; set; }
        public virtual Receipt Receipt { get; set; }
    }
}
