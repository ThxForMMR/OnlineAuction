using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineAuction.Models
{
    public class AuctionContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }

        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
