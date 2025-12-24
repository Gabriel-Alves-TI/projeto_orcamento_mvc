using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.DTO
{
    public class ReciboDTO
    {
        public int? Id { get; set; }
        public int Numeracao { get; set; }
        public DateTime Data { get; set; }
        public string Cliente { get; set; }
        public int? ClienteId { get; set; }
        //Dados Cliente
        public string? CpfCnpj { get; set; }
        public string? Cep { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Telefone { get; set; }
        //Dados Cliente
        public string? Descricao { get; set; }
        public decimal ValorRecibo { get; set; }
        public DateTime? Vencimento { get; set; }
        public int? NumNotaFiscal { get; set; }
    }
}