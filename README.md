# IrcTools

It's a lightweight daemon and a set of ASP.NET pages (that are really unpolished, and there's only one for now) that expose IRC data without being on IRC.

Right now, it has no caching, (flooding for /list protection can break it) so I wouldn't recommend running it at until that's ready.

Code is self-explanatory

## Building and running

Tweak the IrcSercd.cs and ChannelList.aspx for your server.

Build the IrcServd.cs with the latest SmartIrc4net (included for your convience) and run it with your server as the parameter.

Run xsp in the folder.
