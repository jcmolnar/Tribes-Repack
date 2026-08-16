//FGMasterList::setSelected
//TextList::setSelected(RecordingsTextList, %newName);


//Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
//Observer::setOrbitObject(%clientId, Client::getOwnedObject(%id), -3, -3, -3);


function tt($skill) { if($skill == "") return;
	echo("==================");
	for(%i = 1; %i <= 99; %i++) {
		// %hp = %hp@" | "@%i@" \""@floor(%i / $skill)+5@"\" "@floor(((%i / $skill) + 5) * %i) * 2;
		%hp = %hp@" | "@%i@" \""@floor(%i / $skill)+5@"\" ";
		if(%i == 25 || %i == 50 || %i == 75 || %i == 99) {
			echo(%hp); echo(""); %hp = "";
		}
	}
	echo("SKILL: "@$skill);

}

//HP TEST
function thp($skill) { if($skill == "") return;
	echo("==================");
	for(%i = 1; %i <= 99; %i++) {
		// %hp = %hp@" | "@%i@" \""@floor(%i / $skill)+5@"\" "@floor(((%i / $skill) + 5) * %i) * 2;
		%vit = floor(%i / $skill)+5;
		%hp = %hp@" | "@%i@" \""@floor(%i / $skill)+5@"\" "@floor(((%i * %vit) + 5) * 2); //%i) * 2;
		if(%i == 25 || %i == 50 || %i == 75 || %i == 99) {
			echo(%hp); echo(""); %hp = "";
		}
	}
	echo("SKILL: "@$skill);
}

//MP TEST
function tmp($skill) { if($skill == "") return;
	echo("==================");
	for(%i = 1; %i <= 99; %i++) {
		// %mp = %mp@" | "@%i@" \""@floor(%i / $skill)+5@"\" "@floor(((%i / $skill) + 5) * (%i / 4));
		%int = floor(%i / $skill)+5;
		%mp = %mp@" | "@%i@" \""@floor(%i / $skill)+5@"\" "@floor(((%int) + 5) * (%i / 5));
		if(%i == 25 || %i == 50 || %i == 75 || %i == 99) {
			echo(%mp); echo(""); %mp = "";
		}//74 999+
	}
	echo("SKILL: "@$skill);
}

//STR TEST
function ts($skill, %atkpow) { if($skill == "") return;
	echo("==================");
	for(%i = 1; %i <= 99; %i++) {									//ATTACK POWER
		// %ts = %ts@" | "@%i@" \""@floor(%i / $skill)+5@"\" "@floor(((%i / $skill) + 1) * %i);
		%atk = floor(%i / $skill)+5;
		%ts = %ts@" | "@%i@" \""@floor(%i / $skill)+5@"\" "@floor(((%atk) + %atkpow) * (%i / 1.075));
		if(%i == 25 || %i == 50 || %i == 75 || %i == 99) {
			echo(%ts); echo(""); %ts = "";
		}
	}
	echo("SKILL: "@$skill);
}

//STR 2 TEST
function ts2($skill, %opt) { if($skill == "") return;
	echo("==================");
	for(%i = 1; %i <= 99; %i++) {		//ATTACK POWER
		if(%opt) {
			%atk = floor(%i / $skill)+5;
			%ts = %ts@" | "@%i@" \""@floor(%i / $skill)+5@"\" "@floor(((%atk)) * (%i / 2.075));
		}
		else
			%ts = %ts@" | "@%i@" - "@floor(((%i)) * (%i / 4.075));
		if(%i == 25 || %i == 50 || %i == 75 || %i == 99) {
			echo(%ts); echo(""); %ts = "";
		}
	}
	echo("SKILL: "@$skill);
}

//AGI TEST
function ta($skill) { if($skill == "") return;
	echo("==================");
	for(%i = 1; %i <= 99; %i++) {									//SPEED
		// %mp = %mp@" | "@%i@" \""@floor(%i / $skill)+5@"\" "@floor(((%i / $skill) + 1) * %i);
		%agi = floor(%i / $skill)+5;
		%ta = %ta@" | "@%i@" \""@floor(%i / $skill)+5@"\" "@floor(((%i / %agi) + 1) * %i);//FIX!!!!!!!
		if(%i == 25 || %i == 50 || %i == 75 || %i == 99) {
			echo(%ta); echo(""); %ta = "";
		}
	}
	echo("SKILL: "@$skill);
}

//SPI TEST
function tsp($skill) { if($skill == "") return;
	echo("==================");
	for(%i = 1; %i <= 99; %i++) {									//FOR SPELL DAMAGE/HEAL
		%mp = %mp@" | "@%i@" \""@floor(%i / $skill)+5@"\" "@floor(((%i / $skill) + 1) * %i);
		if(%i == 25 || %i == 50 || %i == 75 || %i == 99) {
			echo(%mp); echo(""); %mp = "";
		}
	}
	echo("SKILL: "@$skill);
}

