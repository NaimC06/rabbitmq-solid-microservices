# RabbitMQ SOLID Microservices - Gestor de Alertas de Seguridad

Proyecto académico con un productor y dos consumidores usando RabbitMQ como broker de mensajes. El código aplica principios SOLID, interfaces y buenas prácticas de versionamiento con Git/GitHub.

## Tecnologías

- C# / .NET 8
- RabbitMQ.Client 7.x
- RabbitMQ 4 Management en Docker
- Git y GitHub

## Arquitectura

```text
ProducerService
   │ publica mensajes
   ▼
RabbitMQ Broker
   ├── Exchange Direct: security.direct
   ├── Exchange Topic: security.topic
   └── Exchange Fanout: security.fanout
        │
        ├── security-team.queue  → SecurityConsumerService
        └── audit.queue          → AuditConsumerService
```

## Caso de uso

Sistema de alertas de seguridad:

- `ProducerService`: publica alertas.
- `SecurityConsumerService`: procesa alertas para el equipo de seguridad.
- `AuditConsumerService`: registra eventos para auditoría.

## Exchanges usados

| Tipo | Exchange | Uso |
|---|---|---|
| Direct | `security.direct` | Envía mensajes con routing key exacta, por ejemplo `alert.critical`. |
| Topic | `security.topic` | Envía mensajes por patrones, por ejemplo `security.login.*` y `security.#`. |
| Fanout | `security.fanout` | Envía el mensaje a todas las colas enlazadas. |

## Cómo ejecutar

### 1. Levantar RabbitMQ

Desde la carpeta raíz del proyecto:

```bash
docker compose up -d
```

Abre RabbitMQ Management en el navegador:

```text
http://localhost:15672
```

Usuario:

```text
guest
```

Contraseña:

```text
guest
```

### 2. Restaurar dependencias

```bash
cd src
dotnet restore
```

### 3. Ejecutar los consumidores

Abre una terminal para el consumidor de seguridad:

```bash
cd src
dotnet run --project SecurityConsumerService
```

Abre otra terminal para el consumidor de auditoría:

```bash
cd src
dotnet run --project AuditConsumerService
```

### 4. Ejecutar el productor

Abre otra terminal:

```bash
cd src
dotnet run --project ProducerService
```

Luego elige opciones del menú:

- `1`: publica en Direct.
- `2`: publica en Topic.
- `3`: publica en Topic.
- `4`: publica en Fanout.

## Evidencia para capturas

En RabbitMQ Management toma capturas de:

1. Overview del broker activo.
2. Exchanges: `security.direct`, `security.topic`, `security.fanout`.
3. Queues: `security-team.queue`, `audit.queue`.
4. Bindings de cada cola.
5. Terminales con el productor enviando mensajes.
6. Terminales con consumidores recibiendo mensajes.
7. Historial de commits en GitHub.
8. Ramas creadas en GitHub.

## SOLID aplicado

### S - Single Responsibility Principle

Cada clase tiene una responsabilidad concreta:

- `JsonMessageSerializer`: serializa y deserializa JSON.
- `RabbitMqConnectionProvider`: crea y administra la conexión.
- `RabbitMqTopologyInitializer`: crea exchanges, colas y bindings.
- `RabbitMqMessagePublisher`: publica mensajes.
- `RabbitMqMessageConsumer`: consume mensajes.
- `SecurityAlertHandler` y `AuditAlertHandler`: procesan mensajes según su microservicio.

### O - Open/Closed Principle

Se puede agregar otro consumidor creando otro `IMessageHandler<T>` sin modificar el consumidor base `RabbitMqMessageConsumer`.

### L - Liskov Substitution Principle

Cualquier implementación de `IMessageHandler<SecurityAlert>` puede usarse donde el consumidor espera un handler.

### I - Interface Segregation Principle

Las interfaces son pequeñas y específicas:

- `IMessagePublisher`
- `IMessageConsumer`
- `IMessageSerializer`
- `IMessageHandler<T>`
- `IRabbitMqConnectionProvider`
- `IRabbitMqTopologyInitializer`

Ninguna clase está obligada a implementar métodos que no usa.

### D - Dependency Inversion Principle

Las clases dependen de abstracciones, no de implementaciones concretas. Por ejemplo, `RabbitMqMessagePublisher` depende de `IRabbitMqConnectionProvider` e `IMessageSerializer`.

## Versionamiento recomendado

Ramas sugeridas:

```text
main
feature/docker-rabbitmq
feature/shared-kernel
feature/producer-service
feature/security-consumer
feature/audit-consumer
docs/readme-and-report
```

Commits sugeridos:

```text
chore: initialize repository structure
chore: add docker compose for rabbitmq
feat(shared): add message contracts and interfaces
feat(shared): add rabbitmq connection and topology services
feat(producer): publish messages to direct topic and fanout exchanges
feat(consumer): add security consumer service
feat(consumer): add audit consumer service
docs: add execution guide and solid explanation
```

## Limpieza

Para detener RabbitMQ:

```bash
docker compose down
```

Para detenerlo y borrar datos:

```bash
docker compose down -v
```
