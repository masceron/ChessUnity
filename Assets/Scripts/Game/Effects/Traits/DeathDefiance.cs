using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DeathDefiance : Effect, IBeforePieceActionTrigger, IAfterPieceActionTrigger
    {
        private int _deathDefianceCount;

        public DeathDefiance(PieceLogic piece, int deathDefianceCount) : base(-1, 1, piece, "effect_death_defiance")
        {
            _deathDefianceCount = deathDefianceCount;
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.GetTarget() != Piece) return;
            if (action.Result != ResultFlag.SurvivedHit) return;

            if (_deathDefianceCount <= 0) return;

            _deathDefianceCount--;

            switch (_deathDefianceCount)
            {
                case 3:
                    ActionManager.EnqueueAction(new ApplyEffect(new Carapace(-1, Piece)));
                    break;
                case 2:
                    ActionManager.EnqueueAction(new ApplyEffect(new Adaptation(Piece)));
                    break;
                case 1:
                    ActionManager.EnqueueAction(new ApplyEffect(new Extremophile(Piece)));
                    break;
                default:
                    return;
            }
        }

        BeforeActionPriority IBeforePieceActionTrigger.Priority => BeforeActionPriority.Reaction;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.GetTarget() != Piece) return;
            if (action.Result != ResultFlag.Success) return;
            if (Piece.Effects.Any(e =>
                    e.EffectName is "effect_shield" or "effect_carapace" or "effect_hardened_shield")) return;
            if (_deathDefianceCount <= 0) return;

            action.Result = ResultFlag.SurvivedHit;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 80;
        }
    }
}