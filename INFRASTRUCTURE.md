# Infraestructura como Código (IaC)

Este documento describe la configuración de Infraestructura como Código para el proyecto GH-900.

## 📋 Contenido

- [Descripción General](#descripción-general)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Terraform](#terraform)
- [Docker](#docker)
- [GitHub Actions](#github-actions)
- [Requisitos Previos](#requisitos-previos)
- [Configuración Inicial](#configuración-inicial)
- [Despliegue](#despliegue)

## 🎯 Descripción General

Este proyecto utiliza múltiples herramientas de IaC para gestionar la infraestructura y el despliegue:

- **Terraform**: Provisionamiento de recursos en Azure
- **Docker**: Contenerización de la aplicación
- **GitHub Actions**: CI/CD automatizado

## 📁 Estructura del Proyecto

```
.
├── terraform/              # Configuración de Terraform
│   ├── main.tf            # Recursos principales
│   ├── variables.tf       # Variables de configuración
│   ├── outputs.tf         # Outputs de Terraform
│   └── terraform.tfvars.example  # Ejemplo de variables
├── .github/
│   └── workflows/         # Workflows de GitHub Actions
│       ├── ci.yml         # Pipeline de CI
│       ├── cd.yml         # Pipeline de CD
│       ├── docker.yml     # Build y push de Docker
│       └── terraform.yml  # Provisionamiento de infraestructura
├── Dockerfile             # Configuración de Docker
├── docker-compose.yml     # Configuración de Docker Compose
└── .dockerignore          # Archivos excluidos de Docker
```

## ⚙️ Terraform

### Recursos Provisisionados

La configuración de Terraform crea los siguientes recursos en Azure:

- **Resource Group**: Grupo de recursos para la aplicación
- **App Service Plan**: Plan de servicio para alojar la aplicación
- **Linux Web App**: Aplicación web .NET 8.0

### Variables Principales

| Variable | Descripción | Valor por Defecto |
|----------|-------------|-------------------|
| `resource_group_name` | Nombre del grupo de recursos | `gh900-rg` |
| `location` | Región de Azure | `East US` |
| `app_name` | Nombre de la aplicación | `gh900-app` |
| `app_service_sku` | SKU del App Service Plan | `B1` |
| `environment` | Entorno de despliegue | `development` |

### Uso

```bash
cd terraform

# Inicializar Terraform
terraform init

# Crear archivo de variables
cp terraform.tfvars.example terraform.tfvars
# Editar terraform.tfvars con tus valores

# Ver plan de ejecución
terraform plan

# Aplicar cambios
terraform apply

# Destruir recursos
terraform destroy
```

## 🐳 Docker

### Construcción Local

```bash
# Construir la imagen
docker build -t gh900-app .

# Ejecutar el contenedor
docker run -p 8080:8080 gh900-app
```

### Docker Compose

```bash
# Iniciar servicios
docker-compose up -d

# Detener servicios
docker-compose down
```

La aplicación estará disponible en `http://localhost:8080`

## 🚀 GitHub Actions

### Workflows Disponibles

#### CI - Build and Test (`ci.yml`)
- **Trigger**: Push y Pull Requests a `main` y `develop`
- **Acciones**:
  - Restaurar dependencias
  - Compilar proyecto
  - Ejecutar tests
  - Publicar artefactos

#### CD - Deploy to Azure (`cd.yml`)
- **Trigger**: Después de completar exitosamente el workflow de CI
- **Acciones**:
  - Compilar y publicar aplicación
  - Desplegar a Azure Web App

#### Docker Build and Push (`docker.yml`)
- **Trigger**: Push a `main`, tags, y Pull Requests
- **Acciones**:
  - Construir imagen Docker
  - Subir a GitHub Container Registry (ghcr.io)

#### Terraform Infrastructure (`terraform.yml`)
- **Trigger**: Cambios en `terraform/` o ejecución manual
- **Acciones**:
  - Validar configuración Terraform
  - Planificar cambios
  - Aplicar infraestructura (solo en main)

## 📋 Requisitos Previos

### Para desarrollo local:
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Terraform](https://www.terraform.io/downloads) (v1.6.0+)
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli) (opcional)

### Para CI/CD:
- Cuenta de Azure
- GitHub Secrets configurados (ver sección siguiente)

## 🔧 Configuración Inicial

### 1. Configurar Azure

```bash
# Iniciar sesión en Azure
az login

# Crear Service Principal para Terraform
az ad sp create-for-rbac --name "terraform-gh900" \
  --role Contributor \
  --scopes /subscriptions/{subscription-id}
```

### 2. Configurar GitHub Secrets

Agregar los siguientes secrets en GitHub (Settings > Secrets and variables > Actions):

| Secret | Descripción |
|--------|-------------|
| `AZURE_CLIENT_ID` | Client ID del Service Principal |
| `AZURE_CLIENT_SECRET` | Client Secret del Service Principal |
| `AZURE_SUBSCRIPTION_ID` | ID de la suscripción de Azure |
| `AZURE_TENANT_ID` | Tenant ID de Azure |
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Perfil de publicación del Web App |

### 3. Configurar Backend de Terraform

Antes de usar Terraform, crear los recursos para el estado remoto:

```bash
# Crear Resource Group
az group create --name tfstate-rg --location "East US"

# Crear Storage Account
az storage account create \
  --name tfstategh900 \
  --resource-group tfstate-rg \
  --location "East US" \
  --sku Standard_LRS

# Crear Container
az storage container create \
  --name tfstate \
  --account-name tfstategh900
```

## 🚀 Despliegue

### Despliegue Manual

1. **Provisionar infraestructura**:
```bash
cd terraform
terraform init
terraform apply
```

2. **Construir y desplegar con Docker**:
```bash
docker build -t gh900-app .
docker push ghcr.io/{username}/gh900-app:latest
```

3. **Desplegar a Azure**:
```bash
az webapp deployment source config-zip \
  --resource-group gh900-rg \
  --name gh900-app \
  --src publish.zip
```

### Despliegue Automático

El despliegue automático se activa mediante:

1. Push a la rama `main` → Ejecuta CI/CD completo
2. Crear Pull Request → Ejecuta CI y validaciones
3. Crear tag `v*.*.*` → Construye y publica imagen Docker con versión

## 📚 Referencias

- [Terraform Azure Provider](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Docker Documentation](https://docs.docker.com/)
- [Azure App Service](https://docs.microsoft.com/azure/app-service/)

## 🔒 Seguridad

- Los secrets de Azure deben estar configurados en GitHub Secrets
- El archivo `terraform.tfvars` está excluido de git (contiene información sensible)
- Las credenciales no deben incluirse en el código fuente

## 🤝 Contribuciones

Para contribuir al proyecto:

1. Crear una rama desde `develop`
2. Realizar cambios
3. Crear Pull Request
4. Los workflows de CI se ejecutarán automáticamente
5. Después de la aprobación, fusionar a `develop` o `main`
