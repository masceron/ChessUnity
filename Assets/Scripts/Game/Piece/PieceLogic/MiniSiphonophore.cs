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
        public MiniSiphonophore(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            SetStat(SkillStat.Range, 1);
            SetStat(SkillStat.Unit, 1);

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {   
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    // TODO: làm cho trường hợp Unit > 1
                    var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(this, GetStat(SkillStat.Range));
                    foreach (var target in targets) list.Add(new MiniSiphonophoreActive(this, target));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}