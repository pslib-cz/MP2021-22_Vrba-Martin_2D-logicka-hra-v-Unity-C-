using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Entity
{
	public Wall(EntityConstructor ec, GameObject o)
	{
		this.MappedObject = o;
		this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
	}
}
