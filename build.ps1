if (Test-Path -Path ./Published)
{
    Remove-Item -LiteralPath ./Published -Force -Recurse
}

dotnet publish ./YeahGameWeb/YeahGameWeb.csproj --configuration Release --runtime browser-wasm --self-contained true -p:PublishTrimmed=true --output ./Published
