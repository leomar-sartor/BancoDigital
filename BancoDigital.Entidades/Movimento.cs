using BancoDigital.Entidades.Enumeradores;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDigital.Entidades
{
    public class Movimento : Master 
    {
        public Movimento(decimal valor, TipoOperecao operacao)
        {
            Data = DateTime.Today;
            Valor = valor;
            Operacao = operacao;
        }
        public DateTime Data { get; set; }

        [ForeignKey("ContaId")]
        public long ContaId { get; set; }
        public ContaDigital Conta { get; set; }

        public TipoOperecao Operacao { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }
    }
}
