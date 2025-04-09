using Microsoft.EntityFrameworkCore;
using PlataformaFbj.Models;

namespace PlataformaFbj.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Exemplo: configurar relacionamento entre Jogo e Usuario
            modelBuilder.Entity<Jogo>()
                .HasOne(j => j.Desenvolvedor)
                .WithMany(u => u.Jogos)
                .HasForeignKey(j => j.DesenvolvedorId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UsuarioId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Jogo)
                .WithMany(j => j.Feedbacks)
                .HasForeignKey(f => f.JogoId);
        }


    }
}
