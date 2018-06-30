$Version=$env:APPVEYOR_BUILD_VERSION
Write-Host Starting build $Version

docker build -t d2funlife/blog:latest -t d2funlife/blog:$Version .