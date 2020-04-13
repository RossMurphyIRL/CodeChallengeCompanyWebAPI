using Core;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure
{
    public class CompanyContext : DbContext
    {
        public CompanyContext()
        {
        }
        public CompanyContext(DbContextOptions<CompanyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CompanyDto> Companies { get; set; }
        public virtual DbSet<UserDto> Users { get; set; }
    }
}
