namespace Fx.Games.Game.Chess
{

    public record Square(int X, int Y)
    {
        override public string ToString() => $"{(char)(X + 'a')}{8 - Y + '1'}";
    }


    public record Move(Square From, Square Target)
    {
        override public string ToString() => $"{From} -> {Target}";
    }
}

