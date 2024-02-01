Tu compile and run this please follow the commands bellow

dotnet run

dotnet publish -c Release

docker build -t olympusexample-image -f Dockerfile .

docker create --name olympusexample olympusexample-image

docker run -i -v c:\junk:/OlympusExample/Log olympusexample-image 
