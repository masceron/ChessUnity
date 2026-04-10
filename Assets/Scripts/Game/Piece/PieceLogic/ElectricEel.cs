using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ElectricEel : Commons.PieceLogic, IPieceWithSkill
    {
        public ElectricEel(PieceConfig cfg) : base(cfg, ElectricEelMoves.Quiets, SmallPredatorMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ElectricEelVengeful(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;

                if (isPlayer)
                    list.Add(new ElectricEelActive(this));
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}