//This HUD requires PrestoPack!
//PrestoPack is cool, get it.

EditActionMap("playMap.sae");
bindCommand(keyboard0, make, "2", TO, "toggleRPGhud();");

$rpghudcbdelay = 2.0;

function round(%n)
{
	if(%n < 0)
	{
		%t = -1;
		%n = -%n;
	}	
	else if(%n >= 0)
		%t = 1;

	%f = floor(%n);
	%a = %n - %f;
	if(%a < 0.5)
		%b = 0;
	else if(%a >= 0.5)
		%b = 1;

	return (%f + %b) * %t;
}

function FixDecimals(%c)
{
	%d = round(%c * 10);
	%m = (%d / 10) * 1.000001;

	return %m;
}

function getrpgdata()
{
	rpgfetchdata("MANA");
	rpgfetchdata("MaxMANA");
	rpgfetchdata("HP");
	rpgfetchdata("MaxHP");
	rpgfetchdata("EXP");
	rpgfetchdata("COINS");
	rpgfetchdata("MaxWeight");
	rpgfetchdata("BANK");
	rpgfetchdata("LVL");
	rpgfetchdata("LCK");
	rpgfetchdata("ATK");
	rpgfetchdata("DEF");
	rpgfetchdata("MDEF");
	rpgfetchdata("CLASS");
	rpgfetchdata("RACE");
        rpgfetchdata("zonedesc");
        rpgfetchdata("RemortStep");

	$mana = $rpgdata["MANA"];
	$maxmana = $rpgdata["MaxMANA"];
	$hp =  $rpgdata["HP"];
	$maxhp = $rpgdata["MaxHP"];
	$exp = $rpgdata["EXP"];
	$coins = $rpgdata["COINS"];
	$maxweight = $rpgdata["MaxWeight"];
	$bank = $rpgdata["BANK"];
	$lvl = $rpgdata["LVL"];
	$lck = $rpgdata["LCK"];
	$atk = $rpgdata["ATK"];
	$def = $rpgdata["DEF"];
	$mdef = $rpgdata["MDEF"];
	$class = $rpgdata["CLASS"];
	$race = $rpgdata["RACE"];
        $zonedesc = $rpgdata["zonedesc"];
        $remort = $rpgdata["RemortStep"];

        if($zonedesc == -1)
                $zonedesc = "unknown";

	$expneeded = ((floor($exp / 1000) + 1) * 1000) - $exp;

	if($rfdcnt++ >= (6 / $rpghudcbdelay))
	{
		rpgfetchdata("Weight");
		$weight = FixDecimals($rpgdata["Weight"]);
		$rfdcnt = 0;
	}
}

function UpdateRPGHUD(%hud)
{
	getrpgdata();

        HUD::AddTextLine(%hud, "<f0>Level <f2>" @ $lvl @ "<f0> / <f2>" @ $remort);
        HUD::AddTextLine(%hud, "<f1>" @ $race @ " " @ $class);
        HUD::AddText(%hud, "<f0>Zone: ");
        HUD::AddTextLine(%hud, "<f2>" @ $zonedesc @ "\n");
	HUD::AddText(%hud, "<f0>HP: ");
	HUD::AddTextLine(%hud, "<f2>" @ $hp @ "<f0> / <f2>" @ $maxhp);
	HUD::AddText(%hud, "<f0>MANA: ");
	HUD::AddTextLine(%hud, "<f2>" @ $mana @ "<f0> / <f2>" @ $maxmana);
	HUD::AddText(%hud, "<f0>EXP: ");
	HUD::AddTextLine(%hud, "<f2>" @ $exp @ "<f0> / <f2>" @ $expneeded);
	HUD::AddText(%hud, "<f0>Weight: ");
	HUD::AddTextLine(%hud, "<f2>" @ $weight @ "<f0> / <f2>" @ $maxweight);
	HUD::AddText(%hud, "<f0>Coins: ");
	HUD::AddTextLine(%hud, "<f2>" @ $coins);
	HUD::AddText(%hud, "<f0>Bank: ");
	HUD::AddTextLine(%hud, "<f2>" @ $bank @ "\n");
	HUD::AddText(%hud, "<f0>ATK: ");
	HUD::AddTextLine(%hud, "<f2>" @ $atk);
	HUD::AddText(%hud, "<f0>DEF: ");
	HUD::AddTextLine(%hud, "<f2>" @ $def);
	HUD::AddText(%hud, "<f0>MDEF: ");
	HUD::AddTextLine(%hud, "<f2>" @ $mdef @ "\n");
	HUD::AddText(%hud, "<f0>LCK: ");
	HUD::AddTextLine(%hud, "<f2>" @ $lck);

	return $rpghudcbdelay;
}

function toggleRPGhud()
{
	HUD::ToggleDisplay(RPGHUD);
}

HUD::New(RPGHUD, UpdateRPGHUD, "99%", "99%", 125, 264);
HUD::Display(RPGHUD);
