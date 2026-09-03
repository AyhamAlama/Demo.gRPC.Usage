using GrpcService2.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace GrpcService2.Data;

public class CustomerDbContext(DbContextOptions<CustomerDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
}