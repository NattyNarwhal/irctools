MCS=mcs
MONO=mono
NUGET=nuget

# server
bin/IrcServd.exe: IrcServd.cs bin/SmarIrc4net.dll bin
	$(MCS) -r:bin/SmarIrc4net.dll -o:bin/IrcServd.exe IrcServd.cs

# irc lib dep
bin/SmarIrc4net.dll: bin
	$(NUGET) install -ExcludeVersion -NonInteractive -Verbosity quiet smartirc4net
	cp SmartIrc4net/lib/net40/SmarIrc4net.dll bin/SmarIrc4net.dll

# binary dir
bin:
	mkdir bin

clean:
	rm -rf bin/ SmartIrc4net/
