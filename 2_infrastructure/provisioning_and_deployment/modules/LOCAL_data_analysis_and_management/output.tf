output "volumes" {
  description = "This are the volumes mounted in the management component group"
  value       = docker_container.management.volumes
}

output "location" {
  description = "This are the volumes mounted in the management component group"
  value       = "http://localhost:${var.jupyter_lab_port}"
}
