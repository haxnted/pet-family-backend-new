# PetFamily Backend

Микросервисная backend-система для управления приютом животных. Позволяет волонтёрам регистрироваться, управлять профилями животных, вести чаты по усыновлению, загружать фотографии и получать уведомления.

## Архитектура

```
                           ┌──────────────────────────────────────┐
                           │        API Gateway (Ocelot)          │
                           │              :6000                   │
                           └──────────────────┬───────────────────┘
                                              │
       ┌──────────┬──────────┬────────────────┼────────────┬──────────┬──────────┐
       │          │          │                │            │          │          │
       ▼          ▼          ▼                ▼            ▼          ▼          ▼
  ┌─────────┐┌────────┐┌──────────┐   ┌────────────┐┌──────────┐ ┌────────┐┌────────────┐
  │  Auth   ││Account ││Volunteer │   │    File    ││Notifica- │ │Conver- ││ Volunteer  │
  │  :6001  ││ :6006  ││Management│   │  Storage   ││  tion    │ │ sation ││  Request   │
  │         ││        ││  :6002   │   │   :6004    ││  :6003   │ │ :6005  ││  (planned) │
  └────┬────┘└───┬────┘└────┬─────┘   └─────┬──────┘└────┬─────┘ └───┬────┘└────────────┘
       │         │          │               │            │           │
       └─────────┴──────────┴───────────────┴────────────┴───────────┘
                                            │
             ┌─────────────────────────────┬┴┬─────────────────────────────┐
             │                             │ │                             │
     ┌───────┴───────┐            ┌────────┴─┴────────┐          ┌────────┴────────┐
     │   RabbitMQ    │            │    PostgreSQL     │          │     Redis       │
     │  (Event Bus)  │            │  (per service DB) │          │   (Caching)     │
     └───────────────┘            └───────────────────┘          └─────────────────┘
```

Каждый микросервис имеет два хоста:
- **Endpoints** — HTTP API (ASP.NET Core)
- **Consumers** — обработчики событий из RabbitMQ (MassTransit)

## Микросервисы

| Сервис | Описание | Порт (Docker) | БД порт |
|--------|----------|:-------------:|:-------:|
| **API Gateway** | Единая точка входа, маршрутизация, rate limiting | 6000 | — |
| **Auth** | Аутентификация, JWT, refresh tokens, интеграция с Keycloak | 6001 | 5432 |
| **VolunteerManagement** | Волонтёры, приюты, питомцы, сага усыновления | 6002 | 5433 |
| **Notification** | Email-уведомления, настройки пользователей | 6003 | 5434 |
| **FileStorage** | Хранение файлов в MinIO, presigned URLs | 6004 | — |
| **Conversation** | Чаты между волонтёрами и усыновителями | 6005 | 5438 |
| **Account** | Профили пользователей, фото, контакты | 6006 | 5437 |
| **VolunteerRequest** | Заявки на волонтёрство *(planned)* | — | 5435 |

## Быстрый запуск

### Требования
- Docker & Docker Compose
- .NET SDK 10.0+ (для разработки)

### Настройка секретов

Создайте файл `Solution/.env` на основе примера:

```bash
cp Solution/.env.example Solution/.env
# Заполните реальные значения
```

Необходимые переменные:
```env
GITHUB_USERNAME=your_username
GITHUB_TOKEN=your_github_pat
KEYCLOAK_CLIENT_SECRET=your_keycloak_secret
```

### Запуск через Docker Compose

```bash
cd Solution
docker compose up -d
```

Это поднимет всю инфраструктуру и все микросервисы.

### Запуск для разработки (локально)

Сначала поднимите инфраструктуру:
```bash
cd Solution
docker compose up -d postgres-auth postgres-volunteer-management postgres-notification postgres-account postgres-conversation postgres-keycloak redis rabbitmq minio seq elasticsearch
```

