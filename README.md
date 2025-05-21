# 🛵 ApiMottu – Mapeamento de Motos nos Pátios

API RESTful desenvolvida em ASP.NET Core para o **Challenge FIAP 2025**, em parceria com a **Mottu**.  
Seu objetivo é permitir o **cadastro, consulta, atualização e exclusão** de motos localizadas em pátios das filiais, com suporte a banco de dados **Oracle** e **InMemory** (para testes locais).

---

## 🔧 Tecnologias Utilizadas

- ✅ .NET 8 (SDK)
- ✅ ASP.NET Core Web API
- ✅ Entity Framework Core 8
- ✅ Oracle.EntityFrameworkCore
- ✅ EF Core InMemory (modo de desenvolvimento)
- ✅ Swagger (OpenAPI) para testes interativos

---

## ⚙️ Como Executar Localmente

### ✔️ Pré-requisitos

- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022/2023 **ou** VS Code com C# Dev Kit
- (Opcional) Oracle XE 18c/21c para persistência real

---

### 🚀 Clonando o projeto

```bash
git clone https://github.com/calazans-99/Challenge.Net.git
cd Challenge.Net
```

---

### ▶️ Executando a API

```bash
dotnet restore
dotnet build
dotnet run
```

Acesse:  
📍 [`http://localhost:5000/swagger`](http://localhost:5000/swagger) (ou porta definida no seu `launchSettings.json`)

---

## 🔄 Alternância entre Oracle e InMemory

O comportamento do banco de dados é controlado via `appsettings.json`:

```json
"DatabaseProvider": "Oracle"
```

🔁 Para desenvolvimento sem Oracle, altere para:

```json
"DatabaseProvider": "InMemory"
```

---

## 📦 Estrutura dos Endpoints

| Método | Rota                     | Descrição                                  |
|--------|--------------------------|--------------------------------------------|
| GET    | `/api/Moto`              | Lista todas as motos                       |
| GET    | `/api/Moto/{id}`         | Retorna uma moto específica por ID         |
| GET    | `/api/Moto/buscar`       | Busca por status e/ou modelo (querystring) |
| POST   | `/api/Moto`              | Cadastra uma nova moto                     |
| PUT    | `/api/Moto/{id}`         | Atualiza uma moto existente                |
| DELETE | `/api/Moto/{id}`         | Remove uma moto                            |

---

## 🧠 Funcionalidades Gerais

- 🔍 Consulta dinâmica com parâmetros opcionais
- 🚀 Documentação interativa com Swagger
- 🧪 Banco em memória para testes rápidos
- 🔐 Pronto para integração com Oracle XE via EF Core

---

## 👥 Desenvolvedores

- Marcus Vinicius de Souza Calazans — RM: 556620  
- Lucas Abud Berbel — RM: 557957  

---

## 🗂️ Repositório do Projeto

🔗 [https://github.com/calazans-99/Challenge.Net](https://github.com/calazans-99/Challenge.Net)

---

## 📅 Challenge 2025 – FIAP | 2TDS | 1º Semestre
