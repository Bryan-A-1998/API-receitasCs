using System;
using System.Collections.Generic;

namespace apiReceitasC_.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public virtual ICollection<Receita> Receita { get; set; } = new List<Receita>();
}
