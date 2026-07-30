Event::Attach( eventLeaveServer, DemoNamer::onLeaveServer );

function DemoNamer::onLeaveServer() {
	
	$recorderFileName = "";
}

function setupRecorderFile() {
	
	%str = AutoName::StripString( "(" ~ timestamp::format() ~ ")-" ~ $PCFG::Name );
	$recorderFileName = "recordings\\" @ %str @ ".rec";
	echoc( 2, "Recording to - " @ $recorderFileName );
}

//utility shit
function AutoName::StripString(%str)
{
	%str = string::replace(%str, "[",  "_");
	%str = string::replace(%str, "]",  "_");
	%str = string::replace(%str, "<",  "_");
	%str = string::replace(%str, ">",  "_");
	%str = string::replace(%str, "?",  "_");
	%str = string::replace(%str, ":",  "_");
	%str = string::replace(%str, "*",  "_");
	%str = string::replace(%str, "/",  "_");
	%str = string::replace(%str, "\\", "_");
	%str = string::replace(%str, "|",  "_");
	
	return %str;
}
