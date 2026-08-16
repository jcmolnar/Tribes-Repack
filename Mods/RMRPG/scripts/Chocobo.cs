//By Deus_ex_Machina
//
//
	$Chocobover = "0.49 Alpha";

$MaxChocobo = "6"; //8 is max for now...(Menu shows up to 8 lines per page)
		//Careing of one Chocobo is hard and costs $$
		//But you can make good money if you have a good Chocobo!

//New Save vars
//$Chocobo[%Client] //Num of Chocobo Client has
//$Chocobo[%Client, %i] //Keep track of the Chocobos...
//$ChocoboTakeCare[%Client, %i] //You can pay a trainer to care for your Chocobo(s).
//$ChocoboName[%Client, $Chocobo[%Client, %i]] // Name
//$ChocoboColor[%Client, $Chocobo[%Client, %i]] // Color
//$ChocoboSex[%Client, $Chocobo[%Client, %i]] // Sex (Male or Female)
//$ChocoboYAge[%Client, $Chocobo[%Client, %i]] // Age Years = 40 days
//$ChocoboDAge[%Client, $Chocobo[%Client, %i]] // Age Days = 15 mins(real time)
//$ChocoboTempAge[%Client, $Chocobo[%Client, %i]] // Temp Age (Tracks mins)
//$ChocoboSTR[%Client, $Chocobo[%Client, %i]] //STR
//$ChocoboDEX[%Client, $Chocobo[%Client, %i]] //DEX
//$ChocoboCON[%Client, $Chocobo[%Client, %i]] //CON
//$ChocoboINT[%Client, $Chocobo[%Client, %i]] //INT
//$ChocoboWIS[%Client, $Chocobo[%Client, %i]] //WIS
//$ChocoboEXP[%Client, $Chocobo[%Client, %i]] //EXP
//$ChocoboHealth[%Client, $Chocobo[%Client, %i]] //Health (in %. 100% being max...) 100% = super happy 0% = he WILL die... random chance of dieing/running away at 10% maybe higher?
//$ChocoboHungry[%Client, $Chocobo[%Client, %i]] //Full (in %. 100% means his full! If 0% he WILL die)
//$ChocoboWorth[%Client, $Chocobo[%Client, %i]] // $$$ =p
//$ChocoboMeats[%Client, $Chocobo[%Client, %i]] //Meat = STR+
//$ChocoboFruits[%Client, $Chocobo[%Client, %i]] //Fruits etc = INT+
//$ChocoboVits[%Client, $Chocobo[%Client, %i]] //Vit = CON+
//$ChocoboSeeds[%Client, $Chocobo[%Client, %i]] //Seeds = WIS+
//$ChocoboCandies[%Client, $Chocobo[%Client, %i]] //Candies = DEX+

%i = 0;
$ChocoboNames[%i++] = "Choco";	//Need more names!
$ChocoboNames[%i++] = "Coco";
$ChocoboNames[%i++] = "Ruby";
$ChocoboNames[%i++] = "Luke";
$ChocoboNames[%i++] = "Lucky";
%i = "";

//       Chocobo::Get(2049,  found, "Choco", "Red" , "Male", 1,    13,     14,     50,   46,   20,   75,   71,  83128,   76,       79,     412,    0,       0,     0,      0,     0);
function Chocobo::Get(%Client, %opt, %name, %color, %sex, %YAge, %DAge, %TempAge, %STR, %DEX, %CON, %INT, %WIS, %EXP, %Health, %Hungry, %Worth, %Meats, %Fruits, %Vits, %Seeds, %Candies) { //Buy, Quest or Found
	if($Chocobo[%Client] < $MaxChocobo) {																																														//Found Chocobo always have a name. =\
		if($Chocobo[%Client] == "")
			$Chocobo[%Client] = 0;
		$Chocobo[%Client]++;
		%SaveSlot = Chocobo::GetSaveSlot(%Client);
	}
	else {
		if(%opt == found)
			%opt = find;
		if(%opt == quest)
			Client::sendMessage(%Client, 1, $put_name_here@" tells you, Sorry. You have to many Chocobos! ("@$Chocobo[%Client]@") Came back if you released one.");
		else
			Client::sendMessage(%Client, 0, "You have to many Chocobos! ("@$Chocobo[%Client]@") Release one if you want to "@%opt@" new ones!");
		return;
	}
	if(%opt == buy) {
		Client::sendMessage(%Client, 0, "You bought a "@%color@" Chocobo!");
	}
	else if(%opt == found) {
		Client::sendMessage(%Client, 0, "You found a "@%color@" Chocobo!");
		Client::sendMessage(%Client, 2, %name@" tells you, Kweh! Now that you caught me you better take good care of me!");
	}
	else if(%opt == quest)
		Client::sendMessage(%Client, 0, $put_name_here@" gave you a "@%color@" Chocobo!");

	Chocobo::Add(%Client, %opt, %SaveSlot, %name, %color, %sex, %YAge, %DAge, %TempAge, %STR, %DEX, %CON, %INT, %WIS, %EXP, %Health, %Hungry, %Worth, %Meats, %Fruits, %Vits, %Seeds, %Candies);
}

function Chocobo::Trade(%ClientHost, %ClientBuyer, %opt) { //Trade or Sell
	%Buyername = Client::getname(%ClientBuyer);
	%Hostname = Client::getname(%ClientHost);

	//$MenuTradeBuyerId[%Client]
	$MenuTradeHostId[%ClientBuyer] = %ClientHost;	//Save host Id on buyer
		//$MenuTradeChocoboId::Host[%Client] = "";
		//$MenuTradeChocoboId::Buyer[%Client] = "";

	if($Chocobo[%ClientHost] <= "0") {	//The "host" always has to have one Chocobo
		Client::sendMessage(%ClientHost, 0, "You don't have a Chocobo to "@%opt@"!");
		return false;
	}
	else if(%opt == JustChecking)
		return true;
	if(%opt == trade) {
		if($Chocobo[%ClientBuyer] <= "0") {	//If the host wants to trade the other guy has to have one
			Client::sendMessage(%ClientHost, 0, %Buyername@" doesn't have a Chocobo to trade with!");
			$MenuTradeChocoboId::Host[%ClientHost] = "";
			$MenuTradeChocoboId::Buyer[%ClientHost] = "";
			return false;
		}
		else {
			$IsTradeing[%ClientHost] = true;
			$IsTradeing[%ClientBuyer] = true;
			Client::sendMessage(%ClientBuyer, 0, %Hostname@" wants to trade his "@$ChocoboName[%ClientHost, $MenuTradeChocoboId::Host[%ClientHost]]@" for your "@$ChocoboName[%ClientBuyer, $MenuTradeChocoboId::Buyer[%ClientBuyer]]);
			Client::sendMessage(%ClientBuyer, 0, "Type '#trade yes' if you want to make this trade! Or '#trade no' if you don't.");
			$CanTrade[%ClientBuyer] = false; //if true(for both) then trade goes thru..
			$CanTrade[%ClientHost] = true;  //Host can always type #trade no if he changes his mind before buyer trades.
		}
	}
	if(%opt == Sell) {
		$IsTradeing[%ClientHost] = true;
		$IsTradeing[%ClientBuyer] = true;
		Client::sendMessage(%ClientBuyer, 0, %Hostname@" wants to sell his "@$ChocoboName[%ClientHost, $MenuTradeChocoboId::Host[%ClientHost]]@" for "@FixM($ChocoboWorth[%ClientHost, $MenuTradeChocoboId::Host[%ClientHost]]));
		Client::sendMessage(%ClientBuyer, 0, "Type '#buy yes' if you want to buy this Chocobo! Or '#buy no' if you don't.");
		$CanBuyChocobo[%ClientBuyer] = $MenuTradeChocoboId::Host[%ClientHost];
	//
	}
	if(%opt == failedbuyer) {
		Client::sendMessage(%ClientBuyer, 1, "You canceled the trade!");
		Client::sendMessage(%ClientHost, 1, %Buyername@" canceled the trade!");
		$CanTrade[%ClientBuyer] = "";
		$CanTrade[%ClientHost] = "";
		$MenuTradeBuyerId[%ClientHost] = "";
		$MenuTradeHostId[%ClientBuyer] = "";
		$MenuTradeChocoboId::Host[%ClientHost] = "";
		$MenuTradeChocoboId::Buyer[%ClientHost] = "";
		$IsTradeing[%ClientHost] = "";
		$IsTradeing[%ClientBuyer] = "";
	}
	if(%opt == failedhost) {
		Client::sendMessage(%ClientHost, 1, "You canceled the trade!");
		Client::sendMessage(%ClientBuyer, 1, %Hostname@" canceled the trade!");
		$CanTrade[%ClientBuyer] = "";
		$CanTrade[%ClientHost] = "";
		$MenuTradeBuyerId[%ClientHost] = "";
		$MenuTradeHostId[%ClientBuyer] = "";
		$MenuTradeChocoboId::Host[%ClientHost] = "";
		$MenuTradeChocoboId::Buyer[%ClientHost] = "";
		$IsTradeing[%ClientHost] = "";
		$IsTradeing[%ClientBuyer] = "";
	}
	if(%opt == traded) {
		%HostChoco = $MenuTradeChocoboId::Host[%ClientHost];
		%BuyerChoco = $MenuTradeChocoboId::Buyer[%ClientHost];
		Chocobo::Switch(%ClientHost, %ClientBuyer, %HostChoco, %BuyerChoco, None);
		$CanTrade[%ClientBuyer] = "";
		$CanTrade[%ClientHost] = "";
		$MenuTradeBuyerId[%ClientHost] = "";
		$MenuTradeHostId[%ClientBuyer] = "";
		$MenuTradeChocoboId::Host[%ClientHost] = "";
		$MenuTradeChocoboId::Buyer[%ClientHost] = "";
		$IsTradeing[%ClientHost] = "";
		$IsTradeing[%ClientBuyer] = "";
	}

}

