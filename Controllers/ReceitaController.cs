using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiReceitasC_.Data;
using apiReceitasC_.Models;
using apiReceitasC_.Enums;
using apiReceitasC_.DTOs;

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

        // GET: api/receita
        // Lista todas as receitas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceitaResponseDTO>>> GetReceitas()
        {
            var receitas = await _context.Receitas
                .Include(r => r.ReceitaIngredientes)
                    .ThenInclude(ri => ri.Ingrediente)
                .Include(r => r.Usuario)
                .ToListAsync();

            var resposta = receitas.Select(r => new ReceitaResponseDTO
            {
                Id = r.Id,
                Titulo = r.Titulo,
                Descricao = r.Descricao,
                Usuario = r.Usuario != null ? r.Usuario.Nome : "Desconhecido",
                Ingredientes = r.ReceitaIngredientes.Select(ri => new IngredienteResponseDTO
                {
                    Id = ri.IngredienteId ?? 0,
                    Nome = ri.Ingrediente?.Nome ?? "",
                    Quantidade = ri.Quantidade,
                    Unidade = ri.Unidade.ToString()
                }).ToList()
            });

            return Ok(resposta);
        }

        // GET: api/receita/{id}
        // Retorna uma receita específica
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceitaResponseDTO>> GetReceita(int id)
        {
            var receita = await _context.Receitas
                .Include(r => r.ReceitaIngredientes)
                    .ThenInclude(ri => ri.Ingrediente)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (receita == null)
            {
                return NotFound("Receita não encontrada.");
            }

            var resposta = new ReceitaResponseDTO
            {
                Id = receita.Id,
                Titulo = receita.Titulo,
                Descricao = receita.Descricao,
                Usuario = receita.Usuario?.Nome ?? "Desconhecido",
                Ingredientes = receita.ReceitaIngredientes.Select(ri => new IngredienteResponseDTO
                {
                    Id = ri.IngredienteId ?? 0,
                    Nome = ri.Ingrediente?.Nome ?? "",
                    Quantidade = ri.Quantidade,
                    Unidade = ri.Unidade.ToString()
                }).ToList()
            };

            return Ok(resposta);
        }


        // POST: api/receita
        // Cadastra uma nova receita + ingredientes
        [HttpPost]
        public async Task<ActionResult<Receita>> PostReceita([FromBody] ReceitaRequestDTO request) // trocado o dictionary pelo DTO
        {
            // validações simples
            if (request == null || string.IsNullOrWhiteSpace(request.Titulo) || string.IsNullOrWhiteSpace(request.Descricao))
            {
                return BadRequest("Usuário, título e descrição são obrigatórios.");
            }

            // cria a receita
            var receita = new Receita
            {
                UsuarioId = request.UsuarioId,
                Titulo = request.Titulo,
                Descricao = request.Descricao
            };

            _context.Receitas.Add(receita);
            await _context.SaveChangesAsync();

            // adiciona ingredientes se existirem
            if (request.Ingredientes != null && request.Ingredientes.Any())
            {
                foreach (var ing in request.Ingredientes)
                {
                    if (!Enum.TryParse<UnidadeMedida>(ing.Unidade, out var unidade))
                        return BadRequest($"Unidade inválida: {ing.Unidade}");

                    var receitaIngrediente = new ReceitaIngrediente
                    {
                        ReceitaId = receita.Id,
                        IngredienteId = ing.Id,
                        Quantidade = ing.Quantidade,
                        Unidade = unidade
                    };

                    _context.ReceitaIngredientes.Add(receitaIngrediente);
                }

                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetReceita), new { id = receita.Id }, receita);
        }

        // PUT: api/receita/{id}
        // Atualiza uma receita (somente título e descrição)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReceita(int id, [FromBody] Receita request)
        {
            if (id != request.Id)
            {
                return BadRequest("O ID informado não confere.");
            }

            var receita = await _context.Receitas.FindAsync(id);

            if (receita == null)
            {
                return NotFound("Receita não encontrada.");
            }

            receita.Titulo = request.Titulo;
            receita.Descricao = request.Descricao;

            _context.Entry(receita).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/receita/{id}
        // Remove uma receita e seus ingredientes
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceita(int id)
        {
            var receita = await _context.Receitas
                .Include(r => r.ReceitaIngredientes)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (receita == null)
            {
                return NotFound("Receita não encontrada.");
            }

            _context.ReceitaIngredientes.RemoveRange(receita.ReceitaIngredientes);
            _context.Receitas.Remove(receita);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/receita/buscar-com-ingredientes
        // Busca receitas que contenham ingredientes específicos e as classifica como completas ou parciais
        [HttpPost("buscar-com-ingredientes")]
        public async Task<ActionResult> BuscarReceitasComIngredientes([FromBody] IngredientesRequestDTO request)
        {
            // Log para verificar o que está sendo recebido
            Console.WriteLine($"Ingredientes recebidos: {string.Join(", ", request.Ingredientes)}");

            if (request?.Ingredientes == null || !request.Ingredientes.Any())
            {
                return BadRequest("A lista de ingredientes é obrigatória.");
            }

            // Buscar receitas completas ou parciais
            var resultado = await BuscarReceitasCompletasOuParciais(request.Ingredientes);

            if (!resultado.completas.Any() && !resultado.parciais.Any())
            {
                return NotFound("Nenhuma receita compatível encontrada.");
            }

            // Detalhar as receitas completas e parciais
            var completasDetalhadas = await BuscarDetalhesReceitas(resultado.completas);
            var parciaisDetalhadas = await BuscarDetalhesReceitas(resultado.parciais);

            return Ok(new
            {
                mensagem = "Receitas com ingredientes compatíveis encontradas",
                receitas_completas = completasDetalhadas,
                receitas_parciais = parciaisDetalhadas
            });
        }


        // Buscar receitas completas ou parciais
        private async Task<(List<int> completas, List<int> parciais)> BuscarReceitasCompletasOuParciais(List<int> ingredientes)
        {
            // Buscar receitas COMPLETAS (com todos os ingredientes)
            var completas = await _context.Receitas
                .Where(r => r.ReceitaIngredientes
                    .All(ri => ri.IngredienteId.HasValue && ingredientes.Contains(ri.IngredienteId.Value))) // Todos os ingredientes precisam estar presentes
                .Select(r => r.Id)
                .ToListAsync();

            // Buscar receitas PARCIAIS (com pelo menos 1 ingrediente, mas não todos)
            var parciais = await _context.Receitas
                .Where(r => r.ReceitaIngredientes
                    .Any(ri => ri.IngredienteId.HasValue && ingredientes.Contains(ri.IngredienteId.Value))) // Pelo menos 1 ingrediente presente
                .Where(r => !completas.Contains(r.Id))  // Excluir as receitas que já são completas
                .Select(r => r.Id)
                .ToListAsync();

            return (completas, parciais);
        }

        // Função para detalhar as receitas com seus ingredientes
        private async Task<List<ReceitaDetalhadaDTO>> BuscarDetalhesReceitas(List<int> receitaIds)
        {
            return await _context.Receitas
                .Where(r => receitaIds.Contains(r.Id))
                .Select(r => new ReceitaDetalhadaDTO
                {
                    Id = r.Id,
                    Titulo = r.Titulo,
                    Descricao = r.Descricao ?? "Sem descrição",
                    Ingredientes = r.ReceitaIngredientes.Select(ri => new IngredienteDTO
                    {
                        Nome = ri.Ingrediente != null ? ri.Ingrediente.Nome : "Desconhecido",  // Verifica se Ingrediente é nulo
                        Quantidade = ri.Quantidade,
                        Unidade = ri.Unidade.ToString()  // Converte o valor do enum para string diretamente
                    }).ToList()
                })
                .ToListAsync();
        }

    }
}
