
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//
//  Repair Kit (RepairKit)
//  By Dynamix modified by Alazane, see Contrib.txt
//
//  For installation information, see Install.txt
//
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//  Depends on:
//    40k armor being used (onRepairKit support)
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

$InvList[RepairKit] = 1;
$RemoteInvList[RepairKit] = 1;
$AutoUse[RepairKit] = false;

addAmmo(Misc, RepairKit, 1);


ItemData RepairKit 
{
  description = "Repair Kit";
  shapeFile = "armorKit";
  heading = $InvHead[ihMis];
  shadowDetailMask = 4;
  price = 2;
};

function RepairKit::onUse(%player,%item) 
{
  Player::decItemCount(%player,%item);
  GameBase::repairDamage(%player,0.2);
  %c = Player::getClient(%player);
   // Alliance armor support
  %armor = Player::getArmor(%player);
  eval(%armor @ "::onRepairKit(" @ %player @ ");");
//Sniper Leg Hit Effect Removal
  Player::decItemCount(%this, DeadWeight);
}
