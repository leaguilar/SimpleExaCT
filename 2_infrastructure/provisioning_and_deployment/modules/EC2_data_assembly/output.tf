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
