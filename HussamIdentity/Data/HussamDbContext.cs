using HussamIdentity.Models.catagorys;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HussamIdentity.Data
{
    public class HussamDbContext : IdentityDbContext
    {
        public HussamDbContext(DbContextOptions<HussamDbContext> options) :base(options)
        {
            
        }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Product> Products { get; set; }


        
    }
}
