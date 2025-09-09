using System;
using System.Collections.Generic;

namespace apiReceitasC_.Models;

public partial class Ingrediente
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<ReceitaIngrediente> ReceitaIngredientes { get; set; } = new List<ReceitaIngrediente>();
}
