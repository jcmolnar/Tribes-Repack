newObject(PlayChatMenu, ChatMenu, "Root Menu:");
newObject(CommandChatMenu, ChatMenu, "Command Menu");

function setPlayChatMenu(%heading)
{
   $curPlayChatMenu = %heading;
}

function setCommanderChatMenu(%heading)
{
   $curCommanderChatMenu = %heading;
}

function addPlayTeamChat(%text, %msg, %sound)
{
   if(%sound != "")
   {
      %msg = %msg @ "~w" @ %sound;
   }
   if($curPlayChatMenu != "")
   {
      %text = $curPlayChatMenu @ "\\" @ %text;
   }
   addCMCommand(PlayChatMenu, %text, say, 1, %msg);
}

function addPlayChat(%text, %msg, %sound)
{
   if(%sound != "")
   {
      %msg = %msg @ "~w" @ %sound;
   }
   if($curPlayChatMenu != "")
   {
      %text = $curPlayChatMenu @ "\\" @ %text;
   }
   addCMCommand(PlayChatMenu, %text, say, 0, %msg);
}

function addPlayAnim(%text, %anim, %sound)
{
   if($curPlayChatMenu != "")
   {
      %text = $curPlayChatMenu @ "\\" @ %text;
   }
   addCMCommand(PlayChatMenu, %text, messageAndAnimate, %anim, %sound);
}

function addLocal(%text, %sound)
{
   if($curPlayChatMenu != "")
   {
      %text = $curPlayChatMenu @ "\\" @ %text;
   }
   addCMCommand(PlayChatMenu, %text, localMessage, %sound);
}

function addPlayCMDResponse(%text, %action, %msg, %sound)
{
   if(%sound != "")
      %msg = %msg @ "~w" @ %sound;
   if($curPlayChatMenu != "")
      %text = $curPlayChatMenu @ "\\" @ %text;
   addCMCommand(PlayChatMenu, %text, remoteEval, 2048, "CStatus", %action, %msg);
}

function addCommandResponse(%text, %action, %msg, %sound)
{
   if(%sound != "")
      %msg = %msg @ "~w" @ %sound;
   if($curCommanderChatMenu != "")
      %text = $curCommanderChatMenu @ "\\" @ %text;
   addCMCommand(CommandChatMenu, %text, remoteEval, 2048, "CStatus", %action, %msg);
}


function addContextCommand(%text, %type)
{
   if($curCommanderChatMenu != "")
      %text = $curCommanderChatMenu @ "\\" @ %text;
   addCMCommand(CommandChatMenu, %text, contextCommand, %type);
}

function addCommand(%text, %action, %msg, %sound)
{
   if(%sound != "")
      %msg = %msg @ "~w" @ %sound;
   if($curCommanderChatMenu != "")
      %text = $curCommanderChatMenu @ "\\" @ %text;
   addCMCommand(CommandChatMenu, %text, setIssueCommand, %action, %msg);
}

// Player Chat menu

setPlayChatMenu("vOffense");
    		addPlayTeamChat("aAttack!", "Attack!", attack);
    		addPlayTeamChat("bAttack the enemy base!", "Attack the enemy base!", "attbase");
    		addPlayTeamChat("cCease fire!", "Cease fire!", cease);		
    		addPlayTeamChat("dTake Cover!", "Take cover!!", takcovr);
    		addPlayTeamChat("eRegroup!", "Regroup!", regroup);
    		addPlayTeamChat("fIncoming", "INCOMING RED GUYS!", incom2);
    		addPlayTeamChat("gGoing offense...", "Going offense", ono);	
    		addPlayTeamChat("hHit the deck!", "Hit the deck!", hitdeck);
    		addPlayTeamChat("iIs our base clear?", "Is our base clear?", "isbsclr");	
    		addPlayTeamChat("jCapture the objective!", "Capture the objective!", "capobj");
    		addPlayTeamChat("kOur base is secure", "Our base is secure", "bsclr2");	
    		addPlayTeamChat("lOur base has been taken!", "Our base has been taken!", "basetkn");
    		addPlayTeamChat("mMove out!", "Move out!", moveout);
    		addPlayTeamChat("nAttack the enemy!", "Attack the enemy!", "attenem");	
    		addPlayTeamChat("qATTACK!", "ATTACK!", attac2);	
    		addPlayTeamChat("rRetreat!", "Retreat!", retreat);
    		addPlayTeamChat("sOur base is secure", "Our base is secure", "bsclr2");
    		addPlayTeamChat("tTake Cover!", "Take Cover!", takcovr);
    		addPlayTeamChat("uMount up", "Mount up so we can go regulate these punks!", miscregulators);
    		addPlayTeamChat("vCover me!", "Cover me!", coverme);	
    		addPlayTeamChat("wWait for my signal to attack", "Wait for my signal to attack...", waitsig);
    		addPlayTeamChat("xThe enemy is attacking our base!", "The enemy is attacking our base!", "basundr");
    		addPlayTeamChat("yOur base has been taken!", "Our base has been taken!", "basetkn");	
    		addPlayTeamChat("zAPC Ready to go", "APC Ready to go, waiting for passengers...", waitpas);
    		addPlayTeamChat(" Is our base clear?", "Is our base clear?", "isbsclr");

setPlayChatMenu("tTarget");
		addPlayTeamChat("aTarget acquired", "Target Acquired", tgtacq);
		addPlayTeamChat("fFire on my target", "Fire on my target", firetgt);
		addPlayTeamChat("nTarget needed", "I need a target.", needtgt);
		addPlayTeamChat("oTarget out of range", "Target out of range.", tgtout);
		addPlayTeamChat("dDestroy Enemy Generator", "Destroy the enemy generator.", desgen);
		addPlayTeamChat("eEnemy Generator Destroyed", "Enemy generator destroyed.", gendes);
		addPlayTeamChat("tDestroy Enemy Turret", "Destroy enemy turret.", destur);
		addPlayTeamChat("sEnemy Turret Destroyed", "Enemy turret destroyed.", turdes);

setPlayChatMenu("dDefense");
		addPlayTeamChat("iIncoming Enemies", "Incoming enemies!", incom2);
		addPlayTeamChat("aAttacked", "We are being attacked.", basatt);
		addPlayTeamChat("eEnemy is attacking base", "The enemy is attacking our base.", basundr);
		addPlayTeamChat("nNeed more defense", "We need more defense.", needdef);
		addPlayTeamChat("bDefend our base", "Defend our base.", defbase);
		addPlayTeamChat("gGo on the defensive", "Go on the defensive.", godef);
		addPlayTeamChat("dDefending base", "Defending our base.", defend);
		addPlayTeamChat("tBase Taken", "Base is taken.", basetkn);
		addPlayTeamChat("cBase Clear", "Base is secured.", bsclr2);
		addPlayTeamChat("qIs Base Clear?", "Is our base clear?", isbsclr);

setPlayChatMenu("fFlag");
		addPlayTeamChat("tMine flag", "Mine the flag.", mineflg);
		addPlayTeamChat("gFlag gone", "Our flag is not in the base!", flgtkn1);
		addPlayTeamChat("eEnemy has flag", "The enemy has our flag!", flgtkm2);
		addPlayTeamChat("hHave enemy flag", "I have the enemy flag.", haveflg);
		addPlayTeamChat("sFlag secure", "Our flag is secure.", flaghm);
		addPlayTeamChat("rReturn our flag", "Return our flag to base.", retflag);
		addPlayTeamChat("fGet enemy flag", "Get the enemy flag.", geteflg);
		addPlayTeamChat("mFlag mined", "Our flag is mined.", flgmine);
		addPlayTeamChat("cClear mines", "Clear the mines from our flag.", clrflg);
		addPlayTeamChat("dMines cleared", "MINE??", mineclr);

setPlayChatMenu("rNeed");
		addPlayTeamChat("rNeed Repairs", "Need repairs.", needrep);
		addPlayTeamChat("aNeed APC Pickup", "I need an APC pickup.", needpku);
		addPlayTeamChat("eNeed Escort", "I need an escort back to base.", needesc);
		addPlayTeamChat("tNeed Ammo", "Can anyone bring me some ammo?", needamo);

setPlayChatMenu("eTeam");
		addPlayTeamChat("wWatch Shooting", "Watch where your shooting!", wshoot3);
		addPlayTeamChat("hHi", "Hi.", hello);
		addPlayTeamChat("bBye", "Bye!", bye);
		addPlayTeamChat("zDoh!", "Doh!", oops1);
		addPlayTeamChat("fHow'd that feel?", "How'd that feel?", taunt10);
		addPlayTeamChat("cAh Crap!", "Ah Crap!", color7);
		addPlayTeamChat("oOops!", "Oops!", oops2);
		addPlayTeamChat("rShazbot!", "Shazbot!", color2);
		addPlayTeamChat("xArgh!", "Argh!", dsgst4);
		addPlayTeamChat("dDont know", "I don't know.", dontkno);
		addPlayTeamChat("nNo", "No.", no);
		addPlayTeamChat("yYes", "Yes.", yes);
		addPlayTeamChat("6Sigh", "*sigh*", dsgst5);
		addPlayTeamChat("gWait", "Don't return the flag yet!", wait1);
		addPlayTeamChat("eReady", "Tell me when to return it.", ready);
		addPlayTeamChat("qDamnit", "Damnit!", color6);
		addPlayTeamChat("tThanks", "Me love you long time.", thanks);
		addPlayTeamChat("aNo Problem", "No Problem.", noprob);
		addPlayTeamChat("sSorry", "Sorry.", sorry);
		addPlayTeamChat("uWoo-hoo!", "Woo-hoo!", cheer2);
		addPlayTeamChat("iYeah!", "Yeah!", cheer1);
		addPlayTeamChat("jAlright!", "Alright!", cheer3);
		addPlayTeamChat("vSP - They took our jobs!", "They took our jobs!", tookurjob);
		addLocal("uHurry station", hurystn);
		
setPlayChatMenu("sIncoming Enemies - Direction");
		addPlayTeamChat("wIncoming North", "*** INCOMING NORTH ***", incom2);
		addPlayTeamChat("aIncoming West", "*** INCOMING WEST ***", incom2);
		addPlayTeamChat("sIncoming East", "*** INCOMING EAST ***", incom2);
		addPlayTeamChat("zIncoming South", "*** INCOMING SOUTH ***", incom2);
		addPlayTeamChat("dIncoming HEAVIES", "*** INCOMING HEAVIES ***", hitdeck);

setPlayChatMenu("wWazzzuuup"); 
                addPlayChat("1Wazzup 1", "WAZZZZZUUUUP!", zup1);
                addPlayChat("2Wazzup 2", "Wasssuuuuuuuuuuuuuuup!?", zup2);
                addPlayChat("3Wazzup 3", "Wuzzzaaaaaaaaaaaaa?!", zup3);
                addPlayChat("4Wazzup 4", "Wazzzaaaaap....blaaaaaaah!", zup4);
                addPlayChat("5Wazzup 5", "WAAAaAaAASAAAaAAAP!?!!!", zup5);
                addPlayChat("6Wazzup 6", "Whasssuup?!!", zup6);
                addPlayChat("7Wazzup 7", "Suuuuuuuuup ha ha!", zup7);
		addPlayChat("8Wasssabi 1", "Waassaaaaaaaaabi?!", zup9);
		addPlayChat("9Wasssabi 2", "Wasssaabi!!", zup10);
                addPlayChat("aBud", "Watchin' the game, havin' a Bud.", zupbud);
                addPlayChat("bDookie", "Yo! Where's Dookie?", herduki);
		addPlayChat("cYo Dookie", "Yo Dookie!", zup8);
                addPlayChat("dLong", "Wasssuuuuuuuuu AAAAAh EEEHEEeh AAAAAHh WOO!", zuplong);
                addPlayChat("eWassupB", "So, whassup B?", zupbee);
                addPlayChat("tTrue", "True, True.", zuptrue);
                addPlayChat("fFPS Doug - MyPC OWNS", "So like uh, here's my PC here. It owns. IT OWNS!", mypc);
                addPlayChat("gFPS Doug - Join army", "Sometimes I think maybe I wanna join the army. I mean it's basically like FPS except better graphcis.", joinarmy);
                addPlayChat("hFPS Doug - Headshot", "BOOM, HEADSHOT!", boomhst);
                addPlayChat("jFPS Doug - Headshot2", "BOOM! HEADSHOT, YEAH!", boomhsYEAH);
                addPlayChat("kFPS Doug - Dance all day", "I can dance all day, I can dance all day. Try'n hit me, come on!", dance);
                addPlayChat("lFPS Doug - Gun Head", "Anytime I get a gun in my hand it just automatically points at somebodys head.", gunhead);
                addPlayChat("mFPS Doug - Jog?", "Wanna go for a jog man? We can go for a jog..", jog);
                addPlayChat("nFPS Doug - Laggy POS", "FUCKING LAGGY PIECE OF SHIT!", laggypos);
                addPlayChat("oFPS Doug - Knife?", "OK but, what are you doing with a knife?.", whatknife);
                addPlayChat("pFPS Doug - Run faster knife", "What do you mean? I run faster with a knife.. Everyone runs faster with a knife..", runfaster);addPlayChat("tTrue", "True, True.", runfaster);
                addPlayChat("qFPS Doug - Yeah N00b", "YEAAHAHAHAHA  n00b! Take that you bitch!", yeahnoob);
                addPlayChat("rFPS Doug - Keybaord smash", "Oh yea you - oh you fuckin like that? You motherfucker! Fuckin god damn lag! AHH FUCK! *smashes keyboard*", youlikethatlag);
                addPlayChat("sATHF - Mr.Lazer", "If you have a problem with that maybe you should take it up Mr. Lazer. Yeah Mr. Lazer! Here it comes, you will be destroyed!", lazer2);
                addPlayChat("uATHF - Roommate", "Your roommate is a nerd. Yes, on the moon, nerds get their pants pulled down and they are spanked with moon rocks.", roommatenerd);
                addPlayChat("vATHF - The Bird", "Shoot him the bird!", shootbird);
                addPlayChat("wATHF - The Bird2", "I hope he can see this because I'm doing it as hard as I can,", seethis);
                addPlayChat("xATHF - The Bird3", "Err, hand out the free cigarettes. We smoke as we shoot the bird!", smokebird);
                addPlayChat("yATHF - Whiskey?", "Now where's my whiskey? I'm gon get tore up!", toreup);
                addPlayChat("[ATHF - Key Car", "Using a key to gouge expletives on anothers vehicle is a sign of trust, and friendship.", keycar);
                addPlayChat("]ATHF - Quad-lazer", "No one can defeat the quad-lazer. It is over now! The bullet is enormous, there is no escaping! Jumping is useless!", lazer1);
                addPlayChat("-ATHF - Quad-lazer2", "No one can defeat the quad-lazer.", lazer4);
                addPlayChat("=ATHF - Quad-lazer3", "The bullet is enormous, there is no escaping..", lazer3);

setPlayChatMenu("gGlobal 1");
		addPlayChat("1Dance", "Dance!", "taunt3");
		addPlayChat("2Waiting", "Waiting.", "wait2");
    		addPlayChat("3Ready", "Ready.", "ready");
    		addPlayChat("4Missed Me", "Missed me!", "taunt2");
    		addPlayChat("5Hey!", "Hey!", "wshoot1");
    		addPlayChat("6Sigh", "*sigh*", "dsgst5");
    		addPlayChat("7Yoo-Hoo!", "Yoo-Hoo!", "taunt1");
    		addPlayChat("8faggit.", "", "fagit");
    		addPlayChat("9Death", "RAPE!", "death"); 
   		addPlayChat("0Karate!", "Somebody grab you by the shirt, you like *WaAaZzAaH!*  Show 'em you ain't playin.", "karate");
    		addPlayChat("aNo problem", "No problem", noprob);	
    		addPlayChat("bBye", "Bye!", "bye");
    		addPlayChat("cCrap", "Ah Crap!", "color7");
    		addPlayChat("dI don't know", "I dont know", dontkno);
    		addPlayChat("eDuh!", "Duh!", dsgst1);	
    		addPlayChat("fShazbot", "Shazbot!", "color2");
    		addPlayChat("gI've Had Worse", "I've had worse.", "tautn11");
    		addPlayChat("hHi", "Hi.", "hello");
    		addPlayChat("iYeah!", "Yeah!", "cheer1");
    		addPlayChat("jAlright!", "Alright!", "cheer3");
    		addPlayChat("kHmmmm...", "Hmmm.", "color3");
    		addPlayChat("mMove outta way!", "Move it!", "outway");
    		addPlayChat("nNo", "No!", no);
    		addPlayChat("oOoops", "Oops!", oops2);
    		addPlayChat("pWait", "Wait.", "wait1");
    		addPlayChat("qDamnit!", "Damnit!", color6);
    		addPlayChat("rHow'd That Feel?", "How'd that feel?", "taunt10");
    		addPlayChat("sSorry", "Sorry.", "sorry");
    		addPlayChat("tThanks", "Thanks!", thanks);
    		addPlayChat("uWhoohoo", "Woo-hoo!", "cheer2");
    		addPlayChat("vCome get some!", "Come get some!", "taunt4");
    		addPlayChat("wArgh!", "Argh!", "dsgst4");
    		addPlayChat("xYou idiot!", "You idiot!", dsgst2);
    		addPlayChat("yYes", "Yes!", yes);
    		addPlayChat("zDoh!", "Doh!", oops1);
    		addPlayChat("-Over Here", "Nice shot Corky.", ovrhere);