Затем запустите нужные сервисы:
```bash
# API Gateway
dotnet run --project Solution/ApiGateway/ApiGateway/

# Auth
dotnet run --project Solution/Auth/Hosts/Auth.Endpoints/

# VolunteerManagement
dotnet run --project Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.Endpoints/

# Account
dotnet run --project Solution/Account/Hosts/Account.Hosts.Endpoints/

# Conversation
dotnet run --project Solution/Conversation/Hosts/Conversation.Hosts.Endpoints/

# FileStorage
dotnet run --project Solution/FileStorage/Hosts/FileStorage.Endpoints/

# Notification
dotnet run --project Solution/Notification/Hosts/Notification.Hosts.Endpoints/
```

Consumers (обработка событий из RabbitMQ):
```bash
dotnet run --project Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.Consumers/
dotnet run --project Solution/Account/Hosts/Account.Hosts.Consumers/
dotnet run --project Solution/Conversation/Hosts/Conversation.Hosts.Consumers/
dotnet run --project Solution/FileStorage/Hosts/FileStorage.Consumers/
dotnet run --project Solution/Notification/Hosts/Notification.Hosts.Consumers/
```

### Сборка всего solution

```bash
dotnet build Solution/Solution.slnx
```

## Доступные сервисы

| Сервис | URL | Логин / Пароль |
|--------|-----|:--------------:|
| API Gateway | http://localhost:6000 | — |
| Auth API | http://localhost:6001/swagger | — |
| VolunteerManagement API | http://localhost:6002/swagger | — |
| Notification API | http://localhost:6003/swagger | — |
| FileStorage API | http://localhost:6004/swagger | — |
| Conversation API | http://localhost:6005/swagger | — |
| Account API | http://localhost:6006/swagger | — |
| Keycloak | http://localhost:8080 | admin / admin |
| RabbitMQ Management | http://localhost:15672 | guest / guest |
| MinIO Console | http://localhost:9001 | minioadmin / minioadmin |
| Kibana | http://localhost:5601 | — |
| Prometheus | http://localhost:9090 | — |
| Grafana | http://localhost:3000 | admin / admin |
| Jaeger UI | http://localhost:16686 | — |
| Seq | http://localhost:5341 | — |
| Mailpit (dev email) | http://localhost:8025 | — |

## Структура проекта

