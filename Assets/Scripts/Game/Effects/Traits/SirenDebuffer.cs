using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenDebuffer : Effect, IStartTurnTrigger
    {
        public SirenDebuffer(PieceLogic p) : base(-1, 1, p, "effect_siren_debuffer")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfEnemyTurn;
        }

        public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Debuff;

        public StartTurnEffectType StartTurnEffectType { get; }

        public void OnCallStart(Action.Action lastMainAction)
        {
            var (rank, file) = RankFileOf(Piece.Pos);

            foreach (var (r, f) in MoveEnumerators.AroundUntil(rank, file, 4))
            {
                var pOn = PieceOn(IndexOf(r, f));
                if (pOn == null) continue;

                if (pOn.Color != Piece.Color && pOn.Effects.All(e => e.EffectName != "effect_slow"))
                    ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 1, pOn), Piece));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 80;
        }
    }
}