setPlayChatMenu("bGlobal 2");
		addPlayChat("aBee's everywhere!", "Bee's in the car, bee's everywhere........God they're huge, they're ripping my flesh off and they sting crazy!", bees);
		addPlayChat("bWhat is the holdup??", "Sweet mother of God, what is the holdup?!?!", holdup);
		addPlayChat("cRetarded", "I was checking the uhh.....ss..specs on the endline......for the rotary...girder......I'm retarded.", retarded);
		addPlayChat("dShut yer yapper!", "Padre....Dome une porvoir, ei qieotae su GRANDE YAPPER.", yapper);
		addPlayChat("eStop This Cruel Game!", "Can we STOP THIS CRUEL GAME?!", cruel);
		addPlayChat("fRolling Doobies", "From what I've heard.....you're using your paper not for writing.....but for rolling doobies.", rollin);
		addPlayChat("gYapper!", "I wish you could just shut your big YAPPER!", yapper2);
		addPlayChat("hWatch Your Language", "Watch yer language in front of the lady, PUNK.", punk);
		addPlayChat("iBOOMSTICK", "This.....is my BOOMSTICK!!", boomstick);
		addPlayChat("jYou ain't leading SHIT", "I got news for you pal, you ain't leading but 2 things right now..........jack and shit, and jack left town.", jackshit);
		addPlayChat("kShe Bitch", "Yo, she-bitch.......let's go.", shebitch);
		addPlayChat("lWho Wants Some?", "Alright.........who wants some??", some);
		addPlayChat("mGimme some Sugar", "Gimme some sugar baby.", sugar);
		addPlayChat("nHail to the King", "Hail to the king baby.", hail);
		addPlayChat("oFargin", "You fargin sneaky basteeds, I gonna take a yo dwork, I gonna nail it to da wall...I gonna cut offa yo arms, I gonna shove 'em up your iceholes!!", fargin);
		addPlayChat("pCorksucker", "You miserable corksucker.", cork);
		addPlayChat("qBooth", "Fuck you, ya fucking upity bitch...I'll fucking fuck you and all yer lesbian fish eating friends.  I'm coming outta da BOOOTH!!!", booth);
		addPlayChat("rHard on", "Why you fucking haaaaaard on, I'll fucking cotton fist ya fucking head with a Louisville fuckin slugger.  What do ya think of that assfuck?!?", hardon);
		addPlayChat("sRepeating", "Weehehell, I already heard that one ya fucking unorginal bastard.  Go suck a cock ya piece of fucking repeating shit.", repeating);
		addPlayChat("tHappy Gilmore", "$?#!$ %*^&! ?$# $!#@?! *&!@$.", bleep);
		addPlayChat("uI Award You No Points", "I award you no points.......and may God have mercy on your soul.", point);
		addPlayChat("vT-t-t-today", "T-t-t-today junior!", today);
		addPlayChat("wShut it", "Ahhahahaha..........shut up.", shutit);
		addPlayChat("xYou too stu-stu-stupid boy?", "What's the matter boy, you too stu-stu-stupid to do what your coach tells you?", aterboy2);
		addPlayChat("yStop making fun of me!", "Stop making fun of me!", aterboy);
		addPlayChat("zShampoo is better!", "Shampoo is bettah!  I go on first and clean the hair.", shampoo);
		addPlayChat("1Conditioner is better!", "Conditioner is bettah!  I leave the hair silky and smooooth.", conditioner);
		addPlayChat("2Asshole!", "Asshole!!!", asshole);
		addPlayChat("3K-K-K-Ken", "Hahahahaha...it's K-K-K-Ken...C-C-Coming to K-K-K-Kill me......", revenge);
		addPlayChat("4I'm Your HUCK!", "I'm your huckleberry!  =)", huckle);
		addPlayChat("5Ate Acid", "You just ate...the most acid I've ever seen anybody eat in my life.", acid);
		addPlayChat("6Ahoy There!", "Ahoy there.....hello!?!  Ahoy mutha fucka!", ahoy);
		addPlayChat("7Don't Bullshit Me", "C'mon....don't bullshit me.", bullshit);
		addPlayChat("8Super Model", "Well can you at least make it taste like chicken?  Otherwise I'm gonna shrivel up like a super model.  *Haaaaah, I am so fat, nobody likes me!  People didn't like me in high school*", model);
		addPlayChat("9Purple Sticky Punch", "Purple sticky punch or, hemp, is a excellent source for photosynynsesis.", purple);
		addPlayChat("0Aww, C'mon!", "Awww C'mon!  What the hell?!", cmon);
		
setPlayChatMenu("cGlobal 3");
		addPlayChat("aYou shot me A-Hole", "OOoowwww.......You shot me you A-HOLE.", ushotme);
		addPlayChat("bI'm gonna eat ya!", "Wait a minute......he kinda looks like a baby.  C'mere, I'm gonna EAT YA!  I'm bigger than ya, I'm higher on the food chain....GET IN MY BELLY, c'mon!", belly);
		addPlayChat("cFirst thing's first", "First things first......where's yer shitter?  I've got a turtlehead poking out.", turtle);
		addPlayChat("dCrap on deck", "I'm not kiddin......I got a crap on deck that could choke a donkey.  OOhhh, it's sqwuinchy!  Oohh damn, I'm getting all emotional from it.", crapdeck);
		addPlayChat("eJudochop", "JudooooCHOP......OOoOOOooOooHHhhHh, right in the mommy daddy button!", judochop);
		addPlayChat("fAlotta Fagina", "Her name is Alotta....Alotta Fagina.  Come again?  Alotta Fagina.  Ahh, I'm sorry I'm just not getting it.  It sounded like you said your name was Alot of uhh....nevermind.", allota);
		addPlayChat("gLucky Charms", "They're always after me lucky charms.", charm);
		addPlayChat("hShits and Giggles", "I'm just trying to get a rise out of you, that's all!  Fer shits and giggles.", giggles);
		addPlayChat("iShorn Scrotum", "There really is nothing like a shorn scrotum.....it's breathtaking, I suggest you try it.", shorn);
		addPlayChat("jMost Annoying Sound", "Hey.....wanna hear the most annoying sound in the world?  AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH.", annoy);
		addPlayChat("kRocky Mountains", "I expected the Rocky Mountains to be a little rockier than this.....I was thinking the same thing.  That John Denver's full of shit man.", RockyMountains);
		addPlayChat("lPet's Heads", "We got no food....we got no jobs.  Our Pet's HEADS ARE FALLIN OFF!!", petshead);
		addPlayChat("mThings YOU wanna do", "Let's do all the things that YOU wanna do.", annado);
		addPlayChat("nBalls are Showing", "Excuse me.......your balls are showing.", balls);
		addPlayChat("oBumble Bee Tuna", "Hi there nice to see ya.  Bumble bee tuna....Bumble bee tuna!", bumble);
		addPlayChat("pBye Bye Then", "Take care now, bye bye then.", care);
		addPlayChat("qSpiritual Creaminess", "One must forgo the self....to attain total spiritual creaminess.", cream);
		addPlayChat("rDamn I'm Good!", "DAMN I'M GOOD......can ya feel that?  Huh?  Can ya feel it?", damn);
		addPlayChat("sRover", "351....351....Rover set, hut hut.", hut);
		addPlayChat("tLoser", "Laaaheeeewwww....saaaaaheeerrrr.", loser);
		addPlayChat("uOuch", "OOOooooouuuuuuuccccchhhhhh.", mary);
		addPlayChat("vYou're fired!", "Step into my office.  Why?  Cuz you're fucking FIRED.", office);
		addPlayChat("wSon of Jerel", "KLAAAAANG.......Come Son of Jerrel, kneel before ZOD.", soj);
		addPlayChat("xSnoochie", "Snoooochie boooochie nooochies!", jay);
		addPlayChat("yMongo", "We got this one kid.....Mongo.  He's got a forehead like a drive-in movie theatre, but he's a good shit.  So we don't bust his chops too much.", mongo);
		addPlayChat("zSuck Dick for Coke", "Marijuana is not a drug.......I used to suck dick for coke.", coke);
		addPlayChat("1Shiznit-o-bam!", "This weed was the Shiznito - bam - snip - snap - sap!", eed);
		addPlayChat("2F*ck you", "Fuck you, fuck you, fuck you......you cool, and fuck you - I'm out", fuckyou);
		addPlayChat("3Hefer with Cheese", "You sonnuva bitch, I'm right behind you!  Turn around and ask me for a hefer with cheese yo.", hefer);
		addPlayChat("4I'm Somebody's Bitch", "I'm somebody's bitch!", somebody);
		addPlayChat("5Welcome to hell", "Welcome to hell, biiiiiiiiitch.", elcome);
		addPlayChat("6Freaky Jason", "Eh.....Please don't kill me freaky Jason......I said please don't kill me freaky Jason.", jason);
		addPlayChat("7Freeze sucker bitch", "Freeeeeze sucker bitch!", freeze);
		addPlayChat("8Naughty Jungle of love", "Jungle....666, the mark of the beast.  NO......naughty, nnnnaughty jungle of love.", jungle);
		addPlayChat("9Fruit Cocktail", "You guys gotta do something, there's this guy Nasty Nate who's after my cocktail fruit, and....everyone here likes fresh fish.  And then the Squirrel Master came out of left field and told me I'm his bitch.....HELP", nate);
		addPlayChat("0Hello Neighbor!", "Good morning my neighbors!  *Hey FUCK you*  YES!  YES!  FUCK YOU TOO!", neighbor);		
		
setPlayChatMenu("yGlobal 4");
		addPlayChat("aApu - See You in Hell", "Thank you for coming.......I'll see you in hell.", apu);
		addPlayChat("bApu - Twinkie", "Silly customer......you cannot hurt a twinkie!", apu2);
		addPlayChat("cSweet Merciful Crap!", "Sweet merciful crap!", homer);
		addPlayChat("dBow Down to Bunghole", "You must bow down to the almighty bunghole!", bow);
		addPlayChat("eBungholio", "Bunghole....hmmm, bunghoooliiooooeeeoooo.", bung);
		addPlayChat("fCornholio!", "I am cornholio!  I need TP for my bunghole.  Would you like to see my bunghole?", bunghole);
		addPlayChat("gGonna Score", "This is it Beavis......we're finally gonna score.", score);
		addPlayChat("hPeek-a-boo", "Peeeeeek-a-boooo!!", peek);
		addPlayChat("i10-6-9'er", "Ten-Six-Niner, Ten-Six-Niner, we got whores in the city, we need backup now!", hore);
		addPlayChat("jMr. CandyAss", "You just made a fatal mistake Mr. Candyass, I hope you know something about hand to hand combat", candy);
		addPlayChat("kMonkey's Uncle", "Well I'll be a monkey's bare-assed uncle.", monkey);
		addPlayChat("lYoda", "hehe", yoda);
		addPlayChat("mEwok Song", "I'll be over here playing with myself.", duncka);
		addPlayChat("nImpressive", "Impressive.", impress);
		addPlayChat("oGood Work", "Good work.", good);
		addPlayChat("pJabba the Hut", "Hahahahahahahahahaha.", jabba);
		addPlayChat("qBantha Poodoo", "You are Bantha poodoo.", bantha);
		addPlayChat("rAll Too Easy", "All too easy.", easy);
		addPlayChat("sCartman Pissed", "I...am....going to $*!?'ing kill you guys....seriously.", sp);
		addPlayChat("tCartman and Garrison", "How would you like to suck my BALLS Mr. Garrison?", garrison);
		addPlayChat("uCartman so Horney", "Hello soldier boy.....me so horny.  Me love you long time.", horny);
		addPlayChat("vSouth Park Africans", "A baba glok, glock baba baba glok......baba gung *click* baba....", sp2);
		addPlayChat("wDelta Airlines 1", "If you bout to be up out dis biatch.....peep Delta.  We be flying all over this bitch.", peep);
		addPlayChat("xDelta Airlines 2", "You laying the cut straight sittin on yo ass and getting yo drink on and yo snack on, while we floss and fly this mofo all over this bitch.", delta);
		addPlayChat("yDelta Airlines 3", "You be back at cho crib chillin with a flat spliff.....thinking we were some crazy ass angel.", spliff);
		addPlayChat("zDelta Airlines 4", "You going?  We fly you dere.  You been?  We done already flew up in there.  We got you covered like a jimmy hat.", delta2);
		addPlayChat("1Suck to Blow", "She's gone from suck to blow!", suckblow);
		addPlayChat("2Fat Bearded Bitch", "Come back you fat bearded bitch!", fat);
		addPlayChat("3Keep Firing!", "Keep firing assholes!", ahole);
		addPlayChat("4I'm Bingo", "Hello, my name is Bingo....I like to climb on things, can I have a banana?  Eek Eek.", brak);
		addPlayChat("5The Colonel", "Oh, I hated the colonel, with his *wee beady* eyes, and that smug look on his face.....Oh, you're gonna buy my chicken! Ooooh!", colonel);
		addPlayChat("6Orange on a Toothpick", "Look at the size of that boy's head.  I'm not kiddin, it's like a orange on a toothpick.", head);
		addPlayChat("7Crying to Sleep", "Now that was off-side wasn't it?  He'll be crying himself to sleep tonight, on his huge pillah.", offside);
		addPlayChat("8Move that Melon", "HEAD! PAPER! NOW!  Move that melon of yours and get the paper if ya can.", paper);
		addPlayChat("9The Donger", "Ooooooohh.....No more yankee my wankee.  The Donger need FOOD.", donger);
		addPlayChat("0What's Happening", "*GONG*  What's a happenin hot stuff?", hot);

setPlayChatMenu("xGlobal 5");
		addPlayChat("aWin 4 Quarters!", "They have this game where you put in a dollar......and you win 4 quarters!!  I win everytime!!", quarters);
		addPlayChat("bBurn in Hell!", "Mommy will be upstairs to kiss you goodnight.  Burn in hell!", hell);
		addPlayChat("cJockey's", "I need the secure packaging of Jockey's.  My boyz need a house.", jockey);
		addPlayChat("dNo Soup for You!", "No soup for you!!", soup);
		addPlayChat("eHey Esai", "Hey esai.....ma kiteh moroto ma nasai makento ma taretos....meh kina netoros.", spanish);
		addPlayChat("fKocked da fuck out", "You got knocked da fuck out!", friday);
		addPlayChat("gSteam Roller", "Take off I'm gonna do the steam roller....take off!  No way!  Uhhh...owwww...steam roller, I'm steam rolling you.", roller);
		addPlayChat("hBites the Dust", "Another one bites the dust.....", bites);
		addPlayChat("iBee's and Dog's", "You know bee's and dog's can smell fear?", dog);
		addPlayChat("jWanna die with a man's gun", "You wanna die with a man's gun, not a little sissy gun like this.", gun);
		addPlayChat("kAwesome", "You're doing great...........Awwwesome.", great);
		addPlayChat("lRun Forrest!", "Run Forrest, Run!", forrest);
		addPlayChat("mBetta check my Police record", "Man, what the hell wrong wichoo??  I don't play tha shit man, you betta check my police record.", hatthehell);	
		addPlayChat("nThe Burb's", "Klopeck......what is that, Slavic?  NO.  Oooohh....bout a 9 on the tension scale Rube.", tension);
		addPlayChat("oBarely hanging on", "You HAVE to take control of yourself, ok?  No, YOU Gotta take control, I'm BARELY hanging on here.", roxbury);
		addPlayChat("pHell Yeah!", "Oh hell yeah!", hellyeah);
		addPlayChat("qGotta Hurt", "Ooooohhh, that's gotta hurt!", hurt);
		addPlayChat("rWalk over, Limp back", "Cooooome on Cletis with yoself.....come over.  You gonna limp back.  You walk over, but you're limping back.", limpback);
		addPlayChat("sWhat is that?", "What is that???  WTF IS THAT???", hat);
		addPlayChat("tStack Shit That High", "How tall are you private?  Sir five foot nine sir!  Five foot nine, I didn't know they stacked shit that high.", stacked);
		addPlayChat("uBrown Stain", "Bullshit!  It looks to me like the best part of you ran down the crack of your mamma's ass and ended up as a brown stain on the mattress.  I think you been cheated!", stain);
		addPlayChat("vGame Over Man", "Game over man, GAME OVER!  Wtf are we gonna do now?!", hudson);
		addPlayChat("wRUUUUMBLE!!", "Leeeet's get ready to ruuuuuuuumble!", rumble);
		addPlayChat("xFreeze Mother Bitches!", "Freeeeeeeeeze mudda bitches!!", mudda);
		addPlayChat("yTaunt You a 2nd Time", "Now go away, or I shall taunt you a second time.", taunt);
		addPlayChat("zPisses me off!", "Now this REALLY pisses me off to no end.", noend);
		addPlayChat("1Update", "Update:  We still have no fucking clue where this guy is.", update);
		addplayChat("2Hallelujah", "Hallelujah.", hal);
		addPlayChat("3Applause", "*Applause*", clap);
		addPlayChat("4Bionic sound", "Hell yeah.", bionic);
		addPlayChat("5Chuck", "Looks like Chuck's gonna put the hotdog, in the bun!", chuck);
		addPlayChat("6Stevie Wonder", "Who the hell's piloting this vessel?!?  Stevie fuckin Wonder!?!?", stevie);
		addPlayChat("7Sumbitch!", "Well let's see how fast this sumbitch can go!", sumbitch);
		addPlayChat("8Dammit Man!", "Dammit man.....I swear, you guys rip on me 13 or 14 more times....I'm outta here.", rip);
		addPlayChat("9Les Play", "Les Play.", lesplay);
		addPlayChat("0Ladies Night", "Kick it.", slam);
		
setPlayChatMenu("hGlobal 6");
		addPlayChat("aFriday - Baptist", "'S'cuse me brotha...what we call drugs at 74th St Baptist church, we call a sinny siiiin sin.", baptist);
		addPlayChat("bFriday - Twenty Sac Nigga", "Well 'round here, between Normandy and Weston.  We call this here a little twenty twen twen, Nigga!", twenty);
		addPlayChat("cFriday - Bathroom", "Don't nobody go in da bathroom...for about 35, 45 minutes.  Somebody open a window.", baroom);
		addPlayChat("dFriday - Pleasure", "I grab a dog.....and I choke him!  And I kick the shit outta him!  And I...all day long my foot up a dog's ass.  Just BANG BANG BANG up his ass.  Thas my PLEASHA.", pleasure);
		addPlayChat("eFriday - DAAMN!", "DAAAAAAMN!!", damn2);
		addPlayChat("fFriday - Break Yoself!", "Break yo'self foo!", brkfool);
		addPlayChat("gFriday - And You Know This", "I was just bullshittin......and you know this, maaaaaan!", justbs);
		addPlayChat("hWaterboy - We Suck Again!", "Ohh no!  We suck again!", boy3);
		addPlayChat("iWaterboy - You Can Do It!", "You can do it.  You can do it ALL NIGHT LONG!", doit);
		addPlayChat("jDumb - Hate Goodbye's", "I hate goodbyes!", hbye);
		addPlayChat("kDumb - How About a Hug?", "Hey.....How about a hug?", hug);
		addPlayChat("lDumb - I Don't Care", "Nooooo...and I don't CARE.", dcare);
		addPlayChat("mDumb - Tractor Beam", "Oh yeah...yeah.  Tractor beam.  Sucked me right in.", tbeam);
		addPlayChat("nDumb - Gimme That Booze", "C'mon, give me that booze you little pumpkin pie hair cutted freak....C'mon.", freak2);
		addPlayChat("oDumb - Kick His Ass Seabass", "Kick his ass Seabass!", seabass);
		addPlayChat("pDumb - I Got Robbed", "I got robbed by a sweet old lady on a motorized cart.  And I didn't even see it coming!", igot);
		addPlayChat("qDumb - Kung Fu", "OOooiieeee Yoosaaah!  Oooiieee Yiieeaaa!", kungfu);
		addPlayChat("rDumb - Doing It Buddy!", "We're really doing tho, aren't we buddy!?", buddy);
		addPlayChat("sAce - Spank You Very Much", "Spank you very much.", acey);
		addPlayChat("tAce - Trainer of Dolphins", "HEINS GEITS VELVET.  I am trainer of dolphins.  Ve are making za dolphins disappear, oon den Roy is coming with the vhite tiger and the stuffing in the pants, oon lefkauhn.", velvet);
		addPlayChat("uAce - Heeeey!", "Heeeeeeeeeeeeeeeeyyyy!", hey1);
		addPlayChat("vAce - Three Darts", "Three darts is too muuuuuch.", darts3);
		addPlayChat("wAce - Now a Yak", "And now....a yak.  *YAAAAAAAK*  *YAAAAHAHAAK*", yak);
		addPlayChat("xCable Guy - Oh Billy", "Oooh Billy!", ohbilly);
		addPlayChat("yChong - You Wanna Get High?", "Hey you wanna get high man?  Does Howdy Doody got wooden balls man?", high);
		addPlayChat("zChong - Wow Man", "Wooow man!!", ow);
		addPlayChat("1Chong - Be Right Back", "Hey hang on you guys...I'll be right back.", brb);
		addPlayChat("2Chong - What's the Hassle?", "What's the hassle man?", hassle);
		addPlayChat("3Cheech - Bye Lard Ass!", "Bye bye lard ass!", lard);
		addPlayChat("4Cheech - We're Smoking Dogshit?", "You mean we're smoking dogshit man?!", dogs);
		addPlayChat("5Office Space - My Stapler", "I believe you have my stapler.", stapler);
		addPlayChat("6Office Space - My O Face", "I'm thinking I might take that new chick from Logistics.  Things go well, I might be showing her my Oh! face.  Oh! Oh! Oh! You know what I'm talking about.....Oh!", oface);
		addPlayChat("7Office Space - RollerCoaster", "She's gonna ride on the 'ol bone rollercoaster.  AAAaahhhhH!", coaster);
		addPlayChat("8Office Space - Alright Peter", "Alright Peter! Oooh, Oooh! Right on.", righton);
		addPlayChat("9Office Space - Excuse Me", "Excuse me senior...may I speak to you please?  I asked for a Mai Thai and they brought me a Pina Colada.  And I said no salt, NO SALT for the Margarita, but it had salt on it.", senior);
		addPlayChat("0Office Space - Have a Problem", "Ahhh...we have sort of a problem here.", problem);
		
