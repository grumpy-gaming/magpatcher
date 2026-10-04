# Magrathea Patcher

The client patcher for **Magrathea**, a Hitchhiker's-Guide-themed EverQuest (EQEmu / RoF2) server.

> You must own your own copy of the RoF2 client. This patcher does not provide the EverQuest client.

Based on [eqemupatcher](https://github.com/xackery/eqemupatcher) by xackery, with the 4GB-patch idea borrowed from
THJPatcher. Both are GPL v2, and so is this.

## For players

1. Download `MagratheaPatcher.exe` from the [latest release](../../releases/latest).
2. Put it in your RoF2 folder (the one that contains `eqgame.exe`) and run it.
3. Press **Patch**, then **Play**. Play starts the game with the `patchme` argument for you.

Extras:

* **Apply 4GB Patch** lets EverQuest use more memory (helps prevent crashes). Close the game first. A backup of
  your original is saved as `eqgame.exe.bak`.
* Every file the patcher replaces is first copied into `Magrathea_Backup\<date>` in your game folder.
* Windows SmartScreen may warn about an unsigned program. Choose *More info*, then *Run anyway*.

## For staff: publishing a patch

Files that players receive live in the [`rof/`](rof) folder, laid out exactly like the player's EverQuest folder:

```
rof/eqhost.txt
rof/spells_us.txt
rof/dbstr_us.txt
rof/dinput8.dll
rof/Resources/SkillCaps.txt
rof/Resources/BaseData.txt
```

1. Copy updated files into `rof/` (everything in that folder is shipped to players, so keep it to patch files only).
2. Commit and push to `master`.
3. GitHub Actions builds `MagratheaPatcher.exe`, works out a checksum for every file in `rof/`
   (`filelist_rof.yml`), and publishes a new release. The patcher compares those checksums, so players only
   download files that changed.

`rof/eqhost.txt` currently holds a **placeholder** address. Set it to your real login server before releasing.

### Settings

The defaults come from `.github/workflows/build.yml` and can be overridden with repository variables
(Settings, Secrets and variables, Actions, Variables): `SERVER_NAME`, `FILE_NAME`, `FILELIST_URL`, `PATCHER_URL`,
`STORAGE_URL`. By default files are served from this repo through `raw.githubusercontent.com`
(the repo must be public for that), and the patcher and filelist from this repo's releases.

### Splash image

`EQEmu Patcher/EQEmu Patcher/Resources/rof.png` (400 x 450 PNG) is compiled into the program.

### Deleting old files from players' folders

List one file per line in `rof/delete.txt`. Be careful with this feature.

## Credits and license

GPL v2 (see `LICENSE`). Original patcher: xackery/eqemupatcher. 4GB patch idea: THJPatcher.
Magrathea is not affiliated with, endorsed by, or connected to Daybreak Game Company.
