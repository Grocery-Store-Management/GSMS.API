using GsmsLibrary;
using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Receipt
    {
        public Receipt()
        {
            ReceiptDetails = new HashSet<ReceiptDetail>();
        }

        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public string StoreId { get; set; }

        private DateTime? createdDate;
        public DateTime? CreatedDate { get
            {
                return GsmsUtils.ConvertToUTC7(createdDate.Value);
            }
            set
            {
                createdDate = value;
            }
        }
        public bool? IsDeleted { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