setPlayChatMenu("nGlobal 7");
		addPlayChat("aSP - Hide Them Up My Ass", "But they're gonna search you on your way back to your cell.  I know......that's why I have to hide them up my ass.", hide);
		addPlayChat("bSP - Timmeh", "Oooh, livenlow livenlow, Ooohoohrahrah Timmeh!", timmy);
		addPlayChat("cSP - Timmeh 2", "Timmeh!", timmy2);
		addPlayChat("dSP - BaBa Chomp", "Daaah....Baba Chomp!", chewy);
		addPlayChat("eSP - BaChewy Chomp", "Ahh, ba chomp ause da ba chewy chewy chomp.", chewy2);
		addPlayChat("fSP - Jam Thumb in Butthole", "This is the most poisonous snake in this entire region.  And what I'm gonna do...is carefully sneak up on him, and jam my thumb in his BUTTHOLE!", cman);
		addPlayChat("gSP - Snake Is Really Pissed", "Kracky!  Ooh, this snake is REALLY pissed!  I'm gonna jam my thumb in his butthole now....Oooh yeah, that pissed him off alright!", cman2);
		addPlayChat("hSP - I'm Super!", "I'm sssuper!  Thanks for asking!", gayal);
		addPlayChat("iSP - Special Ed Bus", "You have to take the special ed bus!  *DAAAAHHHH*", speced);
		addPlayChat("jSP - I'm a Bad Ass Cowboy", "Well I'm a bad ass cowboy living in a cowboy's age.  Whickey whickey scratch yo yo bang bang.", rappin);
		addPlayChat("kSP - I'm So Pissed", "I....am....so....pissed off.....right now.", sopiss);
		addPlayChat("lBrak - Whaddya doin?", "Hey  hey  hey  hey, whadya doin?  whadya doin?  whadya doin?,  whadya doin?  whadya doin?  whadya doin?  whadya doin?", haadya);
		addPlayChat("mBrak - Don't Touch Me", "Hey!  Don't touch me!", touch);
		addPlayChat("nBrak - Coffee Makes Me Jittery", "I don't like coffee.....it's make me jittery.", jittery);
		addPlayChat("oStrange Brew - Beauty Eh", "I was kinda like a one man force eh, like Charlton Heston in Omega Man...did you see it?  Beauty.", mforce);
		addPlayChat("pStrange Brew - Three B", "This movie was shot in Three-B.  Three beers and it looks good, eh?  Hoseramer.", threeb);
		addPlayChat("qStrange Brew - Take Off Hoser!", "Take off you hoser!", takeoff);
		addPlayChat("rStrange Brew - Take a Leak", "Geeeez, I gotta take a leak so bad I can taste it.", leak);
		addPlayChat("sStrange Brew - You Farted!", "Ooh, you farted!  No it wasn't me....it was the chair, eh?", farted);
		addPlayChat("tTommy Boy - It's a Clipon", "hehe, it's a clip on.  haha, are ya sure?", clipon);
		addPlayChat("uTommy Boy - Candy Shell", "I think your brain has a thick candy shell.  Yer...yer brain has the shell on it.  Are you talking?  Shut up Richard.", tcandy);
		addPlayChat("vTommy Boy - Gotta Hug", "Brothers don't shake hands....brothers gotta hug!", brother);
		addPlayChat("wTommy Boy - Jam a Oar", "YOU BETTER PRAY TO THE GOD OF SKINNY PUNKS THAT THIS WIND DOESN'T PICK UP.  CUZ I'LL COME OVER THERE, AND JAM AN OAR UP YOUR ASS.", jamoar);
		addPlayChat("xTommy Boy - Fat Guy In a Coat", "Fat guy in a little coat!  Fat guy in a little coat!  Don't.  hehe", fatguy);
		addPlayChat("yTommy Boy - Housekeeping 1", "Housekeeping........No thank you, sleeping.", house);
		addPlayChat("zTommy Boy - Housekeeping 2", "Housekeeping you want towel?  No towels, need sleepy.", house1);
		addPlayChat("1Tommy Boy - Housekeeping 3", "Housekeeping you want mint for pillow?  PLEASE GO AWAY LET ME SLEEP FOR THE LOVE OF GOD.", house2);
		addPlayChat("2Tommy Boy - Housekeeping 4", "Housekeeping you want me jerk you off?  What kind of hotel is this.......Oh, it's you.", house3);
		addPlayChat("3SNL - El Nino", "I am El Nino!  All other tropical storms must bow before....El Nino.", elnino);
		addPlayChat("4SNL - El Nino 2", "El Nino is Spanish for.....The Nino.", elnino2);
		addPlayChat("5Beavis - Pull Your Pants Up", "WHAT!?  Hey, how's it going!  Pull your damn pants up boy!  I don't wanna see that.", catch);
		addPlayChat("6Beavis - I'm a Gringo", "I AM THE GREAT CORNHOLIO!!  I'm a Gringo!  I have no bunghole!", gringo);
		addPlayChat("7Butthead - Klingons", "Those guys better look out for the Klingon's near Uranus.", klingon);
		addPlayChat("8Butthead - Hey Baby", "Hi.  Hey baby...uhhhh, huh, huh.", hbaby);
		addPlayChat("9Butthead - Uhhhh Ok", "Uuuuhhhhhhhh....ok.  Huh, huh", uhk);
		addPlayChat("0Butthead - Bathe Her", "Bathe her....and bring her to me!  Huh, huh.", bathe);

setPlayChatMenu("jGlobal 8");
		addPlayChat("aShuttup Fool!", "Shut up foo!", sfool);
		addPlayChat("bShit Salad", "Well life handed me a whole pile of shit.  What am I supposed to make out of that?  Shit salad?", ssalad);
		addPlayChat("cSnap and Pop", "They call me snap and pop cuz I snap and I will POP yo ass in the mouth, don't mess with me man.", spop);
		addPlayChat("dCinderella Story", "Cinderella story, outta no where.  The former greenskeeper now about to become the Master's champion.", cinder);
		addPlayChat("eGot It On!", "Well...not me personally, but a guy I know.  Him and her GOT IT ON!!  WHOOOOOEEE!  No they didn't.", giton);
		addPlayChat("fOklahoma!", "Ok-La-Homa! Oklahoma! Oklahoma! Oklahoma! Oklahoma! Oklahoma! Oklahoma!", oklah);
		addPlayChat("gFranks and Beans!", "Franks and Beans!", mary34);
		addPlayChat("hDennis Rodman", "Dennis Rodman got a coochie!", coochie);
		addPlayChat("iEverybody Freeze!", "Everybody freeze!  Nobody move!", freeze1);
		addPlayChat("jMichael Jackson", "Michael Jackson's a Puerto Rican!", michael);
		addPlayChat("kShitter was full!", "Merry Christmas!  Shitter was full!", sfull);
		addPlayChat("lMy Name is Serge", "How ya'll doing today?  My name is Serge, and how can I help you?", serge);
		addPlayChat("mBaby Ruth", "Ruuuth...Ruuuth...Ruuuth, Baby Ruuuth!", bruth);
		addPlayChat("nStay With Me Mr. Bean", "Will you stay with me forever Mr. Bean?", mrbean);
		addPlayChat("oLil Joke", "Hahahahahahahahaha.......It's a lil joke!", arthur);
		addPlayChat("pSeen my Wiener?", "Have you seen my wiener?", mary35);
		addPlayChat("qRocky Road", "Rocky Road!  hehe", rocky);
		addPlayChat("rWelcome to McDondalds", "Hi welcome to McDonalds, may I take your order please?  Would you like fries with that, how about so...*SLAP*  Ahhh!", blankman);
		addPlayChat("sSpecial Purpose!", "I've got a special purpose!!", jerk2);
		addPlayChat("tHave No Marbles", "You know......you have no.....you have no marbles!  You have no marbles!", nomar2);
		addPlayChat("uBitch Slapped", "*bitch slapped*", bslap);
		addPlayChat("vTasty Burger", "Mmmhmmm....this IS a tasty burger!", burger);
		addPlayChat("wBreak Your Concentration", "Oh I'm sorry, did I break your concentration?", bcon);
		addPlayChat("xOrange Afro", "You know what those things can do?!?  Suck the paint off your house and give your family a permanent orange afro.", afro);
		addPlayChat("yBubble Gum Ass", "Your ass looks like about 150 pounds of chewed bubble gum Pile, do you know that?", bubble);
		addPlayChat("zThat Would Be Great", "Yeeeah...if you could just go ahead and make sure you do that from now on, that would be greeeeat.", could);
		addPlayChat("1Medieval On Yo Ass", "I'm gonna get Medieval on yo ass.", meval);
		addPlayChat("2High Two", "Hah!  Very good, high two, high two!", high2);
		addPlayChat("3Jive Talk", "Some mofo buttah layin it to the bone, jackin me up.", jive);
		addPlayChat("4Wheeze the Juice", "No wheezing the juice.  *wheeze the juice*  No! no, no wheezing the juice!", juice);
		addPlayChat("5I Need You Now", "I need you now, more than ever....I need you.", need);
		addPlayChat("6Big Bootie Ho", "Big bootie ho's.....hump with it!", oochie);
		addPlayChat("7Peace Out", "Ok, peace out!", peace);
		addPlayChat("8Kiss My Ass", "You people can kiss the fattest part of my ass.", kass);
		addPlayChat("9Light Up a Doobie", "Hey I outta just give you some beer....goes right through ya.  Wonderful!  And while we're at it we can light up a doobie, and watch porn.", porn);
		addPlayChat("0Humpty Song", "*slap*  Ahhh!", humpty2);

setPlayChatMenu("1Global 9");
		addPlayChat("aLooney - Yipe Dog", "Yarraaharaarara!", yipe);
		addPlayChat("bLooney - Wile E. Coyote", "Wile E. Coyote....supergenious.  I like the way that rolls out!", ile);
		addPlayChat("cLooney - My Name is Mud", "Allow me to introduce myself....my name is mud.", mud);
		addPlayChat("dLooney - Throw Him Out!", "Throw him out!  Throw him out!  *WHAP*  SHADDUP.", thim);
		addPlayChat("eLooney - Meep One", "Plbpt plbpt plbt!  Meep meep!", meep);
		addPlayChat("fLooney - Meep Two", "Buuuh Bye!", meep2);
		addPlayChat("gLooney - Kill the Wabbit", "Kill the wabbit, kill the wabbit, kill the wabbit!", krabbit);
		addPlayChat("hLooney - I Got a Rabbit!", "I got a lil rabbit in this hole!  And I'm gonna catch the lil rabbit and eat 'em up!", rabbit);
		addPlayChat("iLooney - Where'd That Rabbit Go?", "Now where'd that skunk of a rabbit go?", srabbit);
		addPlayChat("jLooney - I Hates Rabbits", "I hates rabbits.", hrabbit);
		addPlayChat("kLooney - What You Know", "Well what choo know about that?", uknow);
		addPlayChat("lLooney - I Hate You - Daffy", "I hate you.", hateu);
		addPlayChat("mLooney - I Hate You - Sam", "I hate you.", hateu2);
		addPlayChat("nHalf Baked - Sux To Be You", "Aaahahaha....sucks to be you man!", sux);
		addPlayChat("oHalf Baked - My Nads", "My nads....OoOooHhh.", nads);
		addPlayChat("pHalf Baked - Singing in Shower", "All byyyyy myseeeelf!", myself);
		addPlayChat("qRaise Ten Percent", "All we gotta do is raise ten percent of one million yo.  Which by our calculations i..Fucking impossible man!", tperc);
		addPlayChat("rILC - I'm That Bad!", "Well, uh....that was nice, but you didn't really touch her.  I didn't have to.  *GEEEE*  I'm that bad.", thatbad);
		addPlayChat("sILC - Frenchy", "Hey Rouzee....*HIYAH*  Come on down on man....what ya drinkin man.", frenchy);
		addPlayChat("tILC - Funky Finger Productions", "Now wake up the dead cuz Funky Finger Production about to....*YIAYAH*  Goin upside yo head!", funky2);
		addPlayChat("uCome Get Some", "Venido consiga algo.", east2);
		addPlayChat("vGHEY", "I'm super.", ghey);
		addPlayChat("1Uuooh", "", uuooh);
		addPlayChat("2Dolly", "Ally lam whetcha doOMbaye...", dolly);
		addPlayChat("3Ahhh", "", ahhh);
		addPlayChat("4Doogida", "", doo);
		addPlayChat("5Music", "", music);
		addPlayChat("6Sup Nigga", "OOoohh wsUP NigGA??/  Come on in mang.", sup);
		addPlayChat("7Backwards in Cornfield", "You ever take yer clothes off and run backwards through a cornfield??", corn);

setPlayChatMenu("2Global 10");
		addPlayChat("aBeavis & Butthead - Braces", "Hey baby...uhhhh, huh, huh.  I noticed you have braces.  I have braces too.  Uhhh huh, huh huh, huh.", braces);
		addPlayChat("bBeavis & Butthead - HAAHOO!!", "This is gonna be cool...heh, heh.  HAAAHOOOO!!", cool);
		addPlayChat("cBeavis & Butthead - Help", "Heh hmm, hmm...heheelp...heh, heh nay.  Oohhh noooo....nooo.", noo);
		addPlayChat("dBilly Madison - Damn Bus", "I'll turn this damn bus around.  That'll end your PRECIOUS little field trip.", bus);
		addPlayChat("eBilly Madison - Attempt 2 Cheat", "If there is any attempt by either contestant to cheat....especially with my wife, who is a dirty, dirty tramp....I am just gonna snap.", cheat);
		addPlayChat("fBilly Madison - Kalateestnaye", "Kalateestnaye.....KallownoOoOoOooOOsaye!", gib);
		addPlayChat("gBilly Madison - Swan", "Stop looking at me swan!", swan);
		addPlayChat("hBilly Madison - Retarded", "I heard he's retarded or something.  Yeehahaheheheh.", tard);
		addPlayChat("iBilly Madison - Mr. Anderson", "We're so lucky to have Principal Anderson substituting.  If I were him I would walk my fat ass right into on..coming...traffic.", traffic);
		addPlayChat("jFamily Guy - Cheesy Charlie", "Welcome to Cheesy Charlie's.  HEIL HITLER.", heil);
		addPlayChat("kFamily Guy - Kool Aid", "Yo did ya'll check me when dat hottie was all up my Kool-Aid?", kool);
		addPlayChat("lFamily Guy - Frontin", "Yo, dat sweat's just frontin 'G.  AahahaHaHAhahAHAhahAHa.", frontin);
		addPlayChat("mFamily Guy - Blacky Weather", "Here's Ali Williams with the Blackie Weather Forcast.  Ali?  'Is gonna rain.'  Thanks Ali.", blacky);
		addPlayChat("nFamily Guy - Praise Allah", "Hey Americans, you like movies?  I've got, 'Dude my car is not where I parked it, but praise Allah we are not hurt.'", allah);
		addPlayChat("oFamily Guy - Hitler", "If you're going to be in sa Los Angeles area une vould like tickets to Hitler.  Call 213-DU WERDEST EINE KRANKENSCHWESTER BRAUCHEN!", hitler);
		addPlayChat("pDBMTSC - Niggasaki", "Eh you betta get yo stanky asses up outta here before I cause a nuclear holocaust up in here, fool it be Hiroshima and Niggasaki.", hiro);
		addPlayChat("qDBMTSC - Give me Love", "C'mere you ol fool dog mark ass tricc....give me some luuuuuv.", luv);
		addPlayChat("rDBMTSC - Take yer Manhood", "Anybody evah try and take your manhood?  But then he sees the Warden coming, so he hides you....but you still got that plunger in yer ass.", man);
		addPlayChat("sDBMTSC - Announce yoself", "*BLANG*  Tray that you?  Hey nigga you betta start announcin yo'self before you get smoked up in here nigga.", smoke);
		addPlayChat("tDBMTSC - Sucky Sucky", "How much for this candy bar?  FIVE DOLLA.  What?!?  Betta give me some sucky sucky wit dat for five dollars.  Some love me longtime, or sumpin.", sucky);
		addPlayChat("uDBMTSC - USSR", "I said:  DO - WE - HAVE - A - PROBLEM - HUH?  Oh, U.S.S.R...?", ussr);
		addPlayChat("vDBMTSC - Need Relief", "What I want you to do, when ya get that hot and ya need a lil....RELIEF.  I want ya ta, I want ya ta - CALL MEEE.", relief);
		addPlayChat("wDBMTSC - Don't ask 1", "Don't ask, no questions - Don't ask, How come uh, uh, uh, uh why come the Pastor hafta have him a nice house, huh!", ask);
		addPlayChat("xDBMTSC - Don't ask 2", "Or why come, uh, uh Pastor got ta have a nice car, huh!  Don't ask!  I said don't ask!  I said.....DON'T ASK no questions.  Just give the money.", ask1);
		addPlayChat("yDBMTSC - Which Church?", "Here at the Greater Ebaneezah New Revival Tree 'a Life Institutional double rock on the side of the road to Jericho Missionary Baptist Church of Zion!", church);
		addPlayChat("zDBMTSC - Collection Plate", "Brother Deacon....get the collection plate around on that side.  Nigga get it around on that side.", plate);
		addPlayChat("1DBMTSC - Crumbcakes", "Nigga do you know how many crumbcakes I can get for this?  You know how many chocolate milks?  How many BARS OF SOAP??", cake);
		addPlayChat("2DBMTSC - Phone Check", "MMM?  PHONE CHECK HOMEY - PHONE CHECK.  I got da TOP BUNK.", phone);
		addPlayChat("3DBMTSC - Take me to Jail", "You can me take Jail! *TAKE ME TO JAIL*  Ya can lock me up!  *LOCK ME UP*  I ain't afraid to &#@% somebody in his ass  *EHH WHOOAAAA...*", jail);
		addPlayChat("4DBMTSC - Shower w/Man", "Ya'll ain't nevah been in the shower with a man.  And a, you see the suds....roll down the crack of his ass.  And you jus....", crack);
		addPlayChat("5DBMTSC - Foolin Ya'll!", "AAAAaaahhhhh hahaha!!  I was foolin ya'll!  I was FOOLIN YA'LL.  Those was jokes.", crack2);
		addPlayChat("6DBMTSC - Start the Day", "Hahaha, yeeeah.  That's the way to start the day.", day);
		addPlayChat("7DBMTSC - 3 Shoes", "Choo gonna be walking down the street with 3 shoes.  2 on your feet and 1 in yo ass.  Sucka.", shoes);
		addPlayChat("8DBMTSC - Hurry Up & Buy", "Jaaaaaaaaahhhh hop!  Hurry up and buy!", buy);
		addPlayChat("9DBMTSC - Kissed Every Nigga", "Hey stupid....you realize you just kissed every nigger at the party?", party);
		addPlayChat("0DBMTSC - Your Hobbies", "I see your hobbies are drinking, smoking weed and all types of ill shit.", ill);
		
