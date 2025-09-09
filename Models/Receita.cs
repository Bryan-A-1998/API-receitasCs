using System;
using System.Collections.Generic;

namespace apiReceitasC_.Models;

public partial class Receita
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descricao { get; set; }

    public virtual ICollection<ReceitaIngrediente> ReceitaIngredientes { get; set; } = new List<ReceitaIngrediente>();

    public virtual Usuario? Usuario { get; set; }
}
