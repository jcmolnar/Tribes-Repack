function GroupTrigger::onEnter(%this, %object)
{
%client = Player::getClient(%object);
	if(%this.num == "1A"){
      %positionIn = "75.6476 -5.92959 162.717";
      %positionOut = "41.118 -35.2424 151.845";
   }
      else if(%this.num == "Main2"){
      %positionIn = "646.831 233.683 76.1779";
      %positionOut = "-5.61334 1694.48 1471.42";
   }
   else if(%this.num == "Main3"){
      %positionIn = "-17.8541 1819.83 1483.64";
      %positionOut = "-22.3762 1819.84 1483.47";
   }

  	if(%this.in){ 
         GameBase::setPosition(%client, %positionIn);
         //messageAll(0, "~wshieldhit.wav");
	   Client::SendMessage(%client,0,"~wshieldhit.wav");
      }
      	else if(%this.out){
         GameBase::setPosition(%client, %positionOut);
         //messageAll(0, "~wshieldhit.wav");
         Client::SendMessage(%client,0,"~wshieldhit.wav");
	}
 
} 


