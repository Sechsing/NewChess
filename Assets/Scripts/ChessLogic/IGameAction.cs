using ChessSharp.SquareData;

namespace ChessSharp;

public interface IGameAction : IDeepCloneable<IGameAction>
{
    Player Player { get; }
    Square Source { get; }
    string ToNotation();
}
