// Okay, welcome to my pack.  This is the file where you'll make
// your Presto Preference (tm)(c)(R) changes.
//
// Read the next section very carefully!  This will determine the
// features which are installed.
//
// Where it asks for a key, enter a key name like "v" or "q" or
// "p".  You can also use "control d" or "alt b", in fact, the
// words here are just the same as the words used in the bind()
// command, except that there's no comma between multiple words.
// if you want to disable a feature, set the key to "false".
//
// Some features are just toggles, which you can set to "true"
// if you want the feature or "false" if you don't.
//
// ---------------------------------------------------------------
// Features enabled and bound to keys
// ---------------------------------------------------------------
//
//	New Chat Menu
//
// 	Do you want the new chat menus?  By default I hide these for
// 	distribution.  I think the new chat menus are cool though
// 	and I recommend turning them on.  You can replace your old
// 	menu or put them on a different key.  I tried to make them as
// 	close to the original menus as possible, so you'd want to put
//	them on your V key.  Set this to a key, or "false" to turn
//	off this feature completely.
$PrestoPref::NewChat = "v";
//
//	Old Chat Menu
//
//	This, unfortunately IS NOT AN OPTION.  You must in this file
//	tell me where you have your old voice menu key bound.  Usually
//	this is the V key.  It's whatever you hit to make voice sayings
//	like "need repairs!".  What?  You don't use that menu? Shame
//	on you, you should be a better team player.
//	If you set this to false YOU WILL NOT HAVE YOUR OLD CHAT MENU
//	ANY MORE.  (Which isn't that bad if you have the new chat menu.)
$PrestoPref::OldChat = "control v";
//
//	Inventory Camera
//
// 	Do you want a camera on your inventory station window?  When you
// 	access the station, this feature will also turn you around.
// 	That way you can see who is waiting in line behind you!  This 
// 	is a very cool feature and I turn it on for distribution.
// 	Set this to "true" to turn it on or "false" to turn it off.
$PrestoPref::InvCamera = false;
//
//	Drop Menu
//
//	This is a new menu which will let you drop weapons and ammo
//	by picking them in a menu rather than just dropping whatever
//	you're holding.  I designed it so lights could carry mortar
//	ammo for heavies!  By default I have this menu turned on -
//	but the key I have chosen (control D) is bound to the command
//	"Unable to complete objective" in the default Tribes install.
//	I recommend that before running this script, you go rebind
//	that key.  It's not particularly a "control D" kind of command,
//	and drop is :)
$PrestoPref::DropMenu = "control d";
//
//	Job Menu
//
//	I've introduced a new chat menu, which I call the job menu.
//	This is a version of the V-chat menu which lets you get very
//	specific with what you're doing and what needs to be done!
//	For instance "we need to repair the generator!" instead of
//	just "need repairs."
//	This menu is by default bound to the control-G key.  I would
//	rather you bind it to the G key.  The reason I used control is
//	because G is by default bound to Grenades.  Please reassign 
//	that function.  This menu is designed to go on the G key and 
//	you will find it MUCH more useful on that key.
//	Set it to a key, or to false to disable the feature.
$PrestoPref::JobMenu = "control g";
//
//	Team HUD
//
//	Designed to let you see what's going on with your team, the
//	Team HUD sits at the bottom of your screen and tells you who
//	is on offense, who is on defense, and other useful information.
//	It's especially useful in combination with the Job menu, 
//	because the specific information will be displayed in the HUD.
//	By default, I map this key to control-T.  You can assign it 
//	to a key or "false" to turn off the feature.
$PrestoPref::TeamHud = "control t";
//
//	Dynamic HUD
//
//	This is the feature that got it all started!  KillerBunny's
//	dyn_hud was a great script, and he gave me permission to
//	upgrade it and include it in this script pack.  The DynHud
//	tracks the status of flags in a capture-the-flag game.  It
//	also keeps track of your mines, grenades, and hey, for you
//	statistical types it'll tell you how many painful deaths 
//	you've inflicted ... and how many you've received! :)
//	Bind this to a key, which toggles it on and off - or "false".
$PrestoPref::DynHud = "control h";
//
//	Mute Menu
//
//	The Mute Menu lets you control your client-side muting.
//	From this menu you can mute a variety of messages, including
//	noisy (verbal) messages and repeat messages, for 
//
$PrestoPref::MuteMenu = "control m";
//
//	That's the end of the feature list.  Please read through
//	the rest of the file to see what other options you can set.

