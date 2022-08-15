#!/bin/bash
pushd src/Loon.Desktop || exit
~/dotnet/dotnet publish --runtime linux-x64 --self-contained true -c=Release /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
popd || exit
