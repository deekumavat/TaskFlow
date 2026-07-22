# TaskFlow Standalone Blazor WebAssembly PWA

This version has no ASP.NET application server and can be hosted on GitHub Pages.

## Google configuration
1. Enable Google Drive API in Google Cloud.
2. Create an OAuth 2.0 Client ID of type **Web application**.
3. Add your GitHub Pages origin to **Authorized JavaScript origins**, for example:
   `https://YOUR_USERNAME.github.io`
4. Replace `REPLACE_WITH_YOUR_GOOGLE_CLIENT_ID` in `wwwroot/appsettings.json`.
5. Do NOT put a Google Client Secret in this project. A standalone browser application is a public OAuth client and cannot securely keep a client secret.

## GitHub Pages
Push the project to the repository root, then go to:
Settings > Pages > Source > GitHub Actions.

The included workflow publishes the application automatically.

## Security / architecture
Task data is stored in each user's Google Drive `appDataFolder`. Access tokens live only in browser memory and expire. The user may need to sign in again after refresh/token expiry.

## Important
Google OAuth consent configuration controls who can use the app. In Testing mode, add users as Test Users. For broader public use, configure the consent screen appropriately and comply with Google's OAuth requirements.
