// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// HUD.CS									Presto, March '99 
//
//	A generic interface to create new Heads Up Displays.
//
//	These routines are a toolbox for HUD writers.  You should be able
//	to build custom HUDs easily.
//
//	I couldn't have written these routines without first seeing the
//	dyn_HUD example by KillerBunny.  Many thanks to him for opening our
//	eyes - before dyn_HUD, no one would have guessed it was possible
//	to change the GUI!
//
//	Usage examples:
//
//		HUD::New(hudPresto, UpdateFoo, 100,100, 200,200);
//			Creates a new HUD "hudPresto" at (100,100) with 
//			width (200,200).  The function UpdateFoo will be 
//			called when it needs to be refreshed.
//
//		HUD::New(hudPresto, UpdateFoo, "100 100 200 200");
//			Equivalent to the above
//
//		HUD::New(hudPresto, UpdateFoo, "100%-5 100%-5 50 50");
//			This is where it starts getting cool.  You can
//			use coordinate math to postion your HUDs very
//			specifically.  There are several base notations:
//			* <x>% when used in the width or height will set
//			  the coordinate to that % of the screen dimensions.
//				example: "0 0 75% 25%" makes a HUd in the upper
//				left hand corner which is 75% of the screen
//				width and 25% of the screen height.
//			* <x>% when used in the x or y position will place
//			  that much of the blank space to the left or top
//			  side of the HUD.
//				example: "25% 0 100 50" will create a 100 pixel
//				width HUD.  If the screen is 640 across, that
//				leaves 540 pixels of blank space.  25% of this
//				is on the left side of the HUD, or 135 pixels.
//				So the HUD will be 135 over from the left side.
//				Note that you can very easily left (0%) or 
//				right (100%) align a HUD as well as center it
//				(50%)...
//			* <x>+<y>, <x>-<y>, <x>*<y>, <x>/<y> will do the math.
//				example: an x of 100%-20 will place the 
//				HUD 20 pixels in from the right side of the
//				screen.
//				THERE IS NO GUARANTEED PRECEDENCE.  DON'T TRY
//				TO DO SOMETHING LIKE a*b+c OR EVEN a*d+b/c
//				AS I CANNOT BE RESPONSIBLE FOR WHAT HAPPENS ;)
//			* left(otherhud), right(otherhud), center(otherhud),
//			  top(otherhud), bottom(otherhud), height(otherhud),
//			  width(otherhud):
//			  These are values you can use to place one HUD
//			  relative to another.  Example:
//				"left(hudX) bottom(hudX) width(hudX)/2 20"
//				Here is a diagram of the results:
//					|----------------|
//					|                |  <- hudX
//					|                |
//					|----------------|
//					|        |  <- new hud
//					|--------|
//			Maybe you can see that this coordinate specification 
//			is powerful but a bit complicated to explain ;)
//
//	Now back to the usage examples:
//
//		HUD::NewFrame(hudPresto, "", 100,100, 200,200);
//			As above, but the new HUD >>has no text region<<
//			Use HUD::AddObject to attach controls to it.
//			This example also shows that you can leave the
//			Update function blank.
//			>> This is a powerful function but if you aren't
//			familiar with GUI objects it's dangerous <<
//
//		HUD::Display(hudPresto);
//			Displays the HUD.  HUDs are normally created
//			non-visible!
//		HUD::Display(hudPresto, false);
//			Turns off the display.
//		HUD::ToggleDisplay(hudPresto);
//			Toggles the display of a HUD on and off.
//		%displayed = HUD::GetDisplayed(hudPresto);
//			Queries to see if the HUD is displayed right now.
//
//		HUD::SetGui(hudPresto, CmdInventoryGui);
//			Attaches the HUD to a different GUI set, in this
//			case the one that's visible while you're buying
//			things at the inventory stations.  HUDs are created
//			on "playGui" by default - it's the normal screen.
//
//		HUD::Width(hudPresto)	HUD::Height(hudPresto)
//			Return the width and height, respectively, of the
//			HUD.  You might need those values when creating
//			objects.
//
//		HUD::AddObject(hudPresto, FearGui::ShapeView, 10,10, 40,40);
//			In this case, we're adding a new object of the Shape
//			Viewer class to hudPresto.  The coordinates are local,
//			in other words (0,0) is the top left of the HUD, not
//			the screen.
//			>> this call can really mess up your display if you
//			get it wrong!!<<
//			The function returns the object created so you can
//			do stuff with it.
//
//	Update function example:
//
//		function UpdateMyHUD(%hud)
//		{
//		HUD::AddTextLine(%hud, "my HUD");
//			Writes a line of text.
//		HUD::AddText(%hud,"Status:  ");
//			Writes a line of text >>without a newline<<
//		HUD::AddTextLine(%hud, "<f2>good");
//			Finishes the above line, using font #2.
//		return 5;
//			"Call me back in 5 seconds!"
//		}
//
//	This could then be installed as
//
//		HUD::New(hudPresto, UpdateMyHUD, 100,100, 200,200);
//
//	Note that if you provide an update function to HUD::New, you are
//	responsible for keeping the text updated!  If you don't write the
//	text each time, it will go blank.  So don't just check if nothing
//	changed and return without writing the text again.
//
//	If you don't return a value from the Update function, it won't 
//	call you back again until the HUD is closed & reopened.
//
// ---------------------------------------------------------------------------

