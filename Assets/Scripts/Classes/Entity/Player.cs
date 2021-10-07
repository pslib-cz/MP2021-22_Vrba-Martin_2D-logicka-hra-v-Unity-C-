using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, IDirectionFacingEntity
{
    public Direction direction { get; }
    public Player(Direction direction)
    {
        this.direction = direction;
    }
}