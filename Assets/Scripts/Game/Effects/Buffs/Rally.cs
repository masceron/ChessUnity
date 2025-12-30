using Game.Piece.PieceLogic.Commons;
using Game.Action;
using static Game.Common.BoardUtils;
namespace Game.Effects.Buffs
{
    public class Rally: Effect
    {
        public Rally(sbyte duration, PieceLogic piece) : base(duration, 1, piece, "effect_rally")
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || action.Result != ResultFlag.Success || (action.Flag & ActionFlag.Unblockable) != 0) return;
            if (PieceOn(action.Maker).Color != Piece.Color) return;
            if (Distance(action.Maker, Piece.Pos) > 4) return;
            if (Piece.SkillCooldown > 0) Piece.SkillCooldown--;
        }
    }
}