function Chocobo::Breed(%ClientHost, %ClientBuyer, %opt) {

	%Buyername = Client::getname(%ClientBuyer);
	%Hostname = Client::getname(%ClientHost);

	$MenuBreedHostId[%ClientBuyer] = %ClientHost;

	if(%opt == "Breed") {
		$IsTradeing[%ClientHost] = true;
		$IsTradeing[%ClientBuyer] = true;
	}
	else if(%opt == "Breeding") {

	}
	else if(%opt == "failedbuyer") {
		Client::sendMessage(%ClientBuyer, 1, "You canceled!");
		Client::sendMessage(%ClientHost, 1, %Buyername@" canceled!");


		$IsTradeing[%ClientHost] = "";
		$IsTradeing[%ClientBuyer] = "";
	}
	else if(%opt == "failhost") {
		Client::sendMessage(%ClientHost, 1, "You canceled!");
		Client::sendMessage(%ClientBuyer, 1, %Hostname@" canceled!");


		$IsTradeing[%ClientHost] = "";
		$IsTradeing[%ClientBuyer] = "";
	}
}

function Chocobo::Switch(%Client, %ClientBuyer, %Choco, %BuyerChoco, %opt) {

			Client::sendMessage(%Client, 0, "Trying to 'Switch'  Chocobos..");
	if(%opt != HostSaved) {
		%i = Chocobo::GetSaveSlot(%Client);
		if(%i != "") {
			$Chocobo[%Client, %i] = true;
			$ChocoboTakeCare[%Client, %i] = "false";
			$ChocoboName[%Client, %i] = $ChocoboName[%ClientBuyer, %BuyerChoco];
			$ChocoboColor[%Client, %i] = $ChocoboColor[%ClientBuyer, %BuyerChoco];
			$ChocoboSex[%Client, %i] = $ChocoboSex[%ClientBuyer, %BuyerChoco];
			$ChocoboYAge[%Client, %i] = $ChocoboYAge[%ClientBuyer, %BuyerChoco];
			$ChocoboDAge[%Client, %i] = $ChocoboDAge[%ClientBuyer, %BuyerChoco];
			$ChocoboTempAge[%Client, %i] = $ChocoboTempAge[%ClientBuyer, %BuyerChoco];
			$ChocoboSTR[%Client, %i] = $ChocoboSTR[%ClientBuyer, %BuyerChoco];
			$ChocoboDEX[%Client, %i] = $ChocoboDEX[%ClientBuyer, %BuyerChoco];
			$ChocoboCON[%Client, %i] = $ChocoboCON[%ClientBuyer, %BuyerChoco];
			$ChocoboINT[%Client, %i] = $ChocoboINT[%ClientBuyer, %BuyerChoco];
			$ChocoboWIS[%Client, %i] = $ChocoboWIS[%ClientBuyer, %BuyerChoco];
			$ChocoboEXP[%Client, %i] = $ChocoboEXP[%ClientBuyer, %BuyerChoco];
			$ChocoboHealth[%Client, %i] = $ChocoboHealth[%ClientBuyer, %BuyerChoco];
			$ChocoboHungry[%Client, %i] = $ChocoboHungry[%ClientBuyer, %BuyerChoco];
			$ChocoboWorth[%Client, %i] = $ChocoboWorth[%ClientBuyer, %BuyerChoco];
			$ChocoboMeats[%Client, %i] = $ChocoboMeats[%ClientBuyer, %BuyerChoco];
			$ChocoboFruits[%Client, %i] = $ChocoboFruits[%ClientBuyer, %BuyerChoco];
			$ChocoboVits[%Client, %i] = $ChocoboVits[%ClientBuyer, %BuyerChoco];
			$ChocoboSeeds[%Client, %i] = $ChocoboSeeds[%ClientBuyer, %BuyerChoco];
			$ChocoboCandies[%Client, %i] = $ChocoboCandies[%ClientBuyer, %BuyerChoco];
			Chocobo::Clear(%ClientBuyer, %BuyerChoco);
			%NoRoomHost = false;
		}
		else
			%NoRoomHost = true;
	}
	if(%opt != BuyerSaved) {
		%i = Chocobo::GetSaveSlot(%ClientBuyer);
		if(%i != "") {
			%BuyerChocobo = %i;
			$ChocoboTakeCare[%ClientBuyer, %BuyerChocobo] = "false";
			$Chocobo[%ClientBuyer, %BuyerChocobo] = true;
			$ChocoboName[%ClientBuyer, %BuyerChocobo] = $ChocoboName[%Client, %Choco];
			$ChocoboColor[%ClientBuyer, %BuyerChocobo] = $ChocoboColor[%Client, %Choco];
			$ChocoboSex[%ClientBuyer, %BuyerChocobo] = $ChocoboSex[%Client, %Choco];
			$ChocoboYAge[%ClientBuyer, %BuyerChocobo] = $ChocoboYAge[%Client, %Choco];
			$ChocoboDAge[%ClientBuyer, %BuyerChocobo] = $ChocoboDAge[%Client, %Choco];
			$ChocoboTempAge[%ClientBuyer, %BuyerChocobo] = $ChocoboTempAge[%Client, %Choco];
			$ChocoboSTR[%ClientBuyer, %BuyerChocobo] = $ChocoboSTR[%Client, %Choco];
			$ChocoboDEX[%ClientBuyer, %BuyerChocobo] = $ChocoboDEX[%Client, %Choco];
			$ChocoboCON[%ClientBuyer, %BuyerChocobo] = $ChocoboCON[%Client, %Choco];
			$ChocoboINT[%ClientBuyer, %BuyerChocobo] = $ChocoboINT[%Client, %Choco];
			$ChocoboWIS[%ClientBuyer, %BuyerChocobo] = $ChocoboWIS[%Client, %Choco];
			$ChocoboEXP[%ClientBuyer, %BuyerChocobo] = $ChocoboEXP[%Client, %Choco];
			$ChocoboHealth[%ClientBuyer, %BuyerChocobo] = $ChocoboHealth[%Client, %Choco];
			$ChocoboHungry[%ClientBuyer, %BuyerChocobo] = $ChocoboHungry[%Client, %Choco];
			$ChocoboWorth[%ClientBuyer, %BuyerChocobo] = $ChocoboWorth[%Client, %Choco];
			$ChocoboMeats[%ClientBuyer, %BuyerChocobo] = $ChocoboMeats[%Client, %Choco];
			$ChocoboFruits[%ClientBuyer, %BuyerChocobo] = $ChocoboFruits[%Client, %Choco];
			$ChocoboVits[%ClientBuyer, %BuyerChocobo] = $ChocoboVits[%Client, %Choco];
			$ChocoboSeeds[%ClientBuyer, %BuyerChocobo] = $ChocoboSeeds[%Client, %Choco];
			$ChocoboCandies[%ClientBuyer, %BuyerChocobo] = $ChocoboCandies[%Client, %Choco];
			Chocobo::Clear(%Client, %Choco);
			%NoRoomBuyer = false;
		}
		else
			%NoRoomBuyer = true;
	}

	if(%NoRoomHost == true && %NoRoomBuyer == false)
		Chocobo::Switch(%Client, %ClientBuyer, %Choco, %BuyerChoco, BuyerSaved);
	if(%NoRoomHost == false && %NoRoomBuyer == true)
		Chocobo::Switch(%Client, %ClientBuyer, %Choco, %BuyerChoco, HostSaved);
	if(%NoRoomHost == true && %NoRoomBuyer == true) {
		$tmpChoco[%Client] = 20;
		%i = $tmpChoco[%Client];
		$HastempChoco[%Client] = true;
		$Chocobo[%Client, %i] = true;
		$ChocoboTakeCare[%Client, %i] = "false";
		$ChocoboName[%Client, %i] = $ChocoboName[%ClientBuyer, %BuyerChoco];
		$ChocoboColor[%Client, %i] = $ChocoboColor[%ClientBuyer, %BuyerChoco];
		$ChocoboSex[%Client, %i] = $ChocoboSex[%ClientBuyer, %BuyerChoco];
		$ChocoboYAge[%Client, %i] = $ChocoboYAge[%ClientBuyer, %BuyerChoco];
		$ChocoboDAge[%Client, %i] = $ChocoboDAge[%ClientBuyer, %BuyerChoco];
		$ChocoboTempAge[%Client, %i] = $ChocoboTempAge[%ClientBuyer, %BuyerChoco];
		$ChocoboSTR[%Client, %i] = $ChocoboSTR[%ClientBuyer, %BuyerChoco];
		$ChocoboDEX[%Client, %i] = $ChocoboDEX[%ClientBuyer, %BuyerChoco];
		$ChocoboCON[%Client, %i] = $ChocoboCON[%ClientBuyer, %BuyerChoco];
		$ChocoboINT[%Client, %i] = $ChocoboINT[%ClientBuyer, %BuyerChoco];
		$ChocoboWIS[%Client, %i] = $ChocoboWIS[%ClientBuyer, %BuyerChoco];
		$ChocoboEXP[%Client, %i] = $ChocoboEXP[%ClientBuyer, %BuyerChoco];
		$ChocoboHealth[%Client, %i] = $ChocoboHealth[%ClientBuyer, %BuyerChoco];
		$ChocoboHungry[%Client, %i] = $ChocoboHungry[%ClientBuyer, %BuyerChoco];
		$ChocoboWorth[%Client, %i] = $ChocoboWorth[%ClientBuyer, %BuyerChoco];
		$ChocoboMeats[%Client, %i] = $ChocoboMeats[%ClientBuyer, %BuyerChoco];
		$ChocoboFruits[%Client, %i] = $ChocoboFruits[%ClientBuyer, %BuyerChoco];
		$ChocoboVits[%Client, %i] = $ChocoboVits[%ClientBuyer, %BuyerChoco];
		$ChocoboSeeds[%Client, %i] = $ChocoboSeeds[%ClientBuyer, %BuyerChoco];
		$ChocoboCandies[%Client, %i] = $ChocoboCandies[%ClientBuyer, %BuyerChoco];
		Chocobo::Clear(%ClientBuyer, %BuyerChoco);
		Chocobo::Switch(%Client, %ClientBuyer, %Choco, %BuyerChoco, HostSaved);
	}
	if($HastempChoco[%Client] == true) {
		%BuyerChoco = $tmpChoco[%Client];
		$HastempChoco[%Client] = true;
		$Chocobo[%Client, %i] = true;
		$ChocoboTakeCare[%Client, %i] = "false";
		$ChocoboName[%Client, %i] = $ChocoboName[%Client, %BuyerChoco];
		$ChocoboColor[%Client, %i] = $ChocoboColor[%Client, %BuyerChoco];
		$ChocoboSex[%Client, %i] = $ChocoboSex[%Client, %BuyerChoco];
		$ChocoboYAge[%Client, %i] = $ChocoboYAge[%Client, %BuyerChoco];
		$ChocoboDAge[%Client, %i] = $ChocoboDAge[%Client, %BuyerChoco];
		$ChocoboTempAge[%Client, %i] = $ChocoboTempAge[%Client, %BuyerChoco];
		$ChocoboSTR[%Client, %i] = $ChocoboSTR[%Client, %BuyerChoco];
		$ChocoboDEX[%Client, %i] = $ChocoboDEX[%Client, %BuyerChoco];
		$ChocoboCON[%Client, %i] = $ChocoboCON[%Client, %BuyerChoco];
		$ChocoboINT[%Client, %i] = $ChocoboINT[%Client, %BuyerChoco];
		$ChocoboWIS[%Client, %i] = $ChocoboWIS[%Client, %BuyerChoco];
		$ChocoboEXP[%Client, %i] = $ChocoboEXP[%Client, %BuyerChoco];
		$ChocoboHealth[%Client, %i] = $ChocoboHealth[%Client, %BuyerChoco];
		$ChocoboHungry[%Client, %i] = $ChocoboHungry[%Client, %BuyerChoco];
		$ChocoboWorth[%Client, %i] = $ChocoboWorth[%Client, %BuyerChoco];
		$ChocoboMeats[%Client, %i] = $ChocoboMeats[%Client, %BuyerChoco];
		$ChocoboFruits[%Client, %i] = $ChocoboFruits[%Client, %BuyerChoco];
		$ChocoboVits[%Client, %i] = $ChocoboVits[%Client, %BuyerChoco];
		$ChocoboSeeds[%Client, %i] = $ChocoboSeeds[%Client, %BuyerChoco];
		$ChocoboCandies[%Client, %i] = $ChocoboCandies[%Client, %BuyerChoco];
		Chocobo::Clear(%Client, %BuyerChoco);
		$HastempChoco[%Client] = "";
	}
}

