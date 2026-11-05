# Especificación de Requisitos de Software (SRS)
**Basado en IEEE Std 830-1998**

| Campo | Valor |
| :--- | :--- |
| **Nombre del Proyecto** | Gestor de Tareas y Notas Offline con API Local (GTN-API) |
| **Versión** | v1.0 — Borrador |
| **Fecha** | 2026-11-05 |
| **Preparado por** | Santiago |
| **Estado** | Borrador |

---

## 1. Introducción

### 1.1 Propósito
Este documento especifica los requisitos funcionales y no funcionales del **Gestor de Tareas y Notas Offline con API Local (GTN-API)**. Está dirigido al desarrollador (tú) y a cualquier colaborador o evaluador del proyecto. Su objetivo es servir como guía única para el diseño, implementación y validación del sistema.

### 1.2 Alcance
**Nombre del sistema:** GTN-API.

El sistema es una aplicación de escritorio para Windows que permite **gestionar notas rápidas y tareas organizadas por proyecto** de forma completamente local. Su valor principal es que **no requiere conexión a internet** para funcionar y que expone una **API local** (en `localhost`) para que otros programas (scripts, herramientas de línea de comandos, etc.) puedan consultar y alimentar la información de forma automatizada.

**Delimitación (In / Out of Scope):**

| Área | En Alcance | Fuera de Alcance |
| :--- | :--- | :--- |
| Gestión de Datos | Creación, edición, eliminación y consulta de notas y tareas. | Sincronización con servicios en la nube (Google Keep, Notion, etc.). |
| Organización | Agrupación de elementos por proyecto. | Gestión de usuarios múltiples o roles de acceso. |
| Interfaz de Usuario | Interfaz gráfica nativa para Windows (GUI). | Versión web o móvil. |
| Comunicación Externa | API HTTP local para integración con otros programas. | API pública en internet. |
| Conectividad | Funcionamiento 100% offline. | Cualquier funcionalidad que dependa de una conexión a internet. |

### 1.3 Definiciones, Acrónimos y Abreviaturas

| Abreviatura | Definición |
| :--- | :--- |
| **SRS** | Software Requirements Specification (Especificación de Requisitos de Software). |
| **API** | Application Programming Interface (Interfaz de Programación de Aplicaciones). |
| **GUI** | Graphical User Interface (Interfaz Gráfica de Usuario). |
| **HTTP** | HyperText Transfer Protocol. |
| **JSON** | JavaScript Object Notation (formato ligero de intercambio de datos). |
| **SQLite** | Biblioteca de software que implementa un motor de base de datos SQL relacional, ligero y autónomo. |
| **CRUD** | Create, Read, Update, Delete (Crear, Leer, Actualizar, Borrar). |
| **Localhost** | Dirección de red que identifica al equipo local (127.0.0.1). |
| **Offline** | Sin conexión a internet. |

### 1.4 Referencias
1. IEEE Std 830-1998, *IEEE Recommended Practice for Software Requirements Specifications*.
2. Documento de Necesidad y Visión del proyecto GTN-API (conversación previa).

### 1.5 Visión General
La **Sección 2** describe el contexto general del producto, sus funciones principales, los tipos de usuario y las restricciones. La **Sección 3** detalla los requisitos específicos (funcionales y no funcionales) que el sistema debe cumplir. La **Sección 4** cubre los requisitos de las interfaces externas. La **Sección 5** describe los atributos del sistema.

---

## 2. Descripción General

### 2.1 Perspectiva del Producto
GTN-API es un producto **independiente y autocontenido**. No es parte de un sistema mayor ni reemplaza a otro software existente. Su ecosistema se limita a:
- El **usuario humano**, que interactúa a través de la GUI.
- **Programas externos del usuario** (scripts en Python, PowerShell, etc.) que se comunican a través de la API local.

### 2.2 Funciones del Producto
A alto nivel, el sistema debe proporcionar las siguientes capacidades:
1. **Gestión de Notas:** CRUD de notas de texto rápido.
2. **Gestión de Tareas:** CRUD de tareas, con estados (pendiente/completada) y asociación a un proyecto.
3. **Organización por Proyectos:** Agrupación lógica de notas y tareas.
4. **API Local:** Exposición de endpoints HTTP para que programas externos realicen operaciones CRUD sobre los datos.
5. **Persistencia Local:** Almacenamiento de toda la información en el sistema de archivos local.

