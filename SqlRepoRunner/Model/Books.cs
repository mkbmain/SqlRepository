using System;
using System.Collections.Generic;

namespace SqlRepoRunner.Model
{
    public partial class Books  : MainSqlDb
    {
        public Books()
        {
            BookOrders = new HashSet<BookOrders>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Authour { get; set; }
        public string Blurb { get; set; }

        public virtual ICollection<BookOrders> BookOrders { get; set; }
    }
}
