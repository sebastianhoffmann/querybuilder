using System;
using Deviax.QueryBuilder;

namespace Examples.Postgres
{
    public class Order
    {
        public int Id;
        public DateTime OrderDate;
        public int CustomerId;

        [PrimaryKey(nameof(Id))]
        public class OrderTable : Table<OrderTable>
        {
            public OrderTable(string tableAlias = null) : base("", "orders", tableAlias)
            {
                
            }

            public Field Id;
            public Field OrderDate;
            public Field CustomerId;
        }
    }
}