function Chocobo::NewName(%Client, %opt, %Choco) {
	$ChocoboTempName[%Client] = %Choco;
	if(%opt == NewName) {
		Client::sendMessage(%Client, 0, "Enter a name for your new "@$ChocoboColor[%Client, %Choco]@" Chocobo! (Example: #name "@$ChocoboColor[%Client, %Choco]@"_Choco).");
		$CanName[%Client] = true;
	}
	else if(%opt == OldName) {
		Client::sendMessage(%Client, 0, $ChocoboName[%Client, %Choco]@". What new name would you like? (Example: #name Red_Chocobo Or type just #name  to cancel.).");
		$CanName[%Client] = true;
		$OldName[%Client] = true;
	}
	else if(%opt == TryAgain) {
		Client::sendMessage(%Client, 0, "You can't have spaces in your name! Try again. (Example: #name Red_Chocobo).");
		$CanName[%Client] = true;
	}
}

function Chocobo::Theme(%Client, %object) {

	if($ChocoboSpawn[%Client] == "") {
		%object.driver = "";
		%object.vehicle = "";
	}
	if(%object.driver == 1) {
		remoteEval(%Client, SFXPLAY, 1, "ChocoboTheme.wav", 62/2);
	}
}

$Chocobo::invalidChars = " ><?\\\"{}[]+=:;/.,!@#$%&()|`~";

function Chocobo::InvalidChar(%name) {

	for(%a = 1; %a <= String::len($Chocobo::invalidChars); %a++) {
		%b = String::getSubStr($Chocobo::invalidChars, %a-1, 1);
		if(String::findSubStr(%name, %b) != -1) {
			return true;
		}
	}
	return false;
}

function Chocobo::Getlen(%string) {

	for(%len = 0; String::getSubStr(%string, %len, 1) != ""; %len++) {}
	return %len;
}

function Chocobo::GetSaveSlot(%Client) {
	for(%i = 1; %i <= $MaxChocobo; %i++) {
		if($ChocoboName[%Client, %i] == "")
			$ChocoboName[%Client, %i] = "Free save slot";
		if($Chocobo[%Client, %i] == "")
			$Chocobo[%Client, %i] = "false";
	}
	for(%i = 1; %i <= $MaxChocobo; %i++) {
		if($Chocobo[%Client, %i] == "false")
			return %i;
	}
}

function Chocobo::Add(%Client, %opt, %SaveSlot, %name, %color, %sex, %YAge, %DAge, %TempAge, %STR, %DEX, %CON, %INT, %WIS, %EXP, %Health, %Hungry, %Worth, %Meats, %Fruits, %Vits, %Seeds, %Candies) {

		%i = %SaveSlot;

		$Chocobo[%Client, %i]		= true;
		$ChocoboTakeCare[%Client, %i]= "false";
		$ChocoboName[%Client, %i]	= %name;
		$ChocoboColor[%Client, %i]	= %color;
		$ChocoboSex[%Client, %i]	= %sex;
		$ChocoboYAge[%Client, %i]	= %YAge;
		$ChocoboDAge[%Client, %i]	= %DAge;
		$ChocoboTempAge[%Client, %i]= %TempAge;
		$ChocoboSTR[%Client, %i]	= %STR;
		$ChocoboDEX[%Client, %i]	= %DEX;
		$ChocoboCON[%Client, %i]	= %CON;
		$ChocoboINT[%Client, %i]	= %INT;
		$ChocoboWIS[%Client, %i]	= %WIS;
		$ChocoboEXP[%Client, %i]	= %EXP;
		$ChocoboHealth[%Client, %i]	= %Health;
		$ChocoboHungry[%Client, %i]	= %Hungry;
		$ChocoboWorth[%Client, %i]	= %Worth;
		$ChocoboMeats[%Client, %i]	= %Meats;
		$ChocoboFruits[%Client, %i]	= %Fruits;
		$ChocoboVits[%Client, %i]	= %Vits;
		$ChocoboSeeds[%Client, %i]	= %Seeds;
		$ChocoboCandies[%Client, %i]= %Candies;
}

function Chocobo::Clear(%Client, %Choco) {

	if($Chocobo[%Client, %Choco] == false || $Chocobo[%Client, %Choco] == "" || !$HasLoadedAndSpawned[%Client])
		return;

	%name = Client::getName(%Client);

	Client::sendMessage(%Client, 1, "Clearing "@$ChocoboName[%Client, $Chocobo[%Client, %Choco]]@" Num: "@%Choco); //For testing

	$Chocobo[%Client]--;
	$funk::var["[\""@%name@"\", 5, 1, 10, 11]"] = $Chocobo[%Client];
	$funk::var["[\""@%name@"\", 5, "@%Choco@", 10, 21]"] = "";
	$Chocobo[%Client, %Choco] = "false";
	$ChocoboTakeCare[%Client, %Choco] = "";
	$ChocoboName[%Client, %Choco] = "Free save slot";
	$ChocoboColor[%Client, %Choco] = "";
	$ChocoboSex[%Client, %Choco]  = "";
	$ChocoboYAge[%Client, %Choco]  = "";
	$ChocoboDAge[%Client, %Choco]  = "";
	$ChocoboTempAge[%Client, %Choco]  = "";
	$ChocoboSTR[%Client, %Choco]  = "";
	$ChocoboDEX[%Client, %Choco] = "";
	$ChocoboCON[%Client, %Choco]  = "";
	$ChocoboINT[%Client, %Choco]  = "";
	$ChocoboWIS[%Client, %Choco]  = "";
	$ChocoboEXP[%Client, %Choco]  = "";
	$ChocoboHealth[%Client, %Choco] = "";
	$ChocoboHungry[%Client, %Choco]  = "";
	$ChocoboWorth[%Client, %Choco]  = "";
	$ChocoboMeats[%Client, %Choco] = "";
	$ChocoboFruits[%Client, %Choco] = "";
	$ChocoboVits[%Client, %Choco] = "";
	$ChocoboSeeds[%Client, %Choco] = "";
	$ChocoboCandies[%Client, %Choco] = "";

	if($Chocobo[%Client] < 0)
		Client::sendMessage(%Client, 0, "What happened!? Your Chocobo total count is "@$Chocobo[%Client]@". This isn't a good thing. Contact your Server-host and ask him/her to look over your char file to see if you have any Chocobos left...");
}

