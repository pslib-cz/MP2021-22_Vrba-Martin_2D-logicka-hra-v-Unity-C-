[System.Serializable]
public struct Coordinates
{
    public Coordinates(int x, int y)
	{
        this.x = x;
        this.y = y;
	}

    public static Coordinates operator + (Coordinates coords, Direction direction)
    {
		switch (direction)
		{
			case Direction.None:
				return coords;
			case Direction.Up:
				return new Coordinates(coords.x, coords.y + 1);
			case Direction.Down:
				return new Coordinates(coords.x, coords.y - 1);
			case Direction.Left:
				return new Coordinates(coords.x - 1, coords.y);
			case Direction.Right:
				return new Coordinates(coords.x + 1, coords.y);
			default:
				return coords;
		}
	}

    public int x { get; set; }
    public int y { get; set; }
}