Include("presto\\Event.cs");
Include("presto\\Match.cs");

function HUD::SetCoordValue(%hud, %dim, %value) {
	$HUD::[%hud, coord, %dim] = %value;
	}
function HUD::GetCoordValue(%hud, %dim) {
	return ($HUD::[%hud, coord, %dim] + 0);	// guarantees a number
	}

$HUD::defaultDim[0] = "left";
$HUD::defaultDim[1] = "top";
$HUD::defaultDim[2] = "width";
$HUD::defaultDim[3] = "height";
function HUD::GetCoord(%hud, %dim) {
	for (%i=0; %i<4; %i++) {
		if (%i == %dim || %dim == $HUD::defaultDim[%i])
			return getWord(HUD::GetPosition(%hud), %i);
		}

	return -1;
	}

function HUD::InterpCoord(%hud, %str, %dim) {
	%str = getWord(%str, %dim);
	return HUD::_InterpCoord(%hud, %str, %dim);
	}
function HUD::_InterpCoord(%hud, %str, %dim) {
	if (Match::ParamString(%str,"%a+%b")) {
		%a = Match::Result(a);
		%b = Match::Result(b);
		return HUD::_InterpCoord(%hud, %a, %dim) +
			 HUD::_InterpCoord(%hud, %b, %dim);
		}
	if (Match::ParamString(%str,"%a-%b")) {
		%a = Match::Result(a);
		%b = Match::Result(b);
		return HUD::_InterpCoord(%hud, %a, %dim) -
			 HUD::_InterpCoord(%hud, %b, %dim);
		}
	if (Match::ParamString(%str,"%a/%b")) {
		%a = Match::Result(a);
		%b = Match::Result(b);
		return HUD::_InterpCoord(%hud, %a, %dim) /
			 HUD::_InterpCoord(%hud, %b, %dim);
		}
	if (Match::ParamString(%str,"%a*%b")) {
		%a = Match::Result(a);
		%b = Match::Result(b);
		return HUD::_InterpCoord(%hud, %a, %dim) *
			 HUD::_InterpCoord(%hud, %b, %dim);
		}

	if (Match::ParamString(%str, "&p%", "&")) {
		%percent = Match::Result(p);
		%size = Presto::ScreenSize();

		if (%dim < 2)
			%full = getWord(%size,%dim) - HUD::GetCoordValue(%hud, %dim+2);
		else 	%full = getWord(%size,%dim-2);

		%n = floor((%full * %percent) / 100);
		return %n;
		}

	%coord = (%str + 0);
	if (%coord != 0)
		return %coord;

	if (Match::ParamString(%str,"%m(%h)")) {
		%mod = Match::Result(m);
		%hudRel = Match::Result(h);
		}
	else	{
		%mod = $HUD::defaultDim[%hud];
		%hudRel = %str;
		}
	if (!HUD::Exists(%hud))
		return 0;

	if (String::ICompare(%mod, "left") == 0)
		return HUD::GetCoordValue(%hudRel, 0);
	if (String::ICompare(%mod, "right") == 0)
		return HUD::GetCoordValue(%hudRel, 0) + HUD::GetCoordValue(%hudRel, 2); 
	if (String::ICompare(%mod, "top") == 0)
		return HUD::GetCoordValue(%hudRel, 1);
	if (String::ICompare(%mod, "bottom") == 0)
		return HUD::GetCoordValue(%hudRel, 1) + HUD::GetCoordValue(%hudRel, 3); 
	if (String::ICompare(%mod, "width") == 0)
		return HUD::GetCoordValue(%hudRel, 2); 
	if (String::ICompare(%mod, "height") == 0)
		return HUD::GetCoordValue(%hudRel, 3);
	if (String::ICompare(%mod, "center") == 0) {
		%dim2 = %dim - (floor(%dim / 2)*2);
		return HUD::GetCoordValue(%hudRel, %dim2) + floor(HUD::GetCoordValue(%hudRel, %dim2+2) / 2); 
		}

	return 0;
	}
