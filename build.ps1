if (Test-Path -Path ./Published)
{
    Remove-Item -LiteralPath ./Published -Force -Recurse
}

if (Test-Path -Path ./YeahGameWeb/post-build.bat)
{
    Set-Content -Path "./YeahGameWeb/post-build.bat" -Value ""
}

dotnet publish ./YeahGameWeb/YeahGameWeb.csproj --configuration Release --runtime browser-wasm --self-contained true -p:PublishTrimmed=true --output ./Published
