provider "aws" {
  access_key = var.access_key
  secret_key = var.secret_key
  region     = var.region
}

locals {
  mime_types = jsondecode(file("${path.module}/mime_data/mime.json"))
}

resource "local_file" "collector-config" {
    content     = "{\"dataAssemblyUrl\":\"${var.data_assembly_location}\"}"
    filename = "${path.module}/../../../../3_data_collection/built/config.json"
}

# Create a bucket for the experiment
resource "aws_s3_bucket" "b1" {

  bucket = var.data_collection_bucket_name

  acl    = "private"

  tags = {
    Name        = "Zoll Experiment"
    Environment = "Dev"
  }
}

# Create a bucket for results
resource "aws_s3_bucket" "b2" {

  bucket = var.results_bucket_name

  acl    = "private"

  tags = {
    Name        = "Zoll Results"
    Environment = "Dev"
  }
}

resource "aws_s3_bucket_object" "base-files" {
  for_each = fileset("${path.module}/../../../../3_data_collection/built/ZollMTURK", "**/*")
  bucket = aws_s3_bucket.b1.id
  key = each.value
  source = "${path.module}/../../../../3_data_collection/built/ZollMTURK/${each.value}"
  etag = filemd5("${path.module}/../../../../3_data_collection/built/ZollMTURK/${each.value}")
  acl    = "public-read"
  content_type = lookup(local.mime_types, regex("\\.[^.]+$", each.value), null)
  depends_on=[local_file.collector-config]
}

resource "aws_s3_bucket_object" "config-file" {
  bucket = aws_s3_bucket.b1.id
  key = "config.json"
  source = "${path.module}/../../../../3_data_collection/built/config.json"
  etag = md5(local_file.collector-config.content)
  acl    = "public-read"
  content_type = "application/json"
  depends_on=[aws_s3_bucket_object.base-files,local_file.collector-config]
}
