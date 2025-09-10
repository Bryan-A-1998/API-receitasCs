API de Receitas

Projeto de API REST para gerenciar usuários, receitas e ingredientes.
Desenvolvido em C# utilizando ASP.NET Core e PostgreSQL como banco de dados.

Tecnologias

C# (ASP.NET Core)

PostgreSQL

Entity Framework Core (ORM)


Instalação
1. Criar banco de dados com script scriptBD.txt caso não tenha criado no projeto node

Clone o repositório:

git clone https://github.com/Bryan-A-1998/API-receitasCs


Acesse a pasta do projeto:

cd apiReceitasC#

2. Configuração do Ambiente

Certifique-se de ter o .NET SDK instalado em sua máquina. Você pode verificar isso com o seguinte comando:

dotnet --version


Instale as dependências do projeto:

dotnet restore


Crie o banco de dados e aplique as migrações do Entity Framework Core:

dotnet ef database update

3. Rodar o servidor

Para rodar o servidor, execute o seguinte comando:

dotnet run


O servidor estará rodando na porta configurada (por padrão, http://localhost:5244).

Se tudo estiver correto, você verá a mensagem:

Hosting environment: Development
Now listening on: http://localhost:5244
Application started.

5. Testes Unitários

Os testes unitários estão localizados na pasta /Testes. Utilizando os testes do .txt um a um

6. Uso do Insomnia

Para realizar testes manuais de integração, use o Insomnia
 ou Postman
. Arquivos de testes estão localizados em /Testes/TestesReceitasCs.yaml.

Funcionalidades Implementadas

Cadastro e gerenciamento de usuários

Cadastro e gerenciamento de ingredientes

Cadastro e gerenciamento de receitas

Busca por receitas compatíveis com os ingredientes fornecidos (completa ou parcialmente compatível)

Funcionalidades Cruciais:

CRUD de Usuários: Cadastro, atualização, remoção e listagem de usuários.

CRUD de Ingredientes: Cadastro, atualização, remoção e listagem de ingredientes.

CRUD de Receitas: Cadastro, atualização, remoção e listagem de receitas.

Busca por Receitas Compatíveis: Funcionalidade para listar receitas que contenham os ingredientes informados, ou receitas que quase possuam todos os ingredientes. Receitas completas são marcadas como 100% compatíveis.

Funcionalidades Menos Prioritárias (Que podem ser sacrificadas):

Autenticação de Usuários: No momento, a autenticação de usuários não está implementada.

Padronização de respostas: A padronização de respostas pode ser feita de maneira simplificada, sem usar um serviço específico.

Funcionalidades Imprescindíveis:

CRUD de Usuários

CRUD de Ingredientes

CRUD de Receitas

Busca por Receitas Compatíveis com Ingredientes

Requisitos do Projeto
Problema:

O objetivo é criar uma API onde o usuário possa cadastrar e gerenciar ingredientes e receitas, e ser capaz de pesquisar quais receitas ele pode preparar com os ingredientes que possui.

Coleta de Requisitos:

A aplicação permite realizar as operações CRUD em usuários, ingredientes e receitas, além da funcionalidade de buscar receitas compatíveis com os ingredientes informados. O usuário pode cadastrar, editar e listar os ingredientes e receitas de forma rápida e prática.

Alinhamentos Necessários:

A informação fornecida está parcialmente completa. Para implementar o sistema de estoque de ingredientes e atualizações frequentes, seriam necessários mais detalhes, como um sistema de controle de quantidade e data de validade para os ingredientes.

Para entender o problema de forma completa, alinhamentos adicionais são necessários, como por meio de email, chamada telefônica ou alinhamento pessoal.

Data de Entrega: 10/09/2025