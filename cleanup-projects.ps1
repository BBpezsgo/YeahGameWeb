if (Test-Path -Path ./YeahGameWeb)
{
    Remove-Item -LiteralPath ./YeahGameWeb -Force -Recurse
}

if (Test-Path -Path ./YeahGame)
{
    Remove-Item -LiteralPath ./YeahGame -Force -Recurse
}

if (Test-Path -Path ./Win32-Stuff)
{
    Remove-Item -LiteralPath ./Win32-Stuff -Force -Recurse
}