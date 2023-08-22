using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StriveSteady.Models;

namespace StriveSteady.Data
{
    public class StriveSteadyContext : DbContext
    {
        public StriveSteadyContext (DbContextOptions<StriveSteadyContext> options)
            : base(options)
        {
        }

        public DbSet<StriveSteady.Models.User> User { get; set; } = default!;
        public DbSet<StriveSteady.Models.Goal> Goal { get; set; } = default!;
    }
}