// DynHUD prefs
// ---------------------------------------------------------------
//
// The coordinates of the DynHUD in "x y width height"  You probably
// don't want to change the height (it's correct for the number of
// text lines in the HUD)
$PrestoPref::DynHudPosition = "6 85 120 98";
//
//	Want to see the HUD as soon as you start, instead of having to
//	manually toggle it?
$PrestoPref::DynHudDisplayOnEntry = false;

// Favorites position prefs
// ---------------------------------------------------------------
//
// 	The coordinates of the Favorites window in "x y width height"
$PrestoPref::FavoritesPosition = "210 119 240 40" ;
//	(note that this feature isn't enabled yet ;) )

// Camera HUD prefs (Inventory Camera)
// ---------------------------------------------------------------
//
// 	The coordinates of the CamHUD in "x y width height"
$PrestoPref::CamHudPosition = "450 35 168 124";
//
// 	The Field of Vision (5 to 120 degrees) when you're at a station.
// 	I like it at 120, but it's got a fisheye effect some people 
// 	might not like (90 is normal.)  This will not affect the normal
// 	screen, only the camHUD.  I set it to 90 for the distribution.
$PrestoPref::CamHudFOV = 90;
//
// 	Default:  turn around when using inventory station.  I guess
// 	if you're used to backing in to a station you could set this
// 	to false.
$PrestoPref::CamHudTurnAround = true;
//
//	So you're at a station and you want to look around for a second
//	through the little camera window.  Well, hold this key down
//	and you'll be in free look mode - release it and you'll be back
//	to selecting from the inventory.
//	You can also make free-look the default, in which case the 
//	free look button will temporarily turn your pointer back ON
//	instead of turning it off.
$PrestoPref::CamHudFreeLook = "control b";
$PrestoPref::CamHudFreeLookOnByDefault = false;

// Turn Around prefs (Inventory Camera)
// ---------------------------------------------------------------
//
// 	These are used to turn you around in CamHUD - if you're happy
// 	with the turnaround, leave 'em alone.  Otherwise, adjust very
// 	slightly!  Basically, higher time will turn you further around
// 	as will a higher turn speed.
$PrestoPref::TurnAroundSpeed = 0.33;
$PrestoPref::TurnAroundTime = 0.31;	// (seconds)

// TeamHUD preferences
// ---------------------------------------------------------------
//
//	This specifies the location of the TeamHUD.  Note the use of
//	extended coordinates to size it to the full width, and put
//	it at the bottom of the screen.   If you put it near the bottom
//	of the screen it will size upwards, near the top it will size
//	downwards.  The height you specify here will determine the
//	height of a *single row* - as the HUD resizes it will size in
//	multiples of this number.
$PrestoPref::TeamHUDPosition = "100% 100% 100% 14";
//
// 	The number of columns in the team HUD.  Too close, and the text
// 	will write over the next column.  Note that this is sort of
//	dependant on your resolution, at a higher res you might want
// 	more columns.
$PrestoPref::TeamHudColumns = 5;
//
//	Want to see the HUD as soon as you start, instead of having to
//	manually toggle it?
$PrestoPref::TeamHudDisplayOnEntry = false;
//
//	My personal preferences are to move the teamHUD to the lower
//	right corner @ "100% 100% 60% 14", with 3 columns, leaving 
//	space on the left side for a HUD.  I also tried the setting
//	"100% 100% 20% 14" with one column, which lets it grow against
//	the right side.  Feel free to experiment!

