using System;
using System.Collections.Generic;
using apiReceitasC_.Enums;

namespace apiReceitasC_.Models;

public partial class ReceitaIngrediente
{
    public int Id { get; set; }

    public int? ReceitaId { get; set; }

    public int? IngredienteId { get; set; }

    public decimal Quantidade { get; set; }

    public UnidadeMedida Unidade { get; set; }

    public virtual Ingrediente? Ingrediente { get; set; }

    public virtual Receita? Receita { get; set; }
}
