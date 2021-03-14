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

variable "data_assembly_image_name" {
  type = string
  default = ""
}
