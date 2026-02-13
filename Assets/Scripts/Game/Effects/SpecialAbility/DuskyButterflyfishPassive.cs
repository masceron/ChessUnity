using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class DuskyButterflyfishPassive : Effect, IStartTurnTrigger
    {
        public DuskyButterflyfishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_dusky_butterfly_passive")
        {
        }

        public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Debuff;

        public StartTurnEffectType StartTurnEffectType { get; }

        public void OnCallStart(Action.Action lastMainAction)
        {
            var (rank, file) = RankFileOf(Piece.Pos);

            foreach (var (r, f) in MoveEnumerators.AroundUntil(rank, file, 2))
            {
                var pOn = PieceOn(IndexOf(r, f));
                if (pOn == null || pOn.Color != Piece.Color) continue;
                ActionManager.EnqueueAction(new ApplyEffect(new Silenced(Piece, 1)));
                ActionManager.EnqueueAction(new ApplyEffect(new Broken(1, Piece)));
            }
        }
    }
}