using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Deviax.QueryBuilder;
using Npgsql;

namespace Examples.Postgres;

public class Program
{
    public static void Main(string[] args)
    {
        // Setup for PostgreSQL
        QueryExecutor.DefaultExecutor = new PostgresExecutor();

        // Register Table-Entities to Tables. Only required for insert / update support
        Registry.RegisterTypeToTable<Customer, Customer.CustomerTable>();
        Registry.RegisterTypeToTable<Order, Order.OrderTable>();
        
        using var con = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=querybuilder;User Id=querybuilder;Password=querybuilder;");
        con.Open();

        SetupTables(con);
        InsertTestData(con);
        
        // All query execution methods come in async and synchronous variants 
        FetchByIdAsyncExample(con).GetAwaiter().GetResult();
        FetchByIdSynchronousExample(con);
        
        FetchSimpleListAsyncExample(con).GetAwaiter().GetResult();
        CustomProjectionAsyncExample(con).GetAwaiter().GetResult();

        JoinAsyncExample(con).GetAwaiter().GetResult();
        //PostgresSpecificAggregateExample(con).GetAwaiter().GetResult();

        //FetchSimpleListExample(con);

        Console.ReadKey();
    }

    private static void SetupTables(NpgsqlConnection con)
    {
        // This library does not provide DDL support.

        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS customers ( 
            id serial not null primary key,
            name text not null,
            number text not null, 
            address_line1 text not null 
        );
        
        CREATE TABLE IF NOT EXISTS orders ( 
            id serial not null primary key,
            customer_id int not null references customers(id),
            order_date timestamptz not null
        );
        CREATE INDEX IF NOT EXISTS ord_cid_idx ON orders(customer_id);
        CREATE INDEX IF NOT EXISTS ord_odate_idx ON orders(order_date);
        ";

        cmd.ExecuteNonQuery();
    }

    private static void InsertTestData(NpgsqlConnection con)
    {
        var customers = new List<Customer>()
        {
            new Customer
            {
                Name = "Customer 1",
                Number = "0001",
                AddressLine1 = "Some Address"
            },
            new Customer
            {
                Name = "Customer 2",
                Number = "0002",
                AddressLine1 = "Some other Address"
            },
            new Customer
            {
                Name = "Customer 3",
                Number = "0003",
                AddressLine1 = "Yet another Address "
            }
        };

        // All query execution methods take an optional transaction
        using var tx = con.BeginTransaction();
        
        Q.InsertSync(con, tx, customers.ToArray());
        
        // Auto generated ids are read back on insert, the Id field/property is now filled with the primary key.

        var rng = new Random();

        var orders = customers.SelectMany(c => Enumerable.Range(0, rng.Next(5, 20))
                .Select(_ => new Order
                {
                    CustomerId = c.Id,
                    OrderDate = DateTime.UtcNow.AddDays(-rng.Next(100))
                }))
            .OrderBy(a => a.OrderDate);

        Q.InsertSync(con, tx, orders.ToArray());
        
        tx.Commit();
    }


    private static void FetchByIdSynchronousExample(NpgsqlConnection con)
    {
        var customer = Q.From(Customer.Table)
            .Where(ct => ct.Id.EqV(1))
            .FirstOrDefaultSync<Customer>(con);
        
        // Expected handwritten SQL:
        // SELECT * FROM customers WHERE id = @id // @id=1
        
        // Generated SQL:
        // SELECT *
        // FROM "customers" AS cst
        // WHERE (cst."id" = @id ) 
    }
    
    private static async Task FetchByIdAsyncExample(NpgsqlConnection con)
    {
        var customer = await Q.From(Customer.Table)
            .Where(ct => ct.Id.EqV(1))
            .FirstOrDefault<Customer>(con);
        
        // Handwritten SQL:
        // SELECT *
        // FROM customers
        // WHERE id = @id
        
        // Generated SQL:
        // SELECT *
        // FROM "customers" AS cst
        // WHERE (cst."id" = @id )
    }

    private static async Task FetchSimpleListAsyncExample(NpgsqlConnection con)
    {
        // Expected handwritten SQL:
        // SELECT *
        // FROM customers
        // ORDER BY name ASC
        
        var list = await Q.From(Customer.Table)
            .OrderBy(a => a.Name.Asc())
            .ToList<Customer>(con);

        // Generated SQL:
        // SELECT *
        // FROM "customers" AS cst
        // ORDER BY cst."name" ASC 
    }
    
    public class IdNameProjection
    {
        public int Id;
        public string CustomerName;
    }
    