function HUD::InterpCoords(%hud, %str) {
	// order of evaluation is important - wdith/height first
	HUD::SetCoordValue(%hud, 2, HUD::InterpCoord(%hud,%str,2));
	HUD::SetCoordValue(%hud, 3, HUD::InterpCoord(%hud,%str,3));

	HUD::SetCoordValue(%hud, 0, HUD::InterpCoord(%hud,%str,0));
	HUD::SetCoordValue(%hud, 1, HUD::InterpCoord(%hud,%str,1));

	return HUD::GetCoordValue(%hud,0) @" "@ HUD::GetCoordValue(%hud,1) @" "@
		 HUD::GetCoordValue(%hud,2) @" "@ HUD::GetCoordValue(%hud,3);
	}

function HUD::SetGuiObjectCount(%hud, %num) {
	$HUD::[%hud, count] = %num;
	}
function HUD::GetGuiObjectCount(%hud) {
	return $HUD::[%hud, count];
	}
function HUD::GetGui(%hud) {
	return $HUD::[%hud, gui];
	}
function HUD::SetGui(%hud, %gui) {
	%frame = HUD::GetGUIObject(%hud, frame);
	if (isObject(HUD::GetGui(%hud)))
		removeFromSet(HUD::GetGui(%hud), %frame);
	%isObj = isObject(%gui);
	if (%isObj)
		addToSet(%gui, %frame);
	else	$HUD::pendingAttach[%gui] = true;
	HUD::SetAttached(%hud, %isObj);
	$HUD::[%hud, gui] = %gui;
	}
function HUD::GetGuiObject(%hud, %tag) {
	return $HUD::[%hud, guiObject, %tag];
	}
function HUD::SetGUIObject(%hud, %tag, %obj) {
	$HUD::[%hud, guiObject, %tag] = %obj;
	}
function HUD::AddObject(%hud, %class, %x,%y, %width, %height) {
	%num = HUD::GetGuiObjectCount(%hud);
	%obj = newObject("HUD::"@ %hud @"::"@ %num, %class, %x,%y, %width,%height);
	if (%obj != 0) {
		%frame = HUD::GetGuiObject(%hud, frame);
		addToSet(%frame, %obj);
		HUD::SetGuiObject(%hud, %num, %obj);
		HUD::SetGuiObjectCount(%hud, %num + 1);
		}
	return %obj;
	}

function HUD::GetTextObject(%hud) {
	return $HUD::[%hud, textObject];
	}
function HUD::SetTextObject(%hud, %obj) {
	$HUD::[%hud, textObject] = %obj;
	}
function HUD::GetText(%hud) {
	return $HUD::[%hud, text];
	}
function HUD::SetText(%hud, %text) {
	$HUD::[%hud, text] = %text;
	}
function HUD::ClearText(%hud) {
	HUD::SetText(%hud, "");
	}
function HUD::NewText(%hud, %wrap, %xOffset, %yOffset) {
	if (%wrap)
		%width = HUD::Width(%hud);
	else	%width = 1000;	// Seems to wrap anyway because it's a subwindow :(
	%height = HUD::Height(%hud);
	HUD::SetTextObject(%hud, HUD::AddObject(%hud, FearGuiFormattedText, %xOffset+4,%yOffset-1, %width - 4, %height + 1 ));
	HUD::Update(%hud);
	}
function HUD::AddText(%hud, %text) {
	HUD::SetText(%hud, HUD::GetText(%hud) @ %text);
	}
