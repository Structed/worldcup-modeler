# worldcup-modeler

A static **Blazor WebAssembly** app to model the **FIFA World Cup 2026** — pick who
advances, optionally add scores, and watch the knockout bracket fill in. All state is
saved in your browser's `localStorage`; there is no backend.

## Features

- **48 teams across 12 groups (A–L)**, pre-populated.
- **Group stage:** enter scores to auto-rank each group (points → goal difference →
  goals for), or use the arrows to manually override who finishes where.
- **Best eight third-placed teams** are ranked and advance to the Round of 32.
- **Knockout bracket (Round of 32 → Final)** following the official 2026 layout, plus a
  third-place play-off. Winner-first: click the team you think advances; scores are
  optional, with a penalty-shootout toggle for draws.
- Editing an earlier result **cascades** downstream and clears now-invalid picks.
- **Export the knockout bracket as a PNG** image (📷 button on the Bracket page).
- **Reset everything** from the Home page.

## Running locally

Requires the .NET 10 SDK.

```pwsh
dotnet run --project WorldCupModeler
```

Then open the URL shown in the console (e.g. `http://localhost:5139`).

## Publishing as a static site

```pwsh
dotnet publish WorldCupModeler -c Release
```

The static site is emitted to `WorldCupModeler/bin/Release/net10.0/publish/wwwroot`
and can be hosted on any static host (GitHub Pages, Azure Static Web Apps, etc.).

## Deployment (GitHub Pages)

The app is deployed to **GitHub Pages** automatically. The workflow
[`.github/workflows/deploy-pages.yml`](.github/workflows/deploy-pages.yml) runs on every
push to `main` (i.e. when a pull request is merged) and:

1. publishes the Blazor WebAssembly app,
2. rewrites `<base href>` to `/worldcup-modeler/` (the project-site path),
3. adds a `404.html` SPA fallback and a `.nojekyll` marker, then
4. deploys the output via the official GitHub Pages actions.

The site is served at <https://structed.github.io/worldcup-modeler/>.

**One-time setup:** in the repository, go to **Settings → Pages → Build and deployment**
and set **Source** to **GitHub Actions**. This only needs to be done once and cannot be
configured from the repo itself.

## Notes

- Real flag images are bundled locally via the [`flag-icons`](https://github.com/lipis/flag-icons)
  library (`wwwroot/lib/flag-icons/`), so flags render correctly on every platform
  (including Windows, where emoji flags are not supported) and work fully offline.
- The eight best third-placed teams are assigned to their Round-of-32 slots using a
  deterministic rule that avoids same-group rematches — a simplification of FIFA's full
  Annex C lookup table.
- The team field and bracket structure live in `WorldCupModeler/Data/` and can be
  updated for a future edition.
