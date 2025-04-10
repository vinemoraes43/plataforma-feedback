using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlataformaFbj.Data;
using PlataformaFbj.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PlataformaFbj.DTOs.Usuario;
//using PlataformaFbj.DTOs.Jogo;
//using PlataformaFbj.DTOs.Feedback;
using AutoMapper;

namespace PlataformaFbj.Controllers
{
    [ApiController]
    [Route("api/Usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UsuarioController(AppDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
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

            var result = _mapper.Map<List<DesenvolvedorDto>>(desenvolvedores);

            return Ok(result);
        }

        [HttpGet("testers")]
        public async Task<IActionResult> ListarTesters()
        {
            var testers = await _context.Usuarios
                .Where(u => u.Tipo == "BetaTester")
                .Include(u => u.Feedbacks)
                .ToListAsync();

            var result = _mapper.Map<List<UsuarioTestersDto>>(testers);

            return Ok(result);
        }

        [HttpPost("cadastro")]
        public async Task<IActionResult> Cadastrar([FromBody] UsuarioCadastroDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("E-mail já cadastrado.");

            var usuario = _mapper.Map<Usuario>(dto);

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
        public async Task<IActionResult> Atualizar([FromBody] UsuarioAtualizacaoDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null) return NotFound("Usuário não encontrado.");

            _mapper.Map(dto, usuario); // aplica as mudanças do DTO no objeto existente

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