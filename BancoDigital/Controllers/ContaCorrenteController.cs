using BancoDigital.Entidades;
using BancoDigital.Entidades.Contexto;
using BancoDigital.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BancoDigital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly Context _ctx;
        private readonly ContaDigitalService _sContaDigital;

        public ContaCorrenteController()
        {
            _ctx = new Context();
            _sContaDigital = new ContaDigitalService(_ctx);
        }

        [HttpPost("{conta}")]
        [Route("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] ContaDigital conta)
        {
            if (!ModelState.IsValid)
                return Problem("Erro: Conta Digital inválida!");

            var (Sucesso, Mensagem) = await _sContaDigital.CriarContaDigital(conta);

            if (Sucesso)
                return Ok(Mensagem);
            else
                return Problem(Mensagem);
        }

        [Route("depositar")]
        [HttpPost]
        public async Task<IActionResult> Depositar([FromForm] string numeroConta, [FromForm] decimal valor)
        {
            var (Sucesso, Saldo) = await _sContaDigital.Depositar(numeroConta, valor);

            if (Sucesso)
                return Ok(Saldo);
            else
            {
                if (Saldo == -1M)
                    return Problem("Erro: valor deve ser válido!");
                else
                    return NotFound("Erro: conta não encontrada!");
            }
        }

        [Route("sacar")]
        [HttpPost]
        public async Task<IActionResult> Sacar([FromForm] string numeroConta, [FromForm] decimal valor)
        {
            var (Sucesso, Saldo) = await _sContaDigital.Sacar(numeroConta, valor);

            if (Sucesso)
                return Ok(Saldo);
            else
            {
                if (Saldo == -1M)
                    return Problem("Erro: valor deve ser válido!");
                if (Saldo == -2M)
                    return Problem("Erro: saldo insuficiente!");
                else
                    return NotFound("Erro: conta não encontrada!");
            }
        }

        [Route("saldo")]
        [HttpGet("{numeroConta}")]
        public IActionResult Saldo([FromQuery] string numeroConta)
        {
            var contaExistente = _sContaDigital.GetConta(numeroConta);
            
            if(contaExistente != null)
            {
                var saldo = _sContaDigital.SaldoContaDigital(numeroConta);
                return Ok(saldo);
            }

            return NotFound("Erro: conta não encontrada!");
        }
    }
}
