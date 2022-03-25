using Deviax.QueryBuilder;

namespace Examples.Postgres
{
    public class Customer
    {
        public static CustomerTable Table = new ("cst");
        
        public int Id;
        public string Name;
        public string Number;
        public string AddressLine1;

        [PrimaryKey(nameof(Id))]
        public class CustomerTable : Table<CustomerTable>
        {
            public CustomerTable(string tableAlias = null) : base("", "customers", tableAlias)
            {
                Id = F("id");
                Name = F("name");
                Number = F("number");
                AddressLine1 = F("address_line1");
            }

            public readonly Field Id;
            public readonly Field Name;
            public readonly Field Number;
            public readonly Field AddressLine1;
        }
    }
}