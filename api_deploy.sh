#!/bin/bash

VERSION=$1
PORT=98
NAME=lievlyhomes_api
TAG=$NAME:$VERSION
REPO=brian1998
IMAGE=$REPO/$TAG

docker pull $IMAGE
docker stop $NAME
docker rm $NAME

docker run -d -p $PORT:82\
-e TZ=Africa/Nairobi \
--restart unless-stopped \
--name $NAME $IMAGE

docker logs -f $NAME
