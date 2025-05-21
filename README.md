# 🛵 ApiMottu – Mapeamento de Motos nos Pátios

API RESTful desenvolvida com ASP.NET Core para o projeto Challenge FIAP 2025 em parceria com a Mottu.  
Tem como objetivo permitir o **cadastro, listagem, atualização e exclusão** de motos em pátios, com suporte a banco em memória e Oracle.

---

## 🔧 Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- EF Core InMemory (temporário)
- Oracle Entity Framework Core (configurável)
- Swagger (documentação e testes)

---

## ⚙️ Como executar localmente

### Pré-requisitos
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022 ou 2023 (recomendado)
- (Opcional) Oracle XE instalado para persistência real

### Clonando o projeto
```bash
git clone https://github.com/seu-usuario/ApiMottu.git
cd ApiMottu
