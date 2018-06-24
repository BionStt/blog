$ErrorActionPreference = 'Stop';

docker images

Write-Host Starting deploy
docker login -u="$env:DOCKER_USER" -p="$env:DOCKER_PASS"

docker push d2funlife/blog:latest