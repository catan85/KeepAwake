# KeepAwake
Keeping windows from sleeping.
It avoid Windows to go to sleep.
Every 5 seconds it checks when a configured process has a transfer speed that exeed a configured limit (in Kb/s).
When the speed is over the limit it launch a "Keep awake" loop.
When the speed is below it stops the "Keep awake" loop.

To change the process name and the process speed limit just edit the .config file.

The process must have its name without file extension.

I use it to prevent the sleeping of my HTPC when I'm transferring files from my download box via ftp.
