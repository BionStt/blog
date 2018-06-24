$ErrorActionPreference = 'Stop';

docker images

if (! (Test-Path Env:\APPVEYOR_REPO_TAG_NAME)) {
  Write-Host "No version tag detected. Skip publishing."
  exit 0
}

Write-Host Starting deploy
docker login -u="$env:DOCKER_USER" -p="$env:DOCKER_PASS"

docker push d2funlife/blog:1.0.$env:APPVEYOR_BUILD_NUMBER