function HUD::AddTextLine(%hud, %text) {
	HUD::AddText(%hud, %text @ "\n");
	}

function HUD::GetUpdateFunc(%hud) {
	return $HUD::[%hud, update];
	}
function HUD::SetUpdateFunc(%hud, %func) {
	$HUD::[%hud, update] = %func;
	}
function HUD::_Update(%hud, %time) {
	if (%time != $HUD::[%hud, schedule])
		return;
	HUD::Update(%hud);
	}
function HUD::Update(%hud) {
	$HUD::[%hud, schedule] = "";
	if (!HUD::GetDisplayed(%hud))
		return 0;
	%update = HUD::GetUpdateFunc(%hud);
	if (%update == "")
		return 0;
	%obj = HUD::GetTextObject(%hud);
	if (%obj != "")
		HUD::ClearText(%hud);
	%timer = eval(HUD::GetUpdateFunc(%hud) @ "(%hud);");
	if (%obj != "")
		{
		%objName = Object::GetName(%obj);
		Control::SetValue(%objName, HUD::GetText(%hud));
		}
	if (%timer > 0) {
		%time = "T"@getSimTime();
		$HUD::[%hud, schedule] = %time;
		schedule("HUD::_Update("@%hud@",\""@ %time @"\");", %timer);
		}
	}
function HUD::GetPosition(%hud) {
	return $HUD::[%hud, position];
	}
function HUD::SetPosition(%hud, %pos) {
	$HUD::[%hud, position] = %pos;
	}
function HUD::Width(%hud) {
	return HUD::GetCoordValue(%hud, 2);
	}
function HUD::Height(%hud) {
	return HUD::GetCoordValue(%hud, 3);
	}

function HUD::GetDisplayed(%hud) {
	%visible = Control::GetVisible(Object::GetName(HUD::GetGuiObject(%hud, frame)));
	return $HUD::[%hud, attached] && (%visible != false);
	}
function HUD::SetAttached(%hud, %show) {
	$HUD::[%hud, attached] = %show;
	}
function HUD::GetAttached(%hud) {
	return $HUD::[%hud, attached];
	}
function HUD::Display(%hud, %show) {
	%show = (%show != false);
	if (HUD::GetDisplayed(%hud) == %show)
		return;

	Control::SetVisible(Object::GetName(HUD::GetGuiObject(%hud, frame)), %show);
	if (%show)
		HUD::Update(%hud);
	}
function HUD::ToggleDisplay(%hud) {
	HUD::Display(%hud, !HUD::GetDisplayed(%hud));
	}

function HUD::NewFrame(%hud, %updateFunc, %x, %y, %width, %height) {
	if (%y == "")
		%pos = %x;
	else	%pos = %x @" "@ %y @" "@ %width @" "@ %height;

	if (HUD::Exists(%hud))
		HUD::Delete(%hud);
	HUD::SetPosition(%hud, %pos);
	HUD::SetUpdateFunc(%hud, %updateFunc);

	%pos = HUD::InterpCoords(%hud, %pos);
	%x = getWord(%pos, 0);
	%y = getWord(%pos, 1);
	%width = getWord(%pos, 2);
	%height = getWord(%pos, 3);
	if (%width == 0) {
		echo("zero width");
		return;
		}
	HUD::SetGuiObject(%hud,frame, newObject("HUD::"@%hud, FearGui::FearGuiMenu, %x, %y, %width, %height));

	Control::SetVisible("HUD::"@%hud, false);  // invisible by default
	HUD::SetAttached(%hud, false);
	HUD::SetGui(%hud, playGui);
	HUD::SetGuiObjectCount(%hud, 0);
	$HUD::[%hud, schedule] = "";

	// Reuse a slot when possible.
	%numHUDs = $HUD::numHUDs;
	if (%numHUDs == "")
		%numHUDs = 0;
	for (%i = 0; %i < %numHUDs; %i++)
		if ($HUD::name[%i] == %hud)
			return;
	$HUD::name[%numHUDs] = %hud;
	$HUD::numHUDs = %numHUDs + 1;
	}
function HUD::New(%hud, %updateFunc, %x, %y, %width, %height) {
	HUD::NewFrame(%hud, %updateFunc, %x, %y, %width, %height);
	HUD::NewText(%hud, false);
	}
