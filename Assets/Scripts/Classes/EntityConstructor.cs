using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EntityConstructor
{
	//Entity
	public TileType T; //Type
	public int CoordinateX;
	public int CoordinateY;
	
	//IDirectionFacing
	public Direction Direction;

	//IObstacle
	public bool Opened;
}
