using Game.Action;
using Game.Managers;
using Game.Piece.PieceLogic;
using System.Linq;
using static Game.Common.BoardUtils;
using Game.Augmentation;
using Game.Common;

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

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || action.Result == ActionResult.Failed) return;

            if (Distance(action.Maker, action.Target) < 3) return;
            PieceLogic pieceTarget = BoardUtils.PieceOn(action.Maker);
            if (pieceTarget != null && pieceTarget.HasAugmentation(AugmentationName.ArcherfishAccuracy)) 
            {
                if (!MatchManager.Roll(Probability - 15)) return;
            } else
            {
                if (!MatchManager.Roll(Probability)) return;
            }
            
            action.Result = ActionResult.Failed;
        }
    }
}