setPlayChatMenu("3Global 11");
		addPlayChat("aDeuce Bigalow - Freeeak", "Where you from again?  Norway.  FREEEEEEEAK.", freak);
		addPlayChat("bDeuce Bigalow - Bigfoot", "I hear great things about it.  HOLY SHIT IT'S BIGFOOT.", bfoot);
		addPlayChat("cDeuce Bigalow - Circus", "You know this place has gone way downhill.  HEY, KEEP IT IN THE CIRCUS.", circus);
		addPlayChat("dDeuce Bigalow - Huge Bitch", "Whaddya say we go somewhere else?  That's a HUGE bitch.", huge);
		addPlayChat("eDeuce Bigalow - Shove it", "Nice day, huh?  Yeah, hehe.  SHOVE IT UP YOUR ASS.", shove);
		addPlayChat("fDeuce Bigalow - Asshole", "Deuce taught me to be comfortable with who I am.  ASSHOLE.", deuce);
		addPlayChat("gDumb and Dumber - Happy Place", "Find a happy place.  Find a happy place.", hap);
		addPlayChat("hDumb and Dumber - Karate", "**GONG**  JAAAAAaaaaaaaaaaahHH@#?%&  WHhiieee chaaaaaAHH!", kar);
		addPlayChat("iDumb and Dumber - Where 2 Sign", "You just tell me where to sign bud.  Right on my ass, after ya kiss it.", kiss);
		addPlayChat("jDumb and Dumber - Got Me Mad", "Hoh!  You turned your back on me.  HoHohoh, he got me mad I almost like it.", mad);
		addPlayChat("kDumb and Dumber - Lil Nippy", "We...we're there.  Got a lil nippy going through the pass, huh Har?", nip);
		addPlayChat("lDumb and Dumber - Time Out!", "*FWAP*  AAAaaaahhhhhh.....TIME OUT.", time);
		addPlayChat("mCheech n Chong - Acid", "Hey man I never had no acid before man.  Geez I hope yer not busy for about a month, hehehe.  Shit I'm gonna die man.", busy);
		addPlayChat("nCheech n Chong - 1/4 Pounder", "Eh juhalala chinga....is that a joint man?  Looks like a Quarter Pounder man, hehehe.", chinga);
		addPlayChat("oCheech n Chong - Grab ya Booboo", "Kinda grabs ya by the boo-boo don't it?", grabs);
		addPlayChat("pCheech n Chong - OOoohh shit", "Ooohhmmm.....OoOOhh...Ooohmmmm.  Oh shit.", ooh);
		addPlayChat("qHalf Baked - Kenny's Hole", "*wash off yer hanky!*  Kenny's butthole was in constant jeopardy.", bhole);
		addPlayChat("rHalf Baked - Love Horses", "I love horses.  I love horses.  I love Butterstuff.  ButterCUP.  SAY IT.  Butternuts, CUP, cup, CUP, cup, Cup Ahhhahahah.", butter);
		addPlayChat("sHalf Baked - On Grill B", "HEFER WITH CHEESE.  Why you gotta make me feel inferior cuz I'm on the grill 'B?", grill);
		addPlayChat("tHalf Baked - Hungry Girl?", "Hey girl....ya hungry?  &#@% you nigga.  Hey I'm sorry, I was talking to the horse here.", hungry);
		addPlayChat("uHalf Baked - Impotent", "I'm impotent man.  Get away from me biatch!", imp);
		addPlayChat("vHalf Baked - Window Love", "No more window love.  Go sell it.", indow);
		addPlayChat("wHalf Baked - Janitor", "Ya it's bad enuf yer a janitor yo.  CUSTODIAN dick.", janitor);
		addPlayChat("xHalf Baked - Popcorn", "You like popcorn.  Makes your teeth go, 'pop pop pop pop pop'.  Haheh.", popcorn);
		addPlayChat("yHalf Baked - Sampson", "This is SAMPSON smarty pants.  Sampson this is Sheila.  Momma fell...Shut up bitch!", sampson);
		addPlayChat("zHappy Gilmore - Closer", "Somebody's closer.", closer);
		addPlayChat("1Happy Gilmore - Die Clown", "YER GONNA DIE CLOWN.  YOU THINK THAT'S FUNNY?  I DON'T HEAR YOU LAUGHIN NOW.", clown);
		addPlayChat("2Happy Gilmore - Regulation", "DAMMIT.  IS THAT GOAL REGULATION SIZE OR WHAT??  GEEZ.", goal);
		addPlayChat("3Happy Gilmore - 364 More", "364 more days till next year's hockey tryouts, I gotta toughen up.  *CRACK*  YEEAAAHH.", hockey);
		addPlayChat("4Happy Gilmore - Luck", "Well, some might call it luck I like to call it....well, luck I guess.  So what?", luck);
		addPlayChat("5Happy Gilmore - HAAPPY", "*THUMP*  The price is wrong, bitch.", price);
		addPlayChat("6Happy Gilmore - Like That", "Ya like that?", that);
		addPlayChat("7Rush Hour - Slow Down", "Hey....slow down Chin.  Wha the hell is wrong wichoo?", chin);
		addPlayChat("8Rush Hour - Popeye's", "I don't like my chickens live, ok?  I like 'em dead and deep fried.  Ya ever heard of Popeye's?", popeye);
		addPlayChat("9Rush Hour - Slap You", "Bejing ni da na hoijea.  What?  Bejino.  I will slap you if you don't move this car.  Bejino.  I'm gonna slap you.", slap);
		addPlayChat("0Rush Hour - Godzilla", "When Godzilla's coming, ya'll be trippin.  Giacka, Giacka!", zirra);
		
		
setPlayChatMenu("4Global 12");
		addPlayChat("aNext Friday - Chewin", "Lookit...you been chewin on this shit before you came her....BULLSHIT MUDDA %#!@$.", chew);
		addPlayChat("bNext Friday - Cover", "Well where's the cover to...I don't have no damn cover, kiss my ass so what.", kmass);
		addPlayChat("cNext Friday - Play Sports", "What do you play sports?  I play for the Kookamunga Kracka Killas.  You want tickets?  Hey, don't want any trouble with you.  You don't have to send your uh, posse out here to do a 187 in my ass.", kracka);
		addPlayChat("dNext Friday - Mr. Nasty", "I'm faded....feeling X-Rated.  It's Mr. Nasty time.", nasty);
		addPlayChat("eNext Friday - Mr. Nasty 2", "Ooohh, Mr. Nasty time.  Mr. Nasty time.", nasty2);
		addPlayChat("fNext Friday - Mini-Wheat", "Awww shit.  He's cute, huh?  He's like a little frosted mini-wheat.  Awwwww shit man.", heat);
		addPlayChat("gNext Friday - Aztec", "Joker don't do that man.  You haven't seen my Aztec Warrior holmes.  I can't handle this...YOU can't handle it?", aztec);
		addPlayChat("hNext Friday - Mouth", "You can handle this.  You got a pretty mouth esai.  Awwwww shi...c'mon baby don't do that to me.", mouth);
		addPlayChat("iNext Friday - Couch", "You know you got shit all over the back 'a yo ass?  HEY, don't sit on that couch.", couch);
		addPlayChat("jS. Silverman - B. Midler", "Like my obsession with Bette Midler?  My preference for track lighting?  Oh!  And the fact that I like sucking dick.", midler);
		addPlayChat("kS. Silverman - 3 Balls", "Well you didn't know a lot of things.  You didn't know I was gay.  Is there anything else you wanna tell me?  I got 3 balls.  SHUT UP.", balls3);
		addPlayChat("lS. Silverman - Gay w/ Me", "You wanna be gay with me?  NO@#!%  Alright.", ith);
		addPlayChat("mS. Silverman - Bunsen", "Oh, oh, oh....remember that time in Science class I was lightin farts with a Bunsen burner and I singed my ball sac?", sac);
		addPlayChat("nS. Silverman - Bite Me", "Oh BITE ME.  Blow me.  SKANK.  EUNUCH.  STEALER....OF MY FRIEND.", steal);
		addPlayChat("oS. Silverman - Smoke Pole", "Just get over it.  Your buddy smokes pole and so do you.  Haha, I am SO ungay!", ungay);
		addPlayChat("pS. Silverman - Fun Lvl", "Before Judith, our fun level was at an all time high - 93.  It is now....an 8.", graph);
		addPlayChat("qS. Silverman - Fun Lvl 2", "Girls...never very high at 9, but look now.  2.  This has obviously led to increased whacking off.  I'm chafin!", graph2);
		addPlayChat("rS. Silverman - Darren", "We're not giving up on Darren.  HUH-YEEEEEEEAAAAAAAAHHH!!  Comin on YEEEEEEEEEEEHAAH!", darren);
		addPlayChat("sS. Silverman - Cowboy Wayne", "I'm Cowboy Wayne.  I uh...just bagged me one of them...killer goats that escaped from the zoo.", zoo);
		addPlayChat("tS. Silverman - Manjuice", "Stay away from women.  All they want from you is your manjuice.", mjuice);
		addPlayChat("uS. Silverman - Scrotum", "OOOoooohhhh SCROTUM.", sac2);
		addPlayChat("vTommy Boy - Awesome!", "I swear I've seen a lot of stuff in my life.  But that was AWESOME.  Hahahaha.", awe);
		addPlayChat("wTommy Boy - Called Dr's", "You know a lot of people go to college for 7 years.  I know, they're called Doctors.", doc);
		addPlayChat("xTommy Boy - Killed It", "AAAAAAaaaahhhhhhh.  I KILLED IT.", kill);
		addPlayChat("yTommy Boy - Papa Smurf", "Hey boys and girls it's Papa Smurf!", papa);
		addPlayChat("zTommy Boy - Kung Fu", "*KUNG FU*", kung);
		addPlayChat("1Tommy Boy - Bad Girl", "No one's lookin.  Speakin 'a no one's lookin.  *zip*  Bad girl.", bgirl);
		addPlayChat("2Tommy Boy - Egg Man", "Hey man look at me go, I'm throwing eggs.  I'm the egg man GOO-GOO GAJOO.", gajoo);
		addPlayChat("3Tommy Boy - Whore", "NOSE BITER.  TIME TO PAY THE FIDLER WHORE.", hore2);
		addPlayChat("4You Bitch!", "You bitch!", fag);
		addPlayChat("5I Ain't Playin Nigga", "And I ain't playin NIGGA.", playin);
		addPlayChat("6Took It Like a Man", "You shoulda seen the size of that thing you had inside you, it was like *THIS* man, you took it like a MAN.", man2);
		addPlayChat("7Kaka! Kaka!", "Kaka!  Kakaaah.  KakAAaah.  HAHAHEHE TOOKIETOOKIE TOOKIETOOKIE KAKAKAKAKAKAKA.", kaka);
		addPlayChat("8Cock Knocker", "You and your side kick are finally in the grasp of COCK-KNOCKER.  HEH HEH.", cknock);
		addPlayChat("9Gay Cops", "Ya right, let me see yer badge buddy.  SHOW ME YER ASS.  Show me yer ass?  Ya'll are some gay ass cops.", cops);
		addPlayChat("0Dooter", "Read the script dooter.  *KEE-YAAAAP*", dooter);
		
setPlayChatMenu("5Global 13");
		addPlayChat("aSNL - Turd Ferguson", "Hey uh check out the podium, look at this.  Mr. Reynolds has apparently changed his name to Turd Ferguson.", turd);
		addPlayChat("bSNL - English or Retarded", "Are you English or retarded?  HA HA!", english);
		addPlayChat("cSNL - The Rapists", "I'll take the rapists for $200.  That's Therapists, not the rapists.", rapists);
		addPlayChat("dSNL - A Leather Glove", "And the answer is....you usually drink water out of one of these.  Sean Connery?  A leather glove.  *BZZZZZT*", leather);
		addPlayChat("eSNL - A Toilet", "Minnie Driver?  A toilet!  And you're an idiot.", toilet);
		addPlayChat("fSNL - Apetit", "Mr. Reynolds it's still your board.  Yeah well uh, why don't you give me uh, why don't ya give me Ape Tit for $200.  It's not Ape Tit.", apetit);
		addPlayChat("gSNL - Sound a Doggy Makes", "This is the sound a doggy makes.  Mr. Connery?  Moo.  No.", moo);
		addPlayChat("hSNL - Mom likes it Rough", "We would've accepted 'bow-wow' or 'ruff'.  Aahhh rough, just the way yer mother likes it Trebek.", ruff);
		addPlayChat("iSNL - Dego Mustache", "You think yer pretty smart, don't ya Trebek?  What with your dego mustache and yer greasy hair.  Look....what did I just say about ethnic slurs?", dego); 
		addPlayChat("jSNL - Like Monkeys", "That's great you like monkeys.  Noo, I hate monkeys.  They're awful.  I bit his bloody head off HAHAHAHAHAHAHAHAHA.", bloody);
		addPlayChat("kSNL - Charleston Chew", "Mr. Osbourne you get to choose.  Awright I'll take Charleston Chew's for sixteen million.", charles);
		addPlayChat("lSNL - Cock of the Walk", "Then I'm the cock 'a the walk!", cock);
		addPlayChat("mSNL - Sick Duck", "What's the difference between you and a mallard with a cold?  One's a sick duck.....I can't remember how it ends, but your mother's a whore.", mallard);
		addPlayChat("nSNL - Hot Dog", "HEY.  If you were a hot dog.....and you were starving, would ya eat yerself?", hotdog);
		addPlayChat("oSNL - Stared at the Sun", "I once took a pair of binoculars and stared at the sun fer, over an hour.", sun);
		addPlayChat("pSNL - Stared at the Sun 2", "Why would you do that?  Curiosity I guess.  Heck, I'm curious like a cat.", sun2);
		addPlayChat("qSNL - Moon 1", "Hey, now Kent we all know that the Moon is not made of green cheese.  Yes that's true Harry.", moon);
		addPlayChat("rSNL - Moon 2", "But what if it were made of BBQ Spareribs, would ya eat it then?  What?", moon2);
		addPlayChat("sSNL - Moon 3", "I know I would, heck I'd have seconds.  And then...then polish it off with a tall cool Budweiser.", moon3);
		addPlayChat("tSNL - Moon 4", "Haha...I'm confused.  It's a simple question Doctor, would ya eat the moon if it were made of ribs?", moon4);
		addPlayChat("uSNL - Moon 5", "Well I don...I ahh I don't know how to answer that.  It's not rocket science.  Just say yes and we'll move on.", moon5);
		addPlayChat("vSNL - Brought Pizza", "I brought you a pizza.  I hope that is a piece of ass!", poa);
		addPlayChat("wSNL - That is Disgusting", "Yeah....well that is disgustin.", disgust);
		addPlayChat("xSNL - Mr. President", "Mr. President I salute you for doing your job, while having a job done to you!", prez);
		addPlayChat("ySNL - Now You Get Going", "Yeah that was nice, now you should get goin.", nice);
		addPlayChat("zSNL - Too Much Doggy", "Yeeheeah, well....there is such a thing as too much doggystyle.  Heheheh...wait a second, what'd I just say?", doggy);
		addPlayChat("1SNL - Ladies Man", "I'm doing really good cuz I got my courvoisier right here.  Ahahaheh.", courv);
		addPlayChat("2Translator's Broken", "Doesn't she talk?  Her translator is broken.  *JALALALALAALAH*", galaxy);
		addPlayChat("3Turned Inside Out", "But the animal is inside out.  It turned inside out?  *PLLBBRRPT*  And it exploded!", inside);
		addPlayChat("4That's Not Right", "Ooohh that's not right.  Nooo.", right);
		addPlayChat("5Niggas!", "Ni-gahs!", nig);
		addPlayChat("6Pompasses", "SONS A BITCHES.  POMPASSES.", pompas);
		addPlayChat("7Beavis & Butthead - De Bunghole!", "Yaayaaieee...De Bunghole! Hehe - it is nothing to be ashamed of!", bung2);
		addPlayChat("8Pretty Bird", "Pretty bird.  Yeah can you say pretty bird?  Yes, pretty bird.", pbird);
		addPlayChat("9Darrr!!", "DAAARRRRRR!!!", darr);
		addPlayChat("0Buddy Lee", "*Buddy Lee*", budlee);
		
setPlayChatMenu("6Global 14");
		addPlayChat("aSNL - Anal Bum Cover", "I'll take Anal Bum Cover for Seven Thousand.", anal);
		addPlayChat("bSNL - Anal Bum Cover 2", "That's an Album Cover not Anal Bum Cover.  I can read Trebek.  That says Anal Bum Cover.", anal2);
		addPlayChat("cSNL - Come on Pansy", "Come on ya pansy, let the people see my work.  No we're not going to do that, ok?  I quit.", quit);
		addPlayChat("dSNL - Famous Mothers", "Oh come on, why would they do this?  The category is Famous Mothers.  Hahahaha, my day has come!", famous);
		addPlayChat("eSNL - Legally Retarded", "Boy you might be legally retarded.", legal);
		addPlayChat("fSNL - Ladis I Snogged", "Oh good Mr. Connery wants to say something.  I thought of some more foreign ladies I snogged.", snog);
		addPlayChat("gSNL - Suck it Trebek", "Let's see what you wagered:  *Suck it Trebek*  HAHAHAhahahahahahaha!", suckit);
		addPlayChat("hSNL - Kanuka!", "Really?  Alright, well....Kanuka!  Kanuka!  Hahahahaha.", kanuka);
		addPlayChat("iSNL - That Was Great", "Well, that was great!  Thank you very much I guess.", asgreat);
		addPlayChat("jSNL - Speak Japanese", "MOTHER OF MERCY, I DON'T SPEAK JAPANESE.", jap);
		addPlayChat("kSNL - I DON'T KNOW", "I DON'T KNOW.  NO WAIT...WAIT, WAIT, WAIT.  I know it, I know it.", ait);
		addPlayChat("lSNL - These Guns", "I showed you what these guns could do in the Middle East, now I'm gonna show what they can do in the ring.", guns);
		addPlayChat("mSNL - This is Hard", "Oh what the frick, CUT ME A BREAK, THIS IS HARD.", break);
		addPlayChat("nSNL - Out of Tune", "SONNUVA #*@!$.  Is it humid in here?  Cuz the guitar keeps getting out of tune.", guitar);
		addPlayChat("oSNL - Out of Tune 2", "I SAID THE GUITAR....WAS OUT OF TUNE.  It wasn't my fault.", guitar2);
		addPlayChat("pSNL - Kind of Guitar?", "Yeah, well wha..what kind of guitar is that?  It's a HELL SPAWN MIXTURE OF THE BONES OF FORNICATORS, AAAAHAHAHAHAHA.  It's a...it's a Fender.", guitar3);
		addPlayChat("qSNL - Devil Rap", "*A BU BU CHOO - HOOHOOHOOHOHO* I'm the devil, and I'm here to say I'm the most evil rapper in the U.S.A. all my homies and my bitches say HOOOoooooo.", rap);
		addPlayChat("rSNL - Time for the Jeopardy", "How ya doing there Alex, uhhh ya know it's great to be here, ya know.  TIME FOR DA JEOPARDY.", jeopardy);
		addPlayChat("sSNL - Clone Hot Dogs", "Hey.  If I was a scientist, ya know what I would clone?  Hot dogs.", hotdog2);
		addPlayChat("tSNL - Endless Supply", "Hold on.  Imagine a world uh..of..with a endless supply of hot dogs.", hotdog3);
		addPlayChat("uSNL - Hot Dog", "HEY.  If you were a hot dog.....and you were starving, would ya eat yerself?", hotdog);
		addPlayChat("vSNL - I Know I Would", "I know I would.  First I'd smother myself with brown mustard 'n relish.  I'd be so delicious.", hotdog4);
		addPlayChat("wSNL - So Would Ya?", "So would ya?  I don't know.  Don't jerk me around Norm, it's a simple question.", hotdog5);
		addPlayChat("xSNL - Baby Could Answer", "A baby could answer it.  If you're a hotdog, and you were starving....would ya eat yourself?", hotdog6);
		addPlayChat("ySNL - I Guess So", "I guess so.  OOoh, ya made a wise choice my friend.  If you had said no, I woulda bitten yer ear off.", hotdog7);
		addPlayChat("zSNL - Bitten Many Ears", "I've actually bit a man's ear off on several occasions.  And I'm not proud of it, but it helped me out a many a jam.", earoff);
		addPlayChat("1Half Baked - Bachiotomy", "Doctor said I need a Bachiotomy.", back);
		addPlayChat("2Half Baked - Sex w/Mama", "He had sex with my mama!", mama);
		addPlayChat("3Blue Streak - Rip Guts Out", "I seen him rip somebody's guts out...thr..through their ass, and their eyes fell out.  KUHH.  That's wher..that's where da guts went *SPLECH* - just dropped 'em.", gutsout);
		addPlayChat("4Blue Streak - Take My Shoelace?", "You took my shoelace?  You take my shoelace man?", shoe);
		addPlayChat("5Blue Streak - Floss Your Ass?", "What choo gonna do with one shoelace?  Floss yer ass?  AAAAHAH!  Gotta go.  *HOOOO*", floss);
		addPlayChat("6Blue Streak - Back up NIGGA!", "BACK UP NIGGA.", backup);
		addPlayChat("7Blue Streak - Left Leg BLAOW", "Two times - Left *POW* - One time, break it down get around get by ovah here.  Left leg - *BLAOW* - Bring it 'round...Bop!", blaow);
		addPlayChat("8Blue Streak - Ima Chomp it UP", "I mean it's just like leaving candy around me.  It's not gonna be alright, I'm gonna *CH CH CH CH CH* Chomp it UP.  Ya see what I'm sayin?", chompit);
		addPlayChat("9Blue Streak - What happened???", "DAAAAAMN.  WHAT HAPPENED???  Did you eat the whole time I was in there?", eat);
		addPlayChat("0Blue Streak - Buy Cereal?", "I apologize, you're her cousin!  Can I buy you some cereal?", cereal);
		
