using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BancoDigital.Entidades
{
    public class ContaDigital : Master
    {
        public ContaDigital()
        {
            Data = DateTime.Today;
        }
        public ContaDigital(string numero)
        {
            Numero = numero;
            Data = DateTime.Today;
        }

        [Required(ErrorMessage = "É necessário um número de conta")]
        public string Numero { get; set; }

        public DateTime Data { get; set; }

        public ICollection<Movimento> Movimentacoes { get; set; }

    }
}
