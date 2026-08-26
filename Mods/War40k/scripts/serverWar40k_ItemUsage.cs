$canpilot = 1;
$canride = 2;

function populateDamagescale(%armor, %sdflash, %sdener, %sdplas, %sdchem, %sdacid, %land, %impa, %crus, %bull, %plas, %ener, %expl, %miss, %shra, %debr, %lase, %mort, %blas, %elec, %mine, %snip, %flas, %melt, %deat, %flam, %ddam, %shur, %reap, %shel, %psid, %chem, %krak, %acid, %webd, %maxw, %inte, %scou, %lapc, %hapc, %tempe)
{
//-=-=-=-=-=-=
//DAMAGE TYPES
//-=-=-=-=-=-=
//EMP
	$specdam[%armor, $FlashDamageType] = %sdflash;
//POISON
	$specdam[%armor, $EnergyDamageType] = %sdener;
//FLAMER
	$specdam[%armor, $PlasmaDamageType] = %sdplas;
//BIOTOXIN
	$specdam[%armor, $ChemDamageType] = %sdchem;
//HELLFIRE
	$specdam[%armor, $AcidDamageType] = %sdacid;
//-=-=-=-=-=-=-=
//ARMOR DATA
//-=-=-=-=-=-=-=
	$damagescale[%armor, $landingdamagetype] = %land;
	$damagescale[%armor, $impactdamagetype] = %impa;
	$damagescale[%armor, $crushdamagetype] = %crus;
	$damagescale[%armor, $bulletdamagetype] = %bull;
	$damagescale[%armor, $plasmadamagetype] = %plas;
	$damagescale[%armor, $energydamagetype] = %ener;
	$damagescale[%armor, $explosiondamagetype] = %expl;
	$damagescale[%armor, $missiledamagetype] = %miss;
	$damagescale[%armor, $shrapneldamagetype] = %shra;
	$damagescale[%armor, $debrisdamagetype] = %debr;
	$damagescale[%armor, $laserdamagetype] = %lase;
	$damagescale[%armor, $mortardamagetype] = %mort;
	$damagescale[%armor, $blasterdamagetype] = %blas;
	$damagescale[%armor, $electricitydamagetype] = %elec;
	$damagescale[%armor, $minedamagetype] = %mine;
	$damagescale[%armor, $sniperdamagetype] = %snip;
	$damagescale[%armor, $flashdamagetype] = %flas;
	$damagescale[%armor, $meltadamagetype] = %melt;
	$damagescale[%armor, $deathdamagetype] = %deat;
	$damagescale[%armor, $flamerdamagetype] = %flam;
	$damagescale[%armor, $ddamagetype] = %ddam;
	$damagescale[%armor, $shurikendamagetype] = %shur;
	$damagescale[%armor, $reaperdamagetype] = %reap;
	$damagescale[%armor, $shelldamagetype] = %shel;
	$damagescale[%armor, $psidamagetype] = %psid;
	$damagescale[%armor, $chemdamagetype] = %chem;
	$damagescale[%armor, $krakendamagetype] = %krak;
	$damagescale[%armor, $aciddamagetype] = %acid;
	$damagescale[%armor, $webdamagetype] = %webd;
	$maxweapons[%armor] = %maxw;
	$vehicleuse[%armor, interceptor] = %inte;
	$vehicleuse[%armor, scout] = %scou;
	$vehicleuse[%armor, lapc] = %lapc;
	$vehicleuse[%armor, hapc] = %hapc;
	$vehicleuse[%armor, tempest] = %tempe;
}


