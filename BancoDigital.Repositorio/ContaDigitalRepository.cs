using BancoDigital.Entidades;
using BancoDigital.Entidades.Contexto;
using Carteira.Repository;

namespace BancoDigital.Repositorio
{
    public class ContaDigitalRepository : Repository<ContaDigital>
    {
        public ContaDigitalRepository(Context context) : base(context) { }
    }
}
