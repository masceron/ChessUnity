using System.Linq;
using Game.Managers;
using static Game.Common.BoardUtils;
using Game.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Evasion: Effect
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly int Probability;

        public Evasion(sbyte duration, int probability, PieceLogic piece) : base(duration, 1, piece, "effect_evasion")
        {
            Probability = probability;
        }

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || !action.Succeed) return;
            if (action != ICaptures) return;
            if (Distance(action.Maker, action.Target) < 3) return;
            if (!MatchManager.Roll(Probability)) return;

            if (PieceOn(action.Target).Effects.Any(e => e.EffectName == "effect_bound"))
            {
                var effect = PieceOn(action.Maker).Effects.Find(e => e.EffectName == "effect_snipe_eel_passive");
                if (effect != null)
                {
                    action.Succeed = true;
                    return;
                }
            }

            var pieceTarget = PieceOn(action.Maker);
            if (pieceTarget != null && pieceTarget.HasAugmentation(AugmentationName.ArcherfishAccuracy)) 
            {
                if (!MatchManager.Roll(Probability - 15)) return;
            } else
            {
                if (!MatchManager.Roll(Probability)) return;
            }
            
            action.Succeed = false;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 2 * Probability;
        }
    }
}