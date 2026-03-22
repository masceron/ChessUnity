using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ReefSquid : Commons.PieceLogic
    {
        public ReefSquid(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, PawnPushMoves.Captures)
        {
            SetStat(SkillStat.Chance, 20);
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 20, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new ReefSquidPassive(this)));
        }
    }
}