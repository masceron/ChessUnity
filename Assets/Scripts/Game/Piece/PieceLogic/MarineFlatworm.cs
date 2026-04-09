using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.SpecialAbility;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineFlatworm : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range = 4;
        public MarineFlatworm(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new MarineFlatwormPassive(this, Range)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;

                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);

                    for (var i = rank - GetStat(SkillStat.Range); i <= rank + GetStat(SkillStat.Range); i++)
                    {
                        for (var j = file - GetStat(SkillStat.Range); j <= file + GetStat(SkillStat.Range); j++)
                        {
                            if (!VerifyBounds(i) || !VerifyBounds(j)) continue;
                            var idx = IndexOf(i, j);
                            if (!IsActive(idx)) continue;
                            var piece = PieceOn(idx);
                            if (piece == null)
                                list.Add(new MarineFlatwormActive(this, idx));
                        }
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}