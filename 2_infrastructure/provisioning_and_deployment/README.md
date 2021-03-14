# Kubernetes cluster provisioning

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
afinidata_docker_registry      = "url_registry:port"
afinidata_docker_registry_user = "username"
```

Note: this can be done automatically for a local cluster through the scripts in: [/dev/infrastructure/provisioning/local/](../dev/infrastructure/provisioning/local/README.md)

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





