using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ascend.Data
{
    public partial class LoginDbContext : DbContext //Die Klasse LoginDbContext erbt von der Klasse DbContext. DbContext ist die Hauptklasse im Entity Framework Core, die den Zugriff auf die Datenbank ermöglicht!
    {
        public DbSet<User> Users { get; set; } //Die Eigenschaft Users vom Typ DbSet<User> definiert die Entität User als eine Menge von Benutzern in der Datenbank. Jedes DbSet<> repräsentiert eine Tabelle in der Datenbank!
        public DbSet<Todo> Todos { get; set; }
        public DbSet<UserProgress> UserProgress { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //Die Konfigurationen für die Verbindung zur Datenbank!
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-ESQ50KH\XSQL;Initial Catalog=AscendLoginDB;Integrated Security=True; Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .HasOne(t => t.User) //Beziehung zur User-Tabelle!
                .WithMany()
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Cascade); //Löscht To-dos mit dem User!

            base.OnModelCreating(modelBuilder);
        }
    }
}