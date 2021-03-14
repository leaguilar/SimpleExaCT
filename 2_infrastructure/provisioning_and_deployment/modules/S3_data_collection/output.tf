output "collection_bucket_location" {
  #
  value = "http://${aws_s3_bucket.b1.bucket_domain_name}"
}

output "results_bucket_location" {
  #
  value = "http://${aws_s3_bucket.b2.bucket_domain_name}"
}
