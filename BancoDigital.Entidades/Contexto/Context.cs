using Microsoft.EntityFrameworkCore;
namespace BancoDigital.Entidades.Contexto
{
    public class Context : DbContext
    {
        private string _connectionString = "server=localhost;database=seu_banco;uid=root;pwd=sua_senha;port=3306";
        private string _connectionStringHomologacao = "server=localhost;database=seu_banco_homologacao;uid=root;pwd=sua_senha;port=3306";
        
        public virtual DbSet<ContaDigital> Contas { get; set; }
        public virtual DbSet<Movimento> Movimentos { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }

        public Context() : base()
        {
            _ConfigureDataBase();
        }

        public Context(bool AmbienteDeHomologacao)
        {
            if (AmbienteDeHomologacao)
            {
                _connectionString = _connectionStringHomologacao;
                _ConfigureDataBase();
            }
        }

        private void _ConfigureDataBase()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();

            builder.UseMySQL(_connectionString);
            this.OnConfiguring(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContaDigital>()
                .HasMany(c => c.Movimentacoes)
                .WithOne(m => m.Conta)
                .IsRequired();
        }
    }
}