//---------------------------------- sdfla, sdene, sdpla, sdche, sdaci, land, impa, crus, bull, plas, ener, expl, miss, shra, debr, lase, mort, blas, elec, mine, snip, flas, melt, deat, flam, ddam, shur, reap, shel, psid, chem, krak, acid, webd, maxw,                 inte,                 scou,                 lapc,                 hapc,                tempe
//------------Damage Types --------- ------ ------ ------ ------ ------ ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- --------------------- --------------------- --------------------- --------------------- --------------------
populateDamagescale(armormapoth,      TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.6,  0.7,  0.7,  0.6,  0.6,  0.6,  0.6,  1.0,  1.0,  0.7,  1.0,  0.7,  1.0,  0.8,  1.0,  1.0,  0.6,  1.0,  0.5,  1.0,  0.8,  1.0,  1.0,  0.4,  1.0,  1.0,    3,             $canride,             $canride,             $canride,             $canride,             $canride); //Male Apothecary
populateDamagescale(armorfapoth,      TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.6,  0.7,  0.7,  0.6,  0.6,  0.6,  0.6,  1.0,  1.0,  0.7,  1.0,  0.7,  1.0,  0.8,  1.0,  1.0,  0.6,  1.0,  0.5,  1.0,  0.8,  1.0,  1.0,  0.4,  1.0,  1.0,    3,             $canride,             $canride,             $canride,             $canride,             $canride); //Female Apothecracy
populateDamagescale(armormassault,    TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.5,  0.7,  1.0,  0.6,  0.6,  0.6,  0.6,  1.0,  1.3,  0.7,  1.0,  0.7,  1.0,  1.0,  1.0,  1.0,  0.6,  1.0,  0.5,  1.0,  0.8,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride,             $canride,             $canride,                    0); //Male Assault Marine
populateDamagescale(armorfassault,    TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.5,  0.7,  1.0,  0.6,  0.6,  0.6,  0.6,  1.0,  1.3,  0.7,  1.0,  0.7,  1.0,  1.0,  1.0,  1.0,  0.6,  1.0,  0.5,  1.0,  0.8,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride,             $canride,             $canride,                    0); //Female Assault Marine
populateDamagescale(armorbonesinger,  TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.6,  0.4,  1.0,  0.5,  0.8,  1.0,  1.0,  0.4,  0.8,  0.7,  0.1,  1.0,  1.0,  1.0,  0.5,  1.0,  1.0,  1.0,  0.5,  0.5,  1.0,  1.0,  1.0,  0.3,  1.5,  1.0,    3,             $canride,             $canride,             $canride,                    0, $canpilot | $canride); //Bone Singer
populateDamagescale(armormdreaper,    TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  1.0,  0.9,  0.5,  1.0,  0.5,  0.5,  0.9,  0.9,  1.0,  0.8,  0.8,  1.0,  0.8,  1.0,  1.0,  1.0,  0.9,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride,             $canride,                    0,             $canride); //Male Dark Reaper
populateDamagescale(armorfdreaper,    TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  1.0,  0.9,  0.5,  1.0,  0.5,  0.5,  0.9,  0.9,  1.0,  0.8,  0.8,  1.0,  0.8,  1.0,  1.0,  1.0,  0.9,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride,             $canride,                    0,             $canride); //Female Dark Reaper
populateDamagescale(armormdevastator, TRUE,  TRUE, FALSE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.5,  0.7,  1.0,  0.6,  0.6,  0.6,  0.6,  1.0,  1.0,  0.7,  1.0,  0.7,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.8,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride,             $canride,             $canride,                    0); //Male Devastator Marine
populateDamagescale(armorfdevastator, TRUE,  TRUE, FALSE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.5,  0.7,  1.0,  0.6,  0.6,  0.6,  0.6,  1.0,  1.0,  0.7,  1.0,  0.7,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  1.0,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride,             $canride,             $canride,                    0); //Female Devastator Marine
populateDamagescale(armormdiavg,      TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  1.0,  0.7,  0.7,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.7,  0.5,  1.0,  1.0,  0.4,  1.0,  0.5,  0.8,  0.6,  0.4,  1.0,  0.6,  1.0,  1.0,  0.4,  0.8,  1.0,    3,             $canride,             $canride,             $canride, $canpilot | $canride, $canpilot | $canride); //Male Dire Avenger
populateDamagescale(armorfdiavg,      TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  1.0,  0.7,  0.7,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.7,  0.5,  1.0,  1.0,  0.4,  1.0,  0.5,  0.8,  0.6,  0.4,  1.0,  0.6,  1.0,  1.0,  0.4,  0.8,  1.0,    3,             $canride,             $canride,             $canride, $canpilot | $canride, $canpilot | $canride); //Female Dire Avenger
populateDamagescale(armormerrant,     TRUE, FALSE, FALSE, FALSE,  TRUE,  1.0,  1.0,  0.5,  0.4,  0.5,  1.0,  0.3,  0.4,  0.4,  0.3,  1.0,  1.0,  0.0,  1.5,  0.5,  1.0,  1.0,  1.0,  1.0,  0.3,  0.5,  0.5,  1.0,  0.3,  0.5,  1.0,  1.0,  1.0,  1.0,    3,             $canride,             $canride,             $canride,             $canride,             $canride); //Male Errant
populateDamagescale(armorferrant,     TRUE, FALSE, FALSE, FALSE,  TRUE,  1.0,  1.0,  0.5,  0.4,  0.5,  1.0,  0.3,  0.4,  0.4,  0.3,  1.0,  1.0,  0.0,  1.5,  0.5,  1.0,  1.0,  1.0,  1.0,  0.3,  0.5,  0.5,  1.0,  0.3,  0.5,  1.0,  1.0,  1.0,  1.0,    3,             $canride,             $canride,             $canride,             $canride,             $canride); //Female Errant
populateDamagescale(armormeversor,    TRUE, FALSE,  TRUE, FALSE,  TRUE,  0.3,  0.3,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0, -1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride, $canpilot | $canride,             $canride,                    0); //Male Eversor
populateDamagescale(armorfeversor,    TRUE, FALSE,  TRUE, FALSE,  TRUE,  0.3,  0.3,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0, -1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride, $canpilot | $canride,             $canride,                    0); //Female Eversor
populateDamagescale(armormfidrgn,     TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  1.0,  0.4,  0.7,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.7,  1.0,  1.0,  0.8,  0.8,  0.4,  1.0,  0.4,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.4,  1.0,  1.0,    3,             $canride,             $canride,             $canride, $canpilot | $canride, $canpilot | $canride); //Male Fire Dragon
populateDamagescale(armorffidrgn,     TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  1.0,  0.4,  0.7,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.7,  1.0,  1.0,  0.8,  0.8,  0.4,  1.0,  0.4,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.4,  1.0,  1.0,    3,             $canride,             $canride,             $canride, $canpilot | $canride, $canpilot | $canride); //Female Fire Dragon
populateDamagescale(armormguardian,   TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  0.5,  0.5,  0.5,  0.9,  1.0,  1.0,  1.0,  1.0,  0.7,  0.7,  1.0,  1.3,  0.8,  1.0,  0.8,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  0.5,  1.0,  1.0,  1.0,  0.3,  0.5,  1.0,    3, $canpilot | $canride, $canpilot | $canride,             $canride, $canpilot | $canride, $canpilot | $canride); //Male Guardian
populateDamagescale(armorfguardian,   TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  0.5,  0.5,  0.5,  0.9,  1.0,  1.0,  1.0,  1.0,  0.7,  0.7,  1.0,  1.3,  0.8,  1.0,  0.8,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  0.5,  1.0,  1.0,  1.0,  0.3,  0.5,  1.0,    3, $canpilot | $canride, $canpilot | $canride,             $canride, $canpilot | $canride, $canpilot | $canride); //Female Guardian
populateDamagescale(armormlib,       FALSE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.5,  0.7,  1.0,  0.6,  0.6,  0.6,  0.6,  1.0,  1.0,  0.7,  1.0,  0.7,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  1.0,  0.7,  1.0,  0.3,  0.5,  1.0,    4,             $canride,             $canride,             $canride,             $canride,             $canride); //Male Warp Spider
populateDamagescale(armorflib,       FALSE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.5,  0.7,  1.0,  0.6,  0.6,  0.6,  0.6,  1.0,  1.0,  0.7,  1.0,  0.7,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  1.0,  0.7,  1.0,  0.3,  0.5,  1.0,    4,             $canride,             $canride,             $canride,             $canride,             $canride); //Female Warp Spider
populateDamagescale(armormranger,     TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  1.0,  1.0,  0.7,  0.7,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.7,  1.0,  1.0,  0.8,  0.8,  1.0,  1.0,  0.6,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.4,  1.0,  1.0,    3, $canpilot | $canride, $canpilot | $canride,             $canride, $canpilot | $canride, $canpilot | $canride); //Male Ranger
populateDamagescale(armorfranger,     TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  1.0,  1.0,  0.7,  0.7,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.7,  1.0,  1.0,  0.8,  0.8,  1.0,  1.0,  0.6,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.4,  1.0,  1.0,    3, $canpilot | $canride, $canpilot | $canride,             $canride, $canpilot | $canride, $canpilot | $canride); //Female Ranger
populateDamagescale(armormscout,      TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.8,  1.0,  1.3,  1.0,  1.0,  1.2,  1.2,  1.2,  1.3,  1.3,  1.0,  1.2,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride, $canpilot | $canride,             $canride,                    0); //Male Scout
populateDamagescale(armorfscout,      TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.8,  1.0,  1.3,  1.0,  1.0,  1.2,  1.2,  1.0,  1.3,  1.3,  1.0,  1.2,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride, $canpilot | $canride,             $canride,                    0); //Female Scout
populateDamagescale(armorsdaemon,     TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  0.5,  0.5,  1.0,  1.0,  0.6,  1.0,  1.0,  0.8,  0.9,  0.1,  1.0,  1.0,  0.5,  0.5,  2.0,  0.7,  1.0,  0.5,  0.6,  0.5,  0.8,  0.8,  1.5,  1.0,  0.5,  1.2,  1.5,  1.0,    3,             $canride,             $canride,             $canride,             $canride,             $canride); //Storm Demonew aspect)
populateDamagescale(armormstrscorp,   TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  0.5,  0.5,  0.5,  1.0,  0.7,  0.7,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.7,  1.0,  1.0,  0.8,  0.8,  1.0,  1.0,  0.6,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.4,  1.0,  1.0,    3,             $canride,             $canride,             $canride,             $canride,             $canride); //Male Striking Scorpion
populateDamagescale(armorfstrscorp,   TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  0.5,  0.5,  0.5,  1.0,  0.7,  0.7,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.7,  1.0,  1.0,  0.8,  0.8,  1.0,  1.0,  0.6,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.4,  1.0,  1.0,    3,             $canride,             $canride,             $canride,             $canride,             $canride); //Female Striking Scorpion
populateDamagescale(armormswhawk,     TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  0.5,  1.0,  1.0,  1.3,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.2,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride,             $canride,             $canride,             $canride); //Male Swooping Hawk
populateDamagescale(armorfswhawk,     TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  0.5,  1.0,  1.0,  1.3,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.2,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride,             $canride,             $canride,             $canride); //Female Swooping Hawk
populateDamagescale(armormtactical,   TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.5,  0.7,  1.0,  0.6,  0.6,  0.6,  0.6,  1.0,  1.0,  0.7,  1.0,  0.7,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.8,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride, $canpilot | $canride,             $canride,                    0); //Male Tactical Marine
populateDamagescale(armorftactical,   TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.5,  0.7,  1.0,  0.6,  0.6,  0.6,  0.6,  1.0,  1.0,  0.7,  1.0,  0.7,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.8,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride, $canpilot | $canride,             $canride,                    0); //Female Tactical Marine
populateDamagescale(armormtech,       TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.5,  0.7,  1.0,  0.6,  0.6,  0.6,  0.6,  1.0,  1.0,  0.7,  1.0,  0.7,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.8,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride, $canpilot | $canride,             $canride,                    0); //Male Tech
populateDamagescale(armorftech,       TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  0.5,  0.7,  1.0,  0.6,  0.6,  0.6,  0.6,  1.0,  1.0,  0.7,  1.0,  0.7,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.8,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride, $canpilot | $canride,             $canride,                    0); //Female Tech
populateDamagescale(armorterm,        TRUE,  TRUE,  TRUE,  TRUE, FALSE,  1.0,  1.0,  0.5,  0.2,  0.7,  1.0,  0.6,  0.6,  0.4,  0.4,  1.0,  0.5,  1.3,  1.0,  0.4,  0.5,  1.0,  1.0,  1.0,  0.1,  0.6,  1.0,  1.5,  0.5,  1.0,  1.0,  0.8,  1.5,  1.0,    3,             $canride,             $canride,             $canride,             $canride,                    0); //Terminator Marine
populateDamagescale(armormwarlock,   FALSE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.4,  1.0,  1.0,  0.8,  0.8,  1.0,  1.0,  0.6,  1.0,  1.0,  1.0,  1.0,  0.7,  1.0,  0.4,  1.0,  1.0,    4,             $canride,             $canride,             $canride,             $canride,             $canride); //Male Warlock
populateDamagescale(armorfwarlock,   FALSE,  TRUE,  TRUE,  TRUE,  TRUE,  1.0,  1.0,  0.5,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5,  1.0,  0.4,  1.0,  1.0,  0.8,  0.8,  1.0,  1.0,  0.6,  1.0,  1.0,  1.0,  1.0,  0.7,  1.0,  0.4,  1.0,  1.0,    4,             $canride,             $canride,             $canride,             $canride,             $canride); //Female Warlock
populateDamagescale(armormwarpspider, TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  0.5,  0.5,  0.5,  0.8,  0.7,  0.7,  0.8,  0.8,  1.0,  1.0,  1.2,  1.0,  0.4,  2.0,  1.0,  0.8,  0.8,  1.0,  1.0,  0.6,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.4,  1.0,  0.0,    3, $canpilot | $canride, $canpilot | $canride,             $canride, $canpilot | $canride, $canpilot | $canride); //Male Warpspider
populateDamagescale(armorfwarpspider, TRUE,  TRUE,  TRUE,  TRUE,  TRUE,  0.5,  0.5,  0.5,  0.8,  0.7,  0.7,  0.8,  0.8,  1.0,  1.0,  1.2,  1.0,  0.4,  2.0,  1.0,  0.8,  0.8,  1.0,  1.0,  0.6,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.4,  1.0,  0.0,    3, $canpilot | $canride, $canpilot | $canride,             $canride, $canpilot | $canride, $canpilot | $canride); //Female Warpspider
populateDamagescale(armormwraith,     TRUE, FALSE, FALSE, FALSE, FALSE,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  2.0,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  1.0,  1.0,  0.3,  0.5,  1.0,    3,             $canride,             $canride,             $canride,             $canride,             $canride); //Male Wraithguard
populateDamagescale(armorfwraith,     TRUE, FALSE, FALSE, FALSE, FALSE,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  2.0,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  0.5,  1.0,  0.0,  1.0,  0.5,  1.0,    3,             $canride,             $canride,             $canride,             $canride,             $canride); //Female Wraithguard



