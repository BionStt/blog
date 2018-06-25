Write-Host Start deploy $env:APPVEYOR_REPO_TAG_NAME

docker images

Write-Host Tag docker image

docker tag d2funlife/blog:$env:APPVEYOR_REPO_TAG_NAME

Write-Host Docker login

$env:DOCKER_PASS | docker login --username d2funlife --password-stdin

Write-Host Push to docker hub

docker push d2funlife/blog:latest