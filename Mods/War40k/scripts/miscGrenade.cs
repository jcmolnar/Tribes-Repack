
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Grenade (Grenade)
//  By Dynamix modified by Alazane, see Contrib.txt
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//  Depends on:
//    Only Alliance armor being used (onGrenade support)
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[Grenade] = 1;
$RemoteInvList[Grenade] = 1;
$SellAmmo[Grenade] = 5;

addAmmo(Misc, Grenade, 2);

ItemData Grenade 
{
  description = "Grenade";
  shapeFile = "grenade";
  heading = $InvHead[ihMis];
  shadowDetailMask = 4;
  price = 2;
  className = "HandAmmo";
};

function Grenade::onUse(%player,%item) 
{
  if ($matchStarted && %player.throwTime < getSimTime()) 
  {
    Player::decItemCount(%player,%item);
    %armor = Player::getArmor(%player);
    eval(%armor @ "::onGrenade(" @ %player @ ");");
  }
}
