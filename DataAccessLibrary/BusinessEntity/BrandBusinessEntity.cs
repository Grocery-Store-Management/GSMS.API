using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.BusinessEntity
{
    public class BrandBusinessEntity
    {
        private IUnitOfWork work;
        public BrandBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
