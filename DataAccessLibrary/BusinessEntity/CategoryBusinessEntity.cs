using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.BusinessEntity
{
    public class CategoryBusinessEntity
    {
        private IUnitOfWork work;
        public CategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
