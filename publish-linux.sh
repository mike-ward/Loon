#!/bin/bash
pushd src/Loon
~/dotnet/dotnet publish --runtime linux-x64  --self-contained true -c=Release /p:PublishSingleFile=true
popd
