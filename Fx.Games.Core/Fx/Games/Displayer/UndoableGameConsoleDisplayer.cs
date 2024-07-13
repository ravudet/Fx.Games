using Fx.Games.Game;

namespace Fx.Games.Displayer
{
    public sealed class UndoableGameConsoleDisplayer<TGame, TBoard, TMove, TPlayer> : IDisplayer<UndoableGame<TGame, TBoard, TMove, TPlayer>, TBoard, UndoableMove<TMove>, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly IDisplayer<TGame, TBoard, TMove, TPlayer> gameConsoleDisplayer;

        public UndoableGameConsoleDisplayer(IDisplayer<TGame, TBoard, TMove, TPlayer> gameConsoleDisplayer)
        {
            this.gameConsoleDisplayer = gameConsoleDisplayer;
        }

        public void DisplayAvailableMoves(UndoableGame<TGame, TBoard, TMove, TPlayer> game)
        {
            throw new System.NotImplementedException();
        }

        public void DisplayBoard(UndoableGame<TGame, TBoard, TMove, TPlayer> game)
        {
            throw new System.NotImplementedException();
        }

        public void DisplayOutcome(UndoableGame<TGame, TBoard, TMove, TPlayer> game)
        {
            throw new System.NotImplementedException();
        }

        public void DisplaySelectedMove(UndoableMove<TMove> move)
        {
            throw new System.NotImplementedException();
        }
    }
}
