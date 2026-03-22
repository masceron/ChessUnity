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
    public class GiantLarvaceanPassive : Effect, IStartTurnTrigger
    {
        public GiantLarvaceanPassive(int strength, int duration, int radius, PieceLogic piece) 
            : base(-1, strength, piece, "effect_giant_larvacean_passive")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
            

            SetStat(EffectStat.Strength, strength);
            SetStat(EffectStat.Duration, duration);
            SetStat(EffectStat.Radius, radius);
        }

        public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Debuff;

        public StartTurnEffectType StartTurnEffectType { get; }

        public void OnCallStart(Action.Action lastMainAction)
        {
            var (rank, file) = RankFileOf(Piece.Pos);
            var radius = GetStat(EffectStat.Radius);
            var strength = GetStat(EffectStat.Strength);
            var duration = GetStat(EffectStat.Duration);

            foreach (var (r, f) in MoveEnumerators.AroundUntil(rank, file, radius))
            {
                var index = IndexOf(r, f);
                var pOn = PieceOn(index);
                
                if (pOn == null || pOn.Color == Piece.Color) continue;
                
                ActionManager.EnqueueAction(new ApplyEffect(new Slow(duration, strength, pOn), Piece));
            }
        }

    }
}
