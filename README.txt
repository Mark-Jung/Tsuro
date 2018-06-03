Mark Jung - gujung2022@u.northwestern.edu
Vinay Patel - vinaypatel2018@u.northwestern.edu

# How to run Play-A-Turn
./myplay-a-turn.sh

# How to run tournament player
./player.sh
without any specifications, it will run as name Mark, infinitely, on port 12345, on the local machine

# How to run player with specified inputs
To specify inputs for player: add them in following order, each separated by a space, after this command
dotnet ./TsuroTheSecond/bin/Debug/netcoreapp2.1/TsuroTheSecond.dll 
input:
"player"
name
number of games to play(0 for infinite)
portnumber
ip(default to local)

example: dotnet ./TsuroTheSecond/bin/Debug/netcoreapp2.1/TsuroTheSecond.dll player RobbyisFine 0 12345
