if (Test-Path -Path ./YeahGameWeb) { } else
{
    mkdir ./YeahGameWeb
    git clone https://github.com/BBpezsgo/YeahGameWeb.git ./YeahGameWeb
}

if (Test-Path -Path ./Win32-Stuff) { } else
{
    mkdir ./Win32-Stuff
    git clone https://github.com/BBpezsgo/Win32-Stuff.git ./Win32-Stuff
}

if (Test-Path -Path ./YeahGame) { } else
{
    mkdir ./YeahGame
    git clone https://github.com/BBpezsgo/YeahGame.git ./YeahGame
}