//Deus_ex_Machina
//
//

function getSTA(%ClientId) {
	return $STA[%ClientId];
}

function getSTAMINA(%clientId) {

	%a = $AP[%ClientId, 6];
	%b = $AP[%ClientId, 2];
	%c = getFinalLVL(%ClientId);

	%d = (%a / 4) + (%b / 10) + %c;
	%e = round(%d);

	%sta = Cap(%e, 1, 999);

	$MaxSTA[%ClientId] = %sta;

	return %sta;
}

function setSTAMINA(%ClientId, %val) {

	$STA[%ClientId] += %val;

	$STA[%ClientId] = Cap($STA[%ClientId], 0, $MaxSTA[%ClientId]);
	$staticker[%clientId]++;
	if($staticker[%clientId] >= 3) {
		$staticker[%clientId] = 0;
		remoteEval(%ClientId, "RefreshSTAMPset", Fix($STA[%ClientId], %ClientId, STA), Fix(getHP(%clientId), %ClientId, HP), Fix(getMANA(%clientId), %ClientId, MP));
	}
}

function refreshSTAMINA(%ClientId, %val) {
	setSTAMINA(%clientId, (-%val));
}

function refreshSTAREGEN(%ClientId) {

//	if(%Clientid.sleepMode != "")
//		%a = getFinalLVL(%ClientId)/4;
//	else
		%a = 0.05 + (getFinalLVL(%ClientId) / 75); // 500

	%b = AddPoints(%ClientId, 9) / 100;
	if(%b <= 0)
		%b = 0;
	%c = (%a + %b) * 2; //Updates every zone check (2 sec)

	$STAREGEN[%clientId] = %c;
}