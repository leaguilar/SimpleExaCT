USERNAME="leaguilar"
IMAGE_NAME="simplevr_data_analysis_and_monitoring_service"

DOCKERFILE=$PWD/Dockerfile

docker build -t $USERNAME/$IMAGE_NAME -f $DOCKERFILE .
docker push $USERNAME/$IMAGE_NAME:latest
