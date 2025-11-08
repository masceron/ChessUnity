using Game.Action.Skills;
using Game.Movesets;
using static Game.Common.BoardUtils;
using System.Linq;
using Game.Effects;
using Game.Common;
using UnityEngine;

namespace Game.Piece.PieceLogic.Commons
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Snaggletooths: PieceLogic, IPieceWithSkill
    {
        private bool flag;
        public Snaggletooths(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets, VersatileDefenderMove.Captures)
        {
            Skills = list =>
            {
                flag = false;
                if (SkillCooldown != 0) return;
                var (rank, file) = RankFileOf(Pos);
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null) continue;
                    if (piece.Effects.Any(e => e.EffectName == EffectName.Bleeding))
                    {
                        list.Add(new SnaggletoothsActive(Pos, index, false));
                        flag = true;
                    }
                }
                if (!flag)
                {
                    list.Add(new SnaggletoothsActive(Pos, Pos, true));
                }
            };
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}