```
pet-family-backend-new/
├── Shared/                                  # Общие библиотеки (Shared Kernel)
│   ├── PetFamily.SharedKernel.Domain/       #   Базовые классы DDD (Entity, ValueObject, AggregateRoot)
│   ├── PetFamily.SharedKernel.Application/  #   Интерфейсы, исключения, абстракции
│   ├── PetFamily.SharedKernel.Infrastructure/ # EF Core base, кеширование, транзакции
│   ├── PetFamily.SharedKernel.WebApi/       #   Middleware, расширения, аутентификация
│   ├── PetFamily.SharedKernel.Contracts/    #   Интеграционные события, общие DTO
│   └── PetFamily.SharedKernel.Tests/        #   Тестовая инфраструктура (Testcontainers, Respawn)
│
└── Solution/
    ├── ApiGateway/                          # API Gateway (Ocelot)
    │
    ├── Auth/                                # Сервис аутентификации
    │   ├── Auth.Core/                       #   Доменные модели (User, RefreshToken)
    │   ├── Auth.Application/                #   Сервисы (AuthService)
    │   ├── Auth.Infrastructure/             #   Keycloak клиент, EF Core, Seeder
    │   ├── Auth.Contracts/                  #   Контракты
    │   └── Hosts/Auth.Endpoints/            #   HTTP API
    │
    ├── VolunteerManagement/                 # Управление волонтёрами и питомцами
    │   ├── VolunteerManagement.Domain/      #   Агрегаты (Volunteer, Shelter, Pet)
    │   ├── VolunteerManagement.Infrastructure/ # EF Core, саги MassTransit
    │   ├── Application/
    │   │   ├── VolunteerManagement.Handlers/#   CQRS команды и запросы
    │   │   └── VolunteerManagement.Services/#   Доменные сервисы, саги
    │   ├── Hosts/
    │   │   ├── VolunteerManagement.Hosts.Endpoints/ # HTTP API
    │   │   ├── VolunteerManagement.Hosts.Consumers/ # MassTransit consumers
    │   │   └── VolunteerManagement.Hosts.DI/        # DI модуль
    │   └── Tests/                           #   Unit, Architecture тесты
    │
    ├── Account/                             # Профили пользователей
    │   ├── Account.Domain/                  #   Агрегат Account, Value Objects
    │   ├── Account.Infrastructure/          #   EF Core
    │   ├── Application/
    │   │   ├── Account.Handlers/            #   CQRS запросы
    │   │   └── Account.Services/            #   Сервис аккаунтов
    │   └── Hosts/
    │       ├── Account.Hosts.Endpoints/     #   HTTP API
    │       ├── Account.Hosts.Consumers/     #   Consumer (UserCreatedEvent)
    │       └── Account.Hosts.DI/            #   DI модуль
    │
    ├── Conversation/                        # Чаты
    │   ├── Conversation.Domain/             #   Агрегаты (Chat, Message)
    │   ├── Conversation.Infrastructure/     #   EF Core
    │   ├── Application/
    │   │   ├── Conversation.Handlers/       #   CQRS команды и запросы
    │   │   └── Conversation.Services/       #   ChatService, кеширование
    │   └── Hosts/
    │       ├── Conversation.Hosts.Endpoints/#   HTTP API
    │       ├── Conversation.Hosts.Consumers/#   Consumer (AdoptionChat)
    │       └── Conversation.Hosts.DI/       #   DI модуль
    │
    ├── FileStorage/                         # Хранение файлов
    │   ├── FileStorage.Application/         #   Сервисы, валидаторы
    │   ├── FileStorage.Infrastructure/      #   MinIO клиент
    │   ├── FileStorage.Contracts/           #   HTTP-клиент для других сервисов
    │   └── Hosts/
    │       ├── FileStorage.Endpoints/       #   HTTP API
    │       ├── FileStorage.Consumers/       #   Consumer (FileDeleteRequested)
    │       └── FileStorage.DI/              #   DI модуль
    │
    ├── Notification/                        # Email-уведомления
    │   ├── Notification.Core/               #   Доменные модели
    │   ├── Notification.Application/        #   Сервисы настроек
    │   ├── Notification.Infrastructure/     #   EF Core, EmailService, BackgroundJobs
    │   ├── Notification.Contracts/          #   Контракты
    │   └── Hosts/
    │       ├── Notification.Hosts.Endpoints/#   HTTP API
    │       ├── Notification.Hosts.Consumers/#   Consumers (events, emails)
    │       └── Notification.Hosts.DI/       #   DI модуль
    │
    ├── compose.yaml                         # Docker Compose (все сервисы + инфраструктура)
    └── Solution.slnx                        # Solution файл
```

## Стек технологий

### Backend
| Технология | Версия | Назначение |
|------------|--------|------------|
| .NET | 10.0 | Основной фреймворк |
| ASP.NET Core | 10.0 | Web API |
| Entity Framework Core | 10.0 | ORM |
| Wolverine | 5.9.0 | CQRS, обработка команд и запросов |
| MassTransit | 8.5.7 | Event Bus, Outbox/Inbox, Sagas |
| FluentValidation | 11.9.0 | Валидация |
| Ardalis.Specification | 8.0.0 | Паттерн Specification |
| Minio | 6.0.2 | S3-совместимый клиент |

### API Gateway
| Технология | Версия | Назначение |
|------------|--------|------------|
| Ocelot | 23.3.3 | Reverse proxy, Rate Limiting, QoS |

### Identity & Security
| Технология | Версия | Назначение |
|------------|--------|------------|
| Keycloak | 26.0.7 | Identity Provider, OAuth2/OIDC |
| JWT Bearer | 8.0.11 | Аутентификация |

