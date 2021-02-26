pushd src\Loon
dotnet publish --runtime linuxmint.19.2-x64 --self-contained true -p:PublishSingleFile=true
popd