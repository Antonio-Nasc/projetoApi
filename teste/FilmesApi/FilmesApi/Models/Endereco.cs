using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models
{
    public class Endereco
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        //CINEMA NÃO PRECISA DO ID DE ENDEREÇO, POIS, PARA O CINEMA EXISTIR, PRECISA DA DEPENDENCIA DO ENDEREÇO
        public virtual Cinema Cinema { get; set; }
    }
}