setPlayChatMenu("7Global 15");
		addPlayChat("aNext Friday - Aztec Warrior", "I'm an Aztec Warrior, AhhhAhhhh!", aztec2);
		addPlayChat("bNext Friday - Grill Cheese", "I don't want no grill cheese!  No Leroy, I'm bi-lingual there's a difference.  No more locked doors.  Gracias!", leroy);
		addPlayChat("cNext Friday - Not Stupid", "I'm not stupid, yer stupid.  No YER stupid - Don't call me stupid, I'm sensitive.  YER STUPID.", stupid);
		addPlayChat("dNext Friday - Sabu!", "Sabu!  I seen all your movies man.  Boy you bad, riding that magic carpet boy ain't gotta worry about no gas.  Haha.", magic);
		addPlayChat("eNext Friday - No Play", "That ass little chinese lady gonna take the money and not give me no play.", noplay);
		addPlayChat("fNext Friday - Lucky Nigga", "Well you lucky nigga.  I was just gonna get up in yo ass.", getup);
		addPlayChat("gNext Friday - In Pinky's", "Well I'm gonna show you how we do it up here in Pinky's nigga.", pinky);
		addPlayChat("hNext Friday - Say It", "I - OOHHHH SHIT.  SAY IT AGAIN, SAY SUMPIN ELSE.  OOHHH.  SAY SUMPIN ELSE NIGGA.", nigguh);
		addPlayChat("iNext Friday - Puff Daddy", "I'm 'bout to show you who the REAL puff daddy is.", puff);
		addPlayChat("jNext Friday - Hell No", "Hell no Willy.", illy);
		addPlayChat("kNext Friday - Negro", "Negro...what the hell you doin to my woman?", oman);
		addPlayChat("lNext Friday - Sugah?", "Sugah?  HUUUH?  What the hell you doin to my nephew?  Oh I thought that was you baby.", sugah);
		addPlayChat("mNext Friday - Titties", "Damn daddy, I didn't know you had titties.", titties);
		addPlayChat("nNext Friday - Player", "How ya'll doin anyway?  My name is Dede, I'm your local neighborhood playa.  This is my little buddy Roach - Roach say what's happenin to 'em.  Nice to meet you ladies.", player);
		addPlayChat("oNext Friday - Fat Ass", "Oohh you got a fat ass.  Aight, aight - AIGHT THAS ENUF.", fatass);
		addPlayChat("pNext Friday - Sugah Bowl", "Don't let me catch ya with ya fingah in my sugah bowl, you feel me knockin?", sbowl);
		addPlayChat("qCock 'n Balls", "You know what your problem is?  You're all brains....not enuf cock 'n balls.", brains);
		addPlayChat("rWTF?", "Wtf, slow down - Wha WTF?  NO.  NOOOOO.", sdown);
		addPlayChat("sF* THAT", "NOOOO %!@#* THAT NIGGA %!@#* IT I'M ON MY WAY.", omw);
		addPlayChat("tSantiago", "Ain't that right Santiago?  Si.  Siiiiiiiii.", santia);
		addPlayChat("uC'mon BUDDY", "C'mon Buddy.  C'mooooon Buddy.  Now everybody knew as soon as you walked through the door, yer gonna get some chicken.", chicken);
		addPlayChat("vBlacks 'n Chickens", "It is no secret down here that BLACKS and CHICKENS are quite fond of one another.", fond);
		addPlayChat("1Five to Ten", "Nigga don't do that, that's five to ten!", five);
		addPlayChat("2Nigger Baby", "C'mere little nigger baby. *MMMCHAAA*", nbaby);
		addPlayChat("3BELIEVE ME", "I DIDN'T TOUCH THAT %!@#* NIGGA I KILL U.  NIGGA I KILL U.  PLEASE BELIEVE ME.  PLEEEEEEASE BELIEEEVE ME.", killu);
		addPlayChat("4Country Got Ya Crazy", "Country gotcha crazy.  Di Deeeeeeeeee Di Da Leeeeeeeeee Di.", tdavid);
		addPlayChat("5Filet Mignon", "Young lady wants a Fi-let Mig non and some strawberries and some skrimp cocktale.", lady);
		addPlayChat("6MY OBSTACLE", "Get the #*@! off of my obstacle.", qpyxmez);
		addPlayChat("7WHOOOA TRICC", "WHOOOA TRICC.", hooa);
		addPlayChat("8GunShot", "*BLAM*  AAAHhhhhh!!", gunshot);
		addPlayChat("9Werd Up", "(_)iiiiiiiiiiii-I-iii-LOVE-iii-sLaMiiiiiiiiiiiiD", erd);

setplayChatMenu("8Global 16");
		addPlayChat("aGET ON THE BAG", "Oh wow that sounds interesting, I..I love art myself.  GET ON THE BAG.", art);
		addPlayChat("bGET ON THE BAG 2", "HEY SON.  YOU TRYING TO MAKE AN ASS 'A ME??  GET ON THE BAG.", assme);
		addPlayChat("cSnow Cone", "I could go for snow cones, anyone interested?  Oh thanks Kathy, but uh....I got my own snowcone.  Right here.", snow);
		addPlayChat("dGET ON THE BAG 3", "I WILL CHAIN YOU TO A PIPE IN A CRAWL SPACE IF YOU DON'T GET ON THE BAG.  NOW GET ON THAT BAG.", pipe);
		addPlayChat("eGET ON THE BAG 4", "HEY LISTEN CRYBABY.  I WILL DOWNSIZE YER FACE WITH A SHOVEL IF YOU DON'T GET ON THE BAG, NOW GET ON THAT BAG.", yerface);
		addPlayChat("fCan it on the Base", "*YEE-AH*  Hey.  Can it on the bass.", canit);
		addPlayChat("gTwo Finger of Goulet", "It's a cocktail with a splash 'a Jay Z and 2 fingers of Goulet.", jz);
		addPlayChat("hHardknock Life", "It's a hard knock life - cut the track.  Stuff like that.", life);
		addPlayChat("iBitch on my Back", "Bitch on my back, yak in my bucket smoking that sticky chocolaaaAaAaAate.  Yeah.", myback);
		addPlayChat("jMore COWBELL", "I'm the cock 'a the walk baby.  And if Bruce Dickinson wants more cowbell, we should probably give him more cowbell.  Say it baby.", cbell);
		addPlayChat("kMore COWBELL 2", "But the last time I checked we don't have a whole lotta songs that feature the cowbell.  I GOTTA have more cowbell baby.", cbell2);
		addPlayChat("lMore COWBELL 3", "And I'd be doing myself a disservice, and every member of this band if I didn't perform the HELL outta this.", cbell3);
		addPlayChat("mMore COWBELL 4", "Guess what?  I gotta FEVAH.  And the only prescription - is more cowbell.", cbell4);
		addPlayChat("nEveryone Cool Out", "Just everyone cool out.  COOL OUT#@$", cout);
		addPlayChat("oHatred of Dark Skin", "Well my love of this great and beautiful nation, and my hatred of all people with dark skin led me to write this.", dark);
		addPlayChat("pFueled Creatively", "Few people know that I'm fueled creatively by my massive hatred of immigrants.", fuel);
		addPlayChat("qSmack You in the Mouth", "I'LL SMACK U IN THE MOUTH, I'M NEIL DIAMOND.  Ok that's it, I'm gone.  That's it.  Wait!", smack);
		addPlayChat("rGave the Money Back", "Fine would it help if we gave the money back?  YES, IT WOULD.  CUZ WE DON'T HAVE IT.", mback);
		addPlayChat("sEVERYONE SHUTUP", "EVERYONE NEEDS TO SHUT UP.  SHUT UP.  YOU GUYS SHUT UP.  YOU SHUT YOUR MOUTH.  YOU'VE RUINED MY WEDDING.", shutup);
		addPlayChat("tWade Would Kill a Dog", "But Wade told me for 50 dollars, he'd kill a dog.  *BAM*  I did NOT tell him that.", tell);
		addPlayChat("uDog in Hell", "Look.  Am I happy that that dog is rotting in hell?  Yes.  Did I personally inject a steak with poison and feed it to the dog?  No.", poison);
		addPlayChat("vDodge Stratus", "I drive....I drive....I DRIVE A DODGE STRATUS.", dodge);
		addPlayChat("wI Like Hispanic Men", "I like the meat of young hispanic men.  That's because you're gay.  Maybe I am....and maybe I AM.", hispanic);
		addPlayChat("xOH REALLY?", "OH REALLY?  Then where did THIS tasty lick come from?  *Plink!* *Plink!* *Plink!* *Plink!* *Plink!* *Plink!*", lick);
		addPlayChat("yHow Hot is it?", "HEY, How hot is it?  Unbearable!  How hot is it?!  Unbearable!  Huh, how hot is it?!?  Unbearable!  OOooh!!", howhot);
		addPlayChat("zI'm The Devil", "Not only have I shed 65lbs in 4 days, but guess what?!?  I FOUND OUT I'M THE DEVIL.", tango);
		addPlayChat("1Favorite Planet", "Hey!  Let me ask, what's yer favorite planet?", planet);
		addPlayChat("2Favorite Planet 2", "I..I..E, I don't have a favorite uh....I find them all fascinating, they're all part of uh..MINES the sun!", planet2);
		addPlayChat("3Favorite Planet 3", "Always has been.  I like it cuz it's like the King of Planets.", planet3);
		addPlayChat("4Favorite Planet 4", "Actually Harry it's not uh...not a planet it's a star.  Well planet or star, when that thing burns out we're all gonna be dead.", planet4);
		addPlayChat("5Favorite Planet 5", "Hey, Doctor...have ya ever seen an eclipse?  Uh...ss, y..y..ya I've seen many, yes.", planet5);
		addPlayChat("6Favorite Planet 6", "You know if you stare at it head on, it'll burn yer eyes out.  Well it's not, not, not best to stare at the sun during an eclipse.  But it's hard not to.", planet6);
		addPlayChat("7Favorite Planet 7", "I once took a pair of binoculars and stared at the sun fer, over an hour.", sun);
		addPlayChat("8Favorite Planet 8", "Why would you do that?  Curiosity I guess.  Heck, I'm curious like a cat.", sun2);
		addPlayChat("9Hepatitis Test", "Well that's cool.  I just took a test this morning.  Yea, at the free clinic for Hepatitis.", hepa1);
		addPlayChat("0Hepatitis Test 2", "I kicked ass too.  I got an A...two B's and a C.", hepa2);
		
setPlayChatMenu("9Global 17");
		addPlayChat("aMighty Kong", "Uh-oh, might 'Kong' has woken from his slumber.", kong);
		addPlayChat("bThinker or Stinker", "Alright well I got two poses to choose from.  'The Thinker' or 'The Stinker'.", stinker);
		addPlayChat("cOne Thousand Million Dollars", "Sold to the gentleman in the front row for One Thousand Million dollars.", row);
		addPlayChat("dWith a Dead Guy", "Plus one time he did it with a dead guy.", dead);
		addPlayChat("eGive You Hepatitis", "But if ya ask me you wouldn't recognize real beauty if it was outside in the parking lot waiting to give you Hepatitis.  Which it WILL be.  Ten minutes from now.", hepa3);
		addPlayChat("fMake Lemonade", "Well if life hands you lemons, you may as well make lemonade.  And I've been wanting to make lemonade all day.", lemon);
		addPlayChat("gAAaahhh", "AAAAaaaahhhhhhhhhhh.  *OUUGHH*  *OUUGHHH*  *CAAAA*", ough);
		addPlayChat("hDirty Chink", "What's yer problem ya dirty chink??", chink);
		addPlayChat("iHomo Mexican's", "What're you homo mexican's lookin at??", mexican);
		addPlayChat("jHomosexual Yankee's", "May you spend the next hundred years watching the queer Mets go down on yer homosexual Yankee's.", homo);
		addPlayChat("kBlack Baby", "Track down that black baby that the jews and the pope had together, and kill it before it can destroy the world.", track);
		addPlayChat("lBow Hunt", "Any of you ever been bow huntin?  I BOW HUNT.", hunt);
		addPlayChat("mMankisser's", "Now am I gettin through to you, you mankissers???", mankiss);
		addPlayChat("nIron Eagle", "I love my father.  I like the Iron Eagle movies.", iron);
		addPlayChat("oRat Poison", "I eat rat poison cuz I can't read the box.", rat);
		addPlayChat("pDaddy's Watching", "Stay focussed Rocker, Daddy's watchin.", daddy);
		addPlayChat("qI'm Glen Fry", "I'm Glen Frey....and guess what?  The H is O.", hiso);
		addPlayChat("rNeed To Be Quiet", "That was a great album.  You need to be quiet.  Ok.", quiet);
		addPlayChat("sHotel California", "Welcome to the Hotel California.", hotel);
		addPlayChat("tURRGHH!", "URGH....URRGGHH!!  *UH HUH HUHAHAH...PUHUHUHA*", unf);
		addPlayChat("uEat Like That", "You eat like that.  You eat like that.", ueat);
		addPlayChat("vWanna Be With You", "I wanna be with you.  *What?*  I wanna be with you.", ithu);
		addPlayChat("1Mad Cow Disease", "HEY.  How 'bout this Mad Cow Disease?", mcow);
		addPlayChat("2Mad Cow Disease 2", "What about it?  Well it was here for awhile then it went away.  Your thoughts.", mcow2);
		addPlayChat("3Mad Cow Disease 3", "Yes, yes.  It was what was in the news for awhile, then it disappeared fr..from the news.  Good point.  Geez, I hope I never get it.", mcow3);
		addPlayChat("4Mad Cow Disease 4", "Hey, what about this?  If you had a choice between being the top scientist in your field, or gettin Mad Cow Disease, what would it be?", mcow4);
		addPlayChat("5Mad Cow Disease 5", "Well of co..of course I would choose to be the top scientist in my field.  Oh good!  I was worried ya choose Mad Cow.", mcow5);
		addPlayChat("6Mad Cow Disease 6", "Why would you think that?  I guess I'm just a worrier.  That's....that's why my friends call me Whiskers.", mcow6);
		addPlayChat("7Sweet Irony", "That monkey shot me in the ass and paralyzed me.  OOhhh ssssweet IRONY.", irony);
		addPlayChat("8Macho Man - Freak Show", "Hey FREAK SHOW.....yer goin NOWHERE.", fshow);
		addPlayChat("9Macho Man - PLAY TIME", "I GOTCHA FER THREE MINUTES.  THREE MINUTES OF PLAAYTIME", gotya);
		addPlayChat("0Macho Man - Bonesaw", "BOOOOOOONNNNNNESAAAAW IS READY!", bonesaw);

