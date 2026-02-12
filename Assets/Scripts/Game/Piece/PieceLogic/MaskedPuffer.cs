using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Traits;
namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MaskedPuffer : Commons.PieceLogic, IPieceWithSkill
    {
        public MaskedPuffer(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new MaskedPufferPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;

                if (isPlayer)
                {
                    list.Add(new MaskedPufferActive(Pos, Pos));
                }
                else
                {

                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}