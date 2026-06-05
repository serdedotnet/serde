# Releasing

This document describes the release process for Serde.NET.

## Steps

1. **Bump version**: Update `SerdePkgVersion` in `Directory.Build.props` and submit as a PR.
2. **Draft release**: Create a draft GitHub release with the new tag (e.g., `v0.11.0`) and release notes.
3. **Publish**: After the version PR is merged, trigger the `publish` workflow manually from GitHub Actions. It packs, pushes to NuGet (via OIDC), and finalizes the GitHub release.
