# RocketFinanceira

## Visão Geral

RocketFinanceira é uma aplicação desenvolvida em .NET, que oferece um conjunto de APIs para o gerenciamento de usuários, assinaturas e cobranças. Esta documentação descreve as principais funcionalidades da aplicação e fornece informações sobre como utilizá-las.

## Documentação da API

A documentação detalhada das APIs está disponível no Swagger. Você pode acessá-la em [link_para_o_swagger](#) após iniciar a aplicação.

## Funcionalidades Principais

### API de Usuários

A API de Usuários oferece as seguintes rotas:

- **Signup:** Rota para registrar um novo usuário na aplicação.
- **Signin:** Rota para autenticar um usuário e obter um token de acesso.
- **UpdateUser (PATCH):** Rota para atualizar as informações de um usuário existente.

### API de Assinaturas

A API de Assinaturas inclui as seguintes rotas:

- **Create:** Rota para iniciar uma nova assinatura para um usuário.
- **Cancel (PATCH):** Rota para cancelar uma assinatura existente.

### Serviço de Cobrança (Billing Service)

O Serviço de Cobrança é responsável por receber notificações da fila "billing queue" e processar os pagamentos, bem como enviar notificações relacionadas a cobranças.

## Pré-requisitos

Antes de executar a aplicação, certifique-se de ter as seguintes dependências instaladas e configuradas:

- [SDK .NET](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) 

### Iniciando os Serviços Docker

Para iniciar os serviços de banco de dados e RabbitMQ usando Docker, navegue até o diretório onde está o arquivo `docker-compose.yaml` e execute o seguinte comando:

docker-compose up

Isso iniciará apenas os serviços de banco de dados e RabbitMQ, e não a aplicação.

## Iniciando aplicação

1. Clone este repositório em sua máquina local:

git clone https://github.com/Peduxx/rocketFinanceira.git

2. Execute as migrations de User-Api e Subscription-Api, utilizando o comando:

Update-Database

3. Após isso, poderá ir em cada um dos projetos e executar:

dotnet run

Com isso você poderá executar os testes manuais pelo Swagger.

