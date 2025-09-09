using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiReceitasC_.Data;
using apiReceitasC_.Models;

namespace apiReceitasC_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredienteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IngredienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ingrediente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingrediente>>> GetIngredientes()
        {
            var Ingredientes = await _context.Ingredientes.ToListAsync();
            return Ok(Ingredientes);  // Retorna a lista de ingredientes
        }

        // GET: api/ingrediente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingrediente>> GetIngrediente(int id)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(id);

            if (ingrediente == null)
            {
                return NotFound();  // Retorna 404 caso o ingrediente não exista
            }

            return Ok(ingrediente);  // Retorna o ingrediente
        }

        // POST: api/ingrediente
        [HttpPost]
        public async Task<ActionResult<Ingrediente>> PostIngrediente([FromBody] Ingrediente ingrediente)
        {
            if (ModelState.IsValid)
            {
                _context.Ingredientes.Add(ingrediente);  // Adiciona o novo ingrediente
                await _context.SaveChangesAsync();  // Salva no banco de dados

                return CreatedAtAction(nameof(GetIngredientes), new { id = ingrediente.Id }, ingrediente);  // Retorna 201 com o ingrediente criado
            }

            return BadRequest(ModelState);  // Retorna erro 400 se os dados forem inválidos
        }

        

        // PUT: api/ingrediente/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngrediente(int id, [FromBody] Ingrediente ingrediente)
        {
            if (id != ingrediente.Id)
            {
                return BadRequest();  // Se o ID passado não corresponder ao ID no corpo da requisição
            }

            _context.Entry(ingrediente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();  // Retorna 204 para indicar que a atualização foi bem-sucedida
        }

        // DELETE: api/ingrediente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngrediente(int id)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(id);
            if (ingrediente == null)
            {
                return NotFound();  // Retorna 404 se o ingrediente não for encontrado
            }

            _context.Ingredientes.Remove(ingrediente);  // Remove o ingrediente
            await _context.SaveChangesAsync();

            return NoContent();  // Retorna 204 para indicar que a exclusão foi bem-sucedida
        }
    }
}
