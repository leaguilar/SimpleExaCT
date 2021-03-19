output "instance_location" {
  description = "The location of the data assembly service"
  value       = "http://${aws_eip.data-assembly-eip.public_ip}:8080"
}

output "instance_ip" {
  description = "The public ip for ssh access"
  value       = aws_eip.data-assembly-eip.public_ip
}

output "credentials_path" {
  description = "credential file for ssh access"
  value       = local_file.private-key-pem.filename
}

output "hit_service_location" {
  description = "The location of the hit serving service"
  value       = "http://${aws_eip.data-assembly-eip.public_ip}:8090"
}

output "hit_service_token" {
  description = "The token to communicate with the hit serving service"
  value       = tostring(random_password.django-token.result)
}
