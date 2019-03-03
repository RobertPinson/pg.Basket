using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace pg.Basket.Dal
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BasketDbContext>
    {
        public BasketDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BasketDbContext>();
            optionsBuilder.UseSqlServer(@"Server=.;Database=Basket;User Id=sa;Password=TheBlackDuck72");

            return new BasketDbContext(optionsBuilder.Options);
        }
    }
}
