using Game.Board.Action;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Debuffs
{
    public class Blinded: Effect
    {
        private readonly int probability;

        public Blinded(sbyte duration, int probability, PieceLogic piece) : base(duration, 1, piece, Effects.EffectName.Blinded)
        {
            this.probability = probability;
        }

        public override void OnCall(Action.Action action)
        {
            if (action == null || action.Caller != Piece.Pos) return;
            
            if (MatchManager.Roll(probability))
            {
                action.Result = ActionResult.Failed;
            }
        }

        public override string Description()
        {
            return string.Format(MatchManager.assetManager.EffectData[EffectName].description, probability);
        }
    }
}