function Chocobo::Food(%Client, %name, %lvltype, %STR, %DEX, %CON, %INT, %WIS, %M, %C, %V, %F, %S, %Choco) {

	$ChocoboMeats[%Client, %Choco] += %M;
	$ChocoboCandies[%Client, %Choco] += %C;
	$ChocoboVits[%Client, %Choco] += %V;
	$ChocoboFruits[%Client, %Choco] += %F;
	$ChocoboSeeds[%Client, %Choco] += %S;

	Chocobo::Sim(%Client, %Choco, %name, %lvltype);

	$ChocoboSTR[%Client, %Choco] += %STR;
	$ChocoboDEX[%Client, %Choco] += %DEX;
	$ChocoboCON[%Client, %Choco] += %CON;
	$ChocoboINT[%Client, %Choco] += %INT;
	$ChocoboWIS[%Client, %Choco] += %WIS;

}

function Chocobo::Sim(%Client, %Choco, %opt, %lvltype) { //Called every 5 mins or when you feed your Chocobo

	%name = Client::getname(%Client);
echo("Sim called ["@%Client@"]: %choco: "@%Choco@" | %opt = "@%opt@" | %lvltype = "@%lvltype);
	if($Chocobo[%Client, %Choco] == false || $Chocobo[%Client, %Choco] != true)
		return;

	if(%opt == Take) {

		%t = $ChocoboCandies[%Client, %Choco] + $ChocoboVits[%Client, %Choco] + $ChocoboFruits[%Client, %Choco] + $ChocoboSeeds[%Client, %Choco] - $ChocoboMeats[%Client, %Choco];
		if($ChocoboMeats[%Client, %Choco] > %t)
			$ChocoboHealth[%Client, %Choco] -= 4;
		%t = $ChocoboVits[%Client, %Choco] + $ChocoboFruits[%Client, %Choco] + $ChocoboSeeds[%Client, %Choco] + $ChocoboMeats[%Client, %Choco] - $ChocoboCandies[%Client, %Choco];
		if($ChocoboCandies[%Client, %Choco] > %t)
			$ChocoboHealth[%Client, %Choco] -= 4;
		%t = $ChocoboFruits[%Client, %Choco] + $ChocoboSeeds[%Client, %Choco] + $ChocoboMeats[%Client, %Choco] + $ChocoboCandies[%Client, %Choco] - $ChocoboVits[%Client, %Choco];
		if($ChocoboVits[%Client, %Choco] > %t)
			$ChocoboHealth[%Client, %Choco] -= 4;
		%t = $ChocoboSeeds[%Client, %Choco] + $ChocoboMeats[%Client, %Choco] + $ChocoboCandies[%Client, %Choco] + $ChocoboVits[%Client, %Choco] - $ChocoboFruits[%Client, %Choco];
		if($ChocoboFruits[%Client, %Choco] > %t)
			$ChocoboHealth[%Client, %Choco] -= 4;
		%t = $ChocoboMeats[%Client, %Choco] + $ChocoboCandies[%Client, %Choco] + $ChocoboVits[%Client, %Choco] + $ChocoboFruits[%Client, %Choco] - $ChocoboSeeds[%Client, %Choco];
		if($ChocoboSeeds[%Client, %Choco] > %t)
			$ChocoboHealth[%Client, %Choco] -= 4;

		$ChocoboHealth[%Client, %Choco] -= 1;
		$ChocoboHungry[%Client, %Choco] -= 3;

		%Stats = $ChocoboFruits[%Client, %Choco] + $ChocoboSeeds[%Client, %Choco] + $ChocoboMeats[%Client, %Choco] + $ChocoboCandies[%Client, %Choco] + $ChocoboVits[%Client, %Choco];
		$ChocoboWorth[%Client, %Choco] += round(%Stats / 5) + $ChocoboHealth[%Client, %Choco] + $ChocoboHungry[%Client, %Choco];

		if($ChocoboHungry[%Client, %Choco] <= 50) {
			$ChocoboHealth[%Client, %Choco] -= 2;
			$ChocoboWorth[%Client, %Choco] -= 50;
			%txt = "Chocobo "@$ChocoboName[%Client, %Choco]@" is hungry ";
		}
		if($ChocoboHungry[%Client, %Choco] <= 25) {
			$ChocoboHealth[%Client, %Choco] -= 3; //Your Chocobo can lose up to 10 health every 5 mins
			$ChocoboWorth[%Client, %Choco] -= 250;
			%txt = "Chocobo "@$ChocoboName[%Client, %Choco]@" is very hungry ";
		}
		if($ChocoboHungry[%Client, %Choco] >= 51 && $ChocoboHungry[%Client, %Choco] <= 74) {
			$ChocoboWorth[%Client, %Choco] += 50;
			%txt = "Chocobo "@$ChocoboName[%Client, %Choco]@" is getting a little hungry ";
		}
		if($ChocoboHungry[%Client, %Choco] >= 75 && $ChocoboHungry[%Client, %Choco] <= 89) {
			$ChocoboWorth[%Client, %Choco] += 75;
			%txt = "Chocobo "@$ChocoboName[%Client, %Choco]@" isn't very hungry ";
		}
		if($ChocoboHungry[%Client, %Choco] >= 90 && $ChocoboHungry[%Client, %Choco] <= 100) {
			$ChocoboWorth[%Client, %Choco] += 100;
			%txt = "Chocobo "@$ChocoboName[%Client, %Choco]@" is full ";
		}
		//Client::sendMessage(%Client, 0, %txt);

		if($ChocoboHealth[%Client, %Choco] <= 50) {
			$ChocoboWorth[%Client, %Choco] -= 50;
			%txt2 = "and is getting sick.";
		}
		if($ChocoboHealth[%Client, %Choco] <= 15) {
			$ChocoboWorth[%Client, %Choco] -= 250;
			%txt2 = "and has fallen ill.";
			if(OddsAre(2)) {
				MessageAll(0, %name@"'s "@$ChocoboColor[%Client, %Choco]@" Chocobo "@$ChocoboName[%Client, %Choco]@" has died of poor health.");
				Chocobo::Clear(%Client, %Choco);
				return;
			}
		}
		if($ChocoboHealth[%Client, %Choco] >= 51 && $ChocoboHealth[%Client, %Choco] <= 74) {
			$ChocoboWorth[%Client, %Choco] += 50;
			%txt2 = "and is a little happy.";
		}
		if($ChocoboHealth[%Client, %Choco] >= 75 && $ChocoboHealth[%Client, %Choco] <= 89) {
			$ChocoboWorth[%Client, %Choco] += 75;
			%txt2 = "and is happy.";
		}
		if($ChocoboHealth[%Client, %Choco] >= 90 && $ChocoboHealth[%Client, %Choco] <= 100) {
			$ChocoboWorth[%Client, %Choco] += 100;
			%txt2 = "and is very happy!";
		}

		Client::sendMessage(%Client, 0, %txt @ %txt2);
	}

	if($ChocoboHealth[%Client, %Choco] <= 0) {
		MessageAll(0, %name@"'s "@$ChocoboColor[%Client, %Choco]@" Chocobo "@$ChocoboName[%Client, %Choco]@" has died of poor health.");
		Chocobo::Clear(%Client, %Choco);
		return;
	}
	if($ChocoboHungry[%Client, %Choco] <= 0) {
		MessageAll(0, %name@"'s "@$ChocoboColor[%Client, %Choco]@" Chocobo "@$ChocoboName[%Client, %Choco]@" has died of hunger.");
		Chocobo::Clear(%Client, %Choco);
		return;
	}
	if($ChocoboHealth[%Client, %Choco] >= 100)
		$ChocoboHealth[%Client, %Choco] = 100;
	if($ChocoboHungry[%Client, %Choco] >= 100) {
		$ChocoboHungry[%Client, %Choco] = 100;
		Client::sendMessage(%Client, 0, "Your Chocobo is full.");
	}
	if($ChocoboWorth[%Client, %Choco] < 0)
		$ChocoboWorth[%Client, %Choco] = 0;
}

// (Removed leftover "TEMP" Cap()/round() redefinitions. They loaded AFTER
// rpgfunk.cs and overrode its versions - and this Cap() dropped rpgfunk's
// "inf" (uncapped) handling, so Cap(x,lb,"inf") returned the string "inf".
// That silently broke damage (playerdamage Cap(v-25,0,"inf")), the EXP table
// (rpgstats pow(Cap(i-20,0,"inf"),5)) and blacksmith pricing. rpgfunk.cs owns
// the canonical Cap()/round().)

