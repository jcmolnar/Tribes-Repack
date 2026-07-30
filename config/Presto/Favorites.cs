// --	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	-----	------
// Favorites.CS								Presto, March '99 
//
//	This function is not done.
//
// ---------------------------------------------------------------------------

Include("presto\\Inventory.cs");

function Favorites::UpdateHud() {
	HUD::AddText(hudFavorites, "<f1>Current mod is <f2>" @ $Mod::current @ "<f1>\n");
	HUD::AddText(hudFavorites, Favorites::Description("wpnChaingun wpnLaserRifle packEnergy"));
	return 0;
	}
HUD::New(hudFavorites, Favorites::UpdateHud, getWord($PrestoPref::FavoritesPosition, 0),
							   getWord($PrestoPref::FavoritesPosition, 1)-50,
							   getWord($PrestoPref::FavoritesPosition, 2),
							   getWord($PrestoPref::FavoritesPosition, 3)+50);
HUD::SetGui(hudFavorites, CmdInventoryGui);	// displays on the inventory page.

function Favorites::Description(%invStr) {
	%str = "";
	%i = 0;
	%w = getWord(%invStr, %i);
	while (%w != -1) {
		%name = Inv::GetModName(%w);
		if (%name == "")
			%name = "<?>";
		if (%i > 0)
			%name = ", " @ %name;
		%str = %str @ %name;

		%i++;
		%w = getWord(%invStr, %i);
		}
	return %str;
	}
function Favorites::BuyList(%invStr) {
	%invstr = %invstr @ Mod::GetFreeItems();
	%str = "";
	%i = 0;
	%w = getWord(%invStr, %i);
	while (%w != -1) {
		%num = Inv::GetModNum(%w);
		if (%num != "")
			%str = %str @ "," @ %num;
		%i++;
		%w = getWord(%invStr, %i);
		}
	eval("remoteEval(2048,buyFavorites" @ %str @ ");");
	return %str;
	}
$fav1 = "armorLight packEnergy wpnDiscLauncher wpnLaserRifle wpnGrenadeLauncher";

function Favorites::Reset() {
	%obj = nameToId(FavoritesText);
	while (%obj != -1)
		{
		deleteObject(%obj);
		%obj = nameToId(FavoritesText);
		}
	$Favorites::initialized = false;
	}
Favorites::Reset();

function CmdInventoryGui::onOpen() {
	//OLD DYNAMIX CODE BEGINS
	if($curFavorites == -1) {
	      CmdInventoryGui::favoritesSel(1);
	      Control::performClick("FavButton1");
		}
	//OLD DYNAMIX CODE ENDS

	HUD::Display(hudFavorites, true);
	}


// ---------------------------------------------------------------------------