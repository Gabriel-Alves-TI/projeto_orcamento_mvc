# 🧾 Projeto Serralheria (Orçamentos e Recibos)

Este projeto foi desenvolvido com o objetivo de atender às necessidades reais de um negócio familiar do ramo de serralheria, permitindo o gerenciamento de **orçamentos** e **recibos de pagamento**.

Esta versão representa um **modelo simplificado**, criado para fins de estudo, demonstração e entendimento do funcionamento do sistema real.

---
## 📚 Aprendizados

- Aplicação prática do padrão MVC
- Criação e reutilização de Partial Views
- Uso de AJAX para carregamento dinâmico de dados sem recarregar a página
- Implementação de modais e autocomplete para melhoria da experiência do usuário
- Geração de relatórios em PDF com FastReport
- Relacionamentos entre entidades utilizando Entity Framework Core

### ⚡ Recursos Técnicos Implementados

- Utilização de **Partial Views** para reaproveitamento de componentes e atualização dinâmica da listagem de itens
- Implementação de **modais** para busca e seleção de clientes
- Comunicação assíncrona entre front-end e back-end utilizando **AJAX**
- Autocomplete para facilitar a experiência do usuário na seleção de dados
- Separação de responsabilidades seguindo o padrão **MVC**

## 🛠️ Tecnologias Utilizadas

- C# (.NET 8.0)
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- FastReport (geração de relatórios em PDF)
- Partial Views (ASP.NET MVC)
- Modais (Bootstrap)
- Autocomplete (com AJAX)
<!-- - Docker -->

---

## ⚙️ Pré-requisitos

Certifique-se de ter as seguintes ferramentas instaladas:

- [.NET 8.0](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/pt-br/download/details.aspx?id=104781)
<!-- - (Opcional) Docker – para execução via container -->

---

## 📋 Funcionalidades

### 🏠 Home
- Listagem de Orçamentos e Recibos

### 👤 Clientes
- Listar clientes
- Cadastrar cliente
- Editar cliente
- Excluir cliente

### 📄 Orçamentos
- Pré-cadastro de orçamento
- Cadastrar orçamento
- Editar orçamento
- Excluir orçamento
- Gerenciamento de itens:
  - Adicionar item
  - Editar item
  - Excluir item
- Listagem dinâmica de itens utilizando **Partial Views**
- Autocomplete para busca e seleção de cliente
- Busca de cliente via **modal**, com carregamento assíncrono usando **AJAX**

### 🧾 Recibos
- Cadastrar recibo
- Editar recibo
- Excluir recibo
- Autocomplete para busca e seleção de cliente
- Busca de cliente via **modal**, com carregamento assíncrono usando **AJAX**

---

## 🚀 Como Rodar o Projeto

### 1. **Clone o Repositório**
```
    git clone git@github.com:Gabriel-Alves-TI/projeto_orcamento_mvc.git
    cd projeto_orcamento_mvc
```
### 2. **Configure a ConnectionString do Banco de dados**
Crie um arquivo appsettings.json conforme exemplo abaixo:
```
    {
    "Logging": {
        "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
        }
    },
    "ConnectionStrings": 
    {
        "DefaultConnection": "server= seuServer; database=nome_database; trusted_connection=true; trustservercertificate=true;"
    },
    "AllowedHosts": "*"
    }

```

### 3. **Instale as Dependências**
```
    dotnet restore
```
Após instalação, rode o comando:
```
    dotnet build
    dotnet run
```
O projeto estará disponível em:
👉 http://localhost:(porta especificada no console)

ou se quiser definir uma porta manualmente:
```
    dotnet run --urls="http://localhost:5000"
```

