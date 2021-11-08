using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : Entity
{
	public Floor(EntityConstructor ec, GameObject o)
	{
		this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
		this.MappedObject = o;
	}
}
