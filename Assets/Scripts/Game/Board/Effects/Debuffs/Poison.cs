using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Debuffs
{
    public class Poison: Effect, IEndTurnEffect
    {
        public byte TimeLeft = 3;

        public Poison(sbyte strength, PieceLogic piece) : base(-1, strength, piece, EffectName.Poison)
        {
            EndTurnEffectType = EndTurnEffectType.AtAllyTurn;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (Strength >= 5) TimeLeft--;
            if (TimeLeft <= 0) ActionManager.EnqueueAction(new DestroyPiece(Piece.Pos));
        }

        public EndTurnEffectType EndTurnEffectType { get; set; }
    }
}