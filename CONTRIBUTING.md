Xui Contributions
=================

Docs
----

Install docfx once:
```
dotnet tool install -g docfx
```

Serve locally with live-reload:
```
docfx build docfx.json --serve
```
Opens at http://localhost:8080. The landing page (`index.html`) is at the root;
docs are at `/docs/`, API reference at `/api/`.

> **Important:** use `docfx build`, not `docfx` (without subcommand). The bare
> `docfx docfx.json --serve` also runs `metadata`, which overwrites the
> hand-maintained `www/api/toc.yml` with an auto-generated one.

To regenerate canvas SVG figures (only needed when `Xui/Tests/Docs/Canvas/Views/` changes):
```
dotnet test Xui/Tests/Docs/Xui.Tests.Docs.csproj
docfx build docfx.json --serve
```

### API metadata

To regenerate API metadata from C# source (only needed after public API changes):
```
docfx metadata docfx.json
git checkout -- www/api/toc.yml
```

The generated YAML lands in `www/api/` and is committed alongside the source.
After regenerating, commit the changed `.yml` files but not `toc.yml`.
The full site builds to `_site/` (gitignored) and is deployed via GitHub Actions on push to main.

See [`www/docs/PLAN.md`](www/docs/PLAN.md) for the documentation roadmap.

NuGet Packages
--------------

Build all distributable NuGet packages from the repo root:
```
dotnet pack Xui.Pack.slnf -c Release
```

`Xui.Pack.slnf` is a solution filter that lists only the packable library projects.
Using it (instead of `Xui.sln`) avoids restoring app projects that require optional
workloads (`wasm-tools`, etc.) not available on a stock CI runner.

Packages land in `packages/` at the repo root (gitignored). The output path is
configured globally in `Directory.Build.props` via `<PackageOutputPath>`, so you
get the same location regardless of which directory you invoke `dotnet pack` from.

To make a new library packable, do two things:

1. Add `<IsPackable>true</IsPackable>` to its `.csproj` `<PropertyGroup>`.
2. Add its path to the `projects` array in `Xui.Pack.slnf`.

All projects default to `<IsPackable>false</IsPackable>` (set globally in
`Directory.Build.props`), so neither step alone is sufficient.

The CI/CD pipeline (`CD.yml`) runs the same command and uploads the resulting
`*.nupkg` files as artifacts, then pushes them to NuGet.org on merges to `main`.
