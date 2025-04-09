using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlataformaFbj.Data;
using PlataformaFbj.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlataformaFbj.Controllers
{
    [ApiController]
    [Route("api/Usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios() =>
            Ok(await _context.Usuarios.ToListAsync());

        [HttpGet("desenvolvedores")]
        public async Task<IActionResult> ListarDesenvolvedores()
        {
            var desenvolvedores = await _context.Usuarios
                .Where(u => u.Tipo == "Desenvolvedor")
                .Include(u => u.Jogos)
                .ToListAsync();

            var result = desenvolvedores.Select(d => new DesenvolvedorDto
            {
                Id = d.Id,
                Nome = d.Nome,
                Email = d.Email,
                Tipo = d.Tipo,
                Jogos = d.Jogos.Select(j => new JogoDto
                {
                    Id = j.Id,
                    Nome = j.Nome,
                    Descricao = j.Descricao,
                    Plataforma = j.Plataforma
                }).ToList()
            });

            return Ok(result);
        }


        [HttpGet("testers")]
        public async Task<IActionResult> ListarTesters()
        {
            var testers = await _context.Usuarios
                .Where(u => u.Tipo == "BetaTester")
                .Include(u => u.Feedbacks)
                .ToListAsync();

            var result = testers.Select(t => new UsuarioTestersDto
            {
                Id = t.Id,
                Nome = t.Nome,
                Email = t.Email,
                Tipo = t.Tipo,
                Feedbacks = t.Feedbacks.Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    Comentario = f.Comentario,
                    Nota = f.Nota,
                    DataCriacao = f.DataCriacao
                }).ToList()
            });

            return Ok(result);
        }


        [HttpPost("cadastro")]
        public async Task<IActionResult> Cadastrar([FromBody] Usuario usuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
                return BadRequest("E-mail já cadastrado.");

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Cadastrar), new { id = usuario.Id }, usuario);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            try
            {
                if (login == null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
                    return BadRequest("Email e senha são obrigatórios.");

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == login.Email && u.Senha == login.Senha);

                if (usuario == null)
                    return Unauthorized("Credenciais inválidas.");

                var token = GerarToken(usuario);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("perfil")]
        [Authorize]
        public IActionResult Perfil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok($"Usuário autenticado! ID: {userId}");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Atualizar([FromBody] Usuario usuarioAtt)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null) return NotFound("Usuário não encontrado.");

            usuario.Nome = usuarioAtt.Nome;
            usuario.Email = usuarioAtt.Email;
            usuario.Senha = usuarioAtt.Senha;
            usuario.Tipo = usuarioAtt.Tipo;

            await _context.SaveChangesAsync();
            return Ok(usuario);
        }

        [HttpDelete("excluir")]
        [Authorize]
        public async Task<IActionResult> Deletar()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null) return NotFound("Usuário não encontrado.");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return Ok("Usuário deletado com sucesso.");
        }

        private string GerarToken(Usuario usuario)
        {
            try
            {
                var claims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Tipo)
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao gerar token: " + ex.Message);
                throw; // deixa o erro estourar para o Postman ver como 500, mas com log
            }
        }

    }
}