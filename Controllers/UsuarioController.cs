using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiReceitasC_.Data;
using apiReceitasC_.Models;


namespace apiReceitasC_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);  // Retorna a lista de usuários
        }

        // GET: api/usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();  // Retorna 404 caso o usuário não exista
            }

            return Ok(usuario);  // Retorna o usuário
        }

        // POST: api/usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario([FromBody] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);  // Adiciona o novo usuário
                await _context.SaveChangesAsync();  // Salva no banco de dados

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);  // Retorna 201 com o usuário criado
            }

            return BadRequest(ModelState);  // Retorna erro 400 se os dados forem inválidos
        }

        // PUT: api/usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();  // Se o ID passado não corresponder ao ID no corpo da requisição
            }

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();  // Retorna 204 para indicar que a atualização foi bem-sucedida
        }

        // DELETE: api/usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();  // Retorna 404 se o usuário não for encontrado
            }

            _context.Usuarios.Remove(usuario);  // Remove o usuário
            await _context.SaveChangesAsync();

            return NoContent();  // Retorna 204 para indicar que a exclusão foi bem-sucedida
        }
        // POST: api/usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] Dictionary<string, string> loginRequest)
        {
            // Verifica se os campos email e senha estão presentes
            if (!loginRequest.ContainsKey("email") || !loginRequest.ContainsKey("senha"))
            {
                return BadRequest("Email e Senha são obrigatórios.");
            }

            string email = loginRequest["email"];
            string senha = loginRequest["senha"];

            // Verifica se os campos estão vazios
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                return BadRequest("Email e Senha são obrigatórios.");
            }

            // Tenta encontrar o usuário no banco
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

            if (usuario == null)
            {
                return Unauthorized("Email ou senha inválidos");
            }

            // Retorna sucesso
            return Ok(new { message = "Login bem-sucedido", usuario });
        }

    }

}
