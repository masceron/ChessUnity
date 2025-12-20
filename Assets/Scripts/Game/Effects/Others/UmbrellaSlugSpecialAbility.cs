using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UmbrellaSlugSpecialAbility : Effect
    {
        private int probability = 100;
        public UmbrellaSlugSpecialAbility(PieceLogic piece) : base(-1, 1, piece, "effect_umbrella_slug_special_ability")
        {
            
        }
        

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action is not NormalMove move || BoardUtils.PieceOn(move.Maker) != Piece) return;
            var (rank, file) = BoardUtils.RankFileOf(move.Target);
            
            var pieceUp = BoardUtils.PieceOn(BoardUtils.IndexOf(rank + 1, file));
            var pieceDown = BoardUtils.PieceOn(BoardUtils.IndexOf(rank - 1, file));
            var pieceLeft = BoardUtils.PieceOn(BoardUtils.IndexOf(rank, file - 1));
            var pieceRight = BoardUtils.PieceOn(BoardUtils.IndexOf(rank, file + 1));
            
            if (pieceUp != null && pieceUp.Color == Piece.Color && MatchManager.Roll(probability))
            {
                Effect poison = pieceUp.Effects.Find(effect => effect.EffectName == "effect_poison");
                if (poison != null)
                {
                    poison.Strength -= 1;
                    if (poison.Strength <= 0)
                    {
                        ActionManager.EnqueueAction(new RemoveEffect(poison));
                    }
                }
            }
            
            if (pieceDown != null && pieceDown.Color == Piece.Color && MatchManager.Roll(probability))
            {
                Effect poison = pieceDown.Effects.Find(effect => effect.EffectName == "effect_poison");
                if (poison != null)
                {
                    poison.Strength -= 1;
                    if (poison.Strength <= 0)
                    {
                        ActionManager.EnqueueAction(new RemoveEffect(poison));
                    }
                }
            }
            
            if (pieceLeft != null && pieceLeft.Color == Piece.Color && MatchManager.Roll(probability))
            {
                Effect poison = pieceLeft.Effects.Find(effect => effect.EffectName == "effect_poison");
                if (poison != null)
                {
                    poison.Strength -= 1;
                    if (poison.Strength <= 0)
                    {
                        ActionManager.EnqueueAction(new RemoveEffect(poison));
                    }
                }
            }
            
            if (pieceRight != null && pieceRight.Color == Piece.Color && MatchManager.Roll(probability))
            {
                Effect poison = pieceRight.Effects.Find(effect => effect.EffectName == "effect_poison");
                if (poison != null)
                {
                    poison.Strength -= 1;
                    if (poison.Strength <= 0)
                    {
                        ActionManager.EnqueueAction(new RemoveEffect(poison));
                    }
                }
            }
            
            
        }
    }
}