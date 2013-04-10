# IrcTools

It's a lightweight daemon and a set of ASP.NET controls that expose IRC data without being on IRC.

## Todo

 * Settings
 * Better exception handling
 * Oper stuff (force cache reloads over the wire)

## Building and running

Tweak the IrcSercd.cs and ChannelList.aspx for your server.

Build the IrcServd.cs with the latest SmartIrc4net (included for your convience) and run it with your server as the parameter.

For the controls, embed them on your page (as shown in Sample.aspx) and run the web server, either by putting it in your site directory or for local testing, `xsp2` or `xsp2` in the working directory.

### Usage

Sample.aspx contains how to use the controls on your page. They include:

 * Motd: embed a message of the day
 * ChannelList: a table of channels