setPlayChatMenu("0Global 18");
		addPlayChat("aMock", "MOCK.", mock2);
		addPlayChat("bYeah", "Yeah.", mock3);
		addPlayChat("cIng", "ING.", mock4);
		addPlayChat("dYeah", "Yeah.", mock5);
		addPlayChat("eBird", "BIRD.", mock6);
		addPlayChat("fYeah", "Yeah.", mock7);
		addPlayChat("gYeah", "Yeah.", mock8);
		addPlayChat("hYeah", "Yeah.", mock9);
		addPlayChat("iMockingBird", "HelenKeller's a homo.", mock10);
		addPlayChat("jCows Afraid", "", afraid);
		addPlayChat("kLooney 4", "", loon4);
		addPlayChat("lLooney 3", "", loon3);
		addPlayChat("mLooney 2", "", loon2);
		addPlayChat("nLooney 1", "", loon);
		addPlayChat("oDana Carvey - Choppin Broccoli", "There's a lady I know.  If I didn't know her...she'd be the lady....I didn't know.", cbroc);
  		addPlayChat("pDana Carvey - Choppin Broccoli 2", "And my lady, she went downtown.   S-she bought some broccoli.  She brought it home.  She's choppin broccoli.", cbroc2);
  		addPlayChat("qDana Carvey - Choppin Broccoli 3", "Choppin broccoli....she's choppin brocci-laye.  Choppin bracca-lae-hay.  She's CHOPPIN BROCCA-LAYE.", cbroc3);
  		addPlayChat("rDana Carvey - Choppin Broccoli 4", "SHE'S CHOPPIN BROCCA-LAYE.  SHE CHOP - *UHH*", cbroc4);
  		addPlayChat("sDana Carvey - Choppin Broccoli 5", "SH-GA-BA-VA-GA......LEEE-HAAA-HAAA-HAAA - HEEEEEEEEEE !!11", cbroc5);
		addPlayChat("tEvolution - Here Birdy", "Heeere birdy birdy birdy birdy birdy birdaaaaaaaay.  Birdaaaay.  *TK* *TK* *TK* *TK* *TK* *TK* *TK* *TK* *TK* *TK* *TK* *TK* *TK* *TK* *TK* *TK* *TKAAAAAHH!*  Kaka.  Kaka......kaka.", birdy);
		addPlayChat("uEvolution - Big Lougie", "*BLECH*  OOhhh!  That's like a big loogie!", loogie);
		addPlayChat("vEvolution - Naaasty", "*PPHHLLPPTT*  Ooh....naaasty!", nasty3);
		addPlayChat("wEvolution - Googa-Mooga", "Great googa-mooga!", guug);
		addPlayChat("xEvolution - Rectally", "How ya goin in?  Rectally.  *NOOOOOOO!*", rectally);
		addPlayChat("yHomeless Britney - Mailbox", "I like the mailbox.  Oh I, I stole that becuz it had secrets about me.", mailbox);
  		addPlayChat("zHomeless Britney - Song", "Up and down the sidewalk.  Take a *Doo* *Doo* pie.  I love you.", doodoo);
  		addPlayChat("1Homeless Britney - You like it?", "That was so beautiful.  You mean it?  I do.  I, I kinda wrote it at a real crazy time in my life.", beautiful);
		addPlayChat("2You're It", "You're it.", q);
		addPlayChat("3You're It", "You're it.", w);
		addPlayChat("4Quitsies!", "You're it, quitsies!", e);
		addPlayChat("5Anti-Quitsies", "Anti-quitsies, you're it!  Quitsies, no anti-quitsies no startsies!", r);
		addPlayChat("6You Can't Do That", "You can't do that.  Can to.  Cannot, stamped it.", t);
		addPlayChat("7Double Stamped", "Can to, double stamped it no erasies.", y);
		addPlayChat("8Triple Stamped", "Cannot, TRIPLE stamped it, no erasies touch blue make it true.", u);
		addPlayChat("9Can't Triple Stamp", "You can't triple stamp a double stamp.  You can't triple stamp a double stamp Lloyd, you can't triple stamp a double stamp.  LLOYD.  LLOOOYD.", i);
		addPlayChat("0FOOK YU", "Fook Yu.", fookyu2);
		addPlayChat("-Not a Sissy", "I...am not...a SISSSSSSSSSSYYYYYYYYY.", sissy);
		addPlayChat("=Keyser", "EL CHUPACABRE.", gabe);
		
setPlayChatMenu("-Global 19");
		addPlayChat("aAce Ventura - HEHE", "AH MURMURMUR ME.  EINNNNNN *POP*", hehe);
		addPlayChat("bAce Ventura - Lost 'Em", "I think I lost 'em.  *PEW!* *PEW!* *PEW!* *PEW!*  HEEEEEEEEEEEEEEEEEEYYYYYYYYYYYYY.", lostem);
		addPlayChat("cAce Ventura - The Popular One", "Hahahaha haha ha.  My!  Aren't I the popular one?", popular);
		addPlayChat("dAce Ventura - Guano Bowls", "*KLCHHPPHT*  Guano bowls.  Collect the whole set!", guano);
		addPlayChat("eAce Ventura - Libby Libby", "Libby libby, wah.  *POP*  Chim chimminey chim chim charoo.  HIYA.  *RUH*", chim);
		addPlayChat("fAce Ventura - Chicago", "CHI-CA-GO!  YER OUTTA THERE.  Go on.  Yer gone.  Go on!", chicago);
		addPlayChat("gAce Ventura - Spank You", "Spank you helpy helperton.", spank);
		addPlayChat("hAustin Powers - Yah", "Ya.  Yaaah.  Ya.  Ya.  Yaaah.  Ya.  Ya.  Ya.  Yaah.  Ya.", yeah2);
		addPlayChat("iAustin Powers - Scotty", "Scotty don't.  Yeah well this is very familiar.  Hang on, let me do what I do.  Uh, would ya stop?  Howbyerowa.  How 'bout I what?", scotty);
		addPlayChat("jAustin Powers - Scotty 2", "Howaiyadowa.  What're you..Howaiyadowa   I don't even..Howhyudoin.  Honestly, isn't this - How 'bout you don't ladies and gentlemen, Scotty don't.", scotty2);
		addPlayChat("kAustin Powers - Scotty 3", "Ladies and gentlemen my plan is - Scotty don't.  Aw come on, yer such a lame ass.  Mini-me et mai le chocolate bien, EH?  Wah, oui ya?  *POP*", scotty3);
		addPlayChat("lAustin Powers - Scotty 4", "Scotty nes pas.  OOoh!  OOoooh!  Oooh! OOOHhhh!  Yeah...all thi - Yeah, yeah.", scotty4);
		addPlayChat("mAustin Powers - Scotty 5", "Oh so we'r - *Agejevia*  You know this is causing me serious psychological harm.  Ohh!  I don't know!  Who am I?  Fine.  Well you know what?  Hey, I would love some chocolate.", scotty5);
		addPlayChat("nAustin Powers - Scotty 6", "C'mon, got me a marlin!  Yeah, HOAH!  *OOF*  You ok Mini-me?  Mmm,hmm.  Ya?  Did I pull too hard?  I don't wanna hurt you.", scotty6);
  		addPlayChat("oAustin Powers - Fook Mi", "Your name is?  Fook Mi.  Oh behave baby....yes!", fookmi);
  		addPlayChat("pAustin Powers - Fook Yu", "Fook Mi, that was fast.  Fook Yu!  Oh yer going the right way for a smack botton and I don't care who knows it!", fookmi2);
  		addPlayChat("qAustin Powers - Fook Yu2", "Zis is my twin sista.  Her name a Fook Yu.  Fook Yu.  Fook Mi!  See?", fookyu);
  		addPlayChat("rAustin Powers - From Holland", "Ahaha - Hey everybody, I am from Holland!  Isn't that veird?  Yes!", holland);
  		addPlayChat("sAustin Powers - Engrish", "I was about to make love to this pretty girl.  Is this true?", engrish);
  		addPlayChat("tAustin Powers - Engrish 2", "If you were aroused, why didn't you pleasure yourself?  What, alone?  Indeed.", engrish2);
  		addPlayChat("uAustin Powers - Engrish 3", "Remember Christmas dinner with the Scottish girl?  The insane one?", engrish3);
  		addPlayChat("vAustin Powers - Engrish 4", "She was the wife of the dancer who lived upstairs.  A lawyer who became a policeman in a truck...", engrish4);
  		addPlayChat("wAustin Powers - Engrish 5", "???????????...Tea kettle!", engrish5);
  		addPlayChat("xAustin Powers - Engrish 6", "Shat on a turtle!", engrish6);
  		addPlayChat("yAustin Powers - FASHA", "Welcome to noonteen seventy five Austin Powers and FASHA.", holland2);
  		addPlayChat("zAustin Powers - THE DUTCH", "There only two things I can't stand in this world.  People who are intolerant of other people's cultures.  And the Dutch.", dutch);
  		addPlayChat("1Austin Powers - Daddy's Pent Up", "Being inside the belly of the beast, night after night all alone.  Daddy's all pent up, LET'S FREAK.", daddy2);
  		addPlayChat("2Austin Powers - Nuts Rubbin", "This diaper's makin my NUTS rub together.  It's gonna start a FIRE.", diaper);
  		addPlayChat("3Austin Powers - Diaper Lady", "Hey diaper lady.  Here's my diaper.  HEHEHEHEHEHE.", diaper2);
  		addPlayChat("4Austin Powers - Pinched One Off", "I think I might of pinched one off too soon.  Oh aye, I left a rosebud in there for ya.  Hehehehe.", diaper3);
  		addPlayChat("5Austin Powers - Nurple!", "OOOOOOOOHHHH MY TITTIES.  OH, YA GAVE ME A NURPLE.", nurple);
  		addPlayChat("6Austin Powers - Soil Yourself", "Did you just soil yourself?  Maybe!  EHehehEHE.  It did sound a lil wet didn't it?  Right at the end.  Oooh!", soil);
  		addPlayChat("7Austin Powers - Let's Smell", "Let's have a smell, alright.  *SNIFF*  Ooh...wofting, wofting!  Oh everyone likes their own brand, don't they?  This is magic.", soil2);
  		addPlayChat("8Austin Powers - Analysis", "Alright, analysis.  Ooh, it smells like carrots and throw up.  Ooh, that could gag a maggot.", soil3);
  		addPlayChat("9Austin Powers - Hot Sick Ass", "I smell like hot, sssick ASS in a dead carcas.", soil4);
  		addPlayChat("0Austin Powers - Hicks", "Breaker - breaker one niner, this is Goldywang....over.", hicks);
  		addPlayChat("-Austin Powers - Hick Convoy", "10-4 there Goldywang, this is Rubberducky what's yer 10-20, over.  I've got Preperation-H in my rear and Schmokey the Bear in my backdoor.  Ve got us a convoy, over.", hicks2);
  		addPlayChat("=Austin Powers - Hick Monkey Nuts", "*YEE-HAWW*  Copy that ya sum-bitch pile a monkey nuts!", hicks3);
  		addPlayChat(",Austin Powers - Bloody Mole", "BLOODY MOLE.  I gunna chop it off and cut it and make some guaca-MMMOLAY.", mole);
  		addPlayChat(".Austin Powers - Boobs", "BOOBS.  Boobs Ozzie?  These filmmakers are just *BEEEEP*'in boobs.", ozzie);
  		addPlayChat("/Austin Powers - Same Joke", "Whaddya mean Dad?  Well they're using the same *BEEP*'in joke as they did in the last Austin Powers movie.", ozzie2);
		
setPlayChatMenu("=Global 20");
		addPlayChat("aHow High - Dogs Hump", "When I was 12....I used to luuuuv watching my dogs hump.", dhump);
  		addPlayChat("bHow High - Hoggin it All", "Hoggin all the good sh*t huh, ok that's fine.  *PPPLLTTT*  Let's go Ivory.", hoggin);
  		addPlayChat("cHow High - What?", "What?  What?  What?  What?  What?", yodog);
  		addPlayChat("dHow High - YO DJ", "Yo DJ.  HIT MEEEEEEEEEEEEEEEEEEE.", dj);
  		addPlayChat("eHow High - Family Pimpin", "Man Ima tell you sumpin.  This pimpin that I got in my blood, it came from a family tree.", pimpin);
  		addPlayChat("fHow High - Grandaddy", "My Grandaddy was a pimp.  My Great-Great-Great Grandaddy was a pimp.", pimpin2);
  		addPlayChat("gHow High - Since Been Pimpin", "I'm talkin about pimpin been since pimpin since been pimpin since been pimpin.  It's in yer blood line baby.  And you will never be that.  Ah why what..why bu - I...", pimpin3);
  		addPlayChat("hHow High - ?????", "???????????", pimpin4);
  		addPlayChat("iHow High - Dunno What it Means", "???  Man, I still don't know what that sh*t means, but it sounds good!", pimpin5);
  		addPlayChat("jHow High - ????? Pt 2", "W.  T.  F.", pimpin1);
  		addPlayChat("kHow High - Intellect B*tches", "We come in here to get our old b*tches, but if dey ain't in here....we're gonna go ahead 'n get them intellect b*tches and start our own new stable, ya feel me?  Just keep it pimpalicious tho baby, keep it pimpa-LICIOUS.", pimpa2);
  		addPlayChat("lHow High - Pimpalicious", "Just keep it pimpalicious tho baby, keep it pimpa-LICIOUS.", pimpa);
  		addPlayChat("mHow High - Pimpin Here", "This is pimpin here.  I've been on Wheel of Forture, Price is Righ...Oh bitch it's funny?  Come on wit it.  Oh boy y'all rather piss off barracuda than piss off powda.", pimpin6);
  		addPlayChat("nHow High - Like Da Ladies", "What're you doing in here Phil?  You *are* one of my biggest customers.  You like d'em hand jobs.  B*tches pissin on ya all the time.  I like the ladies!", pimpin0);
  		addPlayChat("oHow High - Pledge Allegiance", "I pledge allegiance to da pimp, of the united pimps of America.  *slap!*", pledge);
  		addPlayChat("pHow High - Pimpology", "Today's class is called Pimpology 1 and 2.  I'm 1, that's 2.", pimpol);
  		addPlayChat("qHow High - Come out Right", "It got to come out right.  And if don't come out right, you ain't gonna get da pimp tonight.", pimpin7);
	        addPlayChat("rHow High - Come on Wit it!", "C'mon wit it!  And llllet it ffffly!", pimpin8);
  		addPlayChat("sHow High - Not Yo 'self", "NOT YO 'SELF.  SH*T.  That was DEEP....that was deep.", pimpin9);
		addPlayChat("tJoe Dirt - Buffalo Bob", "'m only doing all of this cuz I heard that that Buffalo Bob guy shoved a road flare up yer bunghole.  WHAAAAT?  *HOO-RAH*", flare);
  		addPlayChat("uJoe Dirt - A Fork?", "You got like warm water and - A FORK! Yeah. NO! Owww. What?", fork);
  		addPlayChat("vJoe Dirt - Puts the Lotion on", "It puts the lotion on.  You have NO IDEA WHAT KIND OF HELL I CAN BRING YOU.  OOOohhh alriiiiight, enuf ya broken record, ok.", lotion);
  		addPlayChat("wJoe Dirt - Puts the Lotion on 2", "It puts the lotion on its skin.  NOW.  Well, say it don't spray it brother, daaang.  I need a TOWEL now.  IT DOES WHAT ITS TOLD.", lotion2);
  		addPlayChat("xJoe Dirt - Ain't no Meteor", "It ain't no meteor, this is a big ol frozen chunk a shit.  *WHAAA?*  Oh ya.", meteor);
  		addPlayChat("yJoe Dirt - Septic Tank", "Ewww!  Take it away!  Stop it!  Somebody help me.", septic);
  		addPlayChat("zJoe Dirt - Septic Tank 2", "Help me!  Ewww.  Is it done?  How much is in there?", septic2);
  		addPlayChat("1Joe Dirt - What you say?", "What'd you say?  Oohh.  Yer talkin to me all wrong.  It's...it's the wrong tone.  You do it again....I'll stab you in the face with a sodering iron.", soder);
  		addPlayChat("2Joe Dirt - Does your Mother Sew?", "Is that right?  Let me ask you somethin.  UH?  Does your mother sew?  *B-BOOM*  GET ER TO SEW DAT.", sewdat);
		addPlayChat("3Make it Happen Cap'n!", "Ok!  Let's make it happen, Cap'n!", capn);
  		addPlayChat("4Freakin Out", "I'm freakin out man.", fout);
		
setPlayChatMenu("[Global 21");
		addPlayChat("aOffice Space - Hey Guys", "Hey guys.  wsUP g.", supg2);
  		addPlayChat("bOffice Space - wsUP g", "wsUP g.", supg);
  		addPlayChat("cOffice Space - Pronounce Name", "No one in this country can ever pronounce my name right.  Aie, it's..it's not that hard.  Ni-E-Nana-Ja.  Nienanaja.  Yeah well at least your name isn't Michael Bolton.", bolton2);
  		addPlayChat("dOffice Space - Ass Clown", "You know there's nothing wrong with that name.  There *WAS* nothing wrong with it.  Until I was about 12 years old and that no-talent ass clown became famous and started winning Grammy's.", bolton3);
  		addPlayChat("eOffice Space - Go By Mike", "Why don't you just uh...go by Mike, instead of Michael?  No way.  Why should I change?  He's the one who sucks.", bolton4);
  		addPlayChat("fOffice Space - Real Name?", "Is that your real name?  Yeah.  Are you any relation to the pop singer?  No it's just a coincedence.  Because I'll be honest with ya, I love his music!", bolton5);
  		addPlayChat("gOffice Space - Bolton Fan", "I do, I'm a Michael Bolton fan.  For my money, I don't know if it gets any better than when he sings When a Man Loves a Woman.", bolton6);
  		addPlayChat("hOffice Space - Fudgepackers", "I told those fudgepackers I like Michael Bolton's music.  Oh....that is not right Michael.", bolton7);
  		addPlayChat("iOffice Space - Not Gonna Work", "We're gonna be getting rid of these people here, uh...first Mr. Samere Nagahhheeee..yuh Naga...Naga...not gonna work here anymore anyway.  HAHAHA.", notg);
  		addPlayChat("jOffice Space - PC Load Letter?", "That yer supposed to figure out what you would..want to do if....PC Load Letter?  WTF does that mean?", pcload);
  		addPlayChat("kOffice Space - Paper Jam", "No, not again I....WHY does it say paper jam when there IS no paper jam.  I swear ah one of these days I..I..I..I just kick this piece of sh*t out the window.", pjam);
  		addPlayChat("lOffice Space - The Trick is", "He says the trick is, kick someone's ass the first day or become someone's bitch.  Then everything will be alright.", trick);
  		addPlayChat("mOffice Space - MOTHER SHITTER", "MOTHER SHITTER.  SONUVA...ASS.  MMMOO...I JUS *BANG* *BANG* *BANG*", mother);
  		addPlayChat("nOffice Space - The Monday's", "Uh oh!  Sounds like somebody's got a case of the Moondays!", moondays);
  		addPlayChat("oOffice Space - OOhh...yeah", "OOhh...yeah.  Ummm...I'm gonna have to go ahead and sort of disagree with you there.  Yeah.", hmmm);
  		addPlayChat("pOffice Space - Cornhole", "Hey Peter.  Yeah?  Watch out for your cornhole bud.  Ok Lawrence.", cornh);
  		addPlayChat("qOffice Space - Radio", "Well, I..I was told that I could listen to the radio at a reasonable volume.", radio);
  		addPlayChat("rOffice Space - Radio 2", "Well, I..I..I..I told Bill that if if Sandra's going to listen to her headphones while she..while she's filing, then I should be able to listen to the radio while I'm collating, so I don't see why I should have to turn down the radio because....", radio2);
  		addPlayChat("sOffice Space - Talk To Payroll", "Mr. Lumbergh told me talk to payroll, and then payroll told me to talk to Mr. Lumbergh.  And I, I still haven't received my paycheck.  And he took my stapler.", stapler2);
  		addPlayChat("tOffice Space - Moved My Desk", "And he never brought it back.  And then they moved my desk to storage room D, and there was garbage on it.", stapler3);
		addPlayChat("uPimpbot 5000 - All Ages", "A must for children of all ages.  Let the kiddies dress in style.", ages);
  		addPlayChat("vPimpbot 5000 - Sho 'nuff", "Sho 'nuff.", shonuff);
  		addPlayChat("wPimpbot 5000 - Cherrywine", "The Pimpbot is feelin fine 'n Cheeerry wine.", cwine);
  		addPlayChat("xPimpbot 5000 - White Bread", "Here ya go, white bread.", hite);
  		addPlayChat("yPimpbot 5000 - Circuit City", "All the bitches think I'm pretty, bought my face at Circuit City.", circuit);
  		addPlayChat("zPimpbot 5000 - El Dorado", "Here are the keys to my El Dorado.", eldorado);
  		addPlayChat("1Pimpbot 5000 - Scrotum", "Got a brand new high speed modem and a silver-plated scrotum.", scrotum);
  		addPlayChat("2Pimpbot 5000 - Later Whitey", "Later white bread.", later);
  		addPlayChat("3Pimpbot 5000 - IMA PIMP", "Conan, I am a PIMP.", iamapimp);
  		addPlayChat("4Pimpbot 5000 - I'll Cut You", "Mess with any of my ho's, and I'll cut you.", cut);
  		addPlayChat("5Pimpbot 5000 - Like Jewelry", "I like the jewelry.", jewelry);
  		addPlayChat("6Pimpbot 5000 - Gots 2 Impress", "I gots 2 impress da bitches.", dabitches);
  		addPlayChat("7Pimpbot 5000 - Yo Momma", "I got micro-chips from Yokahama, and I'll be turning out yo momma.", yomomma);
  		addPlayChat("8Pimpbot 5000 - Freaky Ho's", "Why not blow off next week shows and come get freaky with my ho's.", freaky);
  		addPlayChat("9Pimpbot 5000 - Cut You Foo", "I will cut you, foo.", cut2);

