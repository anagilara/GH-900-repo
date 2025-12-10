# Terraform Configuration for GH-900

This directory contains the Terraform configuration for provisioning Azure infrastructure.

## Quick Start

1. Copy the example variables file:
```bash
cp terraform.tfvars.example terraform.tfvars
```

2. Edit `terraform.tfvars` with your values

3. Initialize Terraform:
```bash
terraform init
```

4. Plan the changes:
```bash
terraform plan
```

5. Apply the configuration:
```bash
terraform apply
```

## Files

- `main.tf` - Main infrastructure resources
- `variables.tf` - Input variables
- `outputs.tf` - Output values
- `terraform.tfvars.example` - Example configuration

## Prerequisites

- Terraform >= 1.0
- Azure CLI
- Azure subscription
- Service Principal with Contributor role

## Remote State

This configuration uses Azure Storage for remote state. Ensure the backend storage account exists before running `terraform init`.

See the main [INFRASTRUCTURE.md](../INFRASTRUCTURE.md) for detailed setup instructions.
