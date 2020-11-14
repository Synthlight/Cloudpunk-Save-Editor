Cloudpunk Save Editor
---

Fairly bare-bones. I may add more features in the future, maybe not.

You can edit:
- Inventory, add/delete items, change quantities, etc.
- Player stuff incl. map location, car model, money, times repaired.
- Holocash accounts.

Instructions
---

It'll ask you which file to open on startup.<br>
Ctrl+S opens the save dialog.<br>
That's basically it for the interface.

The default save location is `%LOCALAPPDATA%Low\ION LANDS\Cloudpunk\Slot<slot>.sav`<br>

### Use at your own risk, keep save backups, etc. I shouldn't need to tell you this part.

If you want it, there's an 010 template for the save file.<br>
[Save.bt](Save.bt)

Crashes
---

If there's a crash, it'll get recorded in the Windows even viewer. `eventvwr.msc`<br>
Open an issue with the crash log.

If the exe doesn't appear to do anything when run, check for crash logs there.<br>
The other common cause of this is people not extracting the files from the zip before running it. The dlls **MUST** be in the same directory for the program to work.
Running the exe from the zip will not do this and it'll fail to run.

Prerequisites
---

.Net Core/Desktop 3.1 Runtime is required to run this:
- https://dotnet.microsoft.com/download/dotnet-core/3.1
- (x64) https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-desktop-3.1.7-windows-x64-installer
- (x86) https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-desktop-3.1.7-windows-x86-installer

(Direct links may be out-of-date. Look for the "Desktop Runtime" section in the right column.)