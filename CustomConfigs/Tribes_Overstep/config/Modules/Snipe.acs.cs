$Snipe::Weapon = "Laser Rifle";

function Snipe::On() before Zoom::In
{
	if(GetItemCount($Snipe::Weapon) != 0) {
		$Snipe::On = "TRUE";
		$Snipe::PrevWep = getMountedItem(0);
		use($Snipe::Weapon);

	}
}

function Snipe::Off() after Zoom::Out
{
	if($Snipe::On == "TRUE") {
		$Snipe::On = "";
		useItem ($Snipe::PrevWep);

	}
}
