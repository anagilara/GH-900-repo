variable "resource_group_name" {
  description = "Name of the resource group"
  type        = string
  default     = "gh900-rg"
}

variable "location" {
  description = "Azure region for resources"
  type        = string
  default     = "East US"
}

variable "app_name" {
  description = "Name of the application"
  type        = string
  default     = "gh900-app"
}

variable "app_service_sku" {
  description = "SKU for the App Service Plan"
  type        = string
  default     = "B1"
}

variable "environment" {
  description = "Environment name (development, staging, production)"
  type        = string
  default     = "development"
}

variable "tags" {
  description = "Tags to apply to all resources"
  type        = map(string)
  default = {
    Project     = "GH-900"
    ManagedBy   = "Terraform"
    Environment = "Development"
  }
}
