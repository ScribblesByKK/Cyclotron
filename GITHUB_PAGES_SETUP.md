# GitHub Pages Setup Instructions

After merging this PR, the repository owner needs to enable GitHub Pages with GitHub Actions as the source.

## Steps to Enable GitHub Pages:

1. Go to the repository on GitHub
2. Click on **Settings** tab
3. Scroll down to **Pages** in the left sidebar
4. Under **Build and deployment**:
   - Set **Source** to **GitHub Actions**
5. Save the changes

## Verification

Once enabled:
1. The GitHub Actions workflow will run automatically on push to main
2. Check the **Actions** tab to see the deployment progress
3. Once complete, the documentation will be available at:
   
   **https://kumara-krishnan.github.io/Cyclotron/**

## Troubleshooting

If the deployment fails:
- Check the Actions tab for error logs
- Ensure the repository has Actions enabled
- Verify that GitHub Pages is enabled in Settings
- Check that the workflow has proper permissions (already configured in the workflow file)

## Local Development

To test documentation changes locally before pushing:

```bash
cd docs
docfx docfx.json --serve
```

This will start a local server at http://localhost:8080
