using Game.Action;
using Game.Managers;
using static Game.Common.BoardUtils;
using Game.Augmentation;
using Game.Piece.PieceLogic.Commons;
using Game.Action.Captures;
namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Evasion: Effect, IBeforePieceActionEffect
    {

        public Evasion(int duration, int probability, PieceLogic piece) : base(duration, probability, piece, "effect_evasion")
        {
        }

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Target != Piece.Pos || action.Result != ResultFlag.Success) return;
            if (Distance(action.Maker, action.Target) < 3) return;
            if (!MatchManager.Roll(Strength)) return;

            if (PieceOn(action.Target).Effects.Any(e => e.EffectName == "effect_bound"))
            {
                var effect = PieceOn(action.Maker).Effects.Find(e => e.EffectName == "effect_snipe_eel_passive");
                if (effect != null)
                {
                    action.Result = ResultFlag.Evade;
                    return;
                }
            }

            var pieceTarget = PieceOn(action.Maker);
            if (pieceTarget != null && pieceTarget.HasAugmentation(AugmentationName.ArcherfishAccuracy)) 
            {
                if (!MatchManager.Roll(Strength - 15)) return;
            } else
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