$ErrorActionPreference = 'Stop';

docker images

Write-Host Starting deploy
$env:DOCKER_PASS | docker login --username d2funlife --password-stdin

docker push d2funlife/blog:latest