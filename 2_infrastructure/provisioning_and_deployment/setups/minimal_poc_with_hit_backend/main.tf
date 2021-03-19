terraform {
  required_providers {
    docker = {
      source = "kreuzwerker/docker"
    }
    aws = {
      source  = "hashicorp/aws"
      version = ">= 2.7.0"
    }
  }
  required_version = ">= 0.13"
}

provider "aws" {
  access_key = var.access_key
  secret_key = var.secret_key
  region     = var.region
}

module "data_assembly" {
  source    = "../../modules/EC2_data_assembly_and_hit_serving_service/"
  access_key = var.access_key
  secret_key = var.secret_key
  region = var.region
  data_assembly_image_name = var.data_assembly_image_name
  results_bucket_name = var.results_bucket_name
  django_superuser_username=var.django_superuser_username
  django_superuser_password=var.django_superuser_password
  django_email=var.django_email
  participant_support_backend_image_name=var.participant_support_backend_image_name
}

module "data_collection" {
  source    = "../../modules/S3_data_collection/"
  access_key = var.access_key
  secret_key = var.secret_key
  region = var.region
  results_bucket_name = var.results_bucket_name
  data_collection_bucket_name = var.data_collection_bucket_name
  data_assembly_location = module.data_assembly.instance_location
}

module "data_analysis_and_management" {
  source    = "../../modules/LOCAL_data_analysis_and_management/"
  jupyter_lab_port = var.jupyter_lab_port
  jupyter_lab_access_token = var.jupyter_lab_access_token
  data_analysis_and_management_image_name = var.data_analysis_and_management_image_name
  data_assembly_location = module.data_assembly.instance_location
  data_collection_location = module.data_collection.collection_bucket_location
  results_bucket_location = module.data_collection.results_bucket_location
  results_bucket_name = var.results_bucket_name
  access_key = var.access_key
  secret_key = var.secret_key
  region = var.region
  participant_support_backend_location = module.data_assembly.hit_service_location
  participant_support_backend_token = module.data_assembly.hit_service_token
}


