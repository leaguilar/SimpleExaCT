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

variable "results_bucket_name" {
  type = string
  default = ""
}

variable "data_collection_bucket_name" {
  type = string
  default = "zoll-experiment"
}

variable "data_assembly_image_name" {
  type = string
  default = ""
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
