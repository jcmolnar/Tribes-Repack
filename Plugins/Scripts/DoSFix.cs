$DosFix::Debug=false;      // debug?

$DosFix::GoodThrottle=20;    // known remoteevals allowed per $DosFix::GoodWindow
$DosFix::GoodWindow=0.5;

$DosFix::BadThrottle=10;   // unknown remoteevals allowed per $DosFix::BadWindow
$DosFix::BadWindow=1;

$DosFix::BadKick=true;      // kick for bad remote function flood? default is true

function remoteBadFilter(%managerId)
{
   if($DosFix::Debug)
      echo("remoteBadFilter(", %managerId,");");

   %managerId.btries++;

     if(%managerId.btries > $DosFix::BadThrottle)
      {
      if($DosFix::BadKick)
      schedule("net::kick("@%managerid@",'Stop spamming the server.');",0);
      return;
      }
   schedule(%managerId @ ".btries = 0;", $DosFix::BadWindow);
}

function remoteFilter(%managerId, %argc, %func)
{
   if(%func=="remoteEval")
      remoteBadFilter(%managerId);

   if($DosFix::Debug)
      echo("remoteFilter(", %managerId,", ", %argc,", ", %func,");");

   %managerId.rtries++;
     if(%managerId.rtries > $DosFix::GoodThrottle)
      return;

   schedule(%managerId @ ".rtries = 0;", $DosFix::GoodWindow);

   %remote = $remoteArg[0] @ "(" @ %managerId;
   for(%i = 2; %i <= %argc - 1; %i++)
   {
      %remote = %remote @ ", \"" @ $remotearg[%i] @ "\"";
   }
   %remote = %remote @ ");";
   //eval(%remote);
}

