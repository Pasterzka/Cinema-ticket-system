#!/bin/bash

# Check if the first argument is provided
if [ -z "$1" ]; then
    echo "[ERROR] NO ARGUMENT PROVIDED"
    echo "Usage: ./build.sh [api | worker | web | all]"
    exit 1
fi

case "$1" in
    api)
        echo "Building WebApi..."
        docker compose up -d --build api
        ;;
    worker)
        echo "Building Worker..."
        docker compose up -d --build worker
        ;;
    web)
        echo "Building Web..."
        docker compose up -d --build web
        ;;
    all)
        echo "Building All Components..."
        docker compose up -d --build
        ;;
    *)
        echo "[ERROR] INVALID ARGUMENT: $1"
        echo "Usage: ./build.sh [api | worker | web | all]"
        exit 1
        ;;
esac