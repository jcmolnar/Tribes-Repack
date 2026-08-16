
//Booga Quest
$QuestData::[0] = "BoogaQuest";
$QuestData::["BoogaQuest", title] = "Booga Quest";
$QuestData::["BoogaQuest", Pic] = "txt";
$QuestData::["BoogaQuest", Startlog] = "Mayor Alice asks of you to bring the <f1>Cure Potion<f0> to Booga, who lives near the <f1>Goblin Pass<f0>.";
$QuestData::["BoogaQuest", Finishlog] = "After giving Booga the <f1>Cure Potion<f0> he gives you a <f1>Booga Nut<f0>.";
$QuestData::["BoogaQuest", Failed] = "NA"; //Means cannot fail this quest. Can finish it anytime...


//Fredrick Quest
$QuestData::[1] = "FredrickQuest";
$QuestData::["FredrickQuest", title] = "Fredrick Quest";
$QuestData::["FredrickQuest", Pic] = "txt";
$QuestData::["FredrickQuest", time] = 60*3;//3 mins
$QuestData::["FredrickQuest", Startlog] = "Give Fredrick the bag of acrons before times up!";
$QuestData::["FredrickQuest", Finishlog] = "Fredrick takes the acrons that you have given him and asks you to see him later...";
$QuestData::["FredrickQuest", Failed] = "You failed to give Fredrick the bag of acrons!";

//Wika Quest
$QuestData::[3] = "WikaQuest";
$QuestData::["WikaQuest", title] = "Wika Quest";
$QuestData::["WikaQuest", Pic] = "txt";
$QuestData::["WikaQuest", Startlog] = "Wika who lives in lower shildrik needs <f1>Special Shoes<f0> you can a pair by giving Ed in Shildrik 10 playing cards.";
$QuestData::["WikaQuest", Finishlog] = "After giving Wika the <f1>Special Shoes<f0> she gives you a <f1>Mid Bag<f0>.";
$QuestData::["WikaQuest", Failed] = "NA"; //Means cannot fail this quest. Can finish it anytime...





function NewQuest(%Client, %quest) {

	remoteEval(%Client, "QuestText", $QuestData::[%quest, Pic], false);
	if($QuestData::[%quest, time] != "")
		UpdateBonusState(%Client, %quest, floor($QuestData::[%quest, time]/2)+1);

	Schedule("%Client.guiLock = \"\";", 8);
	$ClientData[%Client, %quest] = "started";
}

function FailedQuest(%Client, %quest) {
	Client::sendMessage(%Client, 1, "You failed to complete "@$QuestData::[%quest, title]@"!");
	$ClientData[%Client, %quest] = "failed";
}

function EndQuest(%Client, %quest) {

	remoteEval(%Client, "QuestText", $QuestData::[%quest, Pic], true);
	$ClientData[%Client, %quest] = "done";
	Schedule("%Client.guiLock = \"\";", 8);
}