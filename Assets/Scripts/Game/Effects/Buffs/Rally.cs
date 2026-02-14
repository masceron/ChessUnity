using Game.Action;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Buffs
{
    public class Rally : Effect, IAfterPieceActionTrigger
    {
        public Rally(int duration, PieceLogic piece) : base(duration, 1, piece, "effect_rally")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Result != ResultFlag.Success ||
                (action.Flag & ActionFlag.Unblockable) != 0) return;
            if (PieceOn(action.Maker).Color != Piece.Color) return;
            if (Distance(action.Maker, Piece.Pos) > 4) return;
            if (Piece.SkillCooldown > 0) Piece.SkillCooldown--;
        }
    }
}