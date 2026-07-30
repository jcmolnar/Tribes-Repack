// Pop up an image on flag grab
// -|DF|- Link

//  DO NOT TOUCH!
IDBMP_FLAG_POPUP = 00160010, "Modules/FlagPopup/flag.png";


// Make changes here

$width = 1560; // width of image
$height = 67; // height of image

$popupTime = 5; // duration to show flag on screen (in seconds)
$fadeOutCount = 100; // fade out variable : DO NOT TOUCH!

$flag_img = "flag.png"; // popup image
$flag_status = "stand"; // status of flag (stand / grabbed / capped) -- helps control animation timer
$constant = true; // toggle to show flag indefinitely (true/false)

function FlagPopup::Init()
{
	if ($FlagPopup::Loaded)
		return;
	
	$FlagPopup::Loaded = true;
	
	HUD::New("FlagPopup::Container", 0, 0, $width, $height, FlagPopup::Wake, FlagPopup::Sleep);

	newObject("FlagPopup::BG", FearGui::FGBitmapCtrl, 0, 0, $width, $height);

	HUD::Add("FlagPopup::Container", "FlagPopup::BG");

	Control::SetValue("FlagPopup::BG", "<b0,0:Modules/FlagPopup/"~$flag_img~">");

	// Fade control
	%fade = Control::getId("FlagPopup::BG");
	%fade.extent = "1560 67";
	%fade.bmpTag = "IDBMP_FLAG_POPUP";
	%fade.transparent = "True";
	%fade.alpha = "1.0";
}

function FlagPopup::Wake() { Control::SetVisible("FlagPopup::BG", false); }
function FlagPopup::Sleep() { return; }

function FlagPopup::FadeOut()
{
	%fade = Control::getId("FlagPopup::BG");
	
	%fade.alpha = (0.01 * $fadeOutCount--);
	
	if($fadeOutCount == 0) {
		$fadeOutCount = 100;
		FlagPopup::Reset();
		Schedule::Cancel("FlagPopupFadeOut");
	} else {
		if($flag_status == "grabbed")
			Schedule::Add("FlagPopup::FadeOut();", 0.006, "FlagPopupFadeOut");
	}
}

function FlagPopup::Reset()
{
	// Fade control & image visibility reset
	if( Control::GetVisible("FlagPopup::BG") || Schedule::Check("FlagPopupFadeOut") )
	{
		$flag_status = "reset";
		$fadeOutCount = 100;

		Control::SetVisible("FlagPopup::BG", false);

		Schedule::Cancel("FlagPopupFadeOut");
		Schedule::Cancel("FlagPopupImage");

		%fade = Control::getId("FlagPopup::BG");
		%fade.alpha = "1.0";
	}
}

function FlagPopup::PickUp( %team, %cl )
{
	if (%cl != getManagerId())
		return;
	
	// Hide flag after popup time, or check if constant is true
	// Even if $popupTime has a number, it will be ignored
	$flag_status = "grabbed";
	
	Control::SetVisible("FlagPopup::BG", true); // Constantly show the flag image
	Schedule::Add("FlagPopup::FadeOut();", $popupTime);
	
	if (!$constant)
		Schedule::Add("Control::SetVisible(FlagPopup::BG, false);", $popupTime, "FlagPopupImage");
}

function FlagPopup::Drop( %team, %cl )
{
	if (%cl != getManagerId())
		return;
	
	if (%cl == getManagerId() && %team != Client::GetTeam(getManagerId()))
		FlagPopup::Reset();
}

function FlagPopup::Cap( %team, %cl )
{
	if (%cl != getManagerId())
		return;
	
	if (%cl == getManagerId() && %team != Client::GetTeam(getManagerId()))
		FlagPopup::Reset();
}

FlagPopup::Init();

Event::Attach( eventFlagDrop, FlagPopup::Drop );
Event::Attach( eventFlagGrab, FlagPopup::PickUp );
Event::Attach( eventFlagPickup, FlagPopup::PickUp );
Event::Attach( eventFlagCap, FlagPopup::Cap );
Event::Attach( eventChangeMission, FlagPopup::Reset );