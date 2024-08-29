namespace Fx.Games.Game.Chess
{
    using System;


    public record Square(int File, int Rank)
    {
        override public string ToString() => $"{(char)(File + 'a')}{(char)(Rank + '1')}";
    }

    public record Move(Square From, Square Target, bool Capture = false)
    {
        override public string ToString() => $"{From} {Target}{(Capture ? " x" : "")}";
    }
}

