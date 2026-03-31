using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MiniSiphonophore : Commons.PieceLogic, IPieceWithSkill
    {
        public MiniSiphonophore(PieceConfig cfg) : base(cfg, RangerMove.Quiets, RangerMove.Captures)
        {

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(Pos, 2);
                    foreach (var target in targets) list.Add(new MiniSiphonophoreActive(this, BoardUtils.PieceOn(target)));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}