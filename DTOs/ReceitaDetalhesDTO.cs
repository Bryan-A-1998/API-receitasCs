public class IngredienteDTO
{
    public string Nome { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public string Unidade { get; set; } = string.Empty;
}

public class ReceitaDetalhadaDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty; 
    public string Descricao { get; set; } = string.Empty;
    public List<IngredienteDTO> Ingredientes { get; set; } = new List<IngredienteDTO>(); 
}

