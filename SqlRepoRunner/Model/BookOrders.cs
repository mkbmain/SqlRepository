using System;
using System.Collections.Generic;

namespace SqlRepoRunner.Model
{
    public partial class BookOrders  : MainSqlDb
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int OrderId { get; set; }
        public decimal PriceAtCapture { get; set; }

        public virtual Books Book { get; set; }
        public virtual Orders Order { get; set; }
    }
}
