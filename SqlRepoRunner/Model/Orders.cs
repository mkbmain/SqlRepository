using System;
using System.Collections.Generic;

namespace SqlRepoRunner.Model
{
    public partial class Orders : MainSqlDb
    {
        public Orders()
        {
            BookOrders = new HashSet<BookOrders>();
        }

        public int Id { get; set; }
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public string Customer { get; set; }

        public virtual ICollection<BookOrders> BookOrders { get; set; }
    }
}
