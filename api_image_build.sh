#!/bin/bash

VERSION=$1

if [ -z "$1" ]; then
   VERSION=1.0.0
   echo "   ******************************************************************
   * IMAGE BUILD VERSION NOT PROVIDED USING DEFAULT VERSION $VERSION
   ******************************************************************" 
else
  echo "IMAGE BUILD VERSION $1"
fi

BUILD_DIR=dist
TAG=lievlyhomes_api:$VERSION
REPO=brian1998
IMAGE=$REPO/$TAG

 cp -r ../$BUILD_DIR .
 docker build -f Dockerfile -t $IMAGE .
 rm -rf $BUILD_DIR
 docker push $IMAGE