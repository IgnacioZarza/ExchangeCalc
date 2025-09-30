# ExchangeCalc# 
ExchangeCalc - Calculadora de Tipo de Cambio

Aplicación web desarrollada en **ASP.NET Core MVC** que consume la API pública de tipos de cambio [Frankfurter](https://frankfurter.dev/).  
Permite a los usuarios gestionar monedas favoritas, establecer una divisa principal, realizar conversiones en tiempo real y consultar el historial de tasas de cambio.

---

## Características principales

- **Gestión de divisas favoritas**: agregar, eliminar y persistir en base de datos.
- **Moneda principal** configurable como referencia para todas las conversiones.
- **Panel de control (Dashboard)** con conversión en tiempo real de la moneda principal frente a las favoritas.
- **Historial de tasas de cambio** mediante selección de fecha.
- **Conversor de montos** entre la divisa principal y favoritas.
- **Registro de peticiones HTTP** en base de datos mediante filtro personalizado.

---

## Requisitos

- **.NET 8.0 SDK** o superior
- **SQL Server** (local o remoto)
- **Visual Studio 2022** o **VS Code**
- Conexión a internet (para consumir la API Frankfurter)

---

## Estructura del proyecto

ExchangeCalc/
├── ExchangeCalc.Application/     # Servicios, interfaces y lógica de negocio
├── ExchangeCalc.Domain/          # Entidades, abstracciones y contratos
├── ExchangeCalc.Infrastructure/  # Persistencia, repositorios y servicios externos
├── ExchangeCalc.Controllers/     # Controladores MVC
├── ExchangeCalc.Filters/         # Filtros personalizados (logging)
├── ExchangeCalc.Views/           # Vistas Razor (UI con Bootstrap)
├── ExchangeCalcEFMigrations.sql  # Script con migraciones de base de datos
└── README.md                     # Este archivo

---

## Instalación y Configuración
# Clonar el repositorio
git clone https://github.com/IgnacioZarza/ExchangeCalc.git

# Configurar la base de datos
"ConnectionStrings": {
  "DefaultConnection": ""
}

---

# Aplicar migraciones
Ejecutar archivo script.sql o ExchangeCalcEFMigrations.sql

---

# Arquitectura

## El proyecto sigue una arquitectura por capas:

Domain
Contiene las entidades (CurrencyFavorite, UserSettings, ExchangeLog, FxRate) y contratos.
Incluye la clase abstracta ExchangeCalculatorBase, que define el contrato para cálculos de conversión.

Application
Servicios de aplicación (ExchangeService, IFxRatesApiClient) que orquestan la lógica de negocio.

Infrastructure
Implementa la persistencia (EF Core ApplicationDbContext), repositorios genéricos, patrón Unit of Work y el cliente HTTP para la API Frankfurter.

Controllers
Manejan las peticiones del usuario (ej. DashboardController) y coordinan servicios.

Filters
Implementa RequestLoggingFilter, que registra en la tabla ExchangeLogs cada petición al backend.

Views
Vistas en Razor con Bootstrap para diseño responsivo.

---

##  Bitácora de Peticiones

Cada request se registra en la tabla ExchangeLogs con:

Fecha/Hora (Timestamp)

Ruta solicitada (Route)

Método HTTP (HttpMethod)

QueryString

Cuerpo de la petición (Body)

IP del usuario (UserIp)

---

## Cómo extender el proyecto

Implementar más proveedores de API de tipos de cambio creando nuevas clases que hereden de ExchangeCalculatorBase.

Integrar autenticación y autorización para que cada usuario tenga sus propias preferencias.

Mejorar el UI/UX con frameworks modernos (ej. MaterializeCSS).

Exponer una API REST propia para que clientes externos consulten conversiones.
