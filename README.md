# PetFamily Backend v2

Микросервисная backend-система для управления приютом животных. Позволяет волонтёрам регистрироваться, управлять профилями животных, загружать фотографии и получать уведомления.

## Архитектура

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              API Gateway (Ocelot)                           │
│                                  :6000                                      │
└─────────────────────────────────────┬───────────────────────────────────────┘
                                      │
        ┌─────────────┬───────────────┼───────────────┬─────────────┐
        │             │               │               │             │
        ▼             ▼               ▼               ▼             ▼
   ┌─────────┐  ┌───────────┐  ┌────────────┐  ┌────────────┐  ┌─────────────┐
   │  Auth   │  │ Volunteer │  │ Volunteer  │  │    File    │  │Notification │
   │         │  │Management │  │  Request   │  │  Storage   │  │             │
   └────┬────┘  └─────┬─────┘  └─────┬──────┘  └─────┬──────┘  └──────┬──────┘
        │             │              │               │                │
        └─────────────┴──────────────┴───────────────┴────────────────┘
                                     │
                            ┌────────┴────────┐
                            │    RabbitMQ     │
                            │  (Event Bus)   │
                            └─────────────────┘
```

## Микросервисы

| Сервис | Описание | База данных |
|--------|----------|-------------|
| **API Gateway** | Единая точка входа, маршрутизация, rate limiting | - |
| **Auth** | Аутентификация, JWT токены, интеграция с Keycloak | PostgreSQL |
| **VolunteerManagement** | Управление волонтёрами, видами и породами животных | PostgreSQL |
| **VolunteerRequest** | Обработка заявок на волонтёрство | PostgreSQL |
| **FileStorage** | Хранение файлов в MinIO, presigned URLs | PostgreSQL |
| **Notification** | Email-уведомления через SMTP | PostgreSQL |

## Быстрый запуск

### Требования
- Docker & Docker Compose
- .NET SDK 10.0+ (для разработки)

### Запуск инфраструктуры

```bash
cd Solution
docker compose up -d
```

### Запуск микросервисов (для разработки)

```bash
# API Gateway
dotnet run --project Solution/ApiGateway/ApiGateway/

# Auth
dotnet run --project Solution/Auth/Auth.Endpoints/

# VolunteerManagement
dotnet run --project Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.Endpoints/

# VolunteerRequest
dotnet run --project Solution/VolunteerRequest/Hosts/VolunteerRequest.Endpoints/

# FileStorage
dotnet run --project Solution/FileStorage/Hosts/FileStorage.Api/

# Notification
dotnet run --project Solution/Notification/Hosts/Notification.Hosts.Endpoints/

# Consumers (обработка событий из RabbitMQ)
dotnet run --project Solution/VolunteerManagement/Hosts/VolunteerManagement.Hosts.Consumers/
dotnet run --project Solution/Notification/Hosts/Notification.Hosts.Consumers/
dotnet run --project Solution/FileStorage/Hosts/FileStorage.Consumers/
```

## Доступные сервисы

| Сервис | URL | Логин/Пароль |
|--------|-----|--------------|
| API Gateway | http://localhost:6000 | - |
| Auth API | http://localhost:6001 | - |
| VolunteerManagement API | http://localhost:6002 | - |
| Notification API | http://localhost:6003 | - |
| FileStorage API | http://localhost:6004 | - |
| Keycloak | http://localhost:8080 | admin / admin |
| RabbitMQ | http://localhost:15672 | guest / guest |
| MinIO Console | http://localhost:9001 | minioadmin / minioadmin |
| Kibana | http://localhost:5601 | - |
| Prometheus | http://localhost:9090 | - |
| Grafana | http://localhost:3000 | admin / admin |
| Jaeger | http://localhost:16686 | - |
| Seq | http://localhost:5341 | - |

## Структура проекта

```
pet-family-backend-v2/
├── Shared/                          # Общие библиотеки
│   ├── PetFamily.SharedKernel.WebApi/
│   ├── PetFamily.SharedKernel.Domain/
│   ├── PetFamily.SharedKernel.Application/
│   ├── PetFamily.SharedKernel.Infrastructure/
│   └── PetFamily.SharedKernel.Contracts/
│
└── Solution/
    ├── ApiGateway/                  # API Gateway
    ├── Auth/                        # Сервис аутентификации
    ├── VolunteerManagement/         # Управление волонтёрами
    ├── VolunteerRequest/            # Заявки волонтёров
    ├── FileStorage/                 # Хранение файлов
    ├── Notification/                # Уведомления
    ├── compose.yaml                 # Docker Compose
    └── prometheus.yml               # Конфигурация Prometheus
