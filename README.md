# 🏦 Banco Prometeo — Sistema de Banca Digital

<div align="center">

![.NET](https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Angular](https://img.shields.io/badge/Angular_17-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server_2022-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)

![Status](https://img.shields.io/badge/Estado-En_Desarrollo-yellow?style=flat-square)
![Version](https://img.shields.io/badge/Versión-1.0.0-blue?style=flat-square)
![License](https://img.shields.io/badge/Licencia-MIT-green?style=flat-square)

**Plataforma de banca digital completa con Clean Architecture, CQRS y Angular Material.**  
Cuentas · Transferencias · Préstamos · Tarjetas · Pagos · Inversiones

</div>

---

## 📋 Tabla de Contenidos

- [Descripción](#-descripción)
- [Stack Tecnológico](#-stack-tecnológico)
- [Arquitectura](#-arquitectura)
- [Módulos del Sistema](#-módulos-del-sistema)
- [Roles de Usuario](#-roles-de-usuario)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Base de Datos](#-base-de-datos)
- [API Reference](#-api-reference)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Variables de Entorno](#-variables-de-entorno)
- [Ejecutar el Proyecto](#-ejecutar-el-proyecto)
- [Contribuir](#-contribuir)

---

## 📖 Descripción

**Banco Prometeo** es una solución de banca digital de nueva generación que centraliza los servicios financieros esenciales en una sola plataforma. Está diseñada para cuatro perfiles de usuario — cliente, administrador, cajero y auditoría — cada uno con un conjunto granular de permisos.

El backend expone una **REST API en ASP.NET Core 8** construida sobre **Clean Architecture + CQRS**, con toda la lógica transaccional implementada en **Stored Procedures de SQL Server** para garantizar atomicidad e integridad en cada operación financiera. El frontend es una **SPA en Angular 17 con Angular Material** y gestión de estado con NgRx.

---

## 🛠 Stack Tecnológico

| Capa | Tecnología |
|------|-----------|
| **Frontend** | Angular 17 · Angular Material · NgRx · TypeScript |
| **API** | ASP.NET Core 8 Web API · JWT Bearer · Swagger/OpenAPI |
| **Arquitectura** | Clean Architecture · CQRS · MediatR |
| **Validación** | FluentValidation |
| **Mapeo** | AutoMapper |
| **ORM / Data** | Entity Framework Core 8 · Dapper |
| **Base de datos** | SQL Server 2022 |
| **Autenticación** | ASP.NET Identity · JWT |
| **Logging** | Serilog |
| **Resiliencia** | Polly |
| **Documentación** | Swagger / OpenAPI |
| **Tests** | xUnit · Moq |

---

## 🏛 Arquitectura

El sistema sigue **Clean Architecture** con la regla de dependencia estricta: el Dominio no conoce ninguna capa externa.

```
┌─────────────────────────────────────────────┐
│           Angular 17 + NgRx                 │  ← Frontend SPA
└────────────────────┬────────────────────────┘
                     │ HTTPS · JWT
┌────────────────────▼────────────────────────┐
│         ASP.NET Core 8 Web API               │  ← Presentación
│   Controllers · Middlewares · Swagger        │
└────────────────────┬────────────────────────┘
                     │ MediatR
┌────────────────────▼────────────────────────┐
│          Application Layer                   │  ← Casos de uso
│   Commands · Queries · Validators · DTOs     │
└────────────────────┬────────────────────────┘
                     │ Interfaces
┌────────────────────▼────────────────────────┐
│            Domain Layer                      │  ← Núcleo del negocio
│   Entities · Value Objects · Enums           │
└────────────────────┬────────────────────────┘
                     │ Implementaciones
┌────────────────────▼────────────────────────┐
│         Infrastructure Layer                 │  ← Infraestructura
│   EF Core · Dapper · SPs · Identity          │
└────────────────────┬────────────────────────┘
                     │
┌────────────────────▼────────────────────────┐
│         SQL Server 2022                      │  ← Persistencia
│  4 schemas · 21 tablas · 15 SPs · 5 vistas  │
└─────────────────────────────────────────────┘
```

### Schemas de la base de datos

| Schema | Contenido |
|--------|-----------|
| `Core` | Entidades principales: cuentas, transacciones, préstamos, tarjetas, inversiones |
| `Sec` | Seguridad: usuarios, roles, asignaciones |
| `Audit` | Logs de auditoría, intentos de login, snapshots de saldo |
| `Cat` | Catálogos: tipos de cuenta, monedas, proveedores de servicios, productos de inversión |

---

## 📦 Módulos del Sistema

| Módulo | Descripción | Stored Procedures | Vistas |
|--------|-------------|-------------------|--------|
| **Cuentas & Saldos** | Apertura, bloqueo y cierre de cuentas corrientes y de ahorro | `sp_OpenAccount` · `sp_SetAccountStatus` | `vw_CustomerAccounts` |
| **Transacciones** | Depósitos y retiros en ventanilla o banca en línea | `sp_Deposit` · `sp_Withdraw` | `vw_TransactionDetail` |
| **Transferencias** | Internas y SINPE Móvil con reversión administrativa | `sp_ExecuteTransfer` · `sp_ReverseTransfer` | — |
| **Préstamos** | Solicitud, aprobación, amortización y pago de cuotas | `sp_ApproveLoan` · `sp_PayLoanInstallment` | `vw_LoanPortfolio` |
| **Tarjetas de Crédito** | Emisión, estado de cuenta mensual y pagos | `sp_PayCreditCard` | — |
| **Pagos de Servicios** | Catálogo de proveedores: AyA, ICE, RACSA, CCSS y más | `sp_PayService` | — |
| **Inversiones** | Depósitos a plazo y fondos con simulador de rendimiento | `sp_CreateInvestment` | `vw_InvestmentPortfolio` |
| **Reportes** | Dashboard administrativo y cierre diario | `sp_DailyClosing` | `vw_AdminDashboard` |
| **Auditoría** | Trazabilidad completa de cada operación | `sp_GetEntityAuditTrail` | — |

---

## 👥 Roles de Usuario

| Rol | Descripción | Acceso |
|-----|-------------|--------|
| `Cliente` | Usuario final de banca en línea | Sus propias cuentas, transferencias e inversiones |
| `Admin` | Gestor con acceso total | Crear cuentas, aprobar préstamos, emitir tarjetas, reportes |
| `Cajero` | Operador de ventanilla | Depósitos, retiros, transferencias, cierre diario |
| `Auditoria` | Revisor de solo lectura | Logs de auditoría y reportes históricos |

### Matriz de permisos rápida

| Acción | Cliente | Admin | Cajero | Auditoría |
|--------|:-------:|:-----:|:------:|:---------:|
| Ver propias cuentas | ✅ | ✅ | ✅ | ✅ |
| Crear / cerrar cuenta | — | ✅ | ✅ | — |
| Depósito / retiro | — | ✅ | ✅ | — |
| Transferencia | ✅ | ✅ | ✅ | — |
| Reversar transferencia | — | ✅ | — | — |
| Solicitar préstamo | ✅ | — | — | — |
| Aprobar préstamo | — | ✅ | — | — |
| Pagar tarjeta | ✅ | ✅ | ✅ | — |
| Crear inversión | ✅ | — | — | — |
| Dashboard / reportes | — | ✅ | — | ✅ |
| Registro de auditoría | — | — | — | ✅ |

---

## 📁 Estructura del Proyecto

```
BancoPrometeo/
│
├── src/
│   ├── BancoPrometeo.API/                  # Controladores, middlewares, Program.cs
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── AccountsController.cs
│   │   │   ├── TransactionsController.cs
│   │   │   ├── TransfersController.cs
│   │   │   ├── LoansController.cs
│   │   │   ├── CreditCardsController.cs
│   │   │   ├── ServicePaymentsController.cs
│   │   │   ├── InvestmentsController.cs
│   │   │   ├── ReportsController.cs
│   │   │   └── AuditController.cs
│   │   ├── Middlewares/
│   │   │   ├── ExceptionMiddleware.cs      # Mapea errores SQL 500xx → HTTP
│   │   │   └── RequestLoggingMiddleware.cs
│   │   └── Program.cs
│   │
│   ├── BancoPrometeo.Application/          # CQRS: Commands, Queries, Handlers
│   │   ├── Common/
│   │   │   ├── Interfaces/                 # IAccountRepository, IUnitOfWork, etc.
│   │   │   ├── Behaviors/                  # ValidationBehavior, LoggingBehavior
│   │   │   └── Exceptions/
│   │   └── Features/
│   │       ├── Accounts/
│   │       ├── Transactions/
│   │       ├── Transfers/
│   │       ├── Loans/
│   │       ├── CreditCards/
│   │       ├── ServicePayments/
│   │       ├── Investments/
│   │       └── Reports/
│   │
│   ├── BancoPrometeo.Domain/               # Entidades, enums, value objects
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── ValueObjects/
│   │
│   └── BancoPrometeo.Infrastructure/       # EF Core, Dapper, Identity, servicios externos
│       ├── Persistence/
│       │   ├── AppDbContext.cs
│       │   ├── Configurations/
│       │   └── Repositories/
│       ├── Identity/
│       │   ├── ApplicationUser.cs
│       │   └── JwtTokenService.cs
│       └── ExternalServices/
│           ├── SINPEService.cs
│           └── EmailService.cs
│
├── database/
│   └── BancoPrometeo_v5.sql               # Script completo: schemas, tablas, SPs, vistas
│
├── frontend/
│   └── banco-prometeo-app/                # Aplicación Angular 17
│       └── src/app/
│           ├── core/                      # Auth, guards, interceptors, servicios HTTP
│           ├── shared/                    # Componentes reutilizables, pipes, directivas
│           ├── features/                  # Módulos con lazy loading
│           │   ├── dashboard/
│           │   ├── accounts/
│           │   ├── transfers/
│           │   ├── loans/
│           │   ├── credit-cards/
│           │   ├── service-payments/
│           │   └── investments/
│           └── store/                     # NgRx: actions, reducers, effects, selectors
│
└── tests/
    ├── BancoPrometeo.UnitTests/
    └── BancoPrometeo.IntegrationTests/
```

---

## 🗄 Base de Datos

La base de datos contiene **toda la lógica transaccional** en Stored Procedures. El API simplemente pasa parámetros y recibe resultados.

### Objetos en la DB

| Tipo | Cantidad | Detalle |
|------|----------|---------|
| Schemas | 4 | `Core` · `Sec` · `Audit` · `Cat` |
| Tablas | 21 | Incluyendo catálogos y auditoría |
| Stored Procedures | 15 | Toda la lógica de negocio transaccional |
| Funciones | 5 | Cálculo de cuotas, intereses, saldo disponible, amortización |
| Vistas | 5 | Consultas preconstruidas para el API |
| Triggers | 5 | Inmutabilidad de transacciones + auditoría automática |
| Índices | 23 | Optimizados por patrón de consulta |

### Tablas principales

```
Sec.Users                   → Identidad del usuario (compatible con ASP.NET Identity)
Sec.Roles                   → Admin · Cliente · Cajero · Auditoria
Core.Customers              → Datos personales del cliente
Core.Accounts               → Cuentas corrientes y de ahorro
Core.Transactions           → Inmutables por trigger. Cada movimiento queda registrado.
Core.Transfers              → Transferencias internas y SINPE
Core.Loans                  → Préstamos con estado y seguimiento de mora
Core.LoanInstallments       → Cuotas generadas automáticamente al aprobar un préstamo
Core.CreditCards            → Tarjetas emitidas con límite y saldo
Core.CreditCardTransactions → Consumos y pagos de tarjeta
Core.ServicePayments        → Historial de pagos de servicios públicos
Core.Investments            → Depósitos a plazo y fondos activos
Cat.ServiceProviders        → Catálogo: AyA, ICE, RACSA, CCSS, etc.
Cat.InvestmentProducts      → Productos con tasa y plazo
Audit.AuditLogs             → Registro de INSERT/UPDATE/DELETE en entidades clave
```

> **Nota importante:** Las transacciones (`Core.Transactions`) son **inmutables por diseño**. Un trigger de SQL Server impide eliminarlas o modificar sus campos financieros. Para corregir una operación se usa reversión, nunca edición directa.

---

## 🔌 API Reference

La API sigue el patrón: **el SP hace el trabajo, el endpoint pasa los parámetros**.

### Autenticación

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@bancoprometeo.cr",
  "password": "contraseña"
}
```

Respuesta:
```json
{
  "token": "eyJhbGci...",
  "refreshToken": "...",
  "fullName": "Juan Pérez",
  "roles": ["Cliente"],
  "expiresAt": "2026-03-29T08:00:00Z"
}
```

Todos los endpoints protegidos requieren el header:
```
Authorization: Bearer {token}
```

---

### Endpoints principales

#### Cuentas

```http
POST   /api/accounts                    # Abrir cuenta (Admin, Cajero)
GET    /api/accounts                    # Listar todas (Admin, Cajero)
GET    /api/accounts/my-accounts        # Mis cuentas (Cliente)
GET    /api/accounts/{id}               # Detalle de cuenta
PATCH  /api/accounts/{id}/status        # Bloquear / cerrar (Admin)
```

#### Transacciones

```http
POST   /api/transactions/deposit        # Depósito (Admin, Cajero)
POST   /api/transactions/withdraw       # Retiro (Admin, Cajero)
GET    /api/transactions                # Historial paginado
```

#### Transferencias

```http
POST   /api/transfers                   # Ejecutar transferencia interna o SINPE
POST   /api/transfers/{id}/reverse      # Reversar (Admin)
GET    /api/transfers                   # Historial de transferencias
```

> Si `targetAccountId` viene `null` en el body, la transferencia se procesa como **SINPE** automáticamente.

#### Préstamos

```http
POST   /api/loans                       # Solicitar préstamo (Cliente)
POST   /api/loans/{id}/approve          # Aprobar y desembolsar (Admin)
POST   /api/loans/{id}/pay              # Pagar cuota
GET    /api/loans                       # Portafolio completo (Admin)
GET    /api/loans/my-loans              # Mis préstamos (Cliente)
GET    /api/loans/{id}/amortization     # Tabla de amortización
GET    /api/loans/simulate              # Simulador de cuota mensual
```

#### Tarjetas de Crédito

```http
POST   /api/credit-cards/{id}/pay       # Pagar tarjeta
GET    /api/credit-cards/my-cards       # Mis tarjetas (Cliente)
GET    /api/credit-cards/{id}/statement # Estado de cuenta mensual
```

#### Pagos de Servicios

```http
POST   /api/service-payments            # Pagar servicio
GET    /api/service-payments            # Historial de pagos
GET    /api/catalogs/service-providers  # Catálogo de proveedores
```

#### Inversiones

```http
POST   /api/investments                 # Crear inversión (Cliente)
GET    /api/investments/my-investments  # Mis inversiones (Cliente)
GET    /api/investments/simulate        # Simulador de rendimiento
GET    /api/catalogs/investment-products # Productos disponibles
```

#### Reportes y Auditoría

```http
GET    /api/dashboard/admin             # KPIs en tiempo real (Admin)
POST   /api/reports/daily-closing       # Cierre diario (Admin, Cajero)
GET    /api/audit/{tableName}/{recordId} # Trazabilidad de un registro (Auditoria, Admin)
```

### Manejo de errores

Los Stored Procedures lanzan errores con números entre `50001` y `50098`. El middleware `ExceptionMiddleware` los intercepta y devuelve la respuesta HTTP correspondiente:

```json
{
  "statusCode": 422,
  "message": "Saldo insuficiente en cuenta origen.",
  "errorCode": 50034,
  "timestamp": "2026-03-28T14:32:00Z"
}
```

| Rango SQL | HTTP | Causa |
|-----------|------|-------|
| `50001–50009` | `404 Not Found` | Entidad no encontrada |
| `50010–50029` | `422 Unprocessable` | Validación de depósito / retiro |
| `50030–50039` | `422 Unprocessable` | Validación de transferencia |
| `50040–50059` | `422 Unprocessable` | Validación de préstamos |
| `50060–50079` | `422 Unprocessable` | Validación de tarjeta / servicio |
| `50080–50099` | `400 / 409` | Inversión / reversión / cierre duplicado |

---

## ⚙️ Instalación y Configuración

### Prerrequisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20 LTS](https://nodejs.org/) + Angular CLI 17
- [SQL Server 2022](https://www.microsoft.com/sql-server) (o SQL Server Express)
- [Git](https://git-scm.com/)

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/banco-prometeo.git
cd banco-prometeo
```

### 2. Crear la base de datos

Abre SQL Server Management Studio o `sqlcmd` y ejecuta:

```bash
sqlcmd -S localhost -E -i database/BancoPrometeo_v5.sql
```

Esto crea la base de datos completa con todos los schemas, tablas, SPs, funciones, triggers, vistas y los datos iniciales de catálogos.

### 3. Configurar el backend

```bash
cd src/BancoPrometeo.API
cp appsettings.Example.json appsettings.Development.json
# Edita appsettings.Development.json con tu cadena de conexión y clave JWT
```

Restaurar paquetes y compilar:

```bash
cd ../../
dotnet restore
dotnet build
```

### 4. Configurar el frontend

```bash
cd frontend/banco-prometeo-app
npm install
cp src/environments/environment.example.ts src/environments/environment.ts
# Edita environment.ts con la URL del API
```

---

## 🔐 Variables de Entorno

### Backend — `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BancoPrometeo;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "TU_CLAVE_SECRETA_MINIMO_32_CARACTERES_AQUI",
    "Issuer": "BancoPrometeo.API",
    "Audience": "BancoPrometeo.Client",
    "ExpirationHours": 8,
    "RefreshTokenExpirationDays": 7
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 30
        }
      }
    ]
  },
  "CorsSettings": {
    "AllowedOrigins": ["http://localhost:4200"]
  },
  "TransferLimits": {
    "MaxAmountPerTransfer": 15000000,
    "MaxDailyAmount": 50000000
  }
}
```

> ⚠️ **Nunca subas `appsettings.Development.json` al repositorio.** Ya está en `.gitignore`.

### Frontend — `environment.ts`

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

---

## 🚀 Ejecutar el Proyecto

### Backend

```bash
cd src/BancoPrometeo.API
dotnet run
```

El API queda disponible en:
- `https://localhost:5001` — HTTPS
- `http://localhost:5000` — HTTP
- `https://localhost:5001/swagger` — Documentación interactiva

### Frontend

```bash
cd frontend/banco-prometeo-app
ng serve
```

La app Angular queda disponible en `http://localhost:4200`.

### Credenciales iniciales

La base de datos crea automáticamente un usuario administrador:

```
Email:    admin@bancoprometeo.cr
Password: (configura en appsettings o mediante el endpoint de cambio de contraseña)
Rol:      Admin
```

---

## 🧪 Tests

```bash
# Tests unitarios
dotnet test tests/BancoPrometeo.UnitTests/

# Tests de integración (requiere DB de prueba configurada)
dotnet test tests/BancoPrometeo.IntegrationTests/

# Todos los tests con reporte de cobertura
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📦 Paquetes principales

### Backend

| Paquete | Versión | Uso |
|---------|---------|-----|
| `MediatR` | 12.x | Pipeline CQRS |
| `FluentValidation` | 11.x | Validación de Commands |
| `AutoMapper` | 13.x | Mapeo entre capas |
| `Microsoft.EntityFrameworkCore.SqlServer` | 8.x | ORM para vistas y catálogos |
| `Dapper` | 2.x | Ejecución de Stored Procedures |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 8.x | Autenticación JWT |
| `Serilog.AspNetCore` | 8.x | Logging estructurado |
| `Polly` | 8.x | Resiliencia y reintentos |
| `Swashbuckle.AspNetCore` | 6.x | Swagger / OpenAPI |

### Frontend

| Paquete | Versión | Uso |
|---------|---------|-----|
| `@angular/material` | 17.x | Componentes UI |
| `@ngrx/store` | 17.x | State management |
| `@ngrx/effects` | 17.x | Side effects |
| `@ngrx/entity` | 17.x | Colecciones de entidades |
| `chart.js` + `ng2-charts` | 4.x / 5.x | Gráficos del dashboard |
| `ngx-currency` | 3.x | Formateo de montos |
| `date-fns` | 3.x | Manejo de fechas |

---

## 🤝 Contribuir

1. Haz fork del repositorio
2. Crea una rama para tu feature: `git checkout -b feature/nombre-del-feature`
3. Haz commit de tus cambios: `git commit -m 'feat: descripción del cambio'`
4. Haz push a tu rama: `git push origin feature/nombre-del-feature`
5. Abre un Pull Request

### Convención de commits

```
feat:     nueva funcionalidad
fix:      corrección de bug
docs:     cambios en documentación
refactor: refactorización sin cambio funcional
test:     agregar o corregir tests
chore:    tareas de mantenimiento
```

---

## 📄 Licencia

Este proyecto está bajo la licencia MIT. Ver el archivo [LICENSE](LICENSE) para más detalles.

---

<div align="center">

Desarrollado con ❤️ para **Banco Prometeo**

</div>
