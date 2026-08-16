Delta Air Force Server Source


Here are some simple tips for instalation.
-Place these files in a folder named tribes/DeltaAirForce/
-If that folder already exists, then just put it in with whatever is already there. It will do just fine.
-Let it run once and shut down normally before setting up your particular server's settings.

DISCLAIMER
First off, this mod and the code contained within (that which is not from original DFT or Tribes, which have their own disclaimers) is my intellectual property. Any attempt to take credit for my property is illegal. If I catch wind of it, you will be dealt with in a manner that justifies the U.S. and International Copyright Laws. If you wish to use all or parts of my code, you may do so without contacting me under two conditions: 1) You provide credit where credit is due. That means you provide credit to me the author of the DAF code, as well as credit to original authors of DFT, and 2) If I ask you remove my code from your mod for any reason, or that you remove the server running DAF or any mod that is obviously based off of it and has not been changed 40% or more for any reasons, you must comply. 
Credit to the authors of Starsiege:Tribes should also be given, but I'm pretty sure no one is going to mistake your mod or mine as being based off not the tribes engine, but instead off of one you or I wrote completely our selves that happened to be perfectly compatible with Tribes. 
I reserve the right to change this disclaimer at any time without notice, to which changes you will still be accountable. If such changes occur, and you disagree with them, you have the right to delete/remove/dispose of all files, documents, papers, and/or other recordings that contain any of the intellectual property in question. If you waver this right, you agree to uphold and agree to the changes. 
By downloading this code, you understand and agree with the above terms. 

Running DAF
Running DAF isn't really like running a normal tribes server. It actualy is a tad more work for you the admin. There are a few things to watch out for. 
---Stealing vehicles in this mod is MUCH more detrimental then in most other mods. Vehicles generally have a LOT of TE and time invested into them. 
---Team work is a must. If there is no teamwork, the game just kinda stinks, and your team is going to get their ass kicked. I would advise admin enforced teamwork. 
---Make sure players know and understand that Team Energy IS enabled in DAF. There are very few mods where this is true, so most players won't even know what it is. 
---Make sure players understand that flying aircraft over to crash into bases and try to attack with a pistol is bad. It is a VERY large waste of money, and in missions where the number of vehicles is restricted, a waste of vehicles. Encourage, or enforce, players to return their vehicles to the base for reloads and repairs when they finish their runs, and to leave it for other players' use if they are done (make sure other players know they can use it) 
---Spawn time should be kept at a range of 10 to 20 seconds. There is a very good reason for this. It encourages people to stay ALIVE, to use medics, and to not cnt-k just because they lost an arm or a leg. It encourages them to be more frugil with their player's lives, because it makes them no longer free: when they die, they have to pay in time that they are not playing. 
---Do NOT name your server "HAPPYLAND" or "HAPPYLAND2" or "N-Happyness" or "The Bunnies" or any derivation there of. Those names are reserved for my server, and are sanctioned by the same disclaimer as printed above (exchange any mention of code with mention of these words and your all set). These names are mine and my creations. Try a bit of creativity and make your own up, although for this mod I would actually suggest a bit more of a serious tone. The reason the server and teams were named as they were, was because of a very old long running joke turned habbit between a good friend and me. 
---If you plan on putting any weapons into the game, keep a few critical factors in mind; balance, function, and reality. First and formost, think about how it is going to balance. Ive had lots of people request that i put in such things as a sniper rifle that is more accurate then any of the ones currently in the game, can hold up to 30 bullets a clip, and can blow up or disable tanks in one hit. Yes, i know such a sniper rifle exists, and in the hands of a very experienced militant, it can do all that. But for ingame purposes, it is COMPLETELY illogical. It would make all sniper rifles and the LAW obsoleate. People also have requested I put in dune buggies. Doing so, unfortunately, would completely rape the idea of team work in the mod. No matter how much you want to try and argue, it WILL be used for suicide missions, just as the jets (unfortunately) are already. Seeing as how it would be cheaper, and much less defenseable then a jet, it would be used for this purpose even MORE. I have had people also request that i put in F/A-18 Hornets, and F-22 Raptors. The reason why the only non-mamoth jets in the mod are the F-14, F-16 and A-10, are because they each fullfill its own roll. F-14 is a full air superiority, with light ground assult capabilities, the F-16 is a good fighter with good ground assult capabilities, and the A-10 is a poor fighter with excelent ground assult capabilities. Adding in the F/A-18 would basically make the F-16, and possibly the A-10 and F-14 obsoleate IN GAME. Similarly, adding in the F-22 would basically make the F-14 and possibly the F-16 Obsoleate. These are just a few examples. On the side of funcionality, i've had requests to add in things such as body armor, helmets, poison antidote and the like. Most if not all have the same problem. When would you encounter a situation where you WOULDNT bother buying them? Hence, everyone would ALWAYS have them. With the armor and helmet more specifically, these factors are already built into the armors of the mod. It is because of this that the large armor classes can take more bullets in the chest then other classes. Lastly, is reality. This was supposed to have a large basis in reality. Thats why there are no jetpacks, thats why you bleed, and thats why there are no lasers. Granted, there are MANY unrealistic things about the mod, but many of those are due to the unchangeable engine, serverside requirements, time and complexity. So, when adding new things in, ideas such as "plasma" isn't going to work. One last note, more along the balance aspects, is the idea of upgrading the weapons. The weapons actually are pretty accurate dispite the bad look of perspective. But I do realize that in real life, the guns are MUCH more accurate then they currently are. But reflect on this... in tribes, it is VERY easy to hit things with accurate weapons. Visit any server with perfectly accurate sniper rifles (and decent players) and you can see this. Upgrading the weapons from where they are now, will potentially make the mod a completely slaughterfest, instead of the contemplative, patient teamwork mod that it was intended. 