### Базы данных и хранение
| Технология | Версия | Назначение |
|------------|--------|------------|
| PostgreSQL | 17.2 | Основная СУБД (database per service) |
| Redis | 7.4 | Кеширование |
| MinIO | latest | S3-совместимое хранилище файлов |

### Messaging
| Технология | Версия | Назначение |
|------------|--------|------------|
| RabbitMQ | 3-management | Message Broker |
| MassTransit | 8.5.7 | Абстракция, Outbox/Inbox, Sagas |

### Observability
| Технология | Версия | Назначение |
|------------|--------|------------|
| Serilog | 8.0.0 | Структурированное логирование |
| Elasticsearch | 8.11.3 | Хранение логов |
| Kibana | 8.11.3 | Визуализация логов |
| Seq | latest | Просмотр логов (dev) |
| Prometheus | 2.48.1 | Сбор метрик |
| Grafana | 10.2.3 | Дашборды метрик |
| OpenTelemetry | 1.15.0 | Distributed Tracing |
| Jaeger | 1.54 | Визуализация трейсов |

### Email
| Технология | Версия | Назначение |
|------------|--------|------------|
| MailKit | 4.3.0 | SMTP-клиент |
| Mailpit | latest | Dev SMTP сервер с Web UI |

### Тестирование
| Технология | Версия | Назначение |
|------------|--------|------------|
| xUnit | 2.9.3 | Тестовый фреймворк |
| FluentAssertions | 7.0.0 | Assertions |
| NSubstitute | 5.3.0 | Mocking |
| Bogus | 35.6.1 | Генерация фейковых данных |
| AutoFixture | 4.18.1 | Автогенерация объектов |
| Testcontainers | 4.2.0 | Интеграционные тесты с Docker |
| Respawn | 6.2.1 | Сброс БД между тестами |
| NetArchTest | 1.3.2 | Архитектурные тесты |

## Архитектурные паттерны

- **Clean Architecture** — разделение на Domain, Application, Infrastructure, Presentation
- **Domain-Driven Design (DDD)** — Bounded Contexts, Aggregates, Value Objects, Domain Events
- **CQRS** — разделение команд и запросов через Wolverine
- **Event-Driven Architecture** — асинхронная интеграция через RabbitMQ + MassTransit
- **Saga Pattern** — оркестрация усыновления питомцев через MassTransit State Machine
- **Specification Pattern** — гибкие запросы через Ardalis.Specification
- **Outbox/Inbox Pattern** — гарантированная доставка событий
- **API Gateway Pattern** — единая точка входа через Ocelot
- **Database per Service** — изолированные базы данных для каждого микросервиса

## Health Checks

Каждый микросервис предоставляет endpoint для проверки здоровья:

```
GET /health
```

Проверяется:
- Подключение к PostgreSQL
- Подключение к RabbitMQ
- Доступность Keycloak
- Подключение к Redis (где используется)

## Конфигурация

Конфигурация через `appsettings.json` с профилями окружений:
- `appsettings.json` — базовая конфигурация
- `appsettings.Development.json` — локальная разработка
- `appsettings.Docker.json` — запуск в Docker

Основные секции:
- `ConnectionStrings` — строки подключения к БД
- `RabbitMQ` — настройки брокера сообщений
- `Keycloak` — настройки Identity Provider
- `Elasticsearch` — настройки логирования
- `MinIO` — настройки файлового хранилища (FileStorage)
- `Smtp` — настройки почтового сервера (Notification)
- `Redis` — настройки кеширования

> Секреты не должны храниться в `appsettings.json`. Используйте переменные окружения или Secret Manager.

## Тестирование

```bash
# Все тесты
dotnet test Solution/Solution.slnx

# Unit-тесты домена VolunteerManagement
dotnet test Solution/VolunteerManagement/Tests/Domain.UnitTests/

# Unit-тесты приложения VolunteerManagement
dotnet test Solution/VolunteerManagement/Tests/Application.UnitTests/

# Архитектурные тесты
dotnet test Solution/VolunteerManagement/Tests/ArchitectureTests/
```

## Лицензия

MIT
