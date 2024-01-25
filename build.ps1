if (Test-Path -Path ./Published)
{
    Remove-Item -LiteralPath ./Published -Force -Recurse
}

(Get-Content "./YeahGame/YeahGame.csproj") -replace '<PublishAot>true</PublishAot>', '<PublishAot>false</PublishAot>' | Set-Content "./YeahGame/YeahGame.csproj"

dotnet publish ./YeahGameWeb/YeahGameWeb.csproj --configuration Release --runtime browser-wasm --self-contained true -p:PublishTrimmed=true --output ./Published
