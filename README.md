# IrcTools

It's a lightweight daemon and a set of ASP.NET controls that expose IRC data without being on IRC.

## Todo

 * Settings
 * Better exception handling
 * Oper stuff (force cache reloads over the wire)

## Building and running

As of right now, there is nothing to configure - cache delays, username, etc. cannot be changed.

Build the IrcServd.cs with the latest SmartIrc4net (included for your convience) and run it with your server as the parameter. (e.g: `mono IrcServd.exe irc.example.com`)

For the controls, embed them on your page (as shown in Sample.aspx) and run the web server, either by putting it in your site directory or for local testing, `xsp2` or `xsp4` in the working directory.

### Usage

Sample.aspx contains how to use the controls on your page. They include:

 * Motd: embed a message of the day
 * ChannelList: a table of channels
