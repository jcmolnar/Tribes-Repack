// Mental Trousers 10 Aug 2000
//---------------------------------------------------------------------------
// clientMessage.cs
//
// This script provides exceptionally fast parsing of the incoming client
// messages.
//
// Normally, scripts would attach to the eventClientMessage and use
// match::string, match::paramString, string::findSubStr etc to look for
// a matching piece of text. Each one of those functions start at the first
// character of the incoming message and step through it one character at a
// time. If there are 50 match::string's looking for 50 different pieces of
// text, then the incoming message is parsed 50 times.
//
// This script does a single character by character pass through the incoming
// message, triggering functions provided by other scripts as it finds a
// match.
//
// msg::onMatch (%text, %func, %wildcard)
//       %text is the text to be matched.
//       %func is the function to be called OR string to be evaluated when
//         a match for text is found.
//       %wildcard is the wildcard character to use. If this is "" then
//         the * is used as the wildcard.
//         Also, if a string is provided rather than a function name, the
//         client and message can be passed by using %client and %msg as
//         function parameters in the string.
//
// msg::remove (%text, %func)
//         this is the function that scripts can use to remove or detach
//         from the parser. The %text and %func must be an exact match for
//         the parameters provided to msg::onMatch or this will fail.
//         returns true if successful, false on fail.
//
// NOTE: To mute the incoming client message, trigger the eventMuteClientMessage
// ie event::trigger(eventMuteClientMessage);
// and that will mute the message. This also makes it easy to pick up whether
// the message is muted by another script
//---------------------------------------------------------------------------

function msg::onParamMatch (%text, %func, %wildcard, %p1, %p2, %p3)
{
	$debugThisShit=true;
	if (%text=="" || %func=="")
		return;

	if (%wildcard=="")
		%wildcard="%";

	%text=string::replace (%text, "*", %wildcard@"0");
	%count=string::occurs(%text, %wildcard);
	if (string::findSubStr (%text, %wildcard@"0")!=-1)
		%count=%count-string::occurs (%text, %wildcard@"0");

	//I have no idea why, but heaps of stuff
	//calls this twice. It's weird.
	if ($safety::[%text, %func, %wildcard])
		return;

	$safety::[%text, %func, %wildcard]=true;

	%len=string::len(%text);
	%ind=0;
	%multiple=false;

	%str=%text;
	%index=0;
	$msg::start[%text, %func]=0;
	%ind=string::findSubStr(%str, %wildcard);

	if (string::getSubStr (%text, 0, 1)=="%")
		%atStart=true;

	if (string::getSubStr (%text, string::len(%text)-2, 1)=="%")
		%last=string::len()-2;

	for (%i=0; %ind!=-1; %i++)
		{
		%index=string::getSubStr (%str, %ind+1, 1);
		%str=string::getSubStr (%str, %ind+2, 999);
		%ind=string::findSubStr(%str, %wildcard);
		if (!%atStart)
			{
			%find=string::getSubStr (%str, 0, %ind);
			msg::onMatch (%find, "msg::extractParam (%msg, \""@%text@"\", \""@%func@"\", "@%index@");");
			echo ("Find (not last): '"@%find@"'");
			}
		if (%ind==-1)
			{
			%find=string::getSubStr (%str, 0, string::len(%str));
			msg::onMatch (%find, "msg::extractParamRemaining (%msg, \""@%text@"\", "@%ind@", \""@%func@"\", "@%index@");");
			echo ("Find (last not at end): '"@%find@"'");
			}
		else if (%index==%last)
			{
			%find=string::getSubStr (%str, 0, string::len(%str)-2);
			msg::onMatch (%find, "msg::extractParam (%msg, \""@%text@"\", \""@%func@"\", "@%index@");");
			echo ("Find (at end): '"@%find@"'");
			}
		%atStart=false;
		}
	msg::onMatch (%str, "msg::passToFunction (%msg, \""@%text@"\", \""@%func@"\", "@%count@", \""@%p1@"\", \""@%p2@"\", \""@%p3@"\");");
	return;
}

function msg::extractParam (%msg, %text, %len, %func, %ind)
{
	if (%ind==0)
		return;
//	echo ("currentPos: "@$msg::currentPos@" $msg::start: "@$msg::start[%text, %func]@" %len: "@%len);
	$msg[%text, %func, %ind]=string::getSubStr (%msg, $msg::currentPos-%msg::start[%text, %func], $msg::currentPos-$msg::start[%text, %func]-%len);
	echo ("extracted: '"@$msg[%text, %func, %ind]@"' start: "@$msg::currentPos-%msg::start[%text, %func]@" len: "@$msg::currentPos-$msg::start[%text, %func]-%len);
	$msg::start[%text, %func]=$msg::currentPos;
	return;
}

