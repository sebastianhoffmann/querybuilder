using System;
using Deviax.QueryBuilder;

namespace Examples.Postgres
{
    public class Order
    {
        public static OrderTable Table = new ("ord"); 
        
        public int Id;
        public int CustomerId;
        public DateTime OrderDate;

        [PrimaryKey(nameof(Id))]
        public class OrderTable : Table<OrderTable>
        {
            public OrderTable(string tableAlias = null) : base("", "orders", tableAlias)
            {
                Id = F("id");
                CustomerId = F("customer_id");
                OrderDate = F("order_date");
            }

            public Field Id;
            public Field OrderDate;
            public Field CustomerId;
        }
    }
}