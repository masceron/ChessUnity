using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GulperEel : Commons.PieceLogic, IPieceWithSkill
    {
        public GulperEel(PieceConfig cfg) : base(cfg, FlyingFishMoves.Quiets, FlyingFishMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
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
                }
                else
                {
                    //query for AI in here
                }
            };

            //ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }

        public SkillsDelegate Skills { get; set; }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }

}
