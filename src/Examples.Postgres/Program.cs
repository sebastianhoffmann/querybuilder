using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Deviax.QueryBuilder;

namespace Examples.Postgres
{
    public class Program
    {
        public static void Main(string[] args)
        {
            QueryExecutor.DefaultExecutor = new PostgresExecutor();

            Registry.RegisterTypeToTable<Customer, Customer.CustomerTable>();
            Registry.RegisterTypeToTable<Order, Order.OrderTable>();

            Test().Wait();

            Console.ReadKey();
        }


        public class CustomerAggregate
        {
            public int Id;
            public string Name;
            public int OrderCount;
        }

        public static async Task Test()
        {
            using (var con = await OpenConnection())
            {
                var customer = await Q.From(new Customer.CustomerTable("c"))
                    .Where(ct => ct.Id.EqV(1))
                    .Limit(1)
                    .FirstOrDefault<Customer>(con);
                

                var agg = await Q.From(new Customer.CustomerTable("c"))
                    .InnerJoin(new Order.OrderTable("o")).On((ct, ot) => ct.Id.Eq(ot.CustomerId))
                    .GroupBy((ct, _) => ct.Id, (ct, _) => ct.Name)
                    .Select((ct, _) => ct.Id, (ct, _) => ct.Name, (_, ot) => Q.Count(ot.Id).As(N.Db(nameof(CustomerAggregate.OrderCount))))
                    .ToList<CustomerAggregate>(con);

                var c = new Customer.CustomerTable("c");
                var o = new Order.OrderTable("o");

                var agg2 = Q.From(c).InnerJoin(o).On((ct, ot) => ct.Id.Eq(ot.CustomerId))
                    .GroupBy(c.Id, c.Name)
                    .Select(c.Id, c.Name, Q.Count(o.Id).As(N.Db(nameof(CustomerAggregate.OrderCount))))
                    .ToList<CustomerAggregate>(con);

                var q = Q.From(c);

                if (true)
                {
                    q = q.Where(ct => ct.Name.Like("asdf%"));
                }

                q = q.Where(ct => ct.Id.GtV(5));

                await q.ToList<Customer>(con);

            }
        }

        public static Task<DbConnection> OpenConnection()
        {
            return null;
        }
    }
}
