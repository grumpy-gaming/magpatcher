# Changelog

## [1.1.0] Magrathea fork

- Rebranded for Magrathea: new splash art, gold-on-charcoal theme, gold progress bar
- Added an "Apply 4GB Patch" button (sets the Large Address Aware flag on eqgame.exe, keeps eqgame.exe.bak)
- Files are now downloaded to a temporary file and checksum-verified before the player's copy is replaced
- Replaced files are backed up to Magrathea_Backup\<date>
- No longer refuses to run when eqgame.exe has an unrecognised hash (the 4GB patch changes it); Magrathea is RoF2-only
- Removed the "report unknown client" link to the upstream issue tracker
- Build workflow: newer GitHub actions, fixed PATCHER_URL variable name, release via softprops/action-gh-release
- Placeholder rof/eqhost.txt (the upstream copy pointed at the projecteq login server)

## [1.0.4] 2023-02-11 (upstream)

- Introduced new CICD-friendly pipeline
- Added self updating support (yay)
- Added HTTPS support
- Fixed background threading and context cancellation, so it runs smoother (and cancels smoother)
- Added progress in the taskbar icon
- Added option to point to your self built hosting inside the CICD
- Added a pipeline that makes it easy to pull down upstream changes without impacting your version 
- Fix virustotal from falsely identifying as virus