function populateitemmax(%item, %mdm, %fdm, %msc, %fsc, %msp, %fsp, %msn, %fsn, %mme, %fme, %men, %fen, %mbu, %fbu, %mal, %fal, %cy, %mco, %fco, %de, %mlo, %flo, %msr, %fsr, %c1c, %mfd,%ffd, %mst, %fst, %mda,%fda, %mes,%fes, %mah,%fah, %mwk,%fwk, %mws,%fws, %mfs,%ffs)
{
  $itemmax[armormeversor, %item] = %mdm;
  $itemmax[armorfeversor, %item] = %fdm;
  $itemmax[armormscout, %item] = %msc;
  $itemmax[armorfscout, %item] = %fsc;
  $itemmax[armormAssault, %item] = %msp;
  $itemmax[armorfAssault, %item] = %fsp;
  $itemmax[armormguardian, %item] = %msn;
  $itemmax[armorfguardian, %item] = %fsn;
  $itemmax[armormtactical, %item] = %mme;
  $itemmax[armorftactical, %item] = %fme;
  $itemmax[armormtech, %item] = %men;
  $itemmax[armorftech, %item] = %fen;
  $itemmax[armormdevastator, %item] = %mbu;
  $itemmax[armorfdevastator, %item] = %fbu;
  $itemmax[armormwraith, %item] = %mal;
  $itemmax[armorfwraith, %item] = %fal;
  $itemmax[armorterm, %item] = %cy;
  $itemmax[armormdreaper, %item] = %mco;
  $itemmax[armorfdreaper, %item] = %fco;
  $itemmax[armorbonesinger, %item] = %de;
  $itemmax[armormlib, %item] = %mlo;
  $itemmax[armorflib, %item] = %flo;
  $itemmax[armormswhawk, %item] = %msr;
  $itemmax[armorfswhawk, %item] = %fsr;
  $itemmax[armorsdaemon, %item] = %c1c;
  $itemmax[armormfidrgn, %item] = %mfd;
  $itemmax[armorffidrgn, %item] = %ffd;
  $itemmax[armormstrscorp, %item] = %mst;
  $itemmax[armorfstrscorp, %item] = %mst;
  $itemmax[armormdiavg, %item] = %mda;
  $itemmax[armorfdiavg, %item] = %fda;
  $itemmax[armormranger, %item] = %mes;
  $itemmax[armorfranger, %item] = %fes;
  $itemmax[armormapoth, %item] = %mah;
  $itemmax[armorfapoth, %item] = %fah;
  $itemmax[armormwarlock, %item] = %mwk;
  $itemmax[armorfwarlock, %item] = %fwk;
  $itemmax[armormwarpspider, %item] = %mws;
  $itemmax[armorfwarpspider, %item] = %fws;
  $itemmax[armormerrant, %item] = %mfs;
  $itemmax[armorferrant, %item] = %ffs;
}

