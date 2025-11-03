using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CoffinFishVengeful : Effect
    {
        public readonly int Probability;
        public CoffinFishVengeful(PieceLogic piece, int probability) : base(-1, 1, piece, EffectName.CoffinFishVengeful)
        {
            Probability = probability;
        }

        public override void OnCall(Action.Action action)
        {
            if (action == null) return;

            if (!MatchManager.Roll(Probability)) return;
            if (action.Target == Piece.Pos && Piece.IsDead())
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Relentless(BoardUtils.PieceOn(action.Maker), 1)));
            }
        }
    }
}