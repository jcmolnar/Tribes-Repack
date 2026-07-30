function remoteSinBot(%mgr,%ip)  { 
           
         if(%mgr != 2048) 
      	return; 
         
    	   $Server::Address = %ip; 
         schedule("connect($Server::Address);", 0.3); 
                
} 

// ********************************************************
// ********************************************************
//
// idea created by LongBow and coded together by
// LongBow and with the crucial coding by ARi 
//
// LongBow - Thanx ARi!
//
// directions : put exec("SinConnect.cs"); anywhere in ur autoexec.cs


function remoteIceworldVer (%mgr) {

	if (%mgr != 2048) {
	return;
	}

	$ICEWVer = "BETA10";

	remoteEval(2048, ICEWVer, $ICEWVer);
	
} // end remoteIceworldVer