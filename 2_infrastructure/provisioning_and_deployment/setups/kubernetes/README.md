# Kubernetes cluster provisioning
WIP
This folder contains all the terraform definitions of the different services

## Provisioning

### Basic configuration

Credentials should be put in the credentials folder, the following credentials are required:
- credentials/client_cert.pem  
- credentials/client_key.pem  
- credentials/cluster_cert.pem

The variables must set in their respective *.auto.tfvars

For example in kubernetes.auto.tfvars
```hcl-terraform
host                           = "url_cluster:port"
docker_registry      = "url_registry:port"
docker_registry_user = "username"
```

### Init terraform
```commandline
terraform init
```

### Apply changes
To apply all the current definitions
```commandline
terraform apply
```

### Destroy 
Destroys the provisioned resources
```commandline
terraform destroy
```