    private static async Task CustomProjectionAsyncExample(NpgsqlConnection con)
    {
        // Expected handwritten SQL:
        // SELECT id, name
        // FROM customers
        // WHERE id = @id
        
        var idNamePair = await Q.From(Customer.Table)
            .Where(c => c.Id.EqV(1))
            .Select(
                c => c.Id, 
                c => c.Name.As(N.Db(nameof(IdNameProjection.CustomerName))) 
            )
            .FirstOrDefault<IdNameProjection>(con);
        
        // Generated SQL:
        // SELECT cst."id" , cst."name"  AS customer_name
        // FROM "customers" AS cst
        // WHERE (cst."id" = @id ) 
    }
    
    
    public class JoinedProjection
    {
        public int OrderId;
        public int CustomerId;
        public string CustomerName;
        public string Number;
        public DateTime OrderDate;
    }

    private static async Task JoinAsyncExample(NpgsqlConnection con)
    {
        // Expected handwritten SQL:
        // SELECT       o.id AS order_id, c.id AS customer_id, o.order_date, c.name AS customer_name, c.number
        // FROM         orders AS o
        // INNER JOIN   customers AS c ON o.customer_id = c.id
        // WHERE        c.id = @id 
        // ORDER BY     order_date DESC
        // LIMIT        10
        
        var tenMostRecentOrdersWithCustomerName = await Q.From(Order.Table)
            .InnerJoin(Customer.Table).On((order, customer) => order.CustomerId.Eq(customer.Id))
            .Where((o, c) => c.Id.EqV(1))
            .Select(
               (order, customer) => order.Id.As(N.Db(nameof(JoinedProjection.OrderId))), 
               (order, customer) => customer.Id.As(N.Db(nameof(JoinedProjection.CustomerId))), 
               (order, customer) => order.OrderDate, 
               (order, customer) => customer.Name.As(N.Db(nameof(JoinedProjection.CustomerName))), 
               (order, customer) => customer.Number
            )
            .OrderBy((o,c) => o.OrderDate.Desc())
            .Limit(10)
            .ToList<JoinedProjection>(con);
        
        // Generated SQL:
        // SELECT ord."id"  AS order_id, cst."id"  AS customer_id, ord."order_date" , cst."name"  AS customer_name, cst."number" 
        // FROM "orders" AS ord
        // INNER JOIN "customers" AS cst ON (ord."customer_id" = cst."id" ) 
        // WHERE (cst."id" = @id ) 
        // ORDER BY ord."order_date" DESC 
        // LIMIT 10
    }

    private static async Task ConditionalAndPaginationExample(NpgsqlConnection con)
    {
        var baseQuery = Q.From(Order.Table)
            .InnerJoin(Customer.Table).On((order, customer) => order.CustomerId.Eq(customer.Id));

        if (true)
        {
            baseQuery = baseQuery.Where((o, c) => c.Name.ILike("something%"));
        }

        // Expected handwritten SQL:
        // SELECT       COUNT(c.id)
        // FROM         orders AS o
        // INNER JOIN   customers AS c ON o.customer_id = c.id
        // WHERE        c.name ILIKE @name
        
        var count = await baseQuery
            .Select((o, c) => Q.Count(o.Id))
            .ScalarResult<long>(con);
        
        // Generated SQL:
        // SELECT COUNT(ord."id") 
        // FROM "orders" AS ord
        // INNER JOIN "customers" AS cst ON (ord."customer_id" = cst."id" ) 
        // WHERE (cst."name" ILIKE @name ) 
        
        
        // Expected handwritten SQL:
        // SELECT       o.id AS order_id, c.id AS customer_id, o.order_date, c.name AS customer_name, c.number
        // FROM         orders AS o
        // INNER JOIN   customers AS c ON o.customer_id = c.id
        // WHERE        c.name ILIKE @name 
        // ORDER BY     order_date DESC
        // LIMIT        10
        
        var items = await baseQuery
            .Select(
                (order, customer) => order.Id.As(N.Db(nameof(JoinedProjection.OrderId))), 
                (order, customer) => customer.Id.As(N.Db(nameof(JoinedProjection.CustomerId))), 
                (order, customer) => order.OrderDate, 
                (order, customer) => customer.Name.As(N.Db(nameof(JoinedProjection.CustomerName))), 
                (order, customer) => customer.Number
            )
            .OrderBy((o,c) => o.OrderDate.Desc())
            .Limit(10)
            .ToList<JoinedProjection>(con);
        
        // Generated SQL:
        // SELECT ord."id"  AS order_id, cst."id"  AS customer_id, ord."order_date" , cst."name"  AS customer_name, cst."number" 
        // FROM "orders" AS ord
        // INNER JOIN "customers" AS cst ON (ord."customer_id" = cst."id" ) 
        // WHERE (cst."name" ILIKE @name ) 
        // ORDER BY ord."order_date" DESC 
        // LIMIT 10
    } 
}