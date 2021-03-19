USERNAME="leaguilar"
IMAGE_NAME="simplevr_participant_support_backend_service"

DOCKERFILE=$PWD/Dockerfile

cd ../../../6_management/participant_support_backend/
docker build -t $USERNAME/$IMAGE_NAME -f $DOCKERFILE .
docker push $USERNAME/$IMAGE_NAME:latest
