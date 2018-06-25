$Version=$env:APPVEYOR_BUILD_VERSION
Write-Host Start deploy $Version

docker images

Write-Host Docker login

$env:DOCKER_PASS | docker login --username d2funlife --password-stdin

Write-Host Push to docker hub

docker push d2funlife/blog