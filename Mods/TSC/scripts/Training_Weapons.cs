exec("game.cs");

function fixedTargetsBlaster(%playerId)
{
   schedule("bottomprint(" @ %playerId @ ", \"<f2><jc>One <f1>Track.\", 2);", 5);
   schedule("messageAll(0, \"~wturretFire1.wav\");", 5);
   
   schedule("bottomprint(" @ %playerId @ ", \"<f2><jc>One <f1>Board.\", 2);", 7);
   schedule("messageAll(0, \"~wshell_click.wav\");", 7);
   
   schedule("bottomprint(" @ %playerId @ ", \"<f2><jc>One Hundred <f1>And <f2>Eighty <f1>Miles An Hour.\", 2);", 9);
   schedule("messageAll(0, \"~wshell_click.wav\");", 9);
   
   schedule("bottomprint(" @ %playerId @ ", \"<f1><jc>And Not A Single Hint Of <f2>Fear.\", 2);", 11);
   schedule("messageAll(0, \"~wshell_click.wav\");", 11);
   
   schedule("bottomprint(" @ %playerId @ ", \"<f1><jc>Welcome To <f2>Snow Circuit: Tribes.\", 5);", 13);
   schedule("messageAll(0, \"~wdebris_large.wav\");", 13);

   schedule("bottomprint(" @ %playerId @ ", \"<f1><jc>Let The Games Begin.\", 5);", 18);
   schedule("messageAll(0, \"~wfall_scream.wav\");", 18);  
}