setPlayChatMenu("zGlobal 22");
		addPlayChat("1FAN - You Remedial", "You remedial.  What does that mean?  RETARDED.  Now that wasn't nuthin.", remed);
		addPlayChat("2FAN - Santa Claus", "All I want Santa Claus is 2 fat b*tches and a bag a weed and 2 bags of chips to give 'em t..t..to the fat b*thces.", santa);
		addPlayChat("3FAN - Santa Claus 2", "All I want is 2 fat b*tches that smell like cheeseburgers so chico can lick on 'em.", santa2);
		addPlayChat("4FAN - Santa Claus 3", "All I want is a fat b*tch with a name belt with fake glitter on it.", santa3);
		addPlayChat("5FAN - Blow this Whistle", "You gonna make me blow this whistle, I clear all this sh*t out w - Shut the F*** you AND ur whistle.  *RIGHT!*", clear);
		addPlayChat("6FAN - Top Flight Security", "We're Top Flight Security of da world Craig.  Not just da city, da world.  Been jacked by Santa Claus, all kind 'a sh*t.", top);
		addPlayChat("7FAN - Top Flight Security 2", "I ain't nevah heard of no policy like that, uh-uh.  Cuz you ain't never met a Top Flight Security nigga like me.", topf);
		addPlayChat("8FAN - Got Her Trained", "You got her trained nigga, daaamn!  Haha.", train);
		addPlayChat("9FAN - Triple O.G.", "Yo yo wsUP O.G. triple O.G...O.G. triple triple....O.G.", og);
		addPlayChat("0FAN - Salad Toss", "So either I'ma get my rent money TODAY?  Or somebody gettin their salad tossed tonight!  *DAMN*  Eh, you know what that ain't even necessary Ms. Purdy.", saltoss);
		addPlayChat("aFAN - Lined Up", "Get that lined up.  Don't you worry about it B*TCH, I know somebody like it.", lined);
		addPlayChat("bFAN - Gonna Kick In", "TO-DAY...is the day you mutha F****** is gonna kick-in.", kickin);
		addPlayChat("cFAN - Son's a Fag", "You like cuz yer son's a fag?  Shut up b*tch.", fag2);
		addPlayChat("dFAN - Group Hug Nigga", "Group hug nigga.  C'MON CRAIG.", ghug);
		addPlayChat("eFAN - Merry Christmas", "Merry Christmas niggets.", niggets);
		addPlayChat("fFAN - Magically Delicious", "That nigga look magically delicious, nigga.", magic2);
		addPlayChat("gFAN - Good Observation Buddy", "Oh good observation buddy.", bud);
		addPlayChat("hFAN - Bro's BBQ", "Bro's BBQ.  Tastes so good, make you wanna slap yo momma!  *SLAP*", bbq);
		addPlayChat("iFAN - Pinky Christmas", "Man I feel good!  Hey ya'll everybody.  Merry Christmas NIGGYUH, from Pinky NIGGYUH!", merry);
		addPlayChat("jFAN - The Matrix", "*WHOA*  That was like the Matrix.", matrix);
		addPlayChat("kFAN - NOOOO", "NOOOOOOOOOOOOOO#%@$!", nooo);
		addPlayChat("lFAN - Pimpin In It", "Hold up, wait a minute.  Let me put some pimpin in it.", pimp4);
		addPlayChat("mFAN - Phone Book", "Phone book?  Nigga I ain't even in the phone book, what th...nigga you bett...this ain't funny nigga, I b*tch slap your ass.", phone2);
		addPlayChat("nFAN - Wet Dream?", "Wet dream?  Nigga wait a minute, I'M A PIMP.  You lost your mind?", pimp2);
		addPlayChat("oFAN - Am I In Prison?", "What the hell?  Am I in prison?  Hold on, DONNA - PIMP DOWN.", pimp5);
		addPlayChat("pFAN - Pimpin Pimpin", "PIMPIIIIIIIIIN, PIMPIN PIMPIN.", pimp3);
		addPlayChat("qFAN - Just Gonna Take It", "You was just gonna take it, wasn't ya....mmm?  You thought I was play-pimpin, didn't ya?", pimp6);
		addPlayChat("rFAN - Pimp In Distress", "Man down.  11-30.  Pimp in distress!", pimp7);
		addPlayChat("sFAN - Ho's Will Come", "Yo, if I build it, the ho's will come - learn from the best.  God bless ya Mike!  Keep yer game tight hear playa?  Absolutely.", pimp8);
		addPlayChat("tFAN - Speakin of Cockroaches", "Speaking of cockroaches, where was your atenna's when them nigga's was stealing my sh*t out da store.", roach);
		addPlayChat("uFAN - Studdering", "Huh?  Stop studderin.  Deh deh deh deh, deh deh deh deh?", studder);
		addPlayChat("vFAN - Heard it?", "*OOWWWWWWW*  Have ya heard it?  Huh?  Huh?  Yees.  Yeeeeees.", yees);
		addPlayChat("wFAN - I'm a BOY", "SHUT UP.  What the hell you talkin about?  I AM A BOY.", boy);
		addPlayChat("xFAN - Dismissed", "You know what?  I'm tired of your presence, dismissed.", tired2);
		addPlayChat("yFAN - SUE YOU", "Nah, nah let him through.  Let him through.  C'mon.  C'mon you you little short mutha f****, I'ma SUE YO ASS.  C'MON CUT ME.", sue);
		addPlayChat("zFAN - Plunger", "I say you stick a plunger in his ass.", plunger);
		
setPlayChatMenu("qGlobal 23");
		addPlayChat("1FAN - Ducking & Dodge", "You two nigga's been ducking and dodging me for 'bout THREE WEEKS now.", eeks);
		addPlayChat("2FAN - What the Hell?", "WHAT THE HELL IS GOIN ON AROUND HERE?", th);
		addPlayChat("3FAN - Dislocated", "Like to dislocated my shoulder.  Ever been to the pen?  Hell no.", pen);
		addPlayChat("4FAN - Let it Happen", "You know what this is.  New boot?  Don't fight this sh*t.  Fight what sh*t?  Just let it happen.  Let what happen?  What the hell you doin?", fight);
		addPlayChat("5FAN - C'MON", "Ah we are NOT little no 'mo.  C'MOOOOOOON.", cmon3);
		addPlayChat("6FAN - Burn the Pimp", "That's all you gotta do, just burn the pimp one time.", burn);
		addPlayChat("7FAN - Like a Snitch", "Daddy how I look?  Like a damn snith.  Aww man, that's cold.  Look like you 'bout ready to go tell on somebody right damn now.", snitch);
		addPlayChat("8FAN - Extra Medium", "Extra medium?  Extra Medium?  Man, gotta have somethin for the skinny nigga's.", skinny);
		addPlayChat("9FAN - Santa Claus!", "You done hit Santa Claus foo!", sclaus);
		addPlayChat("0FAN - One that Wink", "Next time I'ma shoot you in the one that wink, and not the one that stink.", ink);
		addPlayChat("aFAN - In Between", "Oh this is bullsh*t.  I'm in between a pimp and a hard place.", hard);
		addPlayChat("bFAN - Baby Gap", "Can I help ya playa?  No.  No?  Well this ain't the Baby Gap.", gap);
		addPlayChat("cFAN - Crisis", "Oh ya, I'm good in a crisis.  Hold on.  Donna, Code Red.", crisis);
		addPlayChat("dFAN - OOHHH!!", "OOOOHHH!!!  *DAMN*  Someone call 9-1-1!", call);
		addPlayChat("eFAN - B-B-B-Q", "Open the door, let's do this.  Put down the B-B-B-B-Barbeque.", bbq2);
		addPlayChat("fFAN - Doesn't Matter", "How are you?  Doesn't matter, get the trunk.  Mm-hm-mm.", trunk);
		addPlayChat("gFAN - Top Flight Security", "I want you to know who you talking to too.  Ya?  Top flight mutha f***** security.", topf3);
		addPlayChat("hFAN - I'm the Law", "And I'm the law around here.  TOP FLIGHT.", topf4);
		addPlayChat("iST - Double Cheeseburger", "Gimme a uh...double bacon cheeseburger.  Double bacon cheeseburger, it's for a cop.  *Roger*", dbc1);
		addPlayChat("jST - Spit in it Now?", "What the hell's that all about?  You gonna spit in it now?  No, I was just telling him that so he makes it good.  Don't spit in that cop's burger.  Ya, thanks.  *Roger, holding the spit*", dbc2);
		addPlayChat("kST - Hold the Spit", "Gimme a uh...pie.  Apple.  Do you want me to hold the spit?  Haha, just kidding Officer Farva.", dbc3);
		addPlayChat("lST - Dippa Size", "So um, do you wanna dippa size your meal for a quarter more?  Want me to puncha-size your face?  FOR FREE?", dipa1);
		addPlayChat("mST - Dippa Size 2", "Now don't give me any lip.  It's just a quarter, and look how much more you get.  I said no.  It's just 25 cents.", dipa2);
		addPlayChat("nST - Liter of Cola", "Gimme a uh...liter of cola.  A what?  A liter of cola.  Liter cola, do we make liter cola?", liter1);
		addPlayChat("oST - Liter is French", "Liter is French for gimme some F***** cola before I break both F***** legs.  Alright, alright RELAX.", liter2);
		addPlayChat("pST - Powdered Sugar", "I'm sorry about the delousing Rod.  It's standard procedure.  It's powerdered sugar.  The lice HATE the sugar.  Listen, Rod - it's delicious.", sugar2);
		addPlayChat("qST - Unit 23", "Unit 23, come in 23.  Do you need me out there?  Do you need my assistance?  Is my pre - Shut up Farva.", farva);
		addPlayChat("rST - Denim Dan", "Look who's talking Denim Dan.  You look the President, Chairman and CEO of Levi-Straus.  Where'd ya get the Canadian tuxedo?  Hah.", denim);
		addPlayChat("sST - Six Schlitz's", "Gimme six Schlitz's.  Uh no Schlitz.  Whatever's free.", free);
		addPlayChat("tST - Chicken F*****", "License and registration.  Chicken F*****.  *BA-CAAWWWK!*", chick);
		addPlayChat("uST - Drug School", "See?  Where'd ya learn that Cheech?  Drug school!  Shut up Farva.", farva2);
		addPlayChat("vST - Lock 'n Load", "Alright assholes.  Quit talking about me.  Lock 'n load Ramathorn, let's kick some tail.  We weren't talking about ya, ya big idiot.  Bullsh*t.", load);
		addPlayChat("wST - Old Locker", "Guess that's it for the ol locker huh?  She stinks like ass, but I'll sure miss her.  I guess you can say that about all my girls.", locker);
		addPlayChat("xST - Car RamRod", "Rabbit.  Say car RamRod!  I got a Plymouth Voyager - Say car RamRod! - Hold on.", ramrod2);
		addPlayChat("yST - Car RamRod 2", "Ya didn't say it.  Oh I forgot.  I wrote it on the paper.  Oh ya!", ramrod3);
		addPlayChat("zST - Home Didlies", "What's up home didlie's?  Did I miss the song?  *ya*  Sing it again, rookie BI-OTCH.", rook);
		
setPlayChatMenu("uGlobal 24");
		addPlayChat("1ST - Viagra", "How bout we uh...pop a couple Viagra.  And issue tickets with raging mega-huge boners!  Hahahaha.", viagra);
		addPlayChat("2ST - Bar of Soap", "Oh look a bar of soap.  Hohoho sh*t!  I got you good you F*****!", soap);
		addPlayChat("3ST - Dippa's Burger", "And yer banned from Dippa's Burger.  *DAMMIT*  Get some rubber gloves.  From now on, yer my cleaning lady.", banned);
		addPlayChat("4ST - Afghanistanimation", "It's Afghanistanimation!  The monkey has a butler?  Great!  Is that what they do in Arabia Thorny?  How the hell should I know?", afghan);
		addPlayChat("5ST - Shenanigans", "Hey farva, what's the name of that restaurant you like with all the goofy sh*t on the walls and the mozarella sticks?  You mean shenanigans?  *OOHHH!*  Talkin about shenanigans right?  PUT THOSE AWAY.", shenan2);
		addPlayChat("6ST - Bite it Rook", "Bite it rook, make him look like a dick.  Nah.", biteit);
		addPlayChat("7ST - My 9", "Sh*t man, I was just about to pull out my 9 and put a cap in that pigs ass.  *Kuhh!*  WTF?", cap);
		addPlayChat("8ST - Bear F*****", "Excuse me.  Bear F*****.  Do you need assistance?", bfer);
		addPlayChat("9ST - Two by Four!", "Look what I found!  A 2x4!", found);
		addPlayChat("0ST - Enchilada Platter", "I will have the enchilada platter with two taco's and no guacamoles.  Mike?  Ya chief.  I'll take a chichilla!  Hahahaha.", mexico1);
		addPlayChat("aST - I'm Mexican", "I don't get it.  Taco's?  They think I'm Mexican.  Yer not Mexican?", mexico2);
		addPlayChat("bST - Boys Like Mexico?", "You boys like Mexico?  *YEEEEEAAAA*", mexico3);
		addPlayChat("cST - A Cat", "Do I look like a cat to ya boy?  Am I jumping around all nimbly-bimbly from tree to tree?  No.  Am I drinking milk from a saucer?  Well do you see me EATING MICE?", meow);
		addPlayChat("dST - Mustache Ride", "Who wants a mustache ride?  I vant vone.  I VANT VONE!  I do!  I do!  I DO!", mride);
		addPlayChat("eST - Stinks Like Sex", "Stinks like sex in here.", stink);
		addPlayChat("fST - Ya sure", "Are you ok?  Ya sure.  Yes sir?  Yes sir.  No did you say yes sir?  I think he said ya sure.", yasure);
		addPlayChat("gST - Ya sure 2", "What you say man?  Well I said ya sure, well literally what I said was ya sure....sir.  So you are ok?  Yes sir.", yasure2);
		addPlayChat("hST - Snozberries", "The snozberries taste like snozberries!  Hahahaha.", snoz);
		addPlayChat("iST - Plz No", "Officer Rabbit and I are gonna stand here while you three smoke the whole bag.  Please no.", plzno);
		addPlayChat("jST - Gravy", "Ooh, c'mon we're like the sons you never had.  If you were my son Mac, I woulda smothered you by now.  Smothered me in gravy ya big dirty man.", gravy);
		addPlayChat("kOS - Faggit", "I recommend you stop being such a FAGIT.", fagit);
		addPlayChat("lOS - Street Legal", "I took the restrictor plate off to give the Red Dragon a little more juice, but uh let's keep that on the down low.", street);
		addPlayChat("mOS - Frank the Tank", "Frank the Tank is not coming back, OK? That part of me is over.  - WE'RE GOING STREAKING!! -", streak);
		addPlayChat("nOS - We're Streaking", "We're STREAKING!  Who's streaking?  There's...there's more coming.", more);
		addPlayChat("oOS - Get in the Car", "Frank, get in the car.  Everybody's doing it!!  NOW.  Ok!", ok);
		addPlayChat("pOS - KFC", "Honey you think KFC's still open?", kfc);
		addPlayChat("qOS - Trust Tree", "What?  What I thought we were in the trust tree with - in the nest, are we not?", tree);
		addPlayChat("rOS - SPEDISH", "*SPEDISH* Do you trust that we provided you with enough slack so that your block will land safely on the lawn???", trust);
		addPlayChat("sOS - And BLUE", "And BLUE.  Yes Sir.  Do you trust that I do not want to see you die here tonight?  Sir Yes Sir.  Blue yer my BOY.  Thank you sir.", blue);
		addPlayChat("tOS - Pace Yourself", "*DAAAAAAK*  Frank, just pace yourself.  Uh...copy that. Just got a little over excited.  Sorry.", pace);
		addPlayChat("uOS - Ice Lemonade", "Hey BLUE?? How come there's no ICE in my lemonade?  Sir, sorry Sir.  You drop down and you give me 10....NOW.  Ye, yes Sir.  LETS GOOO.", lemon2);
		addPlayChat("vOS - See Blue!", "I think I see Blue.  Shhh.  He looks glorious!", blue2);
		addPlayChat("wOS - Freak OUT", "That's RIGHT!  We can't have anyone freak out out there, OK?  WE'VE GOTTA KEEP OUR COMPOSURE.", fout2);
		addPlayChat("xOS - AWESOME", "Awesome.....YES.", ayes);
		addPlayChat("yOS - I am BACK", "I am BACK!  *WOAH*  You KNOW IT.", back2);
		addPlayChat("zOS - I KILL YOU", "When I left she said, 'Wincy...if you screw this up I kill you.'  She showed me the knife!", incy);
		
