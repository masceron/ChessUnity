using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Debuffs
{
    public class Poison: Effect
    {
        public byte TimeLeft = 6;
        
        public Poison(sbyte strength, PieceLogic piece) : base(-1, strength, piece, Effects.EffectName.Poison)
        {}

        public override void OnCall(Action.Action action)
        {
            if (Strength >= 5) TimeLeft--;
            if (TimeLeft <= 0) ActionManager.EnqueueAction(new DestroyPiece(Piece.Pos));
        }

        public override string Description()
        {
            return MatchManager.assetManager.EffectData[EffectName].description;
        }
    }
}