using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Entity
{
	public Box(EntityConstructor ec, GameObject o)
	{
		this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
		this.MappedObject = o;
	}
}
