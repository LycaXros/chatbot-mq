# chatbot-mq

Basic Chat bot, with Identity, RabbitMQ, SignalR, Docker

### Features

- Authentication with IdentityServer
- Chat Room
- Command **/stock=_code_** gets price of stock from Stooq
- Command errors are handled by chat Administrator
- Messages are ordered in descending manner by Time
- Only the last 50 messages are displayed in the chat

### Built with

- Docker
- Entity Framework Core
- IdentityServer
- .NET 6
- RabbitMQ

### Prerequisites

- Docker
- Docker Compose

or

- .NET 6 Build Sdk && .NET 6 Runtime
- Microsoft SQL Server
- RabbitMQ

## How to Set Up

### With Docker

- On Root folder of app. Use command: docker compose build
- Run Command: docker compose up -d
- In a browser open the following url: http://[Local_Machine_IP]:4000
- Create an account
- Go to chat room

### Without Docker

- Start the RabbitMQ Server And Microsoft SQL Server
- Update the credentials on appSettings in Chat.Web/appsettings.json and Chat.Worker/appsettings.json
- On Root Folder

```
  dotnet restore
  dotnet run
```

- Load Up the Worker

```
  cd ChatBot.Bot
  dotnet run
```

- Load Up the Web APP, on another terminal on Root Folder

```
  cd ChatBot.Pages
  dotnet run
```

- In a browser open the following url: https://localhost:7002
- Create an account
- Go to chat room
