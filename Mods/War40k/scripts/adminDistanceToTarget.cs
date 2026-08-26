function DistanceToTarget (%clientId, %targetId)
{
	return (Vector::getDistance(GameBase::getPosition(%clientId),GameBase::getPosition(%targetId)));
}