using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using System.Linq;
using Game.Common;
namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SohalSurgeonfish : Commons.PieceLogic, IPieceWithSkill
    {
        public SohalSurgeonfish(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(Pos, 6);
                    foreach (var target in targets)
                    {
                        var piece = PieceOn(target);
                        if (piece == null) continue;
                        if (piece.Effects.Any(e => e.EffectName == "effect_slow"))
                        {
                            list.Add(new SohalSurgeonfishActive(Pos, target));
                        }
                    }
                }
                else
                {
                    //query for AI in here
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}
