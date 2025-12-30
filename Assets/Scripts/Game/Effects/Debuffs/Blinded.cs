using Game.Action;
using Game.Augmentation;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Blinded: Effect
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly int Probability;

        public Blinded(sbyte duration, int probability, PieceLogic piece) : base(duration, 1, piece, "effect_blinded")
        {
            Probability = probability;
        }

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || action.Maker != Piece.Pos) return;
            
            var pieceTarget = BoardUtils.PieceOn(action.Target);
            if (pieceTarget != null && pieceTarget.HasAugmentation(AugmentationName.ProtectiveLens))
            {
                action.Result = ResultFlag.Missed;
            }
            
            if (MatchManager.Roll(Probability))
            {
                action.Result = ResultFlag.Missed;
            }
        }
        
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }
    
    }
}