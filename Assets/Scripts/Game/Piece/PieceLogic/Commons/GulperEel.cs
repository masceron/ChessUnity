using Game.Movesets;
using Game.Action;
using Game.Effects.Traits;
using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Action.Skills;

namespace Game.Piece.PieceLogic.Commons
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GulperEel : PieceLogic, IPieceWithSkill
    {
        public GulperEel(PieceConfig cfg) : base(cfg, FlyingFishMoves.Quiets, FlyingFishMoves.Captures)
        {
            Skills = list =>
            {
                if (SkillCooldown > 0) return;

                var (rank, file) = RankFileOf(Pos);
                for (var dr = -1; dr <= 1; dr++)
                {
                    var trank = rank + dr;
                    if (!VerifyBounds(trank)) continue;
                    for (var df = -1; df <= 1; df++)
                    {
                        var fileOff = file + df;
                        if (!VerifyBounds(fileOff)) continue;
                        var tpos = IndexOf(trank, fileOff);
                        var pieceAt = PieceOn(tpos);
                        if (pieceAt != null) continue;

                        list.Add(new GulperEelActive(Pos, tpos));
                    }
                }


            };
            
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }

        public SkillsDelegate Skills { get; set; }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }

}
