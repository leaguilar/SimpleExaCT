variable "access_key" {
  type = string 
  default = ""
}

variable "secret_key" {
  type = string
  default = ""
}

variable "region" {
  type = string
  default = "us-east-1"
}

variable "jupyter_lab_port" {
  type = string 
  default = "8888"
}

variable "jupyter_lab_access_token" {
  type = string
  default = ""
}

variable "data_analysis_and_management_image_name" {
  type = string
  default = ""
}

variable "data_assembly_location" {
  type = string
  default = ""
}

variable "data_collection_location" {
  type = string
  default = ""
}

variable "results_bucket_name" {
  type = string
  default = "zoll-results"
}

variable "results_bucket_location" {
  type = string
  default = ""
}

variable "participant_support_backend_location" {
  type = string
  default = ""
}

variable "participant_support_backend_token" {
  type = string
  default = ""
}
