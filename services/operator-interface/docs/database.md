# Схема базы данных - Система "Интерфейс оператора"

## Обзор
Данный документ описывает схему базы данных для системы **"Интерфейс оператора"** - решения для управления очередями операторов, обслуживающих клиентов в сервисных окнах.

## Диаграмма схемы

```mermaid
erDiagram
    operator_sessions {
        uuid id PK
        varchar(100) operator_id
        varchar(50) workplace_code
        text status
        timestamptz session_start_time
        timestamptz session_end_time
        integer current_client_session_id FK
        jsonb[] assigned_services
    }
    
    client_sessions {
        integer id PK
        uuid operator_session_id FK
        varchar(50) ticket_number
        timestamptz assignment_time
        timestamptz service_start_time
        timestamptz service_end_time
        text result
    }
    
    operator_sessions ||--o{ client_sessions : "имеет много"
    operator_sessions ||--o| client_sessions : "текущий клиент"
```

## Описание таблиц

### `operator_sessions`
Хранит рабочие сессии операторов с назначенными услугами и текущим состоянием.

| Колонка | Тип | Описание |
|---------|-----|----------|
| `id` | `uuid` | Первичный ключ, автогенерируемый |
| `operator_id` | `varchar(100)` | Идентификатор оператора (бизнес-ключ) |
| `workplace_code` | `varchar(50)` | Код сервисного окна/рабочего места |
| `status` | `text` | Статус сессии: `Authorized`, `ReadyToWork`, `WaitingAssignment`, `WaitingClient`, `ServingClient`, `Closed` |
| `session_start_time` | `timestamptz` | Время начала работы оператора |
| `session_end_time` | `timestamptz` | Время окончания работы оператора |
| `current_client_session_id` | `integer` | FK к текущей активной сессии клиента (nullable) |
| `assigned_services` | `jsonb[]` | Массив услуг, которые может предоставить оператор |

### `client_sessions`
Отслеживает индивидуальные сессии обслуживания клиентов от назначения до завершения.

| Колонка | Тип | Описание |
|---------|-----|----------|
| `id` | `uuid` | Первичный ключ, автогенерируемый |
| `operator_session_id` | `uuid` | FK к сессии оператора |
| `ticket_number` | `varchar(50)` | Номер талона клиента в очереди |
| `assignment_time` | `timestamptz` | Время назначения клиента оператору |
| `service_start_time` | `timestamptz` | Время фактического начала обслуживания (nullable) |
| `service_end_time` | `timestamptz` | Время завершения обслуживания (nullable) |
| `result` | `text` | Результат: `ServiceCompleted` или `ClientNotCame` (nullable) |

## Связи

- **Один-ко-многим**: `operator_sessions` → `client_sessions`
  - Одна сессия оператора может иметь множество сессий клиентов в течение рабочего дня
  
- **Один-к-одному (опционально)**: `operator_sessions` → `client_sessions` (текущий)
  - Указывает на текущую активную сессию клиента (если есть)
  - Использует `ON DELETE SET NULL` для корректной очистки

## Типы данных и особенности

### Специфичные для PostgreSQL возможности
- **JSONB массивы**: `assigned_services` хранит информацию об услугах как структурированные данные
- **Timestamptz**: Все временные метки включают информацию о часовом поясе
- **GIN индекс**: Обеспечивает эффективный поиск внутри JSONB массивов
- **UUID**: Первичные ключи используют UUID для совместимости с распределенными системами

### Примеры данных

#### Сессия оператора
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "operator_id": "OP123",
  "workplace_code": "ОКНО_01", 
  "status": "ServingClient",
  "assigned_services": [
    {
      "ServiceCode": "PASSPORT",
      "ServiceName": "Паспортные услуги"
    },
    {
      "ServiceCode": "VISA",
      "ServiceName": "Визовые услуги"
    }
  ]
}
```

#### Сессия клиента
```json
{
  "id": "01989543-a8b9-7656-8370-79be6825fe27",
  "operator_session_id": "550e8400-e29b-41d4-a716-446655440000",
  "ticket_number": "A001",
  "assignment_time": "2025-08-10T10:30:00Z",
  "service_start_time": "2025-08-10T10:32:00Z", 
  "service_end_time": null,
  "result": null
}
```
