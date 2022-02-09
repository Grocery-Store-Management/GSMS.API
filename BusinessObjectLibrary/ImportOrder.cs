using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class ImportOrder
    {
        public ImportOrder()
        {
            ImportOrderDetails = new HashSet<ImportOrderDetail>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ProductId { get; set; }
        public string StoreId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; }
    }
}
