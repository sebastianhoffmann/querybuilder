
## SQL DSL:

A C# Query Builder DSL that is very close to SQL but does not rely on naked strings and hence avoids the refactoring and typo issues. 

The core idea is not to avoid SQL but to embrace it, there should be no surprises in the generated SQL. It should be what you see is what you get.

*Simple example*
```csharp
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
```

*More complex example*
```csharp
// Expected handwritten SQL:  
// SELECT       o.id AS order_id, c.id AS customer_id, o.order_date, c.name AS customer_name, c.number  
// FROM         orders AS o  
// INNER JOIN   customers AS c ON o.customer_id = c.id  
// WHERE        c.id = @id 
// ORDER BY     order_date DESC  
// LIMIT        10  
  
var tenMostRecentOrdersWithCustomerName = await Q.From(Order.Table)  
    .InnerJoin(Customer.Table).On(
       (order, customer) => order.CustomerId.Eq(customer.Id)
    )  
    .Where((o, c) => c.Id.EqV(1))  
    .Select(  
       (order, customer) => order.Id.As(N.Db(nameof(JoinedProjection.OrderId))),   
       (order, customer) => customer.Id.As(N.Db(nameof(JoinedProjection.CustomerId))),   
       (order, customer) => order.OrderDate,   
       (order, customer) =>  customer.Name.As(N.Db(nameof(JoinedProjection.CustomerName))),   
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
```

The queries are immutable, each builder method returns a new query. This allows for conditional where clause building as well as an easy way to execute a count query for pagination without repeating the conditions.

```csharp
var baseQuery = Q.From(Order.Table)  
    .InnerJoin(Customer.Table).On(
        (order, customer) => order.CustomerId.Eq(customer.Id)
    );  
  
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
// INNER JOIN "customers" AS cst ON (ord."customer_id" = cst."id" ) // WHERE (cst."name" ILIKE @name )   
  
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
```

The SQL DSL covers Update, Delete and Select queries. ORM-Like Inserts and change tracking based updates also exists.

## Query Projection

Project the result of a DbCommand to a C# class per row. For each selected column / value a Field or Property has to exist on the Projection class. This feature is usually not used on its own but rather indirectly via the FirstOrDefault or ToList methods on the query builder DSL.

```csharp
public class ProjectionExample { public int Id; public string Name; }
var cmd = connection.CreateCommand();
cmd.CommandText = "SELECT id, name FROM customers";
var items = QueryExecutor.DefaultExecutor.ToList<ProjectionExample>(cmd);
```

The first execution for a given projection class has some overhead as it compiles the projection for future calls. 


## Insert

```csharp
// Customer = Id (C#: int, PG: Serial aka auto increment PK), Name (c#: string, PG: text)
var customer = new Customer { Name = "Some Name" };
await Q.InsertOne(con, tx, customer); // tx / transaction is optional
Console.WriteLine(customer.Id); // Will print the PK returned from the DB
```

## Change Tracking
```csharp
var ctc = ChangeTrackingContext.StartWith(customer);
// To track more entities: ctc.Track(xy)

customer.Name = "New Name";

await ctc.Commit(con, tx);
```

---

You can find a complete example in the Examples.Postgres project.
