using ChessSharp.Pieces;
using ChessSharp.SquareData;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ChessSharp;

/// <summary>Represents a fire action in game.</summary>
public class Fire : IDeepCloneable<Fire>
{
    /// <summary>Gets the source <see cref="Square"/> of the <see cref="Fire"/>.</summary>
    public Square Source { get; }
    /// <summary>Gets the target <see cref="Square"/> of the <see cref="Fire"/>.</summary>
    public Square Target { get; }
    /// <summary>Gets the <see cref="Player"/> of the <see cref="Fire"/>.</summary>
    public Player Player { get; }

    public Fire DeepClone()
    {
        return new Fire(Source, Target, Player);
    }

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is Fire fire &&
            fire.Source == Source &&
            fire.Target == Target; 

    public override int GetHashCode() => HashCode.Combine(Source, Target, Player);

    /// <summary>Initializes a new instance of the <see cref="Fire"/> class with the given arguments.</summary>
    /// <param name="source">The source <see cref="Square"/> of the <see cref="Fire"/>.</param>
    /// <param name="target">The target <see cref="Square"/> of the <see cref="Fire"/>.</param>
    /// <param name="player">The <see cref="Player"/> of the <see cref="Fire"/>.</param>
    public Fire(Square source, Square target, Player player) =>
        (Source, Target, Player) = (source, target, player);

    internal int GetAbsDeltaX() => Math.Abs(GetDeltaX());

    internal int GetAbsDeltaY() => Math.Abs(GetDeltaY());
    
    internal int GetDeltaX() => Target.File - Source.File;

    internal int GetDeltaY() => Target.Rank - Source.Rank;
}