function Chocobo::FoodType(%Client, %type, %lvltype, %Choco, %coins, %name) {

	if($ChocoboHungry[%Client, %Choco] >= 100) {
		Client::sendMessage(%Client, 0, "Your Chocobo isn't hungry.");
		return;
	}
	Client::sendMessage(%Client, 0, "You bought "@%type@".~wSoundMoney1.wav");

	%STR = %DEX = %CON = %INT = %WIS = %M = %C = %V = %F = %S = 0;

	$COINS[%Client] += %coins;

	if(%lvltype == 1) //6 lvls-- -1-$1,000  -2-$10,000  -3-$20,000  -4-$40,000  -5-$100,000 -6-$1,000,000
		%d = Between(1, 3); //1-3
	else if(%lvltype == 2)
		%d = Between(5, 15); //5-15
	else if(%lvltype == 3)
		%d = Between(15, 30); //15-30
	else if(%lvltype == 4)
		%d = Between(30, 40); //30-40
	else if(%lvltype == 5)
		%d = Between(50, 70); //50-70
	else if(%lvltype == 6)
		%d = Between(100, 150); //100-150
	$ChocoboWorth[%Client, %Choco] += ((%lvltype * 2) * %d) + (-%coins / 10);
	if(%name == "Meats") {
		$ChocoboHealth[%Client, %Choco] += %lvltype;
		$ChocoboHungry[%Client, %Choco] += 16 - %lvltype;
		Chocobo::Food(%Client, %name, %lvltype, %d, %DEX, %CON, %INT, %WIS, 1, %C, %V, %F, %S, %Choco);
		Client::sendMessage(%Client, 0, $ChocoboName[%Client, %Choco]@"'s STR increased by "@%d@".");
	}
	else if(%name == "Candies") {
		$ChocoboHealth[%Client, %Choco] += %lvltype;
		$ChocoboHungry[%Client, %Choco] += 13 - %lvltype;
		Chocobo::Food(%Client, %name, %lvltype, %STR, %d, %CON, %INT, %WIS, %M, 1, %V, %F, %S, %Choco);
		Client::sendMessage(%Client, 0, $ChocoboName[%Client, %Choco]@"'s DEX increased by "@%d@".");
	}
	else if(%name == "Vits") {
		$ChocoboHealth[%Client, %Choco] += %lvltype;
		$ChocoboHungry[%Client, %Choco] += 10 - %lvltype;
		Chocobo::Food(%Client, %name, %lvltype, %STR, %DEX, %d, %INT, %WIS, %M, %C, 1, %F, %S, %Choco);
		Client::sendMessage(%Client, 0, $ChocoboName[%Client, %Choco]@"'s CON increased by "@%d@".");
	}
	else if(%name == "Fruits") {
		$ChocoboHealth[%Client, %Choco] += %lvltype;
		$ChocoboHungry[%Client, %Choco] += 12 - %lvltype;
		Chocobo::Food(%Client, %name, %lvltype, %STR, %DEX, %CON, %d, %WIS, %M, %C, %V, 1, %S, %Choco);
		Client::sendMessage(%Client, 0, $ChocoboName[%Client, %Choco]@"'s INT increased by "@%d@".");
	}
	else if(%name == "Seeds") {
		$ChocoboHealth[%Client, %Choco] += %lvltype;
		$ChocoboHungry[%Client, %Choco] += 11 - %lvltype;
		Chocobo::Food(%Client, %name, %lvltype, %STR, %DEX, %CON, %INT, %d, %M, %C, %V, %F, 1, %Choco);
		Client::sendMessage(%Client, 0, $ChocoboName[%Client, %Choco]@"'s WIS increased by "@%d@".");
	}
	else if(%name == "Cracker") {
		$ChocoboHealth[%Client, %Choco] += %lvltype;
		$ChocoboHungry[%Client, %Choco] += 16 - %lvltype;
		Chocobo::Food(%Client, %name, %lvltype, %d, %d, %d, %d, %d, 1, 1, 1, 1, 1, %Choco);
		Client::sendMessage(%Client, 0, $ChocoboName[%Client, %Choco]@"'s all stats increased by "@%d@"!");
	}
	if($ChocoboHealth[%Client, %Choco] >= 100)
		$ChocoboHealth[%Client, %Choco] = 100;
	if($ChocoboHungry[%Client, %Choco] >= 100) {
		$ChocoboHungry[%Client, %Choco] = 100;
		Client::sendMessage(%Client, 0, "Your Chocobo is full.");
	}
	//RefreshAll(%Client);
	$CanFeed[%Client] = true;
	processMenuChocoboViewInfo(%Client, Feed);
}

function Chocobo::Timer(%Client, %Choco) { // Gets called in RecursiveWorld
	$ChocoboTempAge[%Client, %Choco]++;
	if($ChocoboTempAge[%Client, %Choco] == 5 || $ChocoboTempAge[%Client, %Choco] == 10 || $ChocoboTempAge[%Client, %Choco] == 15)
		Chocobo::Sim(%Client, %Choco, "Take", None);
	if($ChocoboTempAge[%Client, %Choco] >= 15) {
		$ChocoboTempAge[%Client, %Choco] = 0;
		$ChocoboDAge[%Client, %Choco]++;
	}
	if($ChocoboDAge[%Client, %Choco] >= 40) {
		$ChocoboDAge[%Client, %Choco] = 0;
		$ChocoboYAge[%Client, %Choco]++;
	}
}

//
function MenuChocobo(%Client) {	//================================================== MENUS

	Client::buildMenu(%Client, "Select a Chocobo\n"@$Chocobover, "PickedChocobo", true);

	%curItem = 0;
	for(%i = 1; $Chocobo[%Client, %i] != ""; %i++) {
		Client::addMenuItem(%Client, %curItem++ @ $ChocoboName[%Client, %i], %i);
		if(%curItem == 8)
			return;
	}
}

//Chocobo opts
function processMenuPickedChocobo(%Client, %opt) {

	dbecho($dbechoMode, "processMenuPickedChocobo(" @ %Client @ ", " @ %opt @ ")");

	if(%opt != "" && %opt != Page1 && %opt != Page2)
		$MenuChoco[%Client] = %opt; //Save the # of the Chocobo your looking at
	if($ChocoboName[%Client, $MenuChoco[%Client]] != "Free save slot") {
		if(%opt != "Page2") {       //The %opt is the Chocobo num not the page =]
			Client::buildMenu(%Client, $ChocoboName[%Client, $MenuChoco[%Client]], "ChocoboViewInfo", true);

			Client::addMenuItem(%Client, "1View Stats", "Stats");
			Client::addMenuItem(%Client, "2Feed", "Feed");
			if($ChocoboSpawn[%Client])
				Client::addMenuItem(%Client, "3Return Chocobo", "Return");
			else
				Client::addMenuItem(%Client, "3Ride Chocobo", "Play");
			Client::addMenuItem(%Client, "4Breed", "Breed");
			Client::addMenuItem(%Client, "5Sell", "Sell");
			Client::addMenuItem(%Client, "6Trade/Sell with other players", "Trade");
			Client::addMenuItem(%Client, "nNext>>", "Page2");
			Client::addMenuItem(%Client, "x<<Back", "Back");
		}
		if(%opt == "Page2") {
			Client::buildMenu(%Client, $ChocoboName[%Client, $MenuChoco[%Client]], "ChocoboViewInfo", true);

			Client::addMenuItem(%Client, "1Release Chocobo", "Release");
			Client::addMenuItem(%Client, "2Change Chocobo's name", "NameChange");
			Client::addMenuItem(%Client, "x<<Back", "Page1");
		}
	}
	else {
			Client::buildMenu(%Client, "Free save slot\n", "ChocoboViewInfo", true);
			Client::addMenuItem(%Client, "1No data saved", "Back");
			Client::addMenuItem(%Client, "x<<Back", "Back");
	}
}

