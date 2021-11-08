using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Entity
{
	public Box(EntityConstructor ec)
	{
		this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
	}
}
