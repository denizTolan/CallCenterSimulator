using System;
using System.Collections.Generic;
using System.Text;
using CallCenterSimulator.Agent.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CallCenterSimulator.Agent.Data.Context
{
    public class CallCenterSimulatorDbContext : DbContext
    {
        public CallCenterSimulatorDbContext()
        {
            
        }
        
        public CallCenterSimulatorDbContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.;DataBase=CallCenterSimulatorDb;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Domain.Models.Agent>().HasOne(p => p.Team);
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Domain.Models.Agent> Agents { get; set; }

    }
}
