using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObstacle
{
	public bool Opened { get; }
	public void Open(bool open);
}
