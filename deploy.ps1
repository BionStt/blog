Write-Host Start deploy $env:APPVEYOR_BUILD_VERSION

Write-Host $env:APPVEYOR_BUILD_ID
Write-Host $env:APPVEYOR_BUILD_NUMBER
Write-Host $env:APPVEYOR_BUILD_VERSION
Write-Host $env:APPVEYOR_REPO_NAME

docker images

Write-Host Tag docker image

docker tag d2funlife/blog d2funlife/blog:'$env:APPVEYOR_BUILD_VERSION'

Write-Host Docker login

$env:DOCKER_PASS | docker login --username d2funlife --password-stdin

Write-Host Push to docker hub

docker push d2funlife/blog:latest