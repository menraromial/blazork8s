#login
echo $CR_PAT | docker login ghcr.io -u weibaohui --password-stdin

docker buildx build -t  ghcr.io/weibaohui/blazork8s:latest  -f BlazorApp/Dockerfile   --platform=linux/arm64,linux/amd64 . --push
