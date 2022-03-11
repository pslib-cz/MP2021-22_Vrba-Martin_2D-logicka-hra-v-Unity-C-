using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    public GameObject MappedObject;
    public Coordinates Position;
    protected LevelState state;
    protected SpriteRenderer renderer;
    protected Dictionary<string, Sprite> sprites;

    public virtual void PerformTick(Direction input)
    {
        throw new NotImplementedException();
    }

    public virtual void UpdateSprite()
	{
        throw new NotImplementedException();
	}

    protected virtual void SetSprite(string name)
    {
        renderer.sprite = sprites[name];
    }
}