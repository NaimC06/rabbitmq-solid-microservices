# Informe del Proyecto: Productor y Consumidores con RabbitMQ

## 1. Portada

- Nombre del proyecto: RabbitMQ SOLID Microservices - Gestor de Alertas de Seguridad
- Integrantes:
- Materia:
- Docente:
- Fecha:

## 2. Introducción

Este proyecto implementa una arquitectura basada en microservicios usando RabbitMQ como broker de mensajes. Se desarrolló un productor encargado de publicar alertas de seguridad y dos consumidores encargados de procesar mensajes desde colas diferentes.

## 3. Objetivo general

Implementar un sistema distribuido con productor y consumidores de mensajes usando RabbitMQ, aplicando principios SOLID, buenas prácticas de programación, control de versiones con Git y publicación en un repositorio público de GitHub.

## 4. Objetivos específicos

- Configurar RabbitMQ usando Docker.
- Crear un productor de mensajes.
- Crear dos consumidores independientes.
- Usar exchanges Direct, Topic y Fanout.
- Aplicar interfaces y separación de responsabilidades.
- Documentar el uso de ramas y commits.
- Publicar el código en GitHub.

## 5. Herramientas utilizadas

- Windows
- PowerShell o CMD
- Docker Desktop
- RabbitMQ Management
- .NET SDK
- Visual Studio Code
- Git
- GitHub

## 6. Arquitectura

Insertar captura o diagrama:

ProducerService → RabbitMQ → SecurityConsumerService / AuditConsumerService

## 7. Explicación de RabbitMQ

RabbitMQ funciona como intermediario entre aplicaciones. El productor no envía mensajes directamente a los consumidores, sino a exchanges. Los exchanges enrutan los mensajes hacia colas según el tipo de exchange y el routing key.

## 8. Exchanges implementados

### Direct

Exchange: `security.direct`  
Uso: enviar alertas críticas usando routing key exacta `alert.critical`.

### Topic

Exchange: `security.topic`  
Uso: enviar eventos con patrones como `security.login.*` o `security.#`.

### Fanout

Exchange: `security.fanout`  
Uso: enviar comunicados generales a todas las colas enlazadas.

## 9. Colas implementadas

- `security-team.queue`: cola del equipo de seguridad.
- `audit.queue`: cola de auditoría.

## 10. Aplicación de principios SOLID

### Single Responsibility Principle

Cada clase tiene una sola responsabilidad. Ejemplo: `JsonMessageSerializer` solo maneja JSON y `RabbitMqMessagePublisher` solo publica mensajes.

### Open/Closed Principle

El sistema puede extenderse agregando nuevos handlers sin modificar la lógica base del consumidor.

### Liskov Substitution Principle

Cualquier clase que implemente `IMessageHandler<SecurityAlert>` puede sustituir a otra implementación sin romper el programa.

### Interface Segregation Principle

Las interfaces son pequeñas y específicas, como `IMessagePublisher`, `IMessageConsumer` e `IMessageSerializer`.

### Dependency Inversion Principle

Las clases principales dependen de interfaces, no de clases concretas.

## 11. Buenas prácticas aplicadas

- Separación por microservicios.
- Proyecto compartido para modelos e interfaces.
- Nombres claros de clases, colas y exchanges.
- Mensajes persistentes.
- Colas durables.
- Confirmación manual de mensajes con ACK.
- Manejo de errores con NACK.
- Uso de `.gitignore`.
- Uso de ramas feature.
- Commits descriptivos.

## 12. Evidencias

Pegar capturas de:

1. Docker con RabbitMQ activo.
2. RabbitMQ Management en navegador.
3. Exchanges creados.
4. Colas creadas.
5. Bindings.
6. Productor enviando mensajes.
7. Consumidores recibiendo mensajes.
8. Repositorio público en GitHub.
9. Historial de commits.
10. Ramas usadas.

## 13. Conclusiones

El proyecto permitió implementar una comunicación desacoplada entre microservicios usando RabbitMQ. Además, la aplicación de principios SOLID permitió organizar el código en clases con responsabilidades claras, facilitando el mantenimiento y la extensión del sistema.

## 14. Enlace al repositorio público

Pegar aquí el enlace de GitHub:

