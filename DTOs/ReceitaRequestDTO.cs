namespace apiReceitasC_.DTOs
{
    // Objeto de requisição para cadastrar receita
    public class ReceitaRequestDTO
    {
        public int UsuarioId { get; set; } 
        public string Titulo { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public List<IngredienteDTO>? Ingredientes { get; set; }
    }

    // Objeto para ingredientes da receita
    public class IngredienteDTO
    {
        public int Id { get; set; }
        public decimal Quantidade { get; set; }
        public string Unidade { get; set; } = null!;
    }
}
