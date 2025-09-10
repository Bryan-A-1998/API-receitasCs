namespace apiReceitasC_.DTOs
{
    public class ReceitaResponseDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descricao { get; set; }
        public string Usuario { get; set; } = null!;
        public List<IngredienteResponseDTO> Ingredientes { get; set; } = new();
    }

    public class IngredienteResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public decimal Quantidade { get; set; }
        public string Unidade { get; set; } = null!;
    }
}
