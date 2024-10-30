set -e
dotnet restore Registerly.sln
dotnet build --no-restore -c Release Registerly.sln
dotnet test --no-restore -c Release Registerly.sln
