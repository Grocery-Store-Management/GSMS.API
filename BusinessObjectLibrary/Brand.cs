using GsmsLibrary;
using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Brand
    {
        public Brand()
        {
            Stores = new HashSet<Store>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        private DateTime? createdDate;
        public DateTime? CreatedDate
        {
            get
            {
                return GsmsUtils.ConvertToUTC7(createdDate.Value);
            }
            set
            {
                createdDate = value;
            }
        }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Store> Stores { get; set; }
    }
}
