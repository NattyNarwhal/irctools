MCS=mcs
MONO=mono
NUGET=nuget

# server
bin/IrcServd.exe: IrcServd.cs bin/SmarIrc4net.dll
	$(MCS) -r:bin/SmarIrc4net.dll -out:bin/IrcServd.exe IrcServd.cs

# irc lib dep
bin/SmarIrc4net.dll:
	$(NUGET) install -ExcludeVersion -NonInteractive -Verbosity quiet smartirc4net
	mkdir bin
	cp SmartIrc4net/lib/net40/SmarIrc4net.dll bin/SmarIrc4net.dll

clean:
	rm -rf bin/ SmartIrc4net/
