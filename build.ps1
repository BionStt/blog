$Version=$env:APPVEYOR_BUILD_VERSION

docker build -t d2funlife/blog:latest -t d2funlife/blog:$Version .