# 🍔 Good Hamburger — Sistema de Pedidos

API REST desenvolvida em **ASP.NET Core (.NET 8)** para gerenciar pedidos de uma lanchonete, com frontend em **Blazor WebAssembly**.

---

## Sobre o projeto

Sistema desenvolvido como desafio técnico para a STGenetics, o **Good Hamburger**. Permite registrar, consultar, atualizar e remover pedidos de uma lanchonete, aplicando automaticamente as regras de desconto conforme 
a combinação de itens escolhidos.

---

## Tecnologias utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- [ASP.NET Core Web API](https://learn.microsoft.com/aspnet/core)
- [Blazor WebAssembly](https://learn.microsoft.com/aspnet/core/blazor) *(frontend)*
- [Swagger / OpenAPI](https://swagger.io/) *(documentação da API)*
- [xUnit](https://xunit.net/) *(testes automatizados)*

---

## Estrutura do projeto

```
GoodHamburger/
├── Controllers/
│   ├── MenuController.cs          # Endpoint do cardápio
│   └── OrdersController.cs        # CRUD de pedidos
├── Domain/
│   ├── Entities/
│   │   ├── MenuItem.cs            # Item do cardápio
│   │   └── Order.cs               # Pedido com cálculo de desconto
│   └── Enums/
│       └── MenuItemType.cs        # Tipos: Sandwich, Fries, Drink
├── Application/
│   ├── Services/
│   │   ├── MenuService.cs         # Retorna o cardápio fixo
│   │   └── OrderService.cs        # Regras de negócio dos pedidos
│   └── DTOs/
│       └── OrderResponseDto.cs    # Contrato de resposta da API
├── Infrastructure/
│   └── Repositories/
│       └── InMemoryOrderRepository.cs  # Persistência em memória
├── Middleware/
│   └── ExceptionMiddleware.cs     # Tratamento global de erros
└── Program.cs                     # Configuração da aplicação

GoodHamburger.Blazor/
├── Pages/
│   ├── Orders.razor               # Listagem e criação de pedidos
│   └── Menu.razor                 # Exibição do cardápio
├── Services/
│   └── OrderApiService.cs         # Client HTTP para a API
└── Models/
    └── OrderDto.cs                # Models espelhando a API

GoodHamburger.Tests/
└── Services/
    └── OrderServiceTests.cs       # Testes das regras de negócio
```

---

## Cardápio e regras de desconto

### Cardápio

| # | Item | Tipo | Preço |
|---|------|------|-------|
| 1 | X Burger | Sanduíche | R$ 5,00 |
| 2 | X Egg | Sanduíche | R$ 4,50 |
| 3 | X Bacon | Sanduíche | R$ 7,00 |
| 4 | Batata frita | Acompanhamento | R$ 2,00 |
| 5 | Refrigerante | Bebida | R$ 2,50 |

### Regras de desconto

| Combinação | Desconto |
|------------|----------|
| Sanduíche + Batata + Refrigerante | 20% |
| Sanduíche + Refrigerante | 15% |
| Sanduíche + Batata | 10% |
| Outros | 0% |

> **Restrição:** cada pedido aceita no máximo **um item de cada tipo**. Pedidos com itens duplicados são rejeitados com erro 422.

---

## Endpoints da API

### Cardápio

```
GET /api/menu
```
Retorna todos os itens disponíveis.

---

### Pedidos

```
GET    /api/orders           → Lista todos os pedidos
GET    /api/orders/{id}      → Busca pedido por ID
POST   /api/orders           → Cria novo pedido
PUT    /api/orders/{id}      → Atualiza pedido existente
DELETE /api/orders/{id}      → Remove pedido
```

#### Exemplo de criação de pedido

**Request:**
```http
POST /api/orders
Content-Type: application/json

[1, 4, 5]
```
> O body é uma lista com os **IDs dos itens** do cardápio.

**Response `201 Created`:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "items": [
    { "id": 1, "name": "X Burger", "price": 5.00, "type": "Sandwich" },
    { "id": 4, "name": "Batata frita", "price": 2.00, "type": "Fries" },
    { "id": 5, "name": "Refrigerante", "price": 2.50, "type": "Drink" }
  ],
  "subtotal": 9.50,
  "discountPercentage": 20.0,
  "discountAmount": 1.90,
  "total": 7.60,
  "createdAt": "2024-01-15T10:30:00Z"
}
```

#### Respostas de erro

| Código | Situação |
|--------|----------|
| `400` | Body vazio ou malformado |
| `404` | Pedido não encontrado |
| `422` | Item duplicado ou ID inválido no cardápio |
| `500` | Erro interno inesperado |

---

## Como executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

### Rodando a API

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/good-hamburger.git
cd good-hamburger

# Entre na pasta da API
cd GoodHamburger

# Execute
dotnet run
```

A API estará disponível em `https://localhost:7001`.
A documentação Swagger estará em `https://localhost:7001/swagger`.

### Rodando o frontend Blazor

```bash
# Em outro terminal, entre na pasta do Blazor
cd GoodHamburger.Blazor

# Execute
dotnet run
```

O frontend estará disponível em `https://localhost:7002`.

### Rodando ambos pelo Visual Studio

1. Clique com botão direito na **Solução**
2. Selecione **Definir Projetos de Inicialização**
3. Escolha **Vários projetos de inicialização**
4. Marque `GoodHamburger` e `GoodHamburger.Blazor` como **Iniciar**
5. Pressione **F5**

---

## Executando os testes

```bash
cd GoodHamburger.Tests
dotnet test
```

### Cobertura dos testes

- Desconto de 20% para combinação completa (sanduíche + batata + refrigerante)
- Desconto de 15% para sanduíche + refrigerante
- Desconto de 10% para sanduíche + batata
- Rejeição de pedido com tipo de item duplicado
- Rejeição de pedido com ID de item inválido
- Retorno correto ao tentar deletar pedido inexistente

---

## Decisões de arquitetura

**Organização por camadas dentro de um único projeto**
Optei por separar o código em pastas representando as camadas (Domain, Application, Infrastructure) dentro de um único projeto Web API, em vez de criar múltiplos projetos na solution. Essa abordagem entrega os mesmos benefícios de separação de responsabilidades com menos complexidade de configuração para um projeto de escopo reduzido.

**Repositório em memória**
Os dados são mantidos em um `Dictionary<Guid, Order>` registrado como `Singleton` no container de DI. Isso garante que os pedidos persistam durante toda a execução da aplicação. A troca por um banco de dados real (ex: EF Core + SQL Server) exigiria apenas criar uma nova implementação de `IOrderRepository`, sem alterar nada no `OrderService`.

**Lógica de desconto na entidade `Order`**
O cálculo de desconto vive como propriedade computada diretamente na entidade, pois é uma regra de negócio central do domínio. Dessa forma o desconto é sempre consistente independente de quem consultar o pedido.

**Middleware global de exceções**
Em vez de blocos try/catch nos controllers, um middleware centralizado intercepta as exceções e as mapeia para os códigos HTTP corretos (`400`, `404`, `422`, `500`). Isso mantém os controllers limpos e garante respostas de erro padronizadas em toda a API.

**Validação por exceções tipadas**
`KeyNotFoundException` para item inexistente no cardápio e `InvalidOperationException` para item duplicado. O middleware converte cada tipo de exceção para o status HTTP adequado.

---

## O que ficou fora do escopo

- **Persistência em banco de dados** — os dados são perdidos ao reiniciar a aplicação. A interface `IOrderRepository` está pronta para receber uma implementação com EF Core.
- **Autenticação e autorização** — a API é pública, sem controle de acesso.
- **Paginação** — o endpoint `GET /api/orders` retorna todos os pedidos sem paginação.
- **Docker** — não há `Dockerfile` ou `docker-compose` configurados.
- **Deploy em nuvem** — o projeto roda apenas localmente.
