# 🏍️ Concesionario Hakuna Mamoto

![.NET Framework](https://img.shields.io/badge/.NET_Framework-4.8-512BD4?style=for-the-badge&logo=dotnet)
![WPF](https://img.shields.io/badge/UI-WPF-blue?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/Database-SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![Crystal Reports](https://img.shields.io/badge/Reports-Crystal_Reports-f39c12?style=for-the-badge)

**Hakuna Mamoto** es una aplicación de escritorio integral para la gestión completa de un concesionario de motocicletas. Desarrollada con arquitectura en capas (MVC), garantiza un rendimiento óptimo, código mantenible y una interfaz de usuario moderna.

---

## ✨ Características Principales

El sistema se divide en 5 grandes módulos, accesibles desde un panel de control centralizado:

* **👥 Gestión de Clientes:** Base de datos de compradores con validación estricta de DNI, teléfono y formato de email.
* **🏍️ Catálogo de Stock:** Control del inventario de motocicletas. Los estados (`Disponible`, `Reservada`, `Taller`) se actualizan automáticamente según la lógica de negocio.
* **🔧 Taller y Mantenimientos:** Asignación de reparaciones a mecánicos, control de fechas (entrada/salida) y registro de costes.
* **📅 Reservas y Ventas:** Sistema de bloqueo de stock mediante señales económicas. Filtrado inteligente para mostrar únicamente vehículos disponibles.
* **🖨️ Módulo de Reporting:** Generación de documentos PDF y listados para impresión utilizando *SAP Crystal Reports* y DataSets desconectados.

---

## 🏗️ Arquitectura del Sistema

El proyecto sigue una arquitectura estricta para separar la interfaz de la base de datos:

1.  **View (WPF):** Interfaces limpias en XAML. No contienen lógica de negocio.
2.  **Controller (API):** Clases (`ClienteApi`, `TallerApi`, etc.) encargadas de las reglas de negocio y validaciones de datos antes de tocar la base de datos.
3.  **Model (Repo):** Implementación del *Patrón Repositorio* para encapsular todas las consultas a la base de datos utilizando **Entity Framework 6**.

---

## 🚀 Instalación y Despliegue

### Requisitos Previos
* Visual Studio 2019 / 2022.
* .NET Framework 4.8.
* SQL Server (LocalDB o Express).
* *SAP Crystal Reports para Visual Studio* (Runtime instalado).

### Pasos para ejecutar:

1.  **Clonar el repositorio:**
    ```bash
    git clone [https://github.com/tu-usuario/HakunaMamoto.git](https://github.com/tu-usuario/HakunaMamoto.git)
    ```
2.  **Base de Datos:**
    * Abre SQL Server Management Studio.
    * Ejecuta el script `script_basededatos.sql` (o restaura el archivo `.bak`) para crear la base de datos `ConcesionarioBD`.
3.  **Configurar Conexión:**
    * Abre el proyecto en Visual Studio.
    * Ve al archivo `App.config` de tu proyecto principal y asegúrate de que la cadena de conexión (`connectionStrings`) apunta a tu instancia local de SQL Server:
    ```xml
    <add name="ConcesionarioBDEntities1" connectionString="metadata=...;provider connection string=&quot;data source=LOCALHOST\SQLEXPRESS;initial catalog=ConcesionarioBD;integrated security=True;...&quot;" providerName="System.Data.EntityClient" />
    ```
4.  **Compilar y Ejecutar:**
    * Haz clic en *Iniciar* (o presiona `F5`) en Visual Studio.

---

## 🧪 Testing

El proyecto incluye un módulo de pruebas (`Concesionario.Tests`) que valida la lógica de negocio (Testing Unitario) y el correcto funcionamiento del ORM y la base de datos (Testing de Integración). 

Para ejecutarlos:
* Ve a **Prueba > Explorador de pruebas** en Visual Studio.
* Haz clic en **Ejecutar todas las pruebas**.

---

**Autor:** Mauro Valdes Sanjuan
