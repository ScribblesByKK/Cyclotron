---
applyTo: "docs/**"
---

# Documentation Instructions

These rules apply to every file under `docs/`. They supplement the workspace-wide rules in `.github/copilot-instructions.md`.

---

## Toolchain

- Documentation is built with **DocFX** from XML comments and Markdown source files in `docs/`.
- The published site is deployed to GitHub Pages via `.github/workflows/docfx.yml`.
- Build locally with:
  ```bash
  dotnet tool install -g docfx
  cd docs
  docfx docfx.json --serve
  ```

---

## Markdown Style

- Use ATX-style headings (`#`, `##`, `###`), not Setext underlines.
- One blank line before and after headings, code blocks, lists, and tables.
- Use fenced code blocks with an explicit language identifier (` ```csharp `, ` ```bash `, etc.).
- Wrap inline code and type/member names in backticks.
- Keep line length reasonable (≤ 120 characters) for readability in diff views.
- Prefer hyphen (`-`) for unordered list items.

---

## Content & Structure

- Every public-facing API page should start with a brief purpose sentence, then usage examples.
- Cross-reference related pages and types using DocFX `@TypeName` xref syntax or relative Markdown links where appropriate.
- Do not duplicate content that is already expressed as XML doc comments — link to the generated API reference instead.
- When adding new conceptual pages:
  1. Add the file under `docs/` in the appropriate sub-folder.
  2. Register it in `docs/toc.yml`.
  3. Keep the `toc.yml` entry label concise (≤ 5 words).

---

## XML Doc Comment Quality (applies to C# source feeding DocFX)

- Every public type and member needs `<summary>` at minimum.
- `<summary>` must describe *intent and behavior*, not restate the member name.
- Use `<param>`, `<returns>`, and `<exception>` for parameters, return values, and documented exceptions.
- Prefer `<inheritdoc />` on overrides and interface implementations.
- Reference other types with `<see cref="TypeName"/>` and members with `<see cref="TypeName.Member"/>`.

---

## CI

- DocFX builds are validated in CI via `.github/workflows/docfx.yml` on every push/PR to `main`.
- A failing DocFX build is a blocking issue — ensure `toc.yml` references point to existing files before committing.
