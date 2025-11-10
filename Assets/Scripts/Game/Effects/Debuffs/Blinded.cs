using Game.Action;
using Game.Managers;
using Game.Piece.PieceLogic;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Blinded: Effect
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly int Probability;

        public Blinded(sbyte duration, int probability, PieceLogic piece) : base(duration, 1, piece, EffectName.Blinded)
        {
            Probability = probability;
        }

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || action.Maker != Piece.Pos) return;
            
            if (MatchManager.Roll(Probability))
            {
                action.Result = ActionResult.Failed;
            }
        }
    }
}