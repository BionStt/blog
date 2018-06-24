$ErrorActionPreference = 'Stop';

docker images

Write-Host Starting deploy
docker login d2funlife
$env:DOCKER_USER
$env:DOCKER_PASS
docker push d2funlife/blog:latest