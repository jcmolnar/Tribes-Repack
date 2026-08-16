// To find what key is what
//	KeyId ObjId NumOfKeys

// $KeyList[2049] = $KeyList[2049]@" 2001 TestDoor 1";
// echo($keyList[2049]);
// Keys(2049, 2001, TestDoor, new);
function Keys(%ClientId, %keyId, %ObjId, %opts) {

	if(%opts == new) {
		$KeyList[%clientId] = $KeyList[%ClientId]@" "@%keyId@" "@%ObjId@" 1";
		return;
	}
	else if(%opts == take) {

		for(%i = 0; GetWord($KeyList[%ClientId], %i) != -1; %i+3) {

			%keyId = GetWord($KeyList[%ClientId], %i); //Owner of key Id
			%ObjId2 = GetWord( $KeyList[%ClientId], %i++ ); //Obj Id

			if(%ObjId == %ObjId2) { //Find right key to remove keys

				%keys = GetWord($KeyList[%ClientId], %i++); //keys owned already
				break;
			}
			if(%i > 1000) // just in case...
				break;
		}
		%str = %keyId@" "@%ObjId2@" "@%keys@" ";
		$KeyList[%ClientId] = String::replace($KeyList[%ClientId], %str, "");
		KeyTrim(%ClientId);
		return;
	}
	else {
		for(%i = 0; GetWord($KeyList[%ClientId], %i) != -1; %i+3) {

			%keyId = GetWord($KeyList[%ClientId], %i); //Owner of key Id
			%ObjId2 = GetWord( $KeyList[%ClientId], %i++ ); //Obj Id

			if(%ObjId == %ObjId2) { //Find right key to add more keys too

				%keys = GetWord( $KeyList[%ClientId], %i++); //keys owned already
				break;
			}
			if(%i > 1000) // just in case...
				break;
		}

		%str = %ObjId2@" "@%keys;
		%replace = %ObjId2@" "@%keys + %opts;
		$KeyList[%ClientId] = String::replace($KeyList[%ClientId], %str, %replace); //Add new key(s)
	}
}

function CheckKey(%ClientId, %KeyId, %ObjId, %ObjId2) {

	if(%ObjId == %ObjId2)
		Client::sendMessage(%ClientId, 0, "You opened "@%ObjId2);
		//Do something here...
	return;
}

function KeyTrim(%ClientId) {
	$KeyList[%ClientId] = String::NEWgetSubStr($KeyList[%ClientId], 0, String::len($KeyList[%ClientId]));
}
