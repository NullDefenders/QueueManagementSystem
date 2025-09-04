# Operator Interface Service

## Use Cases

Система поддерживает следующие сценарии использования:

1. **UC-01**: Авторизация оператора
2. **UC-02**: Начать работу в интерфейсе
3. **UC-03**: Запросить клиента для обслуживания
4. **UC-04**: Начать обслуживание клиента
5. **UC-05**: Завершить обслуживание клиента
6. **UC-06**: Закрыть интерфейс

## Документация

- [📋 Use Case диаграмма](docs/use-cases.md)
- [📊 Схема базы данных](docs/database.md)
- [🏗️ Архитектура системы](docs/architecture.md)

## Построение проекта
- [](docs/build.md)

## Запуск из Docker Compose

Postgres доступен Server=postgres; Port=5432; User Id=postgres;Password=postgres; Database=operator_interface
Мок для Сервис авторизации доступен по http://localhost:5001/

Blazor UI доступен по адресу http://localhost:8083/ (health - http://localhost:8083/health)
API доступен по адресу http://localhost:8081/ (health - http://localhost:8081/health)