```

## Стек технологий

### Backend
| Технология | Версия | Назначение |
|------------|--------|------------|
| .NET | 10.0 | Основной фреймворк |
| ASP.NET Core | 10.0 | Web API |
| Entity Framework Core | 10.0 | ORM |
| Wolverine | 5.9.0 | CQRS, обработка команд |
| MassTransit | 8.5.7 | Event Bus, интеграция с RabbitMQ |
| FluentValidation | 11.9.0 | Валидация |
| Ardalis.Specification | 8.0.0 | Паттерн Repository/Specification |
| Swashbuckle.AspNetCore | 7.2.0 | Swagger / OpenAPI |
| Minio | 6.0.2 | S3 SDK для MinIO |

### API Gateway
| Технология | Версия | Назначение |
|------------|--------|------------|
| Ocelot | 23.3.3 | Маршрутизация, Rate Limiting, QoS |
| MMLib.SwaggerForOcelot | 8.2.0 | Агрегация Swagger |

### Identity & Security
| Технология | Версия | Назначение |
|------------|--------|------------|
| Keycloak | 26.0.7 | Identity Provider, OAuth2/OIDC |
| JWT Bearer | 8.0.11 | Аутентификация |

### Базы данных и хранение
| Технология | Версия | Назначение |
|------------|--------|------------|
| PostgreSQL | 17.2 | Основная СУБД |
| MinIO | latest | S3-совместимое хранилище файлов |

### Messaging
| Технология | Версия | Назначение |
|------------|--------|------------|
| RabbitMQ | 3-management | Message Broker |
| MassTransit | 8.5.7 | Абстракция над RabbitMQ |

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

### Тестирование
| Технология | Версия | Назначение |
|------------|--------|------------|
| xUnit | 2.9.3 | Тестовый фреймворк |
| FluentAssertions | 7.0.0 | Assertions |
| NSubstitute | 5.3.0 | Mocking |
| Bogus | 35.6.1 | Генерация данных |
| AutoFixture | 4.18.1 | Автогенерация объектов |
| Testcontainers | 4.2.0 | Интеграционные тесты |
| Respawn | 6.2.1 | Сброс БД между тестами |
| NetArchTest | 1.3.2 | Архитектурные тесты |

## Архитектурные паттерны

- **Clean Architecture** — разделение на Domain, Application, Infrastructure, Presentation
- **Domain-Driven Design (DDD)** — Bounded Contexts, Aggregates, Value Objects
- **CQRS** — разделение команд и запросов через Wolverine
- **Event-Driven Architecture** — асинхронная интеграция через RabbitMQ
- **API Gateway Pattern** — единая точка входа через Ocelot
- **Database per Service** — изолированные базы данных для каждого сервиса

## Health Checks

Каждый микросервис предоставляет endpoint для проверки здоровья:

```
GET /health
```

Проверяется:
- Подключение к PostgreSQL
- Подключение к RabbitMQ
- Доступность Keycloak

## Конфигурация

Конфигурация через `appsettings.json` с профилями:
- `appsettings.json` — базовая конфигурация
- `appsettings.Development.json` — локальная разработка
- `appsettings.Docker.json` — запуск в Docker

Основные секции:
- `ConnectionStrings` — строки подключения к БД
- `RabbitMQ` — настройки брокера сообщений
- `Keycloak` — настройки Identity Provider
- `Elasticsearch` — настройки логирования
- `MinIO` — настройки файлового хранилища (FileStorage)
- `Smtp` — настройки почты (Notification)

## Тестирование

Проект VolunteerManagement содержит полный набор тестов:

```bash
# Unit-тесты
dotnet test Solution/VolunteerManagement/Tests/Domain.UnitTests/
dotnet test Solution/VolunteerManagement/Tests/Application.UnitTests/

# Интеграционные тесты
dotnet test Solution/VolunteerManagement/Tests/IntegrationTests/

# Архитектурные тесты
dotnet test Solution/VolunteerManagement/Tests/ArchitectureTests/
```

## Лицензия

MIT
