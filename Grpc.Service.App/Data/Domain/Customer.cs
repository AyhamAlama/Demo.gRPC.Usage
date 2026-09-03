namespace GrpcService2.Data.Domain
{
    public sealed class Customer
    {
        public Customer(string? name, int age)
        {
            Name = name;
            Age = age;
        }

        public int Id { get; private set; }
        public string? Name { get; private set; }
        public int Age { get; private set; }

        public static Customer Create(string? name, int age)
        {
            return new Customer(name, age);
        }

    }
}
