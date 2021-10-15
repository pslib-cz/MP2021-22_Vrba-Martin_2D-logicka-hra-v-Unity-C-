using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    public GameObject MappedObject;
    public Coordinates Position;

    public virtual void PerformTick(LevelState state, Direction input)
	{
        throw new NotImplementedException();
	}

}