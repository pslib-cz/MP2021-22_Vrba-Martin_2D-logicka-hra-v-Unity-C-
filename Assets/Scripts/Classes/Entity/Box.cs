using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Entity, IMovingEntity
{
	public Box(EntityConstructor ec, GameObject o)
	{
		this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
		this.MappedObject = o;
	}

	public bool Push(LevelState state, Direction direction)
	{
		Tile destination = state[Position + direction];

		if (destination.HasBox || !destination.Opened /*add more if needed*/)
		{
			return false;
		}

		Move(Position + direction);
		return true;
	}

	public void Move(Coordinates destination)
	{
		this.Position = destination;
	}
}
