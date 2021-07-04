using BancoDigital.Entidades;
using BancoDigital.Entidades.Contexto;
using BancoDigital.Repositorio;
using BancoDigital.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace BancoDigital.Testes.ContaCorrente.ComBancoDeDados
{
    [TestClass]
    public class ContaCorrenteDbTeste
    {
        private static Context _context;

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            _context = new Context(true);
            _context.Database.EnsureCreated();
        }

        [ClassCleanup]
        public static void TestFixtureTearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task DeveCadastraUmaContaDigitalDB()
        {
            var conta = new ContaDigital("4.815-3");
            var rConta = new ContaDigitalRepository(_context);

            await rConta.AdicionarAsync(conta);
            await rConta.SalvarAsync();

            var ContasDigitaisDoBanco = await rConta.ListarAsync();
            var PrimeiraConta = ContasDigitaisDoBanco.FirstOrDefault();

            Assert.AreEqual(1, ContasDigitaisDoBanco.Count);
            Assert.AreEqual("4.815-3", PrimeiraConta.Numero);
        }

        [TestMethod]
        public async Task NaoDeveCadastrarDuasContasDigitaisComMesmoNumeroDB()
        {
            var sConta = new ContaDigitalService(_context);

            var primeiraConta = new ContaDigital("28.101-8");
            var segundaConta = new ContaDigital("28.101-8");

            var (ok, mensagem) = await sConta.CriarContaDigital(primeiraConta);

            Assert.AreEqual(true, ok);
            Assert.AreEqual("Sucesso: conta digital cadastrada com sucesso!", mensagem);

            var (ok2, mensagem2) = await sConta.CriarContaDigital(segundaConta);
            Assert.AreEqual(false, ok2);
            Assert.AreEqual("Erro: Conta Digital Inválida!", mensagem2);
        }

        [TestMethod]
        public async Task NaoPermitirDepositoValorinvalidoDB()
        {
            var sConta = new ContaDigitalService(_context);

            var conta = new ContaDigital("24.819-3");
            var (ok, mensagem) = await sConta.CriarContaDigital(conta);

            var (sucesso, saldo) = await sConta.Depositar(conta.Numero, -50.25M);

            Assert.AreEqual(false, sucesso);
            Assert.AreEqual(-1M, saldo);
        }

        [TestMethod]
        public async Task PermitirDepositoENaoPermitirSaqueMaiorDepositoRealizadoDB()
        {
            var sConta = new ContaDigitalService(_context);

            var conta = new ContaDigital("24.819-3");
            var (ok, mensagem) = await sConta.CriarContaDigital(conta);

            var (sucesso, saldo) = await sConta.Depositar(conta.Numero, 50.25M);

            var (sucessoSaque, saldoApoSaque) = await sConta.Sacar(conta.Numero, 50.26M);

            Assert.AreEqual(false, sucessoSaque);
            Assert.AreEqual(-2M, saldoApoSaque);
        }

        [TestMethod]
        public async Task NaoPermitirSacarValorInvalidoDB()
        {
            var sConta = new ContaDigitalService(_context);

            var conta = new ContaDigital("24.819-3");
            var (ok, mensagem) = await sConta.CriarContaDigital(conta);

            var (sucesso, saldo) = await sConta.Sacar(conta.Numero, -50.25M);

            Assert.AreEqual(false, sucesso);
            Assert.AreEqual(-1M, saldo);
        }

        [TestMethod]
        public async Task PermitirFazerDepositoESaqueRetornandoSaldoCorretoDB()
        {
            var sConta = new ContaDigitalService(_context);

            var conta = new ContaDigital("25.819-3");
            var (ok, mensagem) = await sConta.CriarContaDigital(conta);

            var (sucesso, saldo) = await sConta.Depositar(conta.Numero, 100M);
            var (sucesso2, saldo2) = await sConta.Sacar(conta.Numero, 80M);

            Assert.AreEqual(true, sucesso2);
            Assert.AreEqual(20M, saldo2);
        }
    }
}
