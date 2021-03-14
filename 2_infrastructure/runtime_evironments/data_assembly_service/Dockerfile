FROM golang:1.14

WORKDIR /go/src/app
COPY . .

RUN go build -o data_assembly_service .


CMD ["./data_assembly_service"]
