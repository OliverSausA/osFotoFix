#!/bin/bash

NSIS=/usr/bin/makensis
BUILD=win10-x64

if [ -d ${BUILD}-deployment ]; then 
  rm -r ${BUILD}-deployment
fi

if [ ! -d ${BUILD}-setup ]; then
  mkdir ${BUILD}-setup 
fi

echo dotnet publish ../osFotoFix -o ${BUILD}-deployment -r ${BUILD} -p:PublishReadyRun=true -p:PublishSingleFile=false -p:PublishedTrim=true --self-contained true
dotnet publish ../osFotoFix -o ${BUILD}-deployment -r ${BUILD} -p:PublishReadyRun=true -p:PublishSingleFile=false -p:PublishedTrim=true --self-contained true

${NSIS} ${BUILD}.nsi