setPlayChatMenu("mGlobal 25");
		addPlayChat("-OS - Shut Up", "Ain't that right my litt - *MMHH* - oh what?  That's what I thought.  Shut up.", os);
		addPlayChat("=OS - Puncture the Skin", "Yeah it is cool.  They say it can puncture the skin of a Rhino from a *GANK*  GEH DA OOOWW.", os1);
		addPlayChat("3OS - That's Awesome", "OOoohh....Ohh.  Y-E-E-S!  That's AWESOME.", os2);
		addPlayChat("4OS - Jugular", "What?  You just took one in the jugular man!  Hah!  *Whoa*", os3);
		addPlayChat("5OS - Is this Bad?", "Is this bad?  You should pull that out.  That sh*t is not cool.", os4);
		addPlayChat("6OS - Dart in Your Neck", "Wait....wait.  Pull what out?  The dart man.  You got a f*ckin dart in yer neck.", os5);
		addPlayChat("7OS - Yer Crazy!", "Hahaha....yer craz - yer crazy man.  Yer crazy!  I like you.  But yer crazy!", os6);
		addPlayChat("8OS - Feel Tired", "I feel tired.  *MAAAAAAAAAAAAAHHHHHHHHHHH*", tired);
		addPlayChat("9OS - Congratulations", "Alright let me be the first to say congratulations to you then.  You get one vagina for the rest of your life.  Real smart Frank, way to work it through.", vagina);
		addPlayChat("0OS - Watching - Juding", "There's my wife now.  See that?  Always smiling.  Hi honey.  Juding, watching.  Look at the baby, look at the baby.  She's coming down the isle beany, let it go.", isle);
		addPlayChat("aOS - Don't Do It", "To join Franklin and Marisa *COUGH*DON'T DO IT*COUGH*", doit2);
		addPlayChat("bOS - Earmuffs", "Why take time out of my schedule just to try to help you get over th - Earmuffs.  That whore that you dated?", earmuff);
		addPlayChat("cOS - French Kiss", "OOH.  HE JUST FRENCH KISSED ME.", fkiss);
		addPlayChat("dSP - F**K YOU WHALE and F**K YOU DOLPHIN", "F**K YOU WHALE AND F**K YOU DOLPHIN", dolphin);
		addPlayChat("eSP - F**K YOU DOLPHIN AND WHALE", "F**K YOU DOLPHIN AND WHALE", hale1);
		addPlayChat("fSP - F**K YOU DOLPHIN1", "F**K YOU DOLPHIN", dolphin2);
		addPlayChat("gSP - F**K YOU DOLPHIN2", "F**K you DOLPHIN", dolphin3);	
		addPlayChat("hSP - F**K YOU WHALE", "F**K YOU WHALE", hale);
		addPlayChat("iSP - Pokerface intro", "Muh muh muh muh, i wanna hold em like they do in texas plays", poker7);
		addPlayChat("jSP - Pokerface bridge", "But oh, woah", poker8);
		addPlayChat("kSP - Pokerface chorus", "Can't read my, can't read my, no he can't read my pokerface", poker9);		
		addPlayChat("lSP - Muh muh muh", "muh muh muh", poker4);
		addPlayChat("mSP - I don't give a crap about whales", "I don't give a crap about whales", poker5);
		addPlayChat("nSP/Kanye West - You're a gay fish", "You're a gay fish.", gayfish3);	
		addPlayChat("oSP/Kanye West - I am not gay", "I am not gay, and I sure as hell ain't no fish", gayfish);
		addPlayChat("pSP/Kanye West - Yeah I like fishsticks", "Yeah I like fishsticks", gayfish6);
		addPlayChat("qSP/Kanye West - You like to put fish dicks in your mouth", "You like to put fish dicks in your mouth", gayfish7);
		addPlayChat("rSP/Kanye West - I love putting fishdicks in my mouth", "I love fishsticks. I love putting fishsticks in my mouth", fishdicks);
		addPlayChat("sSP/Kanye West - You're a gay fish2", "You're a gay fish", gayfish8);
		addPlayChat("tSP/Kanye West - I am not gay, and i am not a fish", "I am not gay, and i am not a fish...MAN!", gayfish5);
		addPlayChat("uSP/Kanye West - Genius voice of a generation", "And I'm a genius voice of a generation so I'm not gay!", gayfish9);
		addPlayChat("vSP/Kanye West - I would know", "If i was a homosexual, or a fish, I would know.", gayfish2);
		addPlayChat("wSP/Kanye West - No I am not no gay fish", "No, I am not no gay fish", gayfish4);
		addPlayChat("xSP/Kanye West - Hey man, I'm a genius", "Hey Man, I'm a genius, allright?", genius);
		addPlayChat("ySP/Kanye West - Lyrical wordsmith genius", "I'm a lyrical wordsmith", genius2);
		addPlayChat("zSP/Kanye West - Gay Fish Song I", "I've been so lonely girl", fishsong3);
		addPlayChat("1SP/Kanye West - Gay Fish Song II", "All those lonely nights at the grocery store", fishsong4);
		addPlayChat("2SP/Kanye West - Gay Fish Yo", "Gay Fish Yo", fishyo1);

setPlayChatMenu("]Global 26");
		addPlayChat("1SP - Tampon", "You shouldn't a done that", tampon);
		addPlayChat("2SP - N**gers", "N**gers", niggersbeep);
		addPlayChat("3SP - You're a towel", "You're a towel", towel1);
		addPlayChat("4SP - People aren't interested in autobiographies", "People aren't interested in autobiographies of towels", towel2);
		addPlayChat("5SP - Wanna Get high?", "Wanna get high?", gethigh);
		addPlayChat("6Jon LaJoie - Vagina song 1", "I'm the Wayne Gretzky of sexual stuff, I'm the Hulk Hogan of slamming muff", vagina1);
		addPlayChat("7Jon LaJoie - Vagina song 2", "I have really good sex moves that i learned in China", vagina2);	
		addPlayChat("8SP - Can't read my pokerface", "Can't read my pokerface", poker1);
		addPlayChat("9SP - Pa pa pa pokerface", "pa pa pa pokerface", poker2);
		addPlayChat("0SP - Shave their balls", "Yeah, like when Men shave their balls, its fine, but when a woman does it, she's straaange", shaveballs);
		addPlayChat("aSP - Oh God, SO BORED", "Oh god, I'm so bored - somebody help me!", sobored);
		addPlayChat("bSP - FD in the A", "HAH! You just got F'D in the A", fdinthea);
		addPlayChat("cSP/Michael Jackson - Ignorant", "Nooo, no that's ignorant", ignorant1);
		addPlayChat("dSP/Michael Jackson - People are just ignorant", "Noo, that's ignorant. People are just ignorant and lie and spread rumors about me. Like that I'm dead. But if I was dead, how could i do this?", ignorant2);
		addPlayChat("eSP/MJ - Oh look everyone", "Oh look everyone, I told you I was a alive!", ignorant3);
		addPlayChat("fSP/MJ - Lets climb the tree", "Come on, let's climb the tree... HEE HEEE. Mr. Jackson you can't do this. This is not your body", ignorant4);
		addPlayChat("gSP/Chinese - Aw you shot him in the dick", "Aw dude you shot him in the dick. That's not cool Butters, you don't shoot a guy in the dick", indick);
		addPlayChat("hSP/Chinese - Goddammit Butters", "Goddammit Butters what did I say about shooting guys in the dick?", indick1);
		addPlayChat("iSP/Chinese - You dont shoot a guy in the dick", "You don't SHOOT a GUY in the DICK - OK I'm sorry - It's not OK - defeating the Chinese won't mean anything if we do it by going around shooting people in the dick", indick2);
		addPlayChat("jSarah Marshall - Fuck the lemons", "Hey here's the deal: When life gives you lemons, just say 'f*ck the lemons' and bail", lemons);
		addPlayChat("kFG Cavalcade - Ay Ladies1", "Hey ladies! You like nightclub? We goto nightclub, buy champagne, drink from bottle. Just get in sports car", ayladies);
		addPlayChat("lFG Cavalcade - Ay Ladies2", "Hey ladies! You like mouthsex? We rent limo!", ayladies2);
		addPlayChat("mFG Cavalcade - Quentin 1", "Mr, this is a Hatori Hanso sword, ok?", quentin);
		addPlayChat("nFG Cavalcade - Quentin 2", "Quiet kid, you sound like Uma Thurman! Why doesn't my character have a name?", quentin1);
		addPlayChat("oFG Cavalcade - Beaver 1", "Hey Beaver, what the hell are you doing to my river?", beaver);
		addPlayChat("pFG Cavalcade - Beaver 2", "Hey Beaver, did you eat this tree?", beaver1);
		addPlayChat("qFG Cavalcade - Beaver 3", "Hey did you borrow my DVD of The Departed? cause it's scratched now.", beaver2);
		addPlayChat("rFG Cavalcade - Beaver 4", "What .. a vagina! Yeah, a vagina! From now on, Beaver means vagina. YEAH!", beaver3);
		addPlayChat("sSP - They took our jobs!", "They took our jobs!", tookurjob);
		addPlayChat("tSP - Stan - They Took our jobs!", "They took your jobs!", tookurjob2);
		addPlayChat("uSP - Vote Giant Douche", "Dude, you're supposed to vote for Giant Douche", votefordouche);
		addPlayChat("vSP - Vote for Turd Sandwich", "Vote for Turd Sandwich!", voteforturd);
		addPlayChat("wSP - Biggest Douche nomination", "You are SO a douche! I'm nominating you for biggest douche in the universe award, ya douche!", youdouche);
		addPlayChat("xSP - I'm not a douche", "I'm not a douche!", notdouche);
		addPlayChat("ySP - Biggest Douche Song", "Here he is, the biggest douche in the universe.", bigdouche);
		addPlayChat("zSP - Im here for the party", "Oh Hey, whats goin on. I'm, uh, here for the party", forparty);

setPlayChatMenu(".Global 27");
		addPlayChat("aPortal - When we die", "Do you ever wonder what happens when we die? What? You know, if our lives will ever mean something. Not this again...", afterdeath);
		addPlayChat("bPortal - Worst day", "God, this day can't get any worse.", badday);
		addPlayChat("cPortal - Are you still there?", "Ask her if she's still there. Are you still there? She didn't say anything. No shit Sherlock. What should I say now? Come out bitch!", comeoutbitch);
		addPlayChat("dPortal - See you in hell", "I'll see you in hell, Cube.", cuinhell);
		addPlayChat("ePortal - Cursing", "@%#& #^*$ #@&$ &@^%", curse);
		addPlayChat("fPortal - Douche", "OMG, I hate that guy. What a douche.", douche);
		addPlayChat("gPortal - FU", "F**K you.", fu);
		addPlayChat("hPortal - Hifg-5", "High-fives!", high5);
		addPlayChat("jPortal - I see you", "I see you!", icu);
		addPlayChat("kPortal - Floppy drives", "I would insert my floppy into her disk drives.", insert);
		addPlayChat("lPortal - Lame", "Lame..", lame);
		addPlayChat("mPortal - Owned", "Owned.", owned);
		addPlayChat("nPortal - Portal song", "*Portal song*", portalsong);
		addPlayChat("oPortal - Pwned", "Pwned bitches! Dominated!", pwned);
		addPlayChat("pPortal - Strange game", "A strange game. The only winning game is not to play.", strangegame);
		addPlayChat("qPortal - Tear place apart", "Sometimes I want to tear this whole damn place apart.", tearapart);
		addPlayChat("rPortal - You ar a bitch", "You are a bitch.", urabitch);
		addPlayChat("sPortal - Work here is done", "My work here is done.", workdone);
		addPlayChat("tPortal - Owned", "Owned.", owned);
		addPlayChat("1SP - Derp Dee Derp", "Derp dee Derp", derp1);
		addPlayChat("2SP - Derp Durdelee Durr", "Derp dee Derp, Durdelee Durr", derp2);
		addPlayChat("3SP - Derp deedelee Durdelee Durr ", "Derp dee Derp dee deedelee Durdelee Durr", derp3);
		addPlayChat("4SP - Derpity", "Derpity Derpity Derpity", derp4);
		addPlayChat("5SP - Doo doo doo", "Doo doo doo", derp5);
		addPlayChat("6SP - Durka durr", "Durka durrrr", durkadurr);
		addPlayChat("7SP - Durka durr song", "Durka durr, durrka durrrr", dsong);
		addPlayChat("8Chaccaron Maccaron ", "Chaccaron Maccaron ", chac);
		addPlayChat("9Chica Bomb ", "Chica Bomb! ", chicabomb);

setPlayChatMenu(",Global 28");
		addPlayChat("aSure", "Gee let me think. Ummm....Sure.", umsure);
		addPlayChat("bBaddest MFers", "I'm one of the baddest mutha fu***'s of all time.  I'm one of the best singers and one of the best lookin mutha fu***'s you've ever seen.  Hold my drink b*tch.", rj1);
		addPlayChat("cI'm Rick James", "I'm Rick James B*TCH.", rj2);
		addPlayChat("dHabitual Line Stepper", "And I ended hafing to whip his ass, man you know because...you know he would step across the line.  Habitually, he's a habitual line stepper.", rj3);
		addPlayChat("eCharlie Murphy", "CHARLIE MURPHY!  *BAP*", rj4);
		addPlayChat("fCold Blooded", "That was...COLD BLOODED!", rj5);
		addPlayChat("gMy Ghetto Side", "My ghetto side was goin, 'yo stomp this mutha fu*** out right here.'", rj6);
		addPlayChat("hBleeding In The Chest","Yo man my forhead is BUMPIN man.  Now that you mention it....I think I'm bleedin inside my chest.", rj7);
		addPlayChat("iSex With Charlie", "B*TCH.  Come over here and have sex with Charlie Murphy.  I'm Rick James b*tch.", rj8);
		addPlayChat("jNice Place", "Nice place nigga.", rj9);
		addPlayChat("kFuck Your Couch", "F*** yo couch nigga.  Haha, buy another one you rich mutha F****R.  F*** YO COUCH NIGGA, F*** YO COUCH.", rj10);
		addPlayChat("lMidnight Evil", "DARKNESS YOU BLACK MIDNIGHT EEEVIL MUTHA F*** BLACK MAGIC DARKNESS.", rj11);
		addPlayChat("mCold As Ice", "You are cold as ice.", rj12);
		addPlayChat("nRuck Yo Couch 2", "But still, Rick James....even after taking a beating like that.  F*** YO COUCH NIGGA.", rj13);
		addPlayChat("oCocaine!", "Cocaine's a helluva drug.", rj14);
		addPlayChat("pChina Club", "Welcome....to the CHINA CLUB.  A CHUNGA CHUNG CHANG, A CHEYA CHUNG CHUNG CHANG.  HAHAHAHA.", rj15);
		addPlayChat("qFour Thumbs Down", "I wish I had more hands....so I could give those titties four thumbs down!  Hahahahahahaahaha.", rj16);
		addPlayChat("rO.J. Simpson", "And I'll never forget the first thing I seen was O.J. Simpson.  I remember thinking to myself, 'Wow that's O.J. Simpson.  He has a BIG F***IN head man.'", rj17);
		addPlayChat("swSUP Patnuh", "CHARLIE MURPHY!  wsUP PATNUH??", rj18);
		addPlayChat("tFive Fingers", "What did the five fingers say to the face.........what?  *SLAP*!", rj19);
		addPlayChat("uKing Kong", "I'm Rick James b*tch.  EVERYBODY...KING KONG AIN'T GOT SH*T ON ME.", rj20);
		addPlayChat("vSlap A Man", "First of all, you don't slap a man.  *AH-BAAAAM*", rj21);
		addPlayChat("wIt's A Celebration", "It's a celebration!  B*TCH-ES.  C'MERE.  It's a celebration b*tches.  Show Charlie Murhpy your titties.", rj22);
		addPlayChat("xNeva Gave You $$", "They shoulda neva gave u nigga's money.", rj23);


setPlayChatMenu("aAnimations");
		addPlayAnim("aFOOK YU", 1, fookyu2);
		addPlayAnim("iHard On", 8, hardon);
		addPlayAnim("jNo Soup!", 9, soup);
		addPlayAnim("kBingo", 12, brak);
		addPlayAnim("lGame over man", 2, hudson);
		addPlayAnim("mHappy Bleep", 8, bleep);
		addPlayAnim("nHefer with Cheese", 0, hefer);
		addPlayAnim("uYou Shot Me", 3, ushotme);
		addPlayAnim("pFreeze mudda bitch!", 8, mudda);
		addPlayAnim("oOver here", 0, ovrhere);
		addPlayAnim("dMove outta my way", 1, outway);
		addPlayAnim("rSex moves", 2, vagina2);
		addPlayAnim("sMuh Muh Muh Pokerface", 3, poker3);
		addPlayAnim("fMuthaf**kin Gay Fish", 4, gaysong);
		addPlayAnim("zGay Fish Yo", 10, fishyo1);
		addPlayAnim("xPuh Puh Puh Pokerface", 11, poker2);
		addPlayAnim("qCan't read my pokerface", 5, dsong);
		addPlayAnim("eCelebrate 2", 6, cheer2);
		addPlayAnim("wCelebrate 3", 7, cheer3);
		addPlayAnim("vTaunt 1 - how'd that feel?", 8, taunt10);
		addPlayAnim("gTaunt 2 - Come get some", 9, taunt4);
		addPlayAnim("hWave - Hi", 12, hello);
		addPlayAnim("bWave - Bye", 12, bye);
		addPlayAnim("cGoat Ass", 5, goatass);
		addPlayAnim("yAnother Hi", 12, yo);
		addPlayAnim("tRedneck", 6, hick);
		addPlayAnim("nWazzup", 0, zup5);
		addPlayAnim("1Hiyah!", 12, hiyah);
		addPlayAnim("2Yiyah!", 8, yiyah);
		addPlayAnim("3GEE!", 1, gee);
		addPlayAnim("4MEEP MEEP", 12, meep);
		addPlayAnim("5I Hate Goodbyes", 12, hbye);
		addPlayAnim("6Bye Bye Lard Ass", 12, lard);
		addPlayAnim("7Oh Billy", 10, ohbilly);
		addPlayAnim("8We Suck Again", 9, boy3);
		addPlayAnim("9Special Purpose", 7, jerk2);
		addPlayAnim("0Hide Them In My Ass", 5, hide3);


// Commander Menu

function contextIssueCommand(%action, %msg, %sound)
{
   if(%sound != "")
      %msg = %msg @ "~w" @ %sound;
   setIssueCommand(%action, %msg);
}

// $CommandTarget can be one of:

// waypoint
// enemy vehicle
// enemy player
// enemy static
// enemy turret
// enemy sensor
// friendly vehicle
// friendly player
// friendly static
// friendly turret
// friendly sensor


function Commander::StarCommand(%type)
{
   if(%type == "*Attack")
   {
      if($CommandTarget == "enemy static")
			contextIssueCommand(1, "Destroy enemy equipment at waypoint", "attobj"); 
      else if($CommandTarget == "enemy turret")
			contextIssueCommand(1, "Destroy enemy turret at waypoint", "attobj"); 
      else if($CommandTarget == "enemy sensor")
			contextIssueCommand(1, "Destroy enemy sensor at waypoint", "attobj"); 
      else if($CommandTarget == "enemy player" || $CommandTarget == "enemy vehicle")
         contextIssueCommand(1, "Attack enemy " @ $CommandTargetName, "attway");
      else if($CommandTarget == "friendly player")
         contextIssueCommand(1, "Cover " @ $CommandTargetName, "escfr");
      else if($CommandTarget == "friendly vehicle")
         contextIssueCommand(1, "Board APC ", "boarda");
      else
         contextIssueCommand(1, "Attack enemy forces", "attway");
   }
   else if(%type == "*Defend")
   {
		if($CommandTarget == "friendly player")
		   contextIssueCommand(2, "Defend " @ $CommandTargetName, "escfr"); 
		else
		   contextIssueCommand(2, "Defend waypoint", "defway");
   }
   else if(%type == "*Repair")
   {
		if($CommandTarget == "friendly player")
		   contextIssueCommand(2, "Repair " @ $CommandTargetName, "repplyr"); 
		else
		   contextIssueCommand(2, "Repair " @ $CommandTargetName, "repobj");
   }
}

setCommanderChatMenu("");

   addCommand("aAttack", 1, "*Attack");
   addCommand("dDefend", 2, "*Defend");
   addCommand("rRepair", 3, "*Repair");

   setCommanderChatMenu("eDeploy");
		setCommanderChatMenu("eDeploy\\sSensor");
			addCommand("pPulse sensor", 2, "Deploy pulse sensor at waypoint", "deppuls");
			addCommand("jJammer", 2, "Deploy sensor jammer at waypoint", "depjamr");
			addCommand("mMotion sensor", 2, "Deploy motion sensor at waypoint", "depmot");
			addCommand("cCamera", 2, "Deploy camera at waypoint", "depcam");
		setCommanderChatMenu("eDeploy\\aObject");
			addCommand("aAmmo", 2, "Deploy Ammo Station", "depamo");
			addCommand("iInventory", 2, "Deploy Inventory Station", "depinv");
			addCommand("tTurret", 2, "Deploy Turret", "deptur");
			addCommand("bBeacon", 2, "Deploy beacon at waypoint", "depbecn");
		setCommanderChatMenu("eDeploy");
		addCommand("vA.P.C.", 2, "Pilot APC to waypoint", "pilot");

setCommanderChatMenu("kCommand Response");
	addCommandResponse("aAcknwledged", 1, "Command acknowledged", "acknow");
	addCommandResponse("cCompleted", 0, "Objective complete", "objcomp");
	addCommandResponse("uUnable to complete", 0, "Unable to complete objective", "objxcmp");
