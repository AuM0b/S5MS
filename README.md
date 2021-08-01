# S5MS - Steam Five Minutes Sharing
Steam Family Sharing offline mode abuse program.

This tool blocks Steam backend connection to refresh Family Sharing playtime,
which allows to play more than five minutes when library is being used by it's owner.

Currently, program can operate in two modes:
* **Always block connection:** *self-explanatory.*
* **Periodically block connection:** *block connection for a couple of seconds after every minute.*

## Limitations
* You can't play online properly, because game servers will kick you for being disconnected from Steam, even for short period of time.
* Steam is recommended to be launched with *-tcp* parameter for it to react fast on failed packets.
* Program will overwrite your current Steam status.
* Requires administrator rights to work with Firewall.
