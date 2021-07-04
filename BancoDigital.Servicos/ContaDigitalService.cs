using BancoDigital.Entidades;
using BancoDigital.Entidades.Contexto;
using BancoDigital.Entidades.Enumeradores;
using BancoDigital.Repositorio;
using System.Linq;
using System.Threading.Tasks;

namespace BancoDigital.Servicos
{
    public class ContaDigitalService
    {
        private Context _ctx;
        private MovimentoRepository _rMovimento;
        private ContaDigitalRepository _rConta;

        public ContaDigitalService(Context ctx)
        {
            _ctx = ctx;
            _rMovimento = new MovimentoRepository(_ctx);
            _rConta = new ContaDigitalRepository(_ctx);
        }

        public async Task<(bool, string)> CriarContaDigital(ContaDigital conta)
        {
            var contaExistente = _rConta.Buscar(m => m.Numero == conta.Numero).Any();
            if (contaExistente)
                return (false, "Erro: Conta Digital Inválida!");

            await _rConta.AdicionarAsync(conta);
            await _rConta.SalvarAsync();

            return (true, "Sucesso: conta digital cadastrada com sucesso!");
        }

        public decimal SaldoContaDigital(string numeroConta)
        {
            var conta = GetConta(numeroConta);
            var movimentos = _rMovimento.Buscar(m => m.ContaId == conta.Id).ToList();

            if (!movimentos.Any())
                return 0M;

            var maiorId = movimentos.Max(m => m.Id);

            var ultimoRegistro = _rMovimento.Buscar(m => m.Id == maiorId).FirstOrDefault();

            if (ultimoRegistro != null)
                return ultimoRegistro.Saldo;

            return 0M;
        }

        public async Task<(bool, decimal)> Depositar(string numeroConta, decimal valor)
        {
            if (ValorValido(valor))
            {
                var conta = GetConta(numeroConta);

                if (conta != null)
                {
                    var deposito = new Movimento(valor, TipoOperecao.Deposito);

                    var saldoAtual = SaldoContaDigital(numeroConta);
                    deposito.Saldo = AtualizarSaldo(valor, saldoAtual, TipoOperecao.Deposito);
                    deposito.ContaId = conta.Id;
                    await _rMovimento.AdicionarAsync(deposito);
                    await _rMovimento.SalvarAsync();

                    return (true, deposito.Saldo);
                }
            }

            return (false, -1M);
        }

        public async Task<(bool, decimal)> Sacar(string numeroConta, decimal valor)
        {
            if (ValorValido(valor))
            {
                var conta = GetConta(numeroConta);

                if (conta != null)
                {
                    var saque = new Movimento(valor, TipoOperecao.Saque);

                    var saldoAtual = SaldoContaDigital(numeroConta);
                    var saldoFinal = AtualizarSaldo(valor, saldoAtual, TipoOperecao.Saque);
                    saque.ContaId = conta.Id;

                    if (saldoFinal >= 0 && saldoAtual > 0)
                    {
                        saque.Saldo = saldoFinal;

                        await _rMovimento.AdicionarAsync(saque);
                        await _rMovimento.SalvarAsync();

                        return (true, saque.Saldo);
                    }
                    else
                        return (false, -2M);
                }
            }

            return (false, -1M);
        }

        public ContaDigital GetConta(string numeroConta)
        {
            return _rConta.Buscar(m => m.Numero == numeroConta).FirstOrDefault();
        }

        private decimal AtualizarSaldo(decimal valor, decimal saldoAtual, TipoOperecao operacao)
        {
            var novoSaldo = 0M;

            if (operacao == TipoOperecao.Deposito)
                novoSaldo = saldoAtual + valor;
            else
                novoSaldo = saldoAtual - valor;

            return novoSaldo;
        }

        private bool ValorValido(decimal valor)
        {
            if (valor > 0)
                return true;

            return false;
        }
    }
}
