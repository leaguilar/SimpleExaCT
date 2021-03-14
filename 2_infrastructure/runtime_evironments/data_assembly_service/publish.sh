USERNAME="leaguilar"
IMAGE_NAME="simplevr_data_assembly_service"

DOCKERFILE=$PWD/Dockerfile

cd ../../../4_data_assembly/backend/go/
docker build -t $USERNAME/$IMAGE_NAME -f $DOCKERFILE .
docker push $USERNAME/$IMAGE_NAME:latest
