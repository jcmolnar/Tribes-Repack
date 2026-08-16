//This file is part of Tribes RPG.
//Tribes RPG server side scripts
//Written by Jason "phantom" Daley,  Matthiew "JeremyIrons" Bouchard, and more (yet undetermined)

//	Copyright (C) 2014  Jason Daley

//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.

//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.

//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>.

//You may contact the author at beatme101@gmail.com or www.tribesrpg.org/contact.php

//This GPL does not apply to Starsiege: Tribes or any non-RPG related files included.
//Starsiege: Tribes, including the engine, retains a proprietary license forbidding resale.



//Regular crystal
StaticShapeData Crystal
{
	shapeFile = "crystals";
	debrisId = flashDebrisSmall;
	maxDamage = 1.0;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
	description = "Crystal";
};
function Crystal::onDamage()
{
}

//Empty crystal
StaticShapeData EmptyCrystal
{
	shapeFile = "crystals";
	debrisId = flashDebrisSmall;
	maxDamage = 1.0;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
      description = "Empty Crystal";
};

function InitCrystals()
{
	dbecho($dbechoMode, "InitCrystals()");

	%group = nameToID("MissionGroup\\Crystals");

	if(%group != -1)
	{
		for(%i = 0; %i <= Group::objectCount(%group)-1; %i++)
		{
			%this = Group::getObject(%group, %i);
			%info = Object::getName(%this);

			if(%info != "")
			{
				%cnt = 0;
				for(%z = 0; (%p1 = GetWord(%info, %z)) != -1; %z+=2)
				{
					%p2 = GetWord(%info, %z+1);
					%this.bonus[%cnt++] = %p2;
				}
			}
		}
	}
}