function msg::extractParamRemaining (%msg, %text, %func, %ind)
{
	if (%ind==0)
		return;
	$msg[%text, %func, %ind]=string::getSubStr (%msg, $msg::currentPos-$msg::start[%text, %func], 999);
	$msg::start[%text, %func]=0;
//	echo ("extracted: '"@$msg[%text, %func, %ind]@"'");
	return;
}

function msg::passToFunction(%msg, %text, %func, %count, %p1, %p2, %p3)
{
//	echo (%func@" "@%count@" "@%p1@" "@%p2@" "@%p3);
	%temp=%func@"(\""@%msg@"\"";
	$msg::start[%text, %func]=0;
	for (%i=1;%i<=%count;%i++)
		{
		echo ("parameter "@%i@": "@$msg[%text, %func, %i]);
		%temp=%temp@", \""@$msg[%text, %func, %i]@"\"";
		}
	%temp=%temp@");";
	eval(%temp);
	deleteVariables ("$msg"@%text@"_"@%func@"*");
	return;
}

function msg::onMatch (%text, %func, %wildcard)
{
	if (%text=="" || %func=="")
		return;

//	if ($debugThisShit)
//		echo ("%text: '"@%text@"' %func: '"@%func);
	if (%wildcard=="")
		%wildcard="*";

	%string=%func;
	if (string::findSubStr (%func, ";")==-1)
		%string=%string@"(%client, %msg);";

	%chars="abcdefghijklmnopqrstuvwxyz!@#$%^&()_+-=,./<>?|;:[]{} `~\\*";
	%len=String::len(%text);
	%node=0;
	for (%i=0; %i<string::len (%text); %i++)
		{
		%thisLetter=string::getSubStr (%text, %i, 1);

		if ($msg::[%node, %thisLetter]=="" || %thisLetter=="*")
			{
			%newNode=msg::newNode();
			$msg::[%node, %thisLetter]=%newNode;
			if (%thisLetter=="*")
				{
				for (%j=0; %j<string::len(%chars); %j++)
					{
					%temp=string::getSubStr (%chars, %j, 1);
					if (%i<%len-1)
						{
						if (%node==0)
							$msg::[%node, %temp, "string"]=$msg::[%node, %temp, "string"]@"msg::eval ("@%newNode@", msg::letter());";
						else
							$msg::[%node, %temp, "string"]=$msg::[%node, %temp, "string"]@"msg::eval ("@%newNode@", msg::letter(), "@%node@");";
						if ($msg::[%node, %temp]=="")
							$msg::[%node, %temp]=%newNode;
						}
					else
						{
						$msg::[%node, %temp, "string"]=$msg::[%node, %temp, "string"]@%string;
						$msg::final::[%text]=%node;
						}
					}
				}
			else
				{
				if (%i<%len-1)
					$msg::[%node, %thisLetter, "string"]=$msg::[%node, %thisLetter, "string"]@"msg::eval ("@%newNode@", msg::letter());";
				else
					{
					$msg::[%node, %thisLetter, "string"]=$msg::[%node, %thisLetter, "string"]@%string;
					$msg::final::[%text]=%node;
					}
				}
			}
		%oldNode=%node;
		%node=$msg::[%node, %thisLetter];
		}
	return;
}

function msg::remove (%text, %func)
{
	%string=%func;
	if (string::findSubStr (%string, ";")==-1)
		%string=%string@"(%client, %msg);";

	%letter=string::getSubStr (%text, string::len(%text)-1, 1);
	%node=$msg::final::[%text];
	%nodeStr=$msg::[%node, %letter, "string"];
	$msg::[%node, %letter, "string"]=string::replace (%nodeStr, %string, "");
	return (%nodeStr!=$msg::[%node, %letter, "string"]);
}

function msg::incoming (%client, %msg)
{
	event::trigger (eventClientMessagePreParse);
	%len=string::len(%msg);
	$eval_="msg::eval(0, msg::letter());";
	for (%i=0;%i<%len;%i++)
		{
		$msg::currentPos=%i;
		$msg::letter=string::getSubStr (%msg, %i, 1);
		%eval_=$eval_;
		$eval_="msg::eval(0, msg::letter());";
		eval (%eval_);
		}
	%eval_=$eval_;
	eval (%eval_);
	event::trigger (eventClientMessagePostParse);
	if (event::returnedAndClear(eventMuteClientMessage, "mute"))
		return "mute";
	else
		return "true";
}

function msg::eval (%node, %letter, %repeat)
{
	$eval_=$eval_@$msg::[%node, %letter, "string"];
	if (%repeat)
		$eval_=$eval_@"msg::eval ("@%node@", msg::letter(), "@%repeat@");";
}

function msg::letter()
{
	return $msg::letter;
}

function msg::newNode()
{
	return $msg::nextNode++;
}

event::attach (eventClientMessage, msg::incoming);
event::attach (eventMuteClientMessage, "return mute;"); //special event. To mute the client message, just 'event::trigger(eventMuteClientMessage);'
