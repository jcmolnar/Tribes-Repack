// Thanks to Crotalus -aka- Vage:

$Math::PI = "3.14159265358979323";

function Math::rad2deg(%radians)
{
    return %radians * (180 / $Math::PI);
}

function Math::deg2rad(%degrees)
{
    return %degrees * ($Math::PI / 180);
}

function Math::roundDown(%delta)
{
    return floor(%delta - 0.01);
}

function Math::roundUp(%delta)
{
    return ceil(%delta + 0.01);
}

function Math::randomInt(%max)
{
    return floor(getRandom() * (%max - 0.01));
}

function Math::sin(%theta)
{
    if(%theta == "NaN")
        return;
   return (%theta - (pow(%theta,3)/6) + (pow(%theta,5)/120) - (pow(%theta,7)/5040) + (pow(%theta,9)/362880) - (pow(%theta,11)/39916800));
}

function Math::cos(%theta)
{
    if(%theta == "NaN")
        return;
   return (1 - (pow(%theta,2)/2) + (pow(%theta,4)/24) - (pow(%theta,6)/720) + (pow(%theta,8)/40320) - (pow(%theta,10)/3628800));
}

function Math::tan(%theta)
{
    if(%theta == "NaN")
        return;
    return Sin(%theta) / Cos(%theta);
}

function Math::abs(%delta)
{
    return sqrt(pow(%delta,2));
}

function Math::isNaN(%number)
{
    if(%number == "NaN" || String::NumericParse(%number) != %number)
        return true;
    return false;
}

function Vector::rotate(%vec,%rot)
{
   %pi = $Math::PI;
   %rot3= getWord(%rot,2);
   for(%i = 0; %rot3 >= %pi*2; %i++) %rot3 = %rot3 - %pi*2;
   if (%rot3 > %pi) %rot3 = %rot3 - %pi*2;

   %vec1= getWord(%vec,0);
   %vec2= getWord(%vec,1);
   %vc = %vec2;
   %vec3= getWord(%vec,2);

   %ray = %vec1;

   %vec1 = %ray*Math::cos(%rot3);
   %vec2 = %ray*Math::sin(%rot3);
   %vec = %vec1 @" "@ %vec2 @" "@ %vec3;
   %vec = Vector::add(%vec,Vector::getFromRot(%rot,%vc,0));
   return %vec;
}

function Vector::multiply(%vec1, %vec2)
{
    %vec1X = getWord(%vec1, 0);
    %vec1Y = getWord(%vec1, 1);
    %vec1Z = getWord(%vec1, 2);

    %vec2X = getWord(%vec2, 0);
    %vec2Y = getWord(%vec2, 1);
    %vec2Z = getWord(%vec2, 2);

    %vec3X = %vec1X * %vec2X;
    %vec3Y = %vec1Y * %vec2Y;
    %vec3Z = %vec1Z * %vec2Z;

    return %vec3X @ " " @ %vec3Y @ " " @ %vec3Z;
}

function Vector::divide(%vec1, %vec2)
{
    %vec1X = getWord(%vec1, 0);
    %vec1Y = getWord(%vec1, 1);
    %vec1Z = getWord(%vec1, 2);

    %vec2X = getWord(%vec2, 0);
    %vec2Y = getWord(%vec2, 1);
    %vec2Z = getWord(%vec2, 2);

    %vec3X = %vec1X / %vec2X;
    %vec3Y = %vec1Y / %vec2Y;
    %vec3Z = %vec1Z / %vec2Z;

    return %vec3X @ " " @ %vec3Y @ " " @ %vec3Z;
}

function GameBase::getMass(%obj)
{
    %mass = (GameBase::getDataName(%obj).Mass);
    if (getObjectType(%obj) == "Flyer")
        %mass += %obj.PassengerMass;
    return %mass;
}

function GameBase::getMuzzlePosition(%player)
{
        %trans = GameBase::getMuzzleTransform(%player);
        %vec6 = getWord(%trans,9);
        %vec7 = getWord(%trans,10);
        %vec8 = getWord(%trans,11);
        return %vec6@" "@%vec7@" "@%vec8;
}

function GameBase::getNormalRotation(%pos, %pos2)
{
    return Vector::Normalize(Vector::Sub(%pos, %pos2));
}

function Vector::rotate3DVector( %pivot, %pos, %rot )
{
   //Normalize the point to the "origin"
   %pos = Vector::sub( %pos, %pivot );
   
   //Separate out the point vector
   %ax = getWord( %pos, 0 );
   %ay = getWord( %pos, 1 );
   %az = getWord( %pos, 2 );
   
   //Our resultants
   %axr = 0;
   %ayr = 0;
   %azr = 0;
   
   //Separate out the rotation
   %rx = getWord( %rot, 0 );
   %ry = getWord( %rot, 1 );
   %rz = getWord( %rot, 2 );
   
   //Rotate about Z-axis first
   %axr = Math::cos( %rz ) * %az - Math::sin( %rz ) * %ay;
   %ayr = Math::sin( %rz ) * %az + Math::cos( %rz ) * %ay;
   %azr = %az;
   
   //Then about the Y-axis
   %ax = Math::cos( %ry ) * %axr - Math::sin( %ry ) * %azr;
   %ay = %ayr;
   %az = Math::sin( %ry ) * %axr + Math::cos( %ry ) * %azr;
   
   //And finally about the X-axis
   %axr = %ax;
   %ayr = Math::cos( %rx ) * %ay - Math::sin( %rx ) * %az;
   %azr = Math::sin( %rx ) * %ay + Math::cos( %rx ) * %az;
   
   %pos = %axr @ " " @ %ayr @ " " @ %azr;
   
   //Put the point back in world-space
   %pos = Vector::add( %pos, %pivot );
   return %pos;
}