using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Bee_game.Models;

namespace Bee_game.DAL
{
    public class BeeContext : DbContext
    {
        public BeeContext() : base("BeeContext")
        {
        }

        public DbSet<IBee> Bee { get; set; }
    }
}