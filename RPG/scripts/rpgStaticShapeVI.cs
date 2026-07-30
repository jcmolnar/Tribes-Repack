//This file is used when the map is not rpgmap1 or rpgmap5.
//You can modify this condition in server.cs.

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


StaticShapeData fountain{	shapeFile = "fountain";	debrisId = defaultDebrisSmall;	maxDamage = 10000.0;};StaticShapeData fountainwater{	shapeFile = "fountain_water";	debrisId = defaultDebrisSmall;	maxDamage = 10000.0;	disableCollision = true;	isTranslucent = "True";};

StaticShapeData BloodSpot{	shapeFile = "blood1";	maxDamage = 999.0;	isTranslucent = "True";	disableCollision = true;};