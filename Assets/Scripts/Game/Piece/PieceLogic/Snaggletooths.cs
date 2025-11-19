using System.Linq;
using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Snaggletooths: Commons.PieceLogic, IPieceWithSkill
    {
        public Snaggletooths(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets, VersatileDefenderMove.Captures)
        {
            Skills = list =>
            {
                var flag1 = false;
                if (SkillCooldown != 0) return;
                var (rank, file) = RankFileOf(Pos);
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null) continue;
                    if (piece.Effects.Any(e => e.EffectName == "effect_bleeding"))
                    {
                        list.Add(new SnaggletoothsActive(Pos, index, false));
                        flag1 = true;
                    }
                }
                if (!flag1)
                {
                    list.Add(new SnaggletoothsActive(Pos, Pos, true));
                }
            };
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}