﻿using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class ImportOrderDetail
    {
        public string Id { get; set; }
        public string ImportOrderId { get; set; }
        public string Name { get; set; }
        public string Distributor { get; set; }
        public string ProductId { get; set; }
        public int? Quantity { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? Price { get; set; }

        public virtual ImportOrder ImportOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
