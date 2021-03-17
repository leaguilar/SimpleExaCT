# SimpleExaCT

A Simple and Minimalist Experiment as Code Template (SimpleExaCT).
We are working on making this template more user friendly.
Currently it remains at the Proof of Concept State.

# Pre-requisites
- docker
- terraform
- aws account
- git

### Disclaimer

Please consider that for this proof of concept we did our best effort to keep the cloud resources provisioned within the free tier limits. Leaving the provisioned infraestructure running for a couple of weeks under this setup ``should`` ammount to tens of dollars. 

That said you are solely responsible for its usage. The usual disclaimers apply. This software is provided “as is” without warranty of any kind, either expressed or implied. This software is to be used at your own risk.

## Workflow

[Provisioning and Deployment Setups](2_infrastructure/provisioning_and_deployment/setups/) provide specific ways of instantiating the experiment.

### Proof of Concept

1. Clone this repository
```bash
git clone https://github.com/leaguilar/SimpleExaCT.git
```
2. Modify the variables of your [variables.auto.tfvars](2_infrastructure/provisioning_and_deployment/setups/minimal_poc/variables.auto.tfvars), e.g. by adding your AWS credentials

3. Initialize terraform
```bash
cd SimpleExaCT/2_infrastructure/provisioning_and_deployment/setups/minimal_poc/
terraform init
```

4. Apply, plan and/or review the provisioning and deployment
```bash
terraform apply #or plan, save and review it
```
Note that depending on your internet connection this process could take several minutes.
Terraform will:
- create the data assembly service (trying to keep you in the free tier):
    - Provision an EC2 instance, with the minimum requirements to access it remotelly
- create two S3 buckets.
    - It will upload the experiment collection component and prepare for the other bucket for experiment results.
- run a local docker container and mount the data analysis and hardware and participant monitoring scripts in it.
- and finally links everything together by passing around relevant information, e.g. where the component groups can be reached

5. Visit with your browser http://localhost:8888 and you will be in the Jupyter-lab instance that provides access to all the monitoring and data analysis scripts.

Here you can:
- serve MTurk's HITs and reward participants.
- monitor the state of the data assembly service.
- plot the results of the experiment

6. Destroy the infraestructure you have created.
```bash
terraform state rm module.data_collection.aws_s3_bucket.b2 #(Optional, exclude the experiment results bucket state, if you haven't downloaded and deleted your experiment results)
terraform destroy
```
