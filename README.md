
# PlataformaFbj 🎮

Uma plataforma de feedback para jogos indie! Esta API foi construída em ASP.NET Core com Entity Framework, com foco em separar usuários desenvolvedores e beta testers, permitindo o cadastro de jogos e envio de feedbacks.

## 📌 Objetivo

Este projeto é ideal para iniciantes entenderem como funciona:

- Estrutura de uma API com ASP.NET Core
- Controle de autenticação com JWT
- Uso de Entity Framework para persistência de dados
- Organização de código com boas práticas (DTOs, AutoMapper, Services)
- Como consumir a API via Postman

---

## ⚙️ Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/)
- [MySQL](https://www.mysql.com/)
- [AutoMapper](https://automapper.org/)
- [JWT Bearer Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt)
- Postman (para testar os endpoints)

---

## 🧱 Estrutura do Projeto

```
PlataformaFbj/
├── Controllers/
├── DTOs/
├── Models/
├── Profiles/
├── Services/
├── Data/
├── Program.cs
├── appsettings.json
└── ...
```

### Pastas importantes:

- **Controllers/**: Onde estão os endpoints da API.
- **DTOs/**: Representações dos dados trafegados pela API.
- **Models/**: As classes que representam as tabelas do banco.
- **Profiles/**: Configurações do AutoMapper.
- **Services/**: Lógica de autenticação (AuthService).
- **Data/**: O `AppDbContext`, que conecta o código ao banco de dados.

---

## 🚀 Como rodar o projeto localmente

1. **Clone o repositório:**

```bash
git clone https://github.com/vinemoraes43/plataforma-feedback.git
cd PlataformaFbj
```

2. **Configuração o banco de dados MySQL: (Explicando como funciona a conexão ao banco)** 

Crie um banco de dados chamado `PlataformaFbjDb` e atualize a `ConnectionString` no arquivo `appsettings.json`.

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=5079;database=PlataformaFbj;user=root;password=123456"
}
```

3. **Instale os pacotes (caso necessário):**

```bash
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.3
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.3
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.3
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1

dotnet restore
```

4. **Aplique as migrações e atualize o banco:**

```bash
dotnet tool install --global dotnet-ef ("verificar se o CLI do EF Core está instalado")

dotnet ef migrations add Inicial ("Inicial pode ser o nome do motivo dessa migração como: 'CriarTabelas'")

dotnet ef database update

// Caso tenha que voltar a migração use:

dotnet ef migrations remove

dotnet ef database update NomeDaMigracaoAnterior
```

5. **Execute o projeto:**

```bash
dotnet run
```

6. **Acesse a API no navegador ou no Postman:**

```
https://localhost:5079/swagger
```

---

## 🧪 Testando com o Postman

Usuario 

- **Cadastro:** `POST /api/Usuario/cadastro`
- **Login:** `POST /api/Usuario/login` → retorna o token JWT
- **Listar desenvolvedores:** `GET /api/Usuario/desenvolvedores`
- **Listar beta testers:** `GET /api/Usuario/testers`
- **Ver perfil logado:** `GET /api/Usuario/perfil` (Requer token)

Jogo



---

## 📚 Dicas para quem está começando

- Estude a estrutura MVC (Model-View-Controller).
- Explore os conceitos de DTOs e AutoMapper.
- Pratique com o Postman: isso ajuda muito a entender o que cada endpoint faz.
- Se tiver dúvidas, use a documentação oficial do ASP.NET Core — ela é muito boa!

---

## 🙋‍♀️ Contribuindo

Você pode melhorar esse projeto com novas funcionalidades como:

- Interface Web para visualizar os jogos e feedbacks
- Upload de imagens dos jogos
- Sistema de likes nos comentários

---

## 🧑‍💻 Autor

Este projeto faz parte de um portfólio pessoal para praticar e demonstrar habilidades em back-end com ASP.NET Core. 🚀
