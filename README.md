# StartTrek — README 

Visão geral
- API .NET (C#) com arquitetura em camadas: Domain, Application (services/DTOs) e Infrastructure (DbContext, repositories).
- Persistência: EF Core com provider Oracle (UseOracle). Swagger habilitado.

Como rodar (rápido)
1. Clonar
   git clone https://github.com/juliabusso/StartTrekGSDotNet.git && cd StartTrekGSDotNet
2. Restaurar/build
   dotnet restore && dotnet build
3. Configurar connection string (ex.: ConnectionStrings__DefaultConnection)
4. Migrations
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add InitialCreate --project src/StartTrekGS.Infrastructure --startup-project .
   dotnet ef database update --project src/StartTrekGS.Infrastructure --startup-project .
5. Executar
   dotnet run --project .

Membros:
-Julia Damasceno Busso - RM560293 - 2TDSPB

-Gabriel Gomes Cardoso - Rm559597 - 2TDSPB

-Jhonatan Quispe Torrez - rm560601 - 2TDSPB


Variáveis importantes
- ConnectionStrings__DefaultConnection (string Oracle)
- ASPNETCORE_ENVIRONMENT (Development/Production)
- PORT (opcional)

Rotas principais
- GET /trabalhos                      — lista paginada de trabalhos (query: pagina, tamanho)
- GET /api/Usuario/search             — pesquisa de usuários (nome, ativo, pagina, tamanho, ordenarPor, direcao)
- POST /api/Usuario                   — cadastrar usuário (DTO: NomeUsuario, Email, Senha, IdTipoUsuario, ...)
- DELETE /api/Usuario/{id}            — excluir usuário
- Swagger UI: /swagger

Exemplos (curl)
- Listar trabalhos:
  curl -k GET "https://localhost:5001/trabalhos"
- Pesquisar usuários:
  curl -k -G "https://localhost:5001/api/Usuario/search" --data-urlencode "nome=Spock" --data-urlencode "ativo=true"
- Criar usuário (ajuste DTO conforme controller):
  curl -k -X POST "https://localhost:5001/api/Usuario" -H "Content-Type: application/json" -d '{"nomeUsuario":"Spock","email":"spock@ex","senha":"senha1234","idTipoUsuario":1}'
