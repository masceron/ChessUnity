using Game.Action;
using Game.Action.Captures;
using Game.Augmentation;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Evasion : Effect, IBeforePieceActionTrigger
    {
        public Evasion(int duration, int probability, PieceLogic piece) : base(duration, probability, piece,
            "effect_evasion")
        {
            
        }

        public BeforeActionPriority Priority => BeforeActionPriority.Mitigation;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.GetTarget() != Piece || action.Result != ResultFlag.Success) return;
            if (Distance(action.GetFrom(), action.GetTargetPos()) < 3) return;
            if (!MatchManager.Roll(Strength)) return;

            if (((PieceLogic)action.GetTarget()).Effects.Any(e => e.EffectName == "effect_bound"))
            {
                var effect = (action.GetMaker() as PieceLogic)?.Effects.Find(e => e.EffectName == "effect_snipe_eel_passive");
                if (effect != null)
                {
                    action.Result = ResultFlag.Evade;
                    return;
                }
            }

            var pieceTarget = action.GetMaker() as PieceLogic;
            if (pieceTarget != null && pieceTarget.HasAugmentation(AugmentationName.ArcherfishAccuracy))
            {
                if (!MatchManager.Roll(Strength - 15)) return;
            }
            else
            {
                if (!MatchManager.Roll(Strength)) return;
            }

            action.Result = ResultFlag.Evade;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 2 * Strength;
        }
    }
}