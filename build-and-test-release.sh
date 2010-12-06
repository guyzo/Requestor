#! /bin/bash
rm -rf Build
xbuild /p:Configuration=Release
MSPEC_PATH=Tools/mspec.exe Tools/mspec-color.exe Build/Release/Requestor.Specs.dll
