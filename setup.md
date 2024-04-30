# Server
## setup Docker
Source [https://docs.docker.com/engine/install/ubuntu/]

Run the following commands in order.
1. sudo apt-get update
2. sudo apt-get install ca-certificates curl gnupg 
3. sudo install -m 0755 -d /etc/apt/keyrings
4. curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
5. sudo chmod a+r /etc/apt/keyrings/docker.gpg
6. echo \
  "deb [arch="$(dpkg --print-architecture)" signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
  "$(. /etc/os-release && echo "$VERSION_CODENAME")" stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
7. sudo apt-get update
8. sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
9. sudo docker run hello-world

## Setup SQL server
Source [https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash]

Run the following command in order.

1. sudo docker pull mcr.microsoft.com/mssql/server:2022-latest
2. sudo docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Passw0rd>" \
   -p 1433:1433 --name sql1 --hostname sql1 \
   -d \
   mcr.microsoft.com/mssql/server:2022-latest
    > NOTE: Create a strong password or else the container will just crash.

3. Connect to the database using the server ip, and your username

## Setup cassandra

Run the following commands in order

1. docker run -p 9042:9042 -e DS_LICENSE=accept --rm --name my-dse -d datastax/dse-server:6.8.32 -s 
> Go to https://hub.docker.com/r/datastax/dse-studio/tags for the latest version :latest was broken for me
2. $ docker run -e DS_LICENSE=accept --link my-dse --name my-studio -p 9091:9091 -d datastax/dse-studio 
3. and then go to http://10.0.0.103:9091/connections and set the connection to *my-dse*
> The connection string should look something like "cassandra://<SERVER_IP>:9042"



## Setup kubernetes with mini cube
Source [https://minikube.sigs.k8s.io/docs/start/]

Run the following steps 

> Run a sudo
1. curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube_latest_amd64.deb
2. sudo dpkg -i minikube_latest_amd64.deb
3. minikube start
4. kubectl get po -A

## Setup rabbitMQ

source [https://www.architect.io/blog/2021-01-19/rabbitmq-docker-tutorial/]

1. docker pull rabbitmq:3-management
2. docker run --rm -itd -p 15672:15672 -p 5672:5672 rabbitmq:3-management
3. go to <server_url>:15672/
> USERNAME: guest 

>PASSWORD: guest