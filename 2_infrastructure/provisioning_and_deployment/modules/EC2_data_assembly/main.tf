provider "aws" {
  access_key = var.access_key
  secret_key = var.secret_key
  region     = var.region
}

data "aws_ami" "ubuntu" {
  most_recent = true

  filter {
    name   = "name"
    values = ["ubuntu/images/hvm-ssd/ubuntu-focal-20.04-amd64-server-*"]
  }

  filter {
    name   = "virtualization-type"
    values = ["hvm"]
  }

  owners = ["099720109477"] # Canonical
}

resource "tls_private_key" "data-assembly-key" {
  algorithm   = "RSA"
  rsa_bits = "2048"
}

resource "local_file" "public-key-pem" {
    content     = tls_private_key.data-assembly-key.public_key_openssh
    filename = "${path.module}/credentials/example.pem.pub"
}

resource "local_file" "private-key-pem" {
    content     = tls_private_key.data-assembly-key.private_key_pem
    filename = "${path.module}/credentials/example.pem"
    file_permission = 400
}

resource "aws_key_pair" "deployer-ssh-key" {
  key_name   = "data-assembly-ssh-key"
  public_key = tls_private_key.data-assembly-key.public_key_openssh
  depends_on = [tls_private_key.data-assembly-key]
}

resource "aws_vpc" "my_vpc" {
  cidr_block = "192.168.0.0/16"

  tags = {
    Name = "tf-experiment-as-code"
  }
}

resource "aws_subnet" "my_subnet" {
  vpc_id            = aws_vpc.my_vpc.id
  cidr_block        = "192.168.0.0/24"
  map_public_ip_on_launch = true

  tags = {
    Name = "tf-experiment-as-code"
  }
}

# internet gateway
resource "aws_internet_gateway" "internet_gateway" {
  depends_on = [
    aws_vpc.my_vpc,
  ]

  vpc_id = aws_vpc.my_vpc.id

  tags = {
    Name = "internet-gateway"
  }
}


# route table with target as internet gateway
resource "aws_route_table" "IG_route_table" {
  depends_on = [
    aws_vpc.my_vpc,
    aws_internet_gateway.internet_gateway,
  ]

  vpc_id = aws_vpc.my_vpc.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.internet_gateway.id
  }

  tags = {
    Name = "IG-route-table"
  }
}

# associate route table to public subnet
resource "aws_route_table_association" "associate_routetable_to_public_subnet" {
  depends_on = [
    aws_subnet.my_subnet,
    aws_route_table.IG_route_table,
  ]
  subnet_id      = aws_subnet.my_subnet.id
  route_table_id = aws_route_table.IG_route_table.id
}


resource "aws_security_group" "example-data-assembly-sec-group" {
  name        = "da-security-group"
  description = "Allow HTTP, HTTPS and SSH traffic"
  vpc_id      = aws_vpc.my_vpc.id

  ingress {
    description = "SSH"
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    description = "HTTPS"
    from_port   = 8443
    to_port     = 8443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    description = "HTTP"
    from_port   = 8080
    to_port     = 8080
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "DataAssembly"
  }
}

resource "aws_instance" "data-assembly" {
  ami           = data.aws_ami.ubuntu.id
  instance_type = "t2.micro"
  associate_public_ip_address = true
  tags = {
    Name = "DataAssembly"
  }
  
  subnet_id = aws_subnet.my_subnet.id
  
  key_name = aws_key_pair.deployer-ssh-key.key_name
  
  vpc_security_group_ids = [
    aws_security_group.example-data-assembly-sec-group.id
  ]

  connection {
    type        = "ssh"
    user        = "ubuntu"
    private_key = tls_private_key.data-assembly-key.private_key_pem
    host        = self.public_ip
  }

  ebs_block_device {
    device_name = "/dev/sda1"
    volume_type = "gp2"
    volume_size = 30
  }
  
    provisioner "remote-exec" {
    inline = [
      "sudo apt update",
      "sudo apt install -y apt-transport-https ca-certificates curl software-properties-common",
      "curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -",
      "sudo apt-key fingerprint 0EBFCD88",
      "sudo add-apt-repository \"deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable\"",
      "sudo apt update",
      "echo \"INSTALLING DOCKER\"",
      "sudo apt install -y docker-ce",
      "echo \"ENABLING USER TO RUN DOCKER\"",
      "sudo usermod -aG docker ubuntu",
    ]
  }
  
  provisioner "remote-exec" {
    inline = [
      "echo \"##### Creating results directory  #####\n\"",
      "mkdir -p results",
      "docker run -v $PWD/results:/go/src/app/results -e \"S3_BUCKET=${var.results_bucket_name}\" -e \"S3_ACCESS_KEY_ID=${var.access_key}\" -e \"S3_SECRET_ACCESS_KEY=${var.secret_key}\" -e \"S3_REGION=${var.region}\" -p 8080:8080 -d --restart=unless-stopped ${var.data_assembly_image_name}",
    ]
  }
}

resource "aws_eip" "data-assembly-eip" {
  vpc      = false
  instance = aws_instance.data-assembly.id
}