// warning: don't call HUD::Delete yourself, because the HUD list will get out of sync.
function HUD::Delete(%hud) {
	$HUD::[%hud, schedule] = "";
	HUD::Display(%hud, false);
	%frame = HUD::GetGuiObject(%hud, frame);
	%num = Group::ObjectCount(%frame);
	for (%i = %num - 1; %i >= 0; %i--) {
		%obj = Group::GetObject(%frame, %i);
		if (%obj != -1)
			deleteObject(Group::GetObject(%frame, %i));
		}
	deleteObject(%frame);
	HUD::SetPosition(%hud, "");
	}
function HUD::Exists(%hud) {
	return HUD::GetPosition(%hud) != "";
	}
function HUD::Move(%hud, %x,%y,%width,%height) {
	
	if (%y == "")
		%pos = %x;
	else	%pos = %x @" "@ %y @" "@ %width @" "@ %height;

	HUD::SetPosition(%hud, %pos);
	%pos = HUD::InterpCoords(%hud, %pos);
	%x = getWord(%pos, 0);
	%y = getWord(%pos, 1);
	%width = getWord(%pos, 2);
	%height = getWord(%pos, 3);

	if (%width == 0) {
		echo("zero width");
		return;
		}

	%visible = Control::GetVisible(Object::GetName(HUD::GetGuiObject(%hud, frame)));
	%newFrame = newObject("newHUD", FearGui::FearGuiMenu, %x, %y, %width, %height);
	%count = HUD::GetGuiObjectCount(%hud);
	%oldFrame = HUD::GetGuiObject(%hud, frame);
	for (%i = 0; %i < %count; %i++) {
		%obj = HUD::GetGuiObject(%hud, %i);
		removeFromSet(%oldFrame, %obj);
		addToSet(%newFrame, %obj);
		}
	deleteObject(%oldFrame);
	Control::SetVisible(newHud, %visible);
	renameObject(%newFrame, "hud::"@%hud);
	HUD::SetGuiObject(%hud, frame, %newFrame);
	HUD::SetGui(%hud, HUD::GetGui(%hud));
 	}
function HUD::SetCoord(%hud, %dim, %coord) {
	for (%i=0; %i<4; %i++) {
		if (%i == %dim || %dim == $HUD::defaultDim[%i])
			%c[%i] = %coord;
		else	%c[%i] = HUD::GetCoord(%hud, %i);
		}
	HUD::Move(%hud, %c[0],%c[1],%c[2],%c[3]);
	}

function HUD::HideAll() {
	for (%i = 0; %i < $HUD::numHUDs; %i++)
		HUD::Display($HUD::name[%i], false);
	}
function HUD::UpdateAll() {
	for (%i = 0; %i < $HUD::numHUDs; %i++)
		HUD::Update($HUD::name[%i], false);
	}
function HUD::DeleteAll() {
	for (%i = 0; %i < $HUD::numHUDs; %i++)
		HUD::Delete($HUD::name[%i], false);
	%numHUDs = 0;
	}

function HUD::OnGuiOpen(%gui) {
	%res = Presto::ScreenSize();
	if ($HUD::lastRes[%gui] != %res) {
		if ($HUD::lastRes[%gui] != "")
			for (%i = 0; %i < $HUD::numHUDs; %i++) {
				%hud = $HUD::name[%i];
				HUD::SetCoord(%hud,none,0);	// cheap way to resize to same dim
				Event::Trigger(eventHUDResized, %hud);
				}
		$HUD::lastRes[%gui] = %res;
		}
	if ($HUD::pendingAttach[%gui]) {
		for (%i = 0; %i < $HUD::numHUDs; %i++) {
			%hud = $HUD::name[%i];
			%guiHUD = HUD::GetGui(%hud);
			if (%guiHUD == %gui && !HUD::GetAttached(%hud)) {
				addToSet(%gui, HUD::GetGuiObject(%hud, frame));
				HUD::SetAttached(%hud, true);
				HUD::Update(%hud);
				}
			}
		$HUD::pendingAttach[%gui] = "";
		}
	}

Event::Attach(eventGuiOpen, HUD::OnGuiOpen);

// When exiting we delete all HUDs to avoid them getting saved in PLAY.GUI
Event::Attach(eventExit, HUD::DeleteAll, hud);
// ---------------------------------------------------------------------------
