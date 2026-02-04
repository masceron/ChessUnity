using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class SloaneSViperfish : Commons.PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;

        public SloaneSViperfish(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, SmallPredatorMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    var caller = PieceOn(Pos);

                    for (var i = -4; i <= 4; i++)
                    {
                        if (!VerifyBounds(rank + i)) continue;
                        for (var j = -4; j <= 4; j++)
                        {
                            if (!VerifyBounds(file + j)) continue;

                            var idx = IndexOf(rank + i, file + j);

                            var p = PieceOn(idx);
                            if (p == null || p == caller || p.Color == caller.Color) continue;

                            var bleeding = p.Effects.Any(t => t.EffectName == "effect_bleeding");

                            list.Add(new SloaneSViperfishActive(idx, bleeding));
                        }
                    }
                }
                else
                {
                    //query for AI in here
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}