//weapons
//                                mdm, fdm, msc, fsc, msp, fsp, msn, fsn, mme, fme, men, fen, mbu, fbu, mal, fal,  cy, mco, fco,  de, mlo, flo, msr, fsr, c1c, mfd, ffd mst, fst, mda fda,mes, fes, mah, fah, mwk,fwk, mws,fws, mfs,ffs
//                                ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---   --  ---  ---  ---  ---  ---  ---  --- ---  ---  --- --- ---  ---  ---  ---  --- ---  --- --- --- ---
populateitemmax(plaspist,          1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   1,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,  1,   1,   1,  1,   0,   0,   0,   0,   1,  1,   0,  0,   0,  0,   0,  0);
populateitemmax(bolt,              0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(lasblaster,        0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(energyrifle,       0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(fixit,             0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(flamer,            0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   0,  1,   1,   0,  0,   1,   1,   0,   0,   0,  0,   0,  0,   0,  0,   1,  1);
populateitemmax(grenadelauncher,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   1,   1,   1,  1,   1,   1,  1,   1,   1,   1,   1,   1,  1,   0,  0,   0,  0,   1,  1);
populateitemmax(grenadeammo,      10,  10,  10,  10,  10,  10,  10,  10,  20,  20,  20,  20,  30,  30,  20,  20,  35,  20,  20,  30,   0,   0,  20,  20,  30, 20,  20,  15, 15,  20,  20,  12,  12,  20, 20,   0,  0,   0,  0,  25, 25);
populateitemmax(fusiongun,         0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  1,   1,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(melt,              0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   1,  1,   0,  0,   0,  0,   0,  0);
populateitemmax(plasmagun,         0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   1,   1,   1,  1,   1,   0,  0,   1,   1,   0,   0,   1,  1,   0,  0,   0,  0,   1,  1);
populateitemmax(plasmaammo,       40,  40,  30,  30,   0,   0,  30,  30,  40,  40,  40,  40,  50,  50,  40,  40,  70,  20,  20,  30,   0,   0,  20,  20,  50, 50,  50,   0,  0,  20,  20,   0,   0,  20, 20,   0,  0,  30, 30,  30, 30);
populateitemmax(hvybolter,         0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(shurcannon,        0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,  0,   0,   0,  0,   1,   1,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(hvybolterammo,     0,   0,   0,   0,   0,   0,   0,   0, 300, 300,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 200,  0,   0,   0,  0, 100, 100,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(rocketlauncher,    0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   1,  1);
populateitemmax(erocketlauncher,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   0,   0,   0,   0,   1,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(rocketammo,        0,   0,   0,   0,   0,   0,   6,   6,  12,  12,   0,   0,  12,  12,   0,   0,   0,  10,  10,   5,   0,   0,   0,   0,  20,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   6,  6);
populateitemmax(bolter,            0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   1,  1,   0,  0,   0,  0,   1,  1);
populateitemmax(shurcata,          0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   1,   1,   0,  0,   0,   0,  0,   1,   1,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(bolterammo,        0,   0,   0,   0,   0,   0, 200, 200, 200, 200,   0,   0, 250, 250,   0,   0,   0,   0,   0, 200,   0,   0,  70,  70,   0,  0,   0,   0,  0, 350, 350,   0,   0, 200,200,   0,  0,   0,  0, 120, 120);
populateitemmax(sniperrifle,       0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(longrifle,         0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   1,   1,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(sniperammo,        0,   0,  25,  25,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,  30,  30,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(targetinglaser,    1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,  1,   1,   1,  1,   1,   1,   1,   1,   1,  1,   1,  1,   1,  1,   1,  1);
populateitemmax(tranqgun,          0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,  0,   0,   0,  0,   0,   0,   1,   1,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(tranqammo,         0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   6,   6,   0,  0,   0,   0,  0,   0,   0,  12,  12,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(stbolter,          0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(stbolterammo,      0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 200,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(plascan,           0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   1,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(plascanammo,       0,   0,   0,   0,   0,   0,   0,   0,  12,  12,   0,   0,  15,  15,   0,   0,   0,   0,   0,   5,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(shotgun,           1,   1,   1,   1,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   1,  1,   0,  0,   0,  0,   1,  1);
populateitemmax(shotgunammo,      20,  20,  20,  20,  20,  20,   0,   0,  20,  20,  20,  20,  20,  20,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,  20, 20,   0,  0,   0,  0,  20, 20);
populateitemmax(lascannon,         0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(hflamer,           0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(autogun,           0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(autoammo,          0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 400,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(demogun,           0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(demogunammo,       0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  80,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(warp,              0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   1,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(tractordevice,     0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   1,  1);

//                                mdm, fdm, msc, fsc, msp, fsp, msn, fsn, mme, fme, men, fen, mbu, fbu, mal, fal,  cy, mco, fco,  de, mlo, flo, msr, fsr, c1c, fid,      sts,     dvg,      els,      aph,     wlk,     was,     fas
//                                ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---   --  ---  ---  ---  ---  ---  ---  ---  --
populateitemmax(melta,             0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(meltaammo,         0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 200,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(emp,               0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   0,   1,   1,   1,   0,   0,   0,   0,   1,  0,   0,   0,  0,   1,   1,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(empammo,           0,   0,   0,   0,   0,   0,  10,  10,   0,   0,   0,   0,   0,   0,  15,  15,   0,  10,  10,  10,   0,   0,   0,   0,  20,  0,   0,   0,  0,  10,  10,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(grav,              0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(cyclone,           0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(reaper,            0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(cycloneammo,       0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  60,  80,  80,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(evbolter,          1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(evbolterammo,    250, 250,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(poison,            1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(poisonammo,       30,  30,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(scatterlas,        0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   1,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(firepike,          0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  1,   1,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(brightlance,       0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   1,  0,   0,   0,  0,   1,   1,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(vibrocannon,       0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(vibrocannonammo,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   0,  0,   0,  0);
populateitemmax(sword,             1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,  1,   1,   1,  1,   1,   1,   1,   1,   1,  1,   1,  1,   1,  1,   1,  1);
populateitemmax(webgun,            0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0,   1,  1,   0,  0);
populateitemmax(webammo,           0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  50,  50,  50,   0,   0,   0,   0,  50,  0,   0,   0,  0,   0,   0,   0,   0,   0,  0,   0,  0, 200, 200,   0,  0);
PopulateItemMax(BoltPist,          0,   0,   1,   1,   1,   1,   0,   0,   1,   1,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   1,   1,   0,  0,   0,  0,  0,  0);
populateitemmax(shurpist,          0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,  0,   1,   1,   0,  0,   1,   1,   0,   0,   1,  1,   0,  0,  0,  0);
PopulateItemMax(BoltPistAmmo,      0,   0, 200, 200, 200, 200, 200, 200, 250, 250, 220, 220,   0,   0,   0,   0,   0, 160, 160,   0, 120, 120,   0,   0,   0,   0,  0, 120, 120,   0,  0, 250, 250, 120, 120, 120, 120,  0,  0,  0,  0);
populateitemmax(apothheal,         0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,   0,   0,   0,   0,  0,   0,   0,   1,   1,  0,  0,   0,  0,   0,  0);  
populateitemmax(smallflamer,       1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,   0,   0,   0,   0,  0,   0,   0,   1,   1,  0,  0,   0,  0,   0,  0);  

//New Defensive Armaments
populateItemMax(MineLauncher,      0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,   0,   0,   0,   0,  0,   0,   0,   0,   0,  0,  0,   0,  0,   0,  0);  
populateItemMax(MinelAmmo,         0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   8,   8,   0,   0,   0,   0,   0,   0,   0,   8,   0,   0,   0,   0,   0,  0,   0,   0,   0,   0,  0,   0,   0,   0,   0,  0,  0,   0,  0,   0,  0);  

//misc
//                                mdm, fdm, msc, fsc, msp, fsp, msn, fsn, mme, fme, men, fen, mbu, fbu, mal, fal,  cy, mco, fco,  de, mlo, flo, msr, fsr, c1c, fid,      sts,     dvg,      els,      aph,     wlk,     was,     fas
//                                ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---   --  ---  ---  ---  ---  ---  ---  ---  ---      ---       ---  ---  ---  ---
populateitemmax(beacon,            3,   3,   3,   3,   2,   2,   3,   3,   3,   3,   3,   3,   5,   5,   1,   1,   1,   3,   3,   3,   3,   3,   6,   6,   3,  3,   3,   3,  3,   3,   3,   3,   3,   3,  3,   3,   3,  3,  3,   3,  3);
populateitemmax(grenade,           5,   5,   5,   5,   3,   3,   5,   5,   6,   6,   6,   6,   6,   6,   5,   5,   3,   5,   5,   5,   3,   3,   6,   6,   3,  2,   2,   5,  5,   5,   5,   5,   5,   5,  5,   5,   5,  5,  5,   5,  5);
populateitemmax(mineammo,          2,   2,   3,   3,   3,   3,   3,   3,   5,   5,   5,   5,   6,   6,   3,   3,   5,   5,   5,   5,   5,   5,   4,   4,   5,  5,   5,   5,  5,   5,   5,   5,   5,   5,  5,   5,   5,  5,  5,   5,  5);
populateitemmax(repairkit,         1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,  1,   1,   1,  1,   1,   1,   1,   1,   5,  5,   1,   1,  1,  1,   1,  1);

//packs
//                                mdm, fdm, msc, fsc, msp, fsp, msn, fsn, mme, fme, men, fen, mbu, fbu, mal, fal,  cy, mco, fco,  de, mlo, flo, msr, fsr, c1c, fid, sts, dvg, els, aph, wlk, was, fas
//                                ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---   --  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
populateitemmax(ammopack,          0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   0,   0,   1,  1,1, 1,1, 1,1, 1,1, 1,1, 0,0, 0,0, 1,1);
populateitemmax(energypack,        0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   1,  1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 0,0, 1,1);
populateitemmax(repairpack,        1,   1,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   1,  1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 0,0, 1,1);
populateitemmax(shieldpack,        0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   1,   1,   0,   0,   1,  1,1, 0,0, 1,1, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(sensorjammerpack,  1,   1,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   0,   0,   1,  0,0, 1,1, 1,1, 1,1, 0,0, 0,0, 0,0, 0,0);
populateitemmax(flamepack,         0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   0,   0,   0,   0,   0,   0,   0,   0,  1,1, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(mindpack,          0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 1,1, 0,0, 0,0);
populateitemmax(feedpack,          0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(laserpack,         0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   0,   1,   1,   1,   0,   0,   0,   0,   1,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 1,1);
populateitemmax(opticpack,         0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 1,1, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(regenerationpack,  0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(laptop,            0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   1,   1,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 1,1);
populateitemmax(stealthshieldpack, 1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(cloakingdevice,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 1,1, 0,0, 0,0, 0,0, 0,0);
populateitemmax(AssaultPack,       0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(HawkPack,          0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateItemMax(WarpxPack,         0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 1,1, 0,0);
populateItemMax(StarCannonPack,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateItemMax(EyePack,           0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);

//guided systems
//                                 mdm, fdm, msc, fsc, msp, fsp, msn, fsn, mme, fme, men, fen, mbu, fbu, mal, fal,  cy, mco, fco,  de, mlo, flo, msr, fsr, c1c, fid, sts, mer, els, aph, wlk, was, fas
//                                 ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---   --  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
//populateitemmax(imrecpack,          1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   1,   1,   1,  1,1, 1,1, 1,1, 1,1, 1,1, 0,0, 1,1, 1,1);
//populateitemmax(trackermissilepack, 1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   1,   1,   1,  1,1, 1,1, 1,1, 1,1, 1,1, 0,0, 1,1, 1,1);
//populateitemmax(biomissilepack,     0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   1,   1,   0,   0,   0,   0,   0,   1,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 1,1);
//populateitemmax(trackermissileammo, 2,   2,   1,   1,   1,   1,   1,   1,   5,   5,   2,   2,  10,  10,   5,   5,  20,  10,  10,   2,   0,   0,   1,   1,  20,  5,5, 2,2, 5,5, 1,1, 5,5, 0,0, 5,5, 8,8);
//populateitemmax(photonpack,         0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,   0,   0,   1,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);

//deployable weapons
//                               mdm, fdm, msc, fsc, msp, fsp, msn, fsn, mme, fme, men, fen, mbu, fbu, mal, fal,  cy, mco, fco,  de, mlo, flo, msr, fsr, c1c, fid, sts, mer, els, aph, wlk, was, fas
//                               ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---   --  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
populateitemmax(fusionturretpack,  0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(laserturretpack,   0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(railturretpack,    0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(scatturretpack,    0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(plasmaturretpack,  0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(gunbatpack,        0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(shurturretpack,    0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);  
populateitemmax(boltturretpack,    0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(boltcanturretpack, 0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(flameturretpack,   0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(hflameturretpack,  0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(partturretpack,    0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(rocketpack,        0,    0,   0,   0,  0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);

//deployable sensors
//                                          mdm, fdm, msc, fsc, msp, fsp, msn, fsn, mme, fme, men, fen, mbu, fbu, mal, fal,  cy, mco, fco,  de, mlo, flo, msr, fsr, c1c, fid, sts, mer, els, aph, wlk, was, fas
//                                          ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---   --  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
populateitemmax(camerapack,                  0,   0,   1,   1,   1,   1,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(deployablesensorjammerpack,  0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(motionsensorpack,            0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(pulsesensorpack,             0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);

//deployable objects
//                                           mdm, fdm, msc, fsc, msp, fsp, msn, fsn, mme, fme, men, fen, mbu, fbu, mal, fal,  cy, mco, fco,  de, mlo, flo, msr, fsr, c1c, fid, sts, mer, els, aph, wlk, was, fas
//                                           ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---   --  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
populateitemmax(deployableammopack,           0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   0,   0,   0,   0,   0,   1,   1,   1,   0,   0,   0,   0,   0,  1,1, 0,0, 1,1, 1,1, 0,0, 0,0, 0,0, 0,0);
populateitemmax(blastwallpack,                0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(forcefieldpack,               0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(largeforcefieldpack,          0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(inventorypack,                0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(springboard,                  0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(teleportpack,                 0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(acceleratorpack,              0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(camerapack,                   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(pulsesensorpack,              0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(sensorjammerpack,             0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(deployablecompack,            0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(deployableinvpack,            0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(springpack,                   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(acceleratordevicepack,        0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(largeairplatpack,             0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(detpack,                      0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   0,   1,   1,   1,   1,   1,   1,   1,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(biginvpack,                   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(lrmotionsensorpack,           0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(lrpulsesensorpack,            0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(deployablelrsensorjammerpack, 0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(bigfieldpack,                 0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(doorpack,                     0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateitemmax(largedoorpack,                0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateItemMax(SatelliteUplinkPack,          0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateItemMax(CommandCenterPack,            0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);
populateItemMax(CommandCenterPack2,            0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0);

//psionics
//                               mdm,  fdm, msc, fsc, msp, fsp, msn, fsn, mme, fme, men, fen, mbu, fbu, mal, fal,  cy, mco, fco,  de, mlo, flo, msr,  fsr,c1c,  fid,      sts,     mer,     els,      aph,      wlk,    was,    fas
//                               ---   ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---   --  ---  ---  ---   --- ---   ---       ---      ---      ---       ---       ---     ---     ---                                ---
populateitemmax(dcannon,          0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
populateitemmax(stream,           0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
populateitemmax(heal,             0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
populateitemmax(pull,             0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   0,  0,   0,  0,  0,  0);
populateitemmax(zap,              0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
populateitemmax(fireball,         0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   0,  0,   0,  0,  0,  0);
populateitemmax(cloak,            0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
populateitemmax(rain,             0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   0,  0,   0,  0,  0,  0);
populateitemmax(dis,              0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   0,  0,   0,  0,  0,  0);
populateitemmax(psilaser,         0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
populateitemmax(gravi,            0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
populateitemmax(distort,          0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   0,  0,   0,  0,  0,  0);
populateitemmax(rokkitlauncher,   0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   0,  0,   0,  0,  0,  0);
populateitemmax(kannon,           0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
populateitemmax(burst,            0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
populateitemmax(disc,             0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
populateitemmax(runeshield,       0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
populateitemmax(flamewall,        0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   0,  0,   0,  0,  0,  0);
PopulateItemMax(ShockBlast,       0,    0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,  0,   0,   0,   0,  0,   0,   0,   0,  0,   1,  1,   0,  0,  0,  0);
