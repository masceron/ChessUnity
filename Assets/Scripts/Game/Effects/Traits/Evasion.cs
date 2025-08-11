using Game.Action;
using Game.Managers;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Evasion: Effect
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly int Probability;

        public Evasion(sbyte duration, int probability, PieceLogic piece) : base(duration, 1, piece, EffectName.Evasion)
        {
            Probability = probability;
        }

        public override void OnCall(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || action.Result == ActionResult.Failed) return;

            if (Distance(action.Maker, action.Target) < 3) return;
            if (!MatchManager.Roll(Probability)) return;
            
            action.Result = ActionResult.Failed;
        }
    }
}