using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UmbrellaSlugSpecialAbility : Effect, IAfterPieceActionEffect
    {
        public UmbrellaSlugSpecialAbility(PieceLogic piece) : base(-1, 100, piece, "effect_umbrella_slug_special_ability")
        {
            
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not NormalMove move || action.Maker != Piece.Pos || action.Result != ResultFlag.Success) return;
            var (rank, file) = BoardUtils.RankFileOf(move.Target);
            
            foreach (var (rankof, fileof) in MoveEnumerators.AroundUntil(rank, file , 1))
            {
                var piece = BoardUtils.PieceOn(BoardUtils.IndexOf(rankof, fileof));
                if (piece == null || piece.Color != Piece.Color || !MatchManager.Roll(Strength)) continue;
                var poison = piece.Effects.Find(effect => effect.EffectName == "effect_poison");
                if (poison == null) continue;
                poison.Strength -= 1;
                if (poison.Strength <= 0)
                {
                    ActionManager.EnqueueAction(new RemoveEffect(poison));
                }
            }
        }
    }
}