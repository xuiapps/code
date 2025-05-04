Xui Contributions
=================

Docs
----
AI api ref build:
```
dotnet run --project tools/XuiApiExport/XuiApiExport.csproj
dotnet run --project tools/DocsGenerator/DocsGenerator.csproj
```

Docs build:
```
dotnet tool update -g docfx
docfx docfx.json --serve
```
