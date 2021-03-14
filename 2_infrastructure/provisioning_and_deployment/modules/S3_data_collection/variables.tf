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
  default = "zoll-results"
}

variable "data_collection_bucket_name" {
  type = string
  default = "zoll-experiment"
}

variable "data_assembly_location" {
  type = string
  default = ""
}
