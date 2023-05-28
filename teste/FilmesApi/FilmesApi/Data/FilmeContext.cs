using FilmesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmesApi.Data
{
    public class FilmeContext : DbContext
    {
        public FilmeContext(DbContextOptions<FilmeContext> opts) : base(opts) 
        {

        }

        //A entidade sessao tera como chave o id do cinema e o id do filme
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Sessao>().HasKey(sessao => new { sessao.FilmeId, sessao.CinemaId });
            builder.Entity<Sessao>().HasOne(sessao => sessao.Cinema).WithMany(cinema => cinema.Sessoes).HasForeignKey(sessao => sessao.CinemaId);
            builder.Entity<Sessao>().HasOne(sessao => sessao.Filme).WithMany(filme => filme.Sessoes).HasForeignKey(sessao => sessao.FilmeId);

            //Definindo tipo de deleção, evitando deleção em cascata.
            builder.Entity<Endereco>().HasOne(endereco => endereco.Cinema).WithOne(cinema => cinema.Endereco).OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Endereco> Enderecos { get; set;}
        public DbSet<Sessao> Sessoes { get; set; }
    }
}