function processMenuChocoboViewInfo(%Client, %opt) {

	dbecho($dbechoMode, "processMenuChocoboViewInfo(" @ %Client @ ", " @ %opt @ ")");

	%name = Client::getName(%Client);

	if(%opt == "Stats") {
		MenuChocoboStats(%Client, %opt);
		return;
	}
	else if(%opt == "Feed") {
		if($CanFeed[%Client] == true) {
			Client::buildMenu(%Client, "What will you feed your Chocobo?", "Feeding", true);

			if($Feeding[%Client] == Shop1) { //Changes these if you wanna Chee
				$Chocobo::tmpFoodCost[%Client] = 1000;
				$Chocobo::tmpFoodlvl[%Client] = 1;
				Client::addMenuItem(%Client, "1Seeds - 1000 gil", "Seeds|Seeds"); //These add stats to your Chocobo!
				Client::addMenuItem(%Client, "2Fruits - 1000 gil", "Fruits|Fruits");
				Client::addMenuItem(%Client, "3Candy - 1000 gil", "Candy|Candies");
				Client::addMenuItem(%Client, "4Hot dog - 1000 gil", "Hot dog|Meats");
				Client::addMenuItem(%Client, "5Vit A - 1000 gil", "Vit A|Vits");
				Client::addMenuItem(%Client, "6These cost a lot...", "cost");
				Client::addMenuItem(%Client, "x<<Back", "Back");
			}
			else if($Feeding[%Client] == Shop2) {
				$Chocobo::tmpFoodCost[%Client] = 10000;
				$Chocobo::tmpFoodlvl[%Client] = 2;
				Client::addMenuItem(%Client, "1Berrys - 10000 gil", "Berrys|Fruits");
				Client::addMenuItem(%Client, "2Pizza - 10000 gil", "Pizza|Meats");
				Client::addMenuItem(%Client, "3Vit B - 10000 gil", "Vit B|Vits");
				Client::addMenuItem(%Client, "4Blue Seeds - 10000 gil", "Blue Seeds|Seeds");
				Client::addMenuItem(%Client, "5Jam - 10000 gil", "Jam|Candies");
				Client::addMenuItem(%Client, "x<<Back", "Back");
			}
			else if($Feeding[%Client] == shop3) {
				$Chocobo::tmpFoodCost[%Client] = 20000;
				$Chocobo::tmpFoodlvl[%Client] = 3;
				Client::addMenuItem(%Client, "1Banana  - 20000 gil", "Banana|Fruits");
				Client::addMenuItem(%Client, "2Green Seeds - 20000 gil", "Green Seeds|Seeds");
				Client::addMenuItem(%Client, "3Steak - 20000 gil", "Steak|Meats");
				Client::addMenuItem(%Client, "4Vit B+ - 20000 gil", "Vit B+|Vits");
				Client::addMenuItem(%Client, "5Sugar cube - 20000 gil", "Sugar cube|Candies");
				Client::addMenuItem(%Client, "x<<Back", "Back");
			}
			else if($Feeding[%Client] == Shop4) {
				$Chocobo::tmpFoodCost[%Client] = 40000;
				$Chocobo::tmpFoodlvl[%Client] = 4;
				Client::addMenuItem(%Client, "1Blue Berrys  - 40000 gil", "Blue Berrys|Fruits");
				Client::addMenuItem(%Client, "2Yellow Seeds - 40000 gil", "Yellow Seeds|Seeds");
				Client::addMenuItem(%Client, "3Meat Balls - 40000 gil", "Meat Balls|Meats");
				Client::addMenuItem(%Client, "4Vit C - 40000 gil", "Vit C|Vits");
				Client::addMenuItem(%Client, "5Sugar Cookie - 40000 gil", "Sugar Cookie|Candies");
				Client::addMenuItem(%Client, "x<<Back", "Back");
			}
			else if($Feeding[%Client] == Shop5) {
				$Chocobo::tmpFoodCost[%Client] = 100000;
				$Chocobo::tmpFoodlvl[%Client] = 5;
				Client::addMenuItem(%Client, "1Black Berrys  - 100000 gil", "Black Berrys|Fruits"); //INT
				Client::addMenuItem(%Client, "2Black Seeds - 100000 gil", "Black Seeds|Seeds"); //WIS
				Client::addMenuItem(%Client, "3T-Bone Steak - 100000 gil", "T-Bone Steak|Meats"); //STR
				Client::addMenuItem(%Client, "4Vit C+ - 100000 gil", "Vit C+|Vits"); //CON
				Client::addMenuItem(%Client, "5Chocolate - 100000 gil", "Chocolate|Candies"); //DEX
				Client::addMenuItem(%Client, "x<<Back", "Back");
			}
			else if($Feeding[%Client] == Shop6) {
				$Chocobo::tmpFoodCost[%Client] = 1000000;
				$Chocobo::tmpFoodlvl[%Client] = 6;
				Client::addMenuItem(%Client, "1Cracker - 1000000 gil", "Cracker|Cracker"); //ALL STATS+
				Client::addMenuItem(%Client, "x<<Back", "Back");	    //We will have to find a gooood spot for him >=]
			}
		}
		else
			Client::sendMessage(%Client, 0, "Talk to a Chocobo trainer to feed your Chocobo!");

		return;
	}
	else if(%opt == "Play") {
		if($CanFeed[%Client] == true) {
			Client::sendMessage(%Client, 0, "You hop on your "@$ChocoboColor[%Client, $MenuChoco[%Client]]@" Chocobo "@$ChocoboName[%Client, $MenuChoco[%Client]]@".");
			%pos = GameBase::getposition(%Client);
			Chocobo::Spawn(%Client, $MenuChoco[%Client], %pos);
		}
		else {
			Client::sendMessage(%Client, 0, "Talk to a Chocobo trainer if you want to ride your Chocobo!");
			MenuChocobo(%Client);
		}
		return;
	}
	else if(%opt == "Return") {
		if($CanFeed[%Client] == true) {
			Client::setControlObject(%Client, %Client);
			Chocobo::Delete(%Client);
			MenuChocobo(%Client);
		}
		else {
			Client::sendMessage(%Client, 0, "Talk to a Chocobo trainer if you want to return your Chocobo!");
			MenuChocobo(%Client);
		}
		return;
	}
	else if(%opt == "Breed") {
		if($CanFeed[%Client] == true) {
			MenuBreeding(%Client);
			return;	//===
		}
		else {
			Client::sendMessage(%Client, 0, "Talk to a Chocobo trainer if you want to breed your Chocobo!");
			MenuChocobo(%Client);
			return;
		}
		return;
	}
	else if(%opt == "Sell") {
		if($CanFeed[%Client] == true) {
			Client::buildMenu(%Client, "Worth: $"@FixM($ChocoboWorth[%Client, $MenuChoco[%Client]]), "Selling", true);
			Client::addMenuItem(%Client, "1Sell", 1);
			Client::addMenuItem(%Client, "9Don't sell", 9);
		}
		else {
			Client::sendMessage(%Client, 0, "Talk to a Chocobo trainer to sell your Chocobo!");
			Client::buildMenu(%Client, "Your Chocobo is worth: $"@FixM($ChocoboWorth[%Client, $MenuChoco[%Client]]), "Selling", true);
			Client::addMenuItem(%Client, "x<<Back", 9);
		}
		return;
	}
	else if(%opt == "Trade") {
		Client::buildMenu(%Client, "Trade or Sell?", "Tradeing", true);

		Client::addMenuItem(%Client, "1Trade", "Trade");
		Client::addMenuItem(%Client, "2Sell", "Sell");

		Client::addMenuItem(%Client, "x<<Back", Back);
		return;
	}
	else if(%opt == "Release") {
		Client::buildMenu(%Client, "Release "@$ChocoboName[%Client, $MenuChoco[%Client]]@"?", "Releaseing", true);
		Client::addMenuItem(%Client, "1Yes", 1);
		Client::addMenuItem(%Client, "9No", 9);
		if($ChocoboSex[%Client, $MenuChoco[%Client]] == "Male")
			%char = "he";
		else	%char = "she";														//IF we make them able to attack =p
		Client::sendMessage(%Client, 1, "You are about to relase your Chocobo!"); //If you do, "@%char@"'ll fight you or anyone around!")
		return;
	}
	else if(%opt == "TakeCare") {
	//$ChocoboTakeCare[%Client, $MenuChoco[%Client]
	}
	else if(%opt == "NameChange")
		Chocobo::NewName(%Client, OldName, $MenuChoco[%Client]);
	else if(%opt == Page2)
		processMenuPickedChocobo(%Client, Page2);
	else if(%opt == "Back")
		MenuChocobo(%Client);
	else if(%opt == Page1)
		processMenuPickedChocobo(%Client, Page1);
}

function MenuChocoboStats(%Client, %opt) {

	Client::buildMenu(%Client, $ChocoboName[%Client, $MenuChoco[%Client]]@"'s stats:", "StatsInfo", true);

	if(%opt == Stats) {
		Client::addMenuItem(%Client, "1Years old: "@$ChocoboYAge[%Client, $MenuChoco[%Client]]@"", 1);
		Client::addMenuItem(%Client, "2Days old: "@$ChocoboDAge[%Client, $MenuChoco[%Client]]@"", 1);
		Client::addMenuItem(%Client, "3EXP: "@$ChocoboEXP[%Client, $MenuChoco[%Client]]@"", 4);
		Client::addMenuItem(%Client, "4Color: "@$ChocoboColor[%Client, $MenuChoco[%Client]]@"", 5);
		Client::addMenuItem(%Client, "5Health: "@$ChocoboHealth[%Client, $MenuChoco[%Client]]@"%", 7);
		Client::addMenuItem(%Client, "6Full: "@$ChocoboHungry[%Client, $MenuChoco[%Client]]@"%", 8);
		Client::addMenuItem(%Client, "7Worth: $"@FixM($ChocoboWorth[%Client, $MenuChoco[%Client]])@"", 0);

		Client::addMenuItem(%Client, "nNext>> ", "Next");
		return;
	}
	else if(%opt == Stats2) {

		Client::buildMenu(%Client, $ChocoboName[%Client, $MenuChoco[%Client]]@"'s stats:", "StatsInfo", true);

		Client::addMenuItem(%Client, "1STR [ "@$ChocoboSTR[%Client, $MenuChoco[%Client]]@" ]", 1);
		Client::addMenuItem(%Client, "2DEX [ "@$ChocoboDEX[%Client, $MenuChoco[%Client]]@" ]", 2);
		Client::addMenuItem(%Client, "3CON [ "@$ChocoboCON[%Client, $MenuChoco[%Client]]@" ]", 3);
		Client::addMenuItem(%Client, "4INT [ "@$ChocoboINT[%Client, $MenuChoco[%Client]]@" ]", 4);
		Client::addMenuItem(%Client, "5WIS [ "@$ChocoboWIS[%Client, $MenuChoco[%Client]]@" ]", 5);

		Client::addMenuItem(%Client, "b<<Back ", "Back");
		return;
	}
}

function processMenuStatsInfo(%Client, %opt) {

	dbecho($dbechoMode, "processMenuStatsInfo(" @ %Client @ ", " @ %opt @ ")");

	if(%opt == "Next")
		MenuChocoboStats(%Client, Stats2);
	else if(%opt == "Back")
		//MenuChocoboStats(%Client);
		processMenuPickedChocobo(%Client);
}

function processMenuTradeing(%Client, %opt) {

	dbecho($dbechoMode, "processMenuTradeing(" @ %Client @ ", " @ %opt @ ")");

	if(%opt == back) {
		processMenuPickedChocobo(%Client);
		return;
	}

	Client::buildMenu(%Client, %opt@" with?", "TradeOrSell", true);

	if($IsTradeing[%Client] == true) {
		if(%opt == trade)
			%opt = "start a new trade";
		Client::sendmessage(%Client, 0, "You can't "@%opt@" right now.");
		return;
	}
	$MenuTrade[%Client] = %opt; //Save the option the Client picked

	%index = 0;
	%char = "1234567890"; //Array

	%list = GetPlayerIdList();
	for(%i = 0; GetWord(%list, %i) != -1; %i++) {

		%cl = GetWord(%list, %i);
		if(%cl != %Client) {
			if($Chocobo[%cl] >= 1) {
				%name = Client::getName(%cl);
				//Client::getTeam(%cl);

				%choice = String::GetSubStr(%char, %index, 1);

				Client::addMenuItem(%Client, %choice @ %name, %cl);
				%index++;
				if(%index >= 10) //key list "1234567890" only has 10 slots
					return;
			}
		}
	}
}

