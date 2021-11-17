using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//there can be only one IObstacle per tile
public interface IObstacle
{
	public bool Opened { get; }
	public void Open(bool open);
}
