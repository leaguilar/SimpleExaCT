terraform {
  required_providers {
    docker = {
      source = "kreuzwerker/docker"
    }
  }
  required_version = ">= 0.13"
}


provider "docker" {
}

resource "docker_image" "jupyter" {
  name = var.data_analysis_and_management_image_name
  keep_locally = true
}

# Create a container
resource "docker_container" "management" {
  image = docker_image.jupyter.latest
  name  = "management"
  hostname = "management"
  user="root"
  restart="unless-stopped"
  remove_volumes = false
  env = [
  	"JUPYTER_ENABLE_LAB=yes",
  	"DATA_ASSEMBLY_LOCATION=${var.data_assembly_location}",
  	"DATA_COLLECTION_LOCATION=${var.data_collection_location}",
  	"RESULTS_LOCATION=${var.results_bucket_location}",
  	"RESULTS_BUCKET_NAME=${var.results_bucket_name}",
  	"AWS_ACCESS_KEY=${var.access_key}",
  	"AWS_SECRET=${var.secret_key}",
  	"AWS_REGION=${var.region}"
  	]
  command=[
  	"start-notebook.sh",
  	"--NotebookApp.token='${var.jupyter_lab_access_token}'"
  	]
  ports {
  	internal=8888
  	external=var.jupyter_lab_port
  }
  volumes {
  	host_path=abspath("${path.module}/../../../../6_management/")
  	container_path="/home/jovyan/work/management"
  }
  volumes {
  	host_path=abspath("${path.module}/../../../../5_data_analysis/")
  	container_path="/home/jovyan/work/analysis"
  }
  volumes {
  	host_path=abspath("${path.module}/../../../../experiment/")
  	container_path="/home/jovyan/work/experiment"
  }
}


