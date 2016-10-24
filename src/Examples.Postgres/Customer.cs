using Deviax.QueryBuilder;

namespace Examples.Postgres
{
    public class Customer
    {
        public int Id;
        public string Name;

        [PrimaryKey(nameof(Id))]
        public class CustomerTable : Table<CustomerTable>
        {
            public CustomerTable(string tableAlias = null) : base("", "customers", tableAlias)
            {
                Id = F("id");
                Name = F("name");
            }

            public readonly Field Id;
            public readonly Field Name;
        }
    }
}