DEDICATED SERVER OPTIONS
These are options that you can use (type them into the shell) when on a dedicated server. They should also work if your using a listen server.
-$NoTalk = <bool>; Setting notalk to true will mute all use of global chat, and global commands. Actually does increase teamwork, and shuts the annoying spammers, and flamewars off.
-$NoRepeat = <bool>; Setting this to true will give anyone trying to repeat a message on global a nice little message instead.
-$NoLoudTalk = <bool>; Setting this to true will mute all use of sound playing in global messages, giving them a message instead of everyone else.
-$SandBagOwner = <bool>; Setting SandBagOwner to true should display the owner (person that layed it) of the sandbag on the shell every time it is damaged. Handy for finding out who bagged up an enterence so that you can kick them.
-$NukeThroughPrefabs = <bool>; This will disable/enable nuking through prefabs (acts as if the prefabs and terrains arnt even there). I believe it resets to true every mission restart, but is disabled on D&D missions such as war.
-$STEAMTALK = <bool>; Disables the teamchat echo on the shell. Does NOT effect it in game. Makes the shell cleaner so you can concentrate on bugs ect.
-$STALK = <bool>;  Disables the globalchat echo on the shell. Does NOT effect it in game. Makes the shell cleaner so you can concentrate on bugs ect.
-$SKILL = <bool>;   Disables the death/kill echo on the shell. Does NOT effect it in game. Makes the shell cleaner so you can concentrate on bugs ect.
-$SSPAWN = <bool>;   Disables the spawn echo on the shell. Does NOT effect it in game. Makes the shell cleaner so you can concentrate on bugs ect.
-$STKILL = <bool>;  Disables the teamkill echo on the shell. Does NOT effect it in game. Makes the shell cleaner so you can concentrate on bugs ect.
-$STHROW = <bool>;   Disables the "throw"(mines/grenades) echo on the shell. Does NOT effect it in game. Makes the shell cleaner so you can concentrate on bugs ect.
-$SDEPLOY = <bool>;   Disables the deploy echo on the shell. Does NOT effect it in game. Makes the shell cleaner so you can concentrate on bugs ect.
-SAllOff(); Turns the 7 "S" options all off.
-SAllOn(); Turns the 7 "S" options all on.
-SDefault(); Resets the 7 "S" options to their defaults.
-$TKWARNING = <int>;  This value sets on what TK (and after) the client recieves a warning about their tks.
-$TKKILL = <int>;  This value sets on what TK (and after) the client recieves a kill for their tks.
-$TKKICK = <int>;  This value sets on what TK (and after) the client recieves a kick for their tks.
-listplayers();  Will display a list of all the players, their ID number, their IP, their team, their TKs, and their score.
-ban( <clientID>, "<message(optional)>" ); Should add the specified client to the normal tribes ban list (and save the list incase you dont exit normally), and kick them. If you give a message, that message will be displayed when they are booted.
-net::kick( <clientID>, "<message(optional)>" ); Should kick the specified client and display the message if given.
-chat( "<message>" ); Will display a message in the chatbox to all users.
-admintalk( "<message>" ); Will display a message at the bottom of everyone's screen, and let them know its from the admin.
-bombtarget( <clientID>, <bombprojectiletype> ); Will spawn a bomb above the targets head. If you dont specify a person or a bomb, it wont do anything. A fun one is the nukeshell.... 
-ResetTK( <clientID(optional)> ); If a client is specified, it will reset their TK count. If one isnt, then it resets everyone's TKs.
-ClearScreen(); Clears the shell screen.
-Server::loadmission(<name of mission>); Loads the specified mission.

KNOWN BUGS
-Pretty sure there is a memory leak somewhere. It will be because of this that the server will crash occasionally, and almost always when you try to loadup DF_HeartOfDarkness.
-There is a crash bug somewhere. It makes the server crash. Not sure where or what, or else it will be fixed.
-There are some remote bottomprints sent to the players that arnt getting there, because they are sent to their player objects instead of their server ID numbers.
-There are some weapons (bombs probably) that still give a NULL attacker because their owner wasnt set right. Some even cause crashes (i think the nuke now consistantly does this). The crash would be because they have been given the player's object number, instead of their server ID number.
-Tank code blows. I will probably update it when Ice finishes the new code he has been working on for DUNE and for DAFClientside.
-People are placed below the terrain and fall when entering the howitzer turret. THis is because it sets your position behind it, and if you set down on a hill, it sets you below the terrain. To fix this, an LOS set above and aimed at the ground to get the position of the hill behind, and THEN set the person down is what is needed. Examples are in the tourny.cs file.
-Howitzer shells can land next to you and do no damage. probably some bazar interaction with the terrain. When it hits, if the explosion is made to expload JUST above the terrain, then this should fix it.
-Probably other things i am either not aware of, or cannot remember.

If you fix, or find a way to fix any of these bugs, please email me so we can set up to get that new fixed code in the mod, so i can release an update. You will be added to the contributer's list if you do so.

Make sure to put the maps in tribes/DeltaAirForce/Missions/ after you install this source.

Btw, youll probably notice a new file being created in your config folder called "RacistBastards.cs" All this is, is a file containing people that like to be racist bastards on your server. You can ignore or deal with it as you like. Its your server. If you would like to disable it, then go to comchat.cs and find where it parses the chat messages and checks for derogitory language, and remove the relevent code there.



Made by Jesus 11/1/02

Questions can be sent to Deathknight@hehe.com or posted on the forum at www.jmods.bravepages.com