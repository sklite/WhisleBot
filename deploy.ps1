Write-Output "Building docker image"
docker build -t whistlebotconsole .

Write-Output "Change image tag"
docker tag whistlebotconsole sklite/whistlebotconsole

Write-Output "Pushing image to docker repo"
docker push sklite/whistlebotconsole