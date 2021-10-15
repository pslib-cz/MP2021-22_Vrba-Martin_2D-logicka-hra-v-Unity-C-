using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirectionFacingEntity
{
	public Direction GetDirection();
	public void SetDirection(Direction newDir);
	public Coordinates LookingAt();
}

