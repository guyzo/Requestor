#! /bin/bash
rm -rf Build
xbuild /p:Configuration=Release
Tools/mspec.exe Build/Release/Requestor.Specs.dll
