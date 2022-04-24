[System.Serializable]
public struct Coordinates
{
    public Coordinates(int x, int y)
	{
        this.x = x;
        this.y = y;
	}

	public static Direction GetDirection(Coordinates from, Coordinates to)
	{
		int dx = from.x - to.x;
		int dy = from.y - to.y;

		if (dx ==  1 && dy ==  0)
			return Direction.Left;
		if (dx == -1 && dy ==  0)
			return Direction.Right;
		if (dx ==  0 && dy ==  1)
			return Direction.Down;
		if (dx ==  0 && dy == -1)
			return Direction.Up;

		throw new System.NotImplementedException("not adjacent, not implemented");
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

	public override string ToString()
	{
		return "["+x+","+y+"]";
	}
}