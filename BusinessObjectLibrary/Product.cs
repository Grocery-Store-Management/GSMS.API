using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Product
    {
        public Product()
        {
            ImportOrderDetails = new HashSet<ImportOrderDetail>();
            InverseMasterProduct = new HashSet<Product>();
            ProductDetails = new HashSet<ProductDetail>();
            ReceiptDetails = new HashSet<ReceiptDetail>();
        }

        public string Id { get; set; }
        public decimal? AtomicPrice { get; set; }
        public string MasterProductId { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Category Category { get; set; }
        public virtual Product MasterProduct { get; set; }
        public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; }
        public virtual ICollection<Product> InverseMasterProduct { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