function MenuBreeding(%Client) {

	Client::buildMenu(%Client, "Pick player", "PickPlayer::Breed", true);

	if(%opt == "Back") {
		processMenuPickedChocobo(%Client);
		return;
	}

	%index = 0;
	%char = "1234567890"; //Array

	%list = GetPlayerIdList();
	for(%i = 0; GetWord(%list, %i) != -1; %i++) {

		%cl = GetWord(%list, %i);
		if(%cl != %Client) {
			if($Chocobo[%cl] >= 1) {
				%name = Client::getName(%cl);

				%choice = String::GetSubStr(%char, %index, 1);

				Client::addMenuItem(%Client, %choice @ %name, %cl);
				%index++;
				if(%index >= 10) //key list "1234567890" only has 10 slots
					return;
			}
		}
	}
}

function processMenuPickPlayer::Breed(%Client, %opt) {

	dbecho($dbechoMode, "processMenuPickPlayer::Breed(" @ %Client @ ", " @ %opt @ ")");

	$MenuBreedBuyerId[%Client] = %opt; //Save buyers ID


	Client::buildMenu(%Client, "Pick Chocobo", "PickChoco::Breed", true);

 	%curItem = 0;
	for(%i = 1; $Chocobo[%opt, %i] != ""; %i++) {
		Client::addMenuItem(%Client, %curItem++ @ $ChocoboName[%opt, %i], %i);
		if(%curItem == 8)
			return;
	}
}

function processMenuPickChoco::Breed(%Client, %opt) {

	dbecho($dbechoMode, "processMenuPickChoco::Breed(" @ %Client @ ", " @ %opt @ ")");

	Client::buildMenu(%Client, "Breed?", "BreedChocobo", true);

	%Id = $MenuBreedBuyerId[%Client];
	$MenuBreedChocoboId::Buyer[%Client] = %opt;
	$MenuBreedChocoboId::Host[%Client] = $MenuChoco[%Client];

	Client::addMenuItem(%Client, "1Color: "@$ChocoboColor[%Id, %opt]@"", 1);
	Client::addMenuItem(%Client, "2Health: "@$ChocoboHealth[%Id, %opt]@"%", 2);
	Client::addMenuItem(%Client, "3Full: "@$ChocoboHungry[%Id, %opt]@"%", 3);
	Client::addMenuItem(%Client, "4Worth: $"@FixM($ChocoboWorth[%Id, %opt])@"", 4);

	Client::addMenuItem(%Client, "5Breed!", "Breed");
	Client::addMenuItem(%Client, "6Don't Breed", "Back");
}

function processMenuBreedChocobo(%Client, %opt) {

	dbecho($dbechoMode, "processMenuBreedkChocobo(" @ %Client @ ", " @ %opt @ ")");

	if(%opt == "Breed")
		Chocobo::Breed(%Client, $MenuBreedBuyerId[%Client], "Breed");
		//
	else if(%opt == "Back") {
		$MenuBreedBuyerId[%Client] = "";
		$MenuBreedChocoboId::Buyer[%Client] = "";
		$MenuBreedChocoboId::Host[%Client] = "";
		processMenuPickedChocobo(%Client);
	}
}
function processMenuTradeOrSell(%Client, %opt) {

	dbecho($dbechoMode, "processMenuTradeOrSell(" @ %Client @ ", " @ %opt @ ")");

	$MenuTradeBuyerId[%Client] = %opt; //Save buyers ID

	Client::buildMenu(%Client, "What Chocobo will you "@$MenuTrade[%Client]@"?", "PickedChocoboToTradeOrSell", true);

 	%curItem = 0;
	for(%i = 1; $Chocobo[%Client, %i] != ""; %i++) {
		Client::addMenuItem(%Client, %curItem++ @ $ChocoboName[%Client, %i], %i);
		if(%curItem == 8)
			return;
	}
}

function processMenuPickedChocoboToTradeOrSell(%Client, %opt) {

	dbecho($dbechoMode, "processMenuPickedChocoboToTradeOrSell(" @ %Client @ ", " @ %opt @ ")");

	$MenuTradeChocoboId::Host[%Client] = %opt;
	%id = $MenuTradeBuyerId[%Client];

	if($MenuTrade[%Client] == "Sell") {

		Client::buildMenu(%Client, "Sell?", "Sell", true);

		Client::addMenuItem(%Client, "1Color: "@$ChocoboColor[%id, %opt], 1);
		Client::addMenuItem(%Client, "2Health: "@$ChocoboHealth[%id, %opt]@"%", 2);
		Client::addMenuItem(%Client, "3Full: "@$ChocoboHungry[%id, %opt]@"%", 3);
		Client::addMenuItem(%Client, "4Worth: $"@FixM($ChocoboWorth[%id, %opt]), 4);

		Client::addMenuItem(%Client, "5Sell!", "Sell");
		Client::addMenuItem(%Client, "6Don't sell", "Quit");
	}
	else {
		Client::buildMenu(%Client, "Trade for?", "ViewTradeInfo", true);
		%BuyerId = $MenuTradeBuyerId[%Client];
 		%curItem = 0;
		for(%i = 1; $Chocobo[%BuyerId, %i] != ""; %i++) {
			Client::addMenuItem(%Client, %curItem++ @ $ChocoboName[%BuyerId, %i], %i);
			if(%curItem == 8)
				return;
		}
	}
}

function processMenuViewTradeInfo(%Client, %opt) {

	dbecho($dbechoMode, "processMenuViewTradeInfo(" @ %Client @ ", " @ %opt @ ")");

	Client::buildMenu(%Client, "Trade?", "TradeChocobo", true);

	%Id = $MenuTradeBuyerId[%Client];
	$MenuTradeChocoboId::Buyer[%Client] = %opt;

	Client::addMenuItem(%Client, "1Color: "@$ChocoboColor[%Id, %opt], 1);
	Client::addMenuItem(%Client, "2Health: "@$ChocoboHealth[%Id, %opt]@"%", 2);
	Client::addMenuItem(%Client, "3Full: "@$ChocoboHungry[%Id, %opt]@"%", 3);
	Client::addMenuItem(%Client, "4Worth: $"@FixM($ChocoboWorth[%Id, %opt]), 4);

	Client::addMenuItem(%Client, "5Trade!", "Trade");
	Client::addMenuItem(%Client, "6Don't Trade", "Back");
}

function processMenuTradeChocobo(%Client, %opt) {

	dbecho($dbechoMode, "processMenuTradeChocobo(" @ %Client @ ", " @ %opt @ ")");

	if(%opt == "Trade") {
		Chocobo::Trade(%Client, $MenuTradeBuyerId[%Client], Trade);
		return; //=====================================================================================
	}

	else if(%opt == "Back") {
		$MenuTradeChocoboId::Host[%Client] = "";
		$MenuTradeChocoboId::Buyer[%Client] = "";
		$MenuTradeBuyerId[%Client] = "";
		$MenuTrade[%Client] = "";
		processMenuPickedChocobo(%Client);
	}
}

function processMenuFeeding(%Client, %opt) {

	%Choco = $MenuChoco[%Client];

	%cost = $Chocobo::tmpFoodCost[%Client];
	%foodlvl = $Chocobo::tmpFoodlvl[%Client];
	$Chocobo::tmpFoodCost[%Client] = "";
	$Chocobo::tmpFoodlvl[%Client] = "";

	if(%opt == "cost") {
		Client::sendMessage(%Client, 2, "Chocobo trainer tells you, All our food is specially made just for Chocobos.");
		processMenuPickedChocobo(%Client);
		return;
	}
	else if(%opt == "Back") {
		processMenuPickedChocobo(%Client, %Choco);
		return;
	}

	if($COINS[%Client] >= %cost) {
		//Black Berrys|Fruits
		%pos = String::findSubStr(%opt, "|");
		%food = String::getSubStr(%opt, 0, %pos);
		%name = String::getSubStr(%opt, %pos+1, 9999);
		Chocobo::FoodType(%Client, %food, %foodlvl, %Choco, -%cost, %name);
	}
	else
		Client::sendMessage(%Client, 1, "You don't have enough coins!");




}

function Chocobo::Talk(%Client, %choco, %opt) {

	$CanFeed[%Client] = true;
	Schedule::add("Chocobo::Talk(%Client, %Choco, Clear);", 30, "ChocoTalk"@%Client);
	//Schedule::Cancel("ChocoTalk"@%Client);
	//Call this everytime you talk to a Trainer

	if(%opt == Clear)
		$CanFeed[%Client] = "";
}
function processMenuSelling(%Client, %opt) {

	dbecho($dbechoMode, "processMenuSelling(" @ %Client @ ", " @ %opt @ ")");

	if(%opt == 1) {
		$COINS[%Client] += $ChocoboWorth[%Client, $MenuChoco[%Client]];
		RefreshAll(%Client);
		Client::sendMessage(%Client, 1, "You sold "@$ChocoboName[%Client, $MenuChoco[%Client]]@" for "@FixM($ChocoboWorth[%Client, $MenuChoco[%Client]])@" coins.~wSoundMoney1.wav");
		Chocobo::Clear(%Client, $MenuChoco[%Client]);
	}
	else if(%opt == 9)
		  processMenuPickedChocobo(%Client);
}

