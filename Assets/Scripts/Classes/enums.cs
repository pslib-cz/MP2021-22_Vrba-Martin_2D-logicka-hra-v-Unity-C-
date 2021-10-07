using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum TileType
{
    Nothing = 0,
    Wall = 1,
    Floor = 2,
    Player = 3,
}

enum TileLayer
{
    //  [CAMERA]
    Object,
    Environment,
}

public struct Coordinates
{
    int x;
    int y;
}


public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public interface IDirectionFacingEntity
{
    public Direction direction { get; }
}