// Muting preferences
// ---------------------------------------------------------------
//
//	While client-side muting can save you from a spammer, it
//	also eats a little processor time for every message whether
//	you end up muting it or not!
//	If you want to turn off muting, set this to false.
$PrestoPref::Mute = true;
//
//	Next, you can set your default muting preferences.
//
//	Note that the following preferences will take effect even if
//	you don't have the MuteMenu installed.  However you can use
//	the mute menu to turn them on & off, and also add particular
//	clients to the mute list (which you can't do with these 
//	defaults)
//
// 	In each of the following defaults, you can use the following
// 	words in a space-separated string:
//		* all - every message is muted
//		* noise - any message with noise is muted
//		* repeat - any repeated messages are muted
//		* packs - "Pack" messages are muted.  You'll need to
//			install the mute packs to have this work.
//	For example "noise packs".  In this example *both* noises and 
//	packs will be muted.
//
//	See MUTE.CS for more information about "packs" muting and 
//	how you can build your own muting packs.
//
//	Messages by anyone in the game:
$PrestoPref::Mute[all] = "";
//	Messages by friendly team members:
$PrestoPref::Mute[friendly] = "";
//	Messages by enemy team members:
$PrestoPref::Mute[enemy] = "";
//
//	Obviously if you have something in friendly AND enemy, it might
//	as well be in "all" instead.
//
// 	The following lines are commented out, meaning it will not
// 	actually run.  But they are my default mute settings, which 
//	you can use if you want - by taking out the // in front.
// $PrestoPref::Mute[all] = "repeat packs"; // unnecessary stuff
// $PrestoPref::Mute[friendly] = "";
// $PrestoPref::Mute[enemy] = "noise";	// i don't hear taunts
//	This last one is a bit dangerous because you might miss
//	voice messages like "yes" and "no" or "hi" and "bye".
//
//	When a message is muted, a small HUD will appear to tell you
//	you just missed a message.  This is so that you can't
//	accidentally mute all, and then wonder why you aren't
//	getting any messages.  The default position for the mute
//	HUD is at the top center of the screen, right above the
//	chat window.  If you've moved your chat window you might 
//	want to move the mute HUD too.
$PrestoPref::MutePosition = "50% 0 300 10";
//
//	When a message is muted the HUD will appear for this many
//	seconds to let you know you muted a message.  Set it to
//	false or 0 and you will not see the HUD.
$PrestoPref::MuteDisplayTime = 15;
//
//	If you're worried you might miss a message due to muting,
//	this setting will display the first few characters of each
//	muted message, in the muting HUD.  This will be enough to
//	read "yes" or "no", for instance.  Note that muted messages
//	are always displayed in the console so if you see a muted
//	message you think you might have wanted to see, just pop
//	open the console and read it there in its entirety.
$PrestoPref::MuteDisplayMessage = true;
//
//	Messages are detected as repeats if the same person says the
//	same thing more than one time in (N) seconds.  Here is where
//	you set it.
$PrestoPref::MuteRepeatTime = 15;

// Kill-Tracking preferences
// ---------------------------------------------------------------
//
// 	Kill-tracking is cool and lots of HUDs & scripts use it.  But
//	for some it might be unnecessary overhead, since it compares
//	every message you get against a (potentially long) list of 
//	death messages.  If you want to save a little processor power
//	at the expense of not getting any death tracking, set this to
//	false.
$PrestoPref::KillTrak = true;

// Scripting preference
// ---------------------------------------------------------------
//
// 	Are you a scripter?  Then you'll probably want to turn on noise
// 	because it may have some interesting information for you.
$PrestoPref::ScriptingNoise = false;

// Strict Name Checking preference
// ---------------------------------------------------------------
//
// 	Enable this only if you think people are purposely screwing 
// 	around with your scripts by using funny names.  It could slow
// 	things down, I'm not sure by how much (but not /that/ much).
$PrestoPref::StrictNameChecking = false;

// Pack Status Window preferences
// ---------------------------------------------------------------
//
// 	You can turn off the pack status window (the one on the
//	main menu.  Just set this to "false" instead of "true."
$PrestoPref::ShowPackStatus = true;
//
// 	If you have a favorite banner you can specify it here.  Maybe
//	it's your own and you're testing the banner ad, or maybe you 
//	just like someone's banner.
//	By default I leave this blank, so it will cycle through the
//	script banners.  But if you know the name of a script (the 
//	name they registered the banner with) you can enter it here.
$PrestoPref::FavoriteBanner = "";
//
//	The time (in seconds) that each script banner is displayed.
$PrestoPref::BannerCycleTime = 10;

