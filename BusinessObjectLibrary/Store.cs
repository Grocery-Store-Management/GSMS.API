using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Store
    {
        public Store()
        {
            ImportOrders = new HashSet<ImportOrder>();
            Receipts = new HashSet<Receipt>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string BrandId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? IsDeleted { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<ImportOrder> ImportOrders { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