### 2.3 Características de los Usuarios

| Actor | Nivel Técnico | Derechos de Acceso | Notas |
| :--- | :--- | :--- | :--- |
| **Desarrollador (Usuario Principal)** | Experto | Control total sobre la GUI y la configuración del sistema. | Utiliza la aplicación para gestionar su propio flujo de trabajo. |
| **Programa Externo** | N/A (Es un software) | Acceso a los endpoints definidos de la API local para operaciones CRUD. | No tiene interfaz gráfica. Su acceso está limitado por el diseño de la API. |

### 2.4 Restricciones
1. **Plataforma:** La aplicación debe compilar y ejecutarse nativamente en **Microsoft Windows**.
2. **Conectividad:** El sistema **no debe requerir conexión a internet** para ninguna de sus funciones principales.
3. **Almacenamiento:** Los datos deben persistir localmente (por ejemplo, en un archivo SQLite o JSON).
4. **Lenguaje:** Se puede utilizar cualquier lenguaje de programación siempre que cumpla con los puntos anteriores (ej. C#, Python, Java, etc.).

### 2.5 Suposiciones y Dependencias
- **Suposición:** El usuario tiene los permisos necesarios para ejecutar aplicaciones y abrir puertos locales (para la API).
- **Dependencia:** El correcto funcionamiento de la API depende de que el sistema operativo permita la escucha en `localhost` en el puerto designado.
- **Dependencia:** La integridad de los datos depende del correcto cierre de la aplicación y del manejo de transacciones de la base de datos.

### 2.6 Evolución Previsible
- **v1.1:** Añadir etiquetas (tags) a notas y tareas.
- **v1.2:** Implementar un sistema de búsqueda y filtrado avanzado en la GUI y en la API.
- **v2.0:** Soporte para archivos adjuntos locales en las notas.

---

## 3. Requisitos Específicos

### 3.1 Requisitos Funcionales (RF)

| ID | Nombre | Descripción | Prioridad |
| :--- | :--- | :--- | :--- |
| **RF-01** | Crear Nota | El sistema **debe** permitir al usuario crear una nueva nota de texto a través de la GUI. | Alta |
| **RF-02** | Listar Notas | El sistema **debe** mostrar una lista de todas las notas existentes en la GUI. | Alta |
| **RF-03** | Editar Nota | El sistema **debe** permitir modificar el contenido de una nota existente. | Alta |
| **RF-04** | Eliminar Nota | El sistema **debe** permitir eliminar una nota, previa confirmación. | Alta |
| **RF-05** | Crear Tarea | El sistema **debe** permitir crear una tarea con título, descripción, estado (pendiente/completada) y proyecto asociado. | Alta |
| **RF-06** | Listar Tareas | El sistema **debe** mostrar una lista de tareas, filtrable por proyecto y estado. | Alta |
| **RF-07** | Editar Tarea | El sistema **debe** permitir modificar los campos de una tarea existente. | Alta |
| **RF-08** | Eliminar Tarea | El sistema **debe** permitir eliminar una tarea, previa confirmación. | Alta |
| **RF-09** | Gestionar Proyectos | El sistema **debe** permitir crear, listar y eliminar proyectos para agrupar notas y tareas. | Media |
| **RF-10** | API - Listar Recursos | La API local **debe** exponer un endpoint `GET` para obtener la lista de notas y otro para tareas (ej. `GET /api/notes`, `GET /api/tasks`). | Alta |
| **RF-11** | API - Crear Recurso | La API local **debe** exponer un endpoint `POST` para crear una nota o tarea (ej. `POST /api/notes`, `POST /api/tasks`). | Alta |
| **RF-12** | API - Actualizar Recurso | La API local **debe** exponer un endpoint `PUT` para actualizar una nota o tarea existente (ej. `PUT /api/notes/{id}`). | Alta |
| **RF-13** | API - Eliminar Recurso | La API local **debe** exponer un endpoint `DELETE` para eliminar una nota o tarea (ej. `DELETE /api/tasks/{id}`). | Alta |

### 3.2 Requisitos No Funcionales (RNF)

| ID | Categoría | Descripción | Métrica / Criterio |
| :--- | :--- | :--- | :--- |
| **RNF-01** | Rendimiento | La GUI debe responder a las acciones del usuario en un tiempo aceptable. | Las operaciones CRUD desde la GUI deben completarse en **< 500 ms** en condiciones normales. |
| **RNF-02** | Rendimiento | La API local debe tener una latencia baja. | Las peticiones a la API local deben responderse en **< 200 ms** para operaciones simples. |
| **RNF-03** | Fiabilidad | El sistema no debe perder datos ante un cierre inesperado. | Uso de transacciones o escritura atómica en el almacenamiento local. |
| **RNF-04** | Usabilidad | La interfaz de usuario debe ser intuitiva y minimalista. | Un usuario con conocimientos básicos debe poder crear una tarea en **< 1 minuto** sin instrucciones previas. |
| **RNF-05** | Seguridad | La API local solo debe ser accesible desde la misma máquina (`localhost`). | El servidor de la API debe estar vinculado exclusivamente a `127.0.0.1`. |
| **RNF-06** | Portabilidad | La aplicación debe poder ejecutarse en versiones modernas de Windows (10/11). | Debe funcionar sin dependencias externas no incluidas en el instalador. |
| **RNF-07** | Mantenibilidad | El código debe estar modularizado (núcleo, GUI, API). | Separación clara de responsabilidades en el código fuente. |

---

## 4. Requisitos de Interfaces Externas

### 4.1 Interfaces de Usuario
- **GUI de Escritorio:** Debe ser una ventana nativa de Windows con las siguientes áreas principales:
  - Panel lateral: Lista de proyectos.
  - Área central: Lista de notas y tareas del proyecto seleccionado.
  - Barra de herramientas: Botones para "Nueva Nota", "Nueva Tarea", "Guardar", etc.

### 4.2 Interfaces de Hardware
- **Ratón y Teclado:** Dispositivos estándar de entrada para la GUI.
- **Disco Duro:** Espacio para la instalación de la aplicación y el archivo de base de datos local.

### 4.3 Interfaces de Software
- **Sistema Operativo:** Windows 10 o superior.
- **API Local:** El núcleo de la aplicación **debe** actuar como un servidor HTTP ligero que escuche en `http://localhost:{puerto}`. Los detalles de los endpoints se definen en la **Sección 3.1 (RF-10 a RF-13)**.
- **Formato de Datos:** La API debe aceptar y devolver datos en formato **JSON**.

### 4.4 Interfaces de Comunicación
- **HTTP Local:** El único protocolo de comunicación externa permitido es HTTP sobre `localhost`. **No se permite ninguna otra forma de comunicación de red (sockets a internet, etc.)**.

---

## 5. Atributos del Sistema

### 5.1 Disponibilidad
El sistema debe estar disponible para el usuario siempre que la aplicación se esté ejecutando. No se requiere alta disponibilidad (99.9%) ya que es una herramienta de escritorio personal.

### 5.2 Seguridad
- **Integridad:** Los datos no deben corromperse. Se debe validar la entrada de la API (ej. tipos de datos, campos requeridos).
- **Confidencialidad:** Los datos son locales y no se transmiten por la red. El acceso a la API está restringido a `localhost`.

### 5.3 Mantenibilidad
- El código debe estar estructurado en al menos tres capas: **Núcleo de Datos**, **API** y **GUI**.
- Se deben incluir comentarios en las secciones críticas del código.

### 5.4 Portabilidad
El instalador debe ser autocontenido. Si se utilizan runtimes (como .NET o Python), deben incluirse o instalarse como parte del proceso de instalación.

---

## Anexo: Glosario
- **Nota:** Texto breve para recordar una idea, snippet o bug.
- **Tarea:** Unidad de trabajo con un estado (pendiente/completada) y asociada a un proyecto.
- **Proyecto:** Contenedor lógico para agrupar notas y tareas relacionadas.

---

**Fin del documento.**

