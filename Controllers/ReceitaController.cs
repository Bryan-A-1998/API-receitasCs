using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiReceitasC_.Data;
using apiReceitasC_.Models;
using apiReceitasC_.Enums;

namespace apiReceitasC_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceitaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReceitaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/receita
        [HttpPost]
        public async Task<ActionResult<Receita>> PostReceita([FromBody] Dictionary<string, object> receitaRequest)
        {
            // Verifica se os campos obrigatórios estão presentes
            if (!receitaRequest.ContainsKey("usuarioId") || !receitaRequest.ContainsKey("titulo") || !receitaRequest.ContainsKey("descricao"))
            {
                return BadRequest("Usuário, título e descrição são obrigatórios.");
            }

            var usuarioId = Convert.ToInt32(receitaRequest["usuarioId"]);
            var titulo = receitaRequest["titulo"].ToString();
            var descricao = receitaRequest["descricao"].ToString();

            if (string.IsNullOrEmpty(titulo) || string.IsNullOrEmpty(descricao))
            {
                return BadRequest("Título e descrição não podem estar vazios.");
            }

            // Cadastrar a receita
            var receita = new Receita
            {
                UsuarioId = usuarioId,
                Titulo = titulo,
                Descricao = descricao
            };

            _context.Receitas.Add(receita);
            await _context.SaveChangesAsync();

            // Agora, cadastra os ingredientes relacionados
            if (receitaRequest.ContainsKey("ingredientes"))
            {
                var ingredientes = (List<Dictionary<string, object>>)receitaRequest["ingredientes"];

                foreach (var ingrediente in ingredientes)
                {
                    var ingredienteId = Convert.ToInt32(ingrediente["ingredienteId"]);
                    var quantidade = Convert.ToDecimal(ingrediente["quantidade"]);
                    var unidadeString = ingrediente["unidade"]?.ToString();
                    if (Enum.TryParse(typeof(UnidadeMedida), unidadeString, out var unidade))
                    {
                        // Se a conversão for bem-sucedida, você tem a variável `unidade` do tipo `UnidadeMedida`
                    }
                    else
                    {
                        // Se não conseguir converter, você pode tratar o erro aqui
                        return BadRequest("Unidade inválida");
                    }
 

                    var unidade = (UnidadeMedida)Enum.Parse(typeof(UnidadeMedida), unidadeString);

                    // Cadastra na tabela intermediária
                    var receitaIngrediente = new ReceitaIngrediente
                    {
                        ReceitaId = receita.Id,
                        IngredienteId = ingredienteId,
                        Quantidade = quantidade,
                        Unidade = unidade
                    };

                    _context.ReceitaIngredientes.Add(receitaIngrediente);
                }

                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetReceita), new { id = receita.Id }, receita);
        }


        // GET: api/receita
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receita>>> GetReceitas()
        {
            var receitas = await _context.Receitas.ToListAsync();
            return Ok(receitas);
        }

        // GET: api/receita/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Receita>> GetReceita(int id)
        {
            var receita = await _context.Receitas.FindAsync(id);

            if (receita == null)
            {
                return NotFound();
            }

            return Ok(receita);
        }

        // PUT: api/receita/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReceita(int id, [FromBody] Receita request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var receita = await _context.Receitas.FindAsync(id);

            if (receita == null)
            {
                return NotFound();
            }

            receita.Titulo = request.Titulo;
            receita.Descricao = request.Descricao;

            _context.Entry(receita).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/receita/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceita(int id)
        {
            var receita = await _context.Receitas.FindAsync(id);

            if (receita == null)
            {
                return NotFound();
            }

            _context.Receitas.Remove(receita);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Buscar receitas compatíveis por ingrediente
        [HttpPost("buscar-com-ingredientes")]
        public async Task<ActionResult<IEnumerable<Receita>>> BuscarReceitasComIngredientes([FromBody] List<int> ingredientesIds)
        {
            if (ingredientesIds == null || ingredientesIds.Count == 0)
            {
                return BadRequest("A lista de ingredientes é obrigatória.");
            }
            var ingredienteId = ingrediente["ingredienteId"] != null ? Convert.ToInt32(ingrediente["ingredienteId"].ToString()) : 0;
            var receitas = await _context.Receitas
                .Where(r => r.ReceitaIngredientes.Any(ri => ingredientesIds.Contains(ri.IngredienteId.GetValueOrDefault())))
                .ToListAsync();

            if (!receitas.Any())
            {
                return NotFound("Nenhuma receita compatível encontrada.");
            }

            return Ok(receitas);
        }
    }

}
