function String::len( %string )
{
   for(%i=0;String::getSubStr(%string,%i+50,1)!="";%i=%i+50) {}
   for(%i;String::getSubStr(%string,%i+10,1)!="";%i=%i+10) {}
   for(%i;String::getSubStr(%string,%i,1)!="";%i++) {}
   return %i;
}

function String::replace(%string, %search, %replace)
{
	dbecho($dbechoMode, "String::replace(" @ %string @ ", " @ %search @ ", " @ %replace @ ")");

	%loc = String::findSubStr(%string, %search);

	if(%loc != -1)
	{
		%ls = String::len(%search);

		%part1 = String::NEWgetSubStr(%string, 0, %loc);
		%part2 = String::NEWgetSubStr(%string, %loc + %ls, 99999);

		%string = %part1 @ %replace @ %part2;
	}

	return %string;
}

//Added replace all function
function String::replaceAll(%string, %search, %replace)
{
	dbecho($dbechoMode, "String::replace(" @ %string @ ", " @ %search @ ", " @ %replace @ ")");

	%loc = String::findSubStr(%string, %search);

	if(%loc != -1)
	{
		%ls = String::len(%search);

		%part1 = String::NEWgetSubStr(%string, 0, %loc);
		%part2 = String::replaceAll(String::NEWgetSubStr(%string, %loc + %ls, 99999), %search, %replace);

		%string = %part1 @ %replace @ %part2;
	}

	return %string;
}

function String::create(%c, %len)
{
	dbecho($dbechoMode, "String::create(" @ %c @ ", " @ %len @ ")");

	%f = "";
	for(%i = 1; %i <= %len; %i++)
		%f = %f @ %c;

	return %f;
}

function String::ofindSubStr(%s, %f, %o)
{
	dbecho($dbechoMode, "String::ofindSubStr(" @ %s @ ", " @ %f @ ", " @ %o @ ")");

	%ns = String::NEWgetSubStr(%s, %o, 99999);
	return String::findSubStr(%ns, %f);
}

function String::NEWgetSubStr(%s, %x, %y)
{
	dbecho($dbechoMode, "String::NEWgetSubStr(" @ %s @ ", " @ %x @ ", " @ %y @ ")");

	%len = %y;
	%chunks = floor(%len / 255) + 1;

	%q = %len;
	%nx = %x;
	%final = "";

	for(%i = 1; %i <= %chunks; %i++)
	{
		%q = %q - 255;
		if(%q <= 0)
			%chunkLen = %q+255;
		else
			%chunkLen = 255;

		//This basically shorts circuits the code if you have put in 99999 as length...this should speed things up
		%str = String::getSubStr(%s, %nx, %chunkLen);
		if(%str == "")
			return %final;
		%final = %final @ %str;
		%nx = %nx + %chunkLen;
	}

	return %final;
}

function String::nfindSubStr(%string, %find, %startInd) {
	%loc = String::findSubStr(String::getSubStr(%string, %startInd, 9999), %find);
	if(%loc == -1)
		return -1;
	return %loc+1;//+%startInd;//fixed by phantom
}

function String::removeBetween(%string, %strBeg, %strEnd) {
	%ind1 = String::findsubstr(%string, %strBeg);
	%ind2 = String::nfindsubstr(%string, %strEnd, %ind1 + String::len(%strBeg));

	if(%ind1 == -1 || %ind2 == -1) {
		return %string;
	}

	%beg = String::getSubStr(%string, 0, %ind1);
	%end = String::getSubStr(%string, %ind2+%ind1+String::len(%strEnd), 9999);
	return (%beg@%end);
}