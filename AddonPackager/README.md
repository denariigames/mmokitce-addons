# MmoKitCE Addon Packager

**Addon Packager** is a custom Unity Editor tool designed for [**MmoKitCE**](https://github.com/denariigames/MmoKitCE) community contributions. It simplifies the process of packaging your addons by:

- Exporting your addon folder as a `.unitypackage` with the required guid file
- Automatically generating a properly formatted `package.json` manifest

This makes it incredibly easy to share your addons with the MmoKitCE community.

<img src="https://github.com/denariigames/mmokitce-addons/blob/master/AddonPackager/dist/screenshot.png" alt="AddonPackager" height="350">

## Features

- **One-click export** of `.unitypackage` + `package.json`
- **Smart GUID handling**  
  - Generate a new unique GUID (default)  
  - Or manually enter a fixed GUID for updates
- **GitHub-ready package URL** auto-generation
- Supports all required `package.json` fields used by MmoKitCE Addon Manager

## Example Output

After exporting **LegolasAimBot**:

**Files created:**

```
LegolasAimBot.unitypackage
package.json
```

**package.json content:**
```json
{
  "name": "LegolasAimBot",
  "guid": "a1b2c3d4-e5f6-7890-g1h2-i3j4k5l6m7n8",
  "packageUrl": "https://github.com/MirkwoodElves/LegolasAimBot/raw/refs/heads/main/LegolasAimBot.unitypackage",
  "version": "1.0.0",
  "updateDate": "2026-01-03",
  "author": {
    "name": "Legolas Greenleaf",
    "url": "https://mirkwood.archery"
  },
  "category": "Combat",
  "description": "Precisely targets enemies with elven accuracy. No one has ever seen it miss.",
  "screenshot": "screenshot.png"
}
```
