Mark Jung - gujung2022@u.northwestern.edu
Vinay Patel - vinaypatel2018@u.northwestern.edu

# How to run Play-A-Turn
run ./myplay-a-turn.sh

# How to run tournament
First, navigate to stored_directory/TsuroTheSecond/bin/Debug/netcoreapp2.1
Run this command: dotnet TsuroTheSecond.dll server <port_number/> <number of games to play/> <number_of_network_players/> <number_of_total_players/>

After running this command, the program will say what name, color, portnumber, and ipaddress to connect to.
Copy and paste those parameters and go onto the "How to run tournament player"


# How to run tournament player
First, navigate to stored_directory/TsuroTheSecond/bin/Debug/netcoreapp2.1
Run this command: dotnet TsuroTheSecond.dll player <name/> <color/> <number_of_games_to_play/> <port_number/> <ip_address(default to local)/>


Repeat "How to run tournament player" for however many network player you want. Just launch them in separate windows.



