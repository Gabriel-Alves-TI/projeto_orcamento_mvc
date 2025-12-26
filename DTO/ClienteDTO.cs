using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace projeto_orcamento_mvc.DTO
{
    public class ClienteDTO
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Digite o nome do cliente!")]
        public string Nome { get; set; }
        public string? CpfCnpj { get; set; }
        [Required(ErrorMessage = "Digite o Cep!")] 
        public int Cep { get; set; }
        [Required(ErrorMessage = "Digite o Logradouro!")] 
        public string Logradouro { get; set; }
        [Required(ErrorMessage = "Digite o Numero!")] 
        public int Numero { get; set; }
        [Required(ErrorMessage = "Digite o Cidade!")] 
        public string Cidade { get; set; }
        [Required(ErrorMessage = "Digite o Estado!")] 
        public string Estado { get; set; }
        [Required(ErrorMessage = "Digite o Telefone!")] 
        public string Telefone { get; set; }
    }
}