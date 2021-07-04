using BancoDigital.Entidades;
using BancoDigital.Entidades.Contexto;
using Carteira.Repository;

namespace BancoDigital.Repositorio
{
    public class MovimentoRepository : Repository<Movimento>
    {
        public MovimentoRepository(Context context) : base(context) { }
    }
}