function processMenuReleaseing(%Client, %opt) {

	dbecho($dbechoMode, "processMenuReleaseing(" @ %Client @ ", " @ %opt @ ")");

	if(%opt == 1) {
		Client::sendMessage(%Client, 1, "You released "@$ChocoboName[%Client, $MenuChoco[%Client]]@".");
		Chocobo::Clear(%Client, $MenuChoco[%Client]);
	}
	else if(%opt == 9)
		  processMenuPickedChocobo(%Client);
}

$MaxWorldChocobo = 12;
function Chocobo::Spawn(%Client, %Choco, %pos) {
	if($test == true) {
		%j = 1;
			$CSpawn[%j] = newObject("Fly","flier",Scout,true);
			if($CSpawn[%j]) {
				addToSet("MissionCleanup",$CSpawn[%j]);
				//GameBase::startFadeOut($CSpawn[%j]);
				GameBase::setPosition($CSpawn[%j],%pos);
			}
	return;
	}
	if($ChocoboSpawn[%Client]) {
		Client::sendMessage(%Client, 0, "You already have a Chocobo out!");
		return;
	}
	else {
		%armor = Chocobo::SetArmor(%Client, %Choco);
		if(%armor == None)
			return;
		echo("Spawning Chocobo #"@%Choco@" in %armor: "@%armor);
		$ChocoboSpawn[%Client] = newObject("Choco_"@%Client,"Player",%armor,true);
		if($ChocoboSpawn[%Client]) {
			addToSet("MissionCleanup",$ChocoboSpawn[%Client]);
			//GameBase::startFadeOut($ChocoboSpawn[%Client]);
			GameBase::setPosition($ChocoboSpawn[%Client],%pos);
			//%this = Client::getOwnedObject($ChocoboSpawn[%Client]); echo("Spawned %this: "@%this);
			$isChocobo[$ChocoboSpawn[%Client]] = true;
		}
		else
			echo("Error in Chocobo::Spawn.");
	}
}

function Chocobo::SetArmor(%Client, %Choco) {

	%Color = $ChocoboColor[%Client, %Choco];

	%STR = $ChocoboSTR[%Client, %Choco];
	%DEX = $ChocoboDEX[%Client, %Choco];
	%CON = $ChocoboCON[%Client, %Choco];
	%INT = $ChocoboINT[%Client, %Choco];
	%WIS = $ChocoboWIS[%Client, %Choco];

	%a = ChocoboArmor1;

	%Stats = %STR + %DEX + %CON + %INT + %WIS;
	if(%Color != Gold) {

		if(%Stats >= 0 && %Stats <= 250)
			%a = ChocoboArmor4;
		if(%Stats >= 551 && %Stats <= 1099)
			%a = ChocoboArmor5;
		if(%Stats >= 1100) // && %Stats <= 2099)
			%a = ChocoboArmor6;
		if(%DEX > %INT + %WIS) {
				%a = ChocoboArmorDex1;
			if(%Stats >= 500 && %DEX >= 100)
				%a = ChocoboArmorDex1;
			if(%Stats >= 2000 && %DEX >= 350)
				%a = ChocoboArmorDex2;
			if(%Stats >= 4000 && %DEX >= 750)
				%a = ChocoboArmorDex3;
			if(%Stats >= 5000 && %DEX >= 1500)
				%a = ChocoboArmorDex4;
			if(%Stats >= 9000 && %DEX >= 3000)
				%a = ChocoboArmorDex5;
		}
	}
	else if(%Color == Gold) {
		if(%Stats >= 0 && %Stats <= 250)
			%a = ChocoboArmor4;
		if(%Stats >= 551 && %Stats <= 1099)
			%a = ChocoboArmor5;
		if(%Stats >= 1100) //&& %Stats <= 2099)
			%a = ChocoboArmor6;
		if(%DEX > %INT + %WIS) {
				%a = ChocoboArmorDex1;
			if(%Stats >= 500 && %DEX >= 100)
				%a = ChocoboArmorDex1;
			if(%Stats >= 2000 && %DEX >= 350)
				%a = ChocoboArmorDex2;
			if(%Stats >= 4000 && %DEX >= 750)
				%a = ChocoboArmorDex3;
			if(%Stats >= 5000 && %DEX >= 1500)
				%a = ChocoboArmorDex4;
			if(%Stats >= 9000 && %DEX >= 3000)
				%a = ChocoboArmorDex5;
		}
		if(%INT + %WIS > %DEX) {
				%a = ChocoboArmorfly1;
			if(%Stats >= 400 && %INT >= 200 && %WIS >= 200)
				%a = ChocoboArmorfly1;
			if(%Stats >= 1000 && %INT >= 300 && %WIS >= 200)
				%a = ChocoboArmorfly2;
			if(%Stats >= 2000 && %INT >= 350 && %WIS >= 350)
				%a = ChocoboArmorfly3;
			if(%Stats >= 4000 && %INT >= 500 && %WIS >= 400)
				%a = ChocoboArmorfly4;
			if(%Stats >= 5000 && %INT >= 850 && %WIS >= 850)
				%a = ChocoboArmorfly5;
		}
		if(%Stats >= 10000)
			%a = ChocoboArmorSuper;
	}
	if($ChocoboHealth[%Client, %Choco] <= 50 || $ChocoboHungry[%Client, %Choco] <= 50)
		%a = ChocoboArmor3;
	if($ChocoboHealth[%Client, %Choco] <= 40 || $ChocoboHungry[%Client, %Choco] <= 40)
		%a = ChocoboArmor2;
	if($ChocoboHealth[%Client, %Choco] <= 30 || $ChocoboHungry[%Client, %Choco] <= 30)
		%a = ChocoboArmor1;
	if($ChocoboHealth[%Client, %Choco] <= 20 || $ChocoboHungry[%Client, %Choco] <= 20) {
		Client::sendMessage(%Client, 1, "Chocobo "@$ChocoboName[%Client, %Choco]@" won't let you on!");
		%a = None;
	}
	return %a;
}

function Chocobo::Delete(%Client) {

	$isChocobo[$ChocoboSpawn[%Client]] = "";
	deleteObject($ChocoboSpawn[%Client]);
	$ChocoboSpawn[%Client] = "";
}

function Armor::onCollision(%this, %object) {

	if($isChocobo[%this] == "isUsed")
		return;

	%armor = Player::getArmor(%this);

//	if(String::findSubStr(%armor, "ChocoboArmor") != -1)
//		$isChocobo[%this] = true;

//echo("Is it a Choco? "@$isChocobo[%this]);
	if(!$isChocobo[%this]) {
		Client::Bumped(%this, %object);
		return;
	}

	%Client = Player::getClient(%object);
//	%ChocoId = Player::getClient(%this); //Id -1

	if(%object.driver != 1) {

echo("Can mount PASS %this "@%this);

		%weapon = Player::getMountedItem(%object,$WeaponSlot);
		if(%weapon != -1) {
			%object.lastWeapon = %weapon;
			Player::unMountItem(%object,$WeaponSlot);
		}
		Player::setMountObject(%object, %this, 1);
		Client::setControlObject(%Client, %this);
		//playSound(GameBase::getDataName(%this).mountSound, GameBase::getPosition(%this));

		%object.driver = 1;
		Chocobo::Theme(%Client, %object);
		$isChocobo[%this] = "isUsed";
		$OwnersChocoboThis[%Client] = %this;

		//%a = Client::getGender(%Client);
		//Player::setArmor(%Client, %a);

		%object.vehicle = %this;
		%this.clLastMount = %Client;
	}
//else
//echo("Can mount FAILED %this "@%this);
}

function Armor::jump(%this, %mom)
{
	// The jump callback may fire with %this = the controlled chocobo OR the mounted
	// RIDER (players and chocobos share className "Armor"). Handle both; anyone else
	// falls through to the stock vehicle-dismount handling in Player::jump.
	if($isChocobo[%this] == "isUsed") {
		Armor::dismount(%this, %mom);	//%this IS the ridden chocobo
		return;
	}
	%mnt = Player::getMountObject(%this);
	if($isChocobo[%mnt] == "isUsed") {
		Armor::dismount(%mnt, %mom);	//%this is the RIDER; dismount via its chocobo
		return;
	}
	Player::jump(%this, %mom);
}

function Armor::dismount(%this, %mom) {
	%cl = %this.clLastMount;	//the rider's client, stored on mount (Armor::onCollision)
   if(%cl != -1 && %cl != "")
   {
      %pl = Client::getOwnedObject(%cl);
      if(getObjectType(%pl) == "Player")
      {
		   // dismount the player
			if(GameBase::testPosition(%pl, Vehicle::getMountPoint(%this,0))) {
				%pl.lastMount = %this;
				%pl.newMountTime = getSimTime() + 3.0;
				Player::setMountObject(%pl, %this, 0);
        	 	Player::setMountObject(%pl, -1, 0);
				%rot = GameBase::getRotation(%this);
				%rotZ = getWord(%rot,2);
				GameBase::setRotation(%pl, "0 0 " @ %rotZ);
				Player::applyImpulse(%pl,%mom);
        	 	Client::setControlObject(%cl, %pl);
				playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));
				if(%pl.lastWeapon != "") {
					Player::useItem(%pl,%pl.lastWeapon);
					%pl.lastWeapon = "";
      		}
				%pl.driver = "";
				%pl.vehicle = "";
				if($ChocoboSpawn[%cl] == %this)
					Chocobo::Delete(%cl);	//owner hopped off: stable the bird (menu "Return" cleanup)
				else {
					$isChocobo[%this] = true;	//not the rider's bird: leave it standing, free to mount again
					%this.clLastMount = "";
				}
			}
			else
				Client::sendMessage(%cl,0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
		}
   }
}