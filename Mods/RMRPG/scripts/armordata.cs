$spdlower = 3;
$spdlow = 4;
$spdlowmed = 5;
$spdmed = 6;
$spdmedhigh = 7;
$spdhigh = 8;
$spdhigher = 9;

$spdgm = 50;

//damage skin data

DamageSkinData armorDamageSkins
{
   bmpName[0] = "dskin1_armor";
   bmpName[1] = "dskin2_armor";
   bmpName[2] = "dskin3_armor";
   bmpName[3] = "dskin4_armor";
   bmpName[4] = "dskin5_armor";
   bmpName[5] = "dskin6_armor";
   bmpName[6] = "dskin7_armor";
   bmpName[7] = "dskin8_armor";
   bmpName[8] = "dskin9_armor";
   bmpName[9] = "dskin10_armor";
};

exec("HumanArmors.cs");

exec("EnemyArmors.cs");
//LoadEnemyArmorData();

exec("SpecialArmors.cs");
