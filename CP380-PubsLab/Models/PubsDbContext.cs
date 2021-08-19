using System;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace CP380_PubsLab.Models
{
    public class PubsDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbpath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\pubs.mdf"));
            optionsBuilder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Integrated Security=true;AttachDbFilename={dbpath}");
        }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<Titles> Titles { get; set; }
        public DbSet<Stores> Stores { get; set; }
        public DbSet<Sales> Sales { get; set; }
        // TODO: Add DbSets

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO
            modelBuilder.Entity<Sales>()
               .HasOne(NewMethod())
               .WithMany(b => b.Sales)
               .HasForeignKey(bc => bc.stor_id);
            modelBuilder.Entity<Sales>()
                .HasOne(bc => bc.Title)
                .WithMany(c => c.Sales)
                .HasForeignKey(bc => bc.title_id);
        }

        private static Expression<Func<Sales, Stores>> NewMethod()
        {
            return bc => bc.Store;
        }
    }


    public class Titles
    {
        // TODO
        [Key]
        public string title_id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public Decimal? price { get; set; }
        public Decimal? advance { get; set; }
        public int? royalty { get; set; }
        public int? ytd_sales { get; set; }
        public string? notes { get; set; }
        public DateTime pubdate { get; set; }

        public List<Sales> Sales { get; set; }
    }


    public class Stores 
    {
        // TODO
        [Key]
        public string stor_id { get; set; }
        public string stor_name { get; set; }
        public string stor_address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }

        public List<Sales> Sales { get; set; }
    }


    public class Sales
    {
        // TODO
        [Key]
        public string ord_num { get; set; }
        public DateTime ord_date { get; set; }
        public Int16 qty { get; set; }
        public string payterms { get; set; }

        public string stor_id { get; set; }
        [ForeignKey("stor_id")]
        public Stores Store { get; set; }

        public string title_id { get; set; }
        [ForeignKey("title_id")]
        public Titles Title { get; set; }
    }

    public class Employee
    {
        // TODO
        [Key]
        public string emp_id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public Int16 job_id { get; set; }
        [ForeignKey("job_id")]
        public Jobs Job { get; set; }
    }

    public class Jobs
    {
        // TODO
        [Key]
        public Int16 job_id { get; set; } 
        public string job_desc { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
