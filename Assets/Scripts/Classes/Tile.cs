using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
	public Tile() 
	{
	
	}


	public IList<Entity> Entities { get; private set; }

	public void Add(Entity entity)
	{

	}

	public bool Opened { 
		get 
		{ 
			//returns true if contains floor but doesnt contain closed obstacle
			//returns false if 
		}
		private set { }
	}
}
