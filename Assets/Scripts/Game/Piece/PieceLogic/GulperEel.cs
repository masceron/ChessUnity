using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GulperEel : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range = 1;
        public GulperEel(PieceConfig cfg) : base(cfg, FlyingFishMoves.Quiets, FlyingFishMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    for (var dr = -Range; dr <= Range; dr++)
                    {
                        var trank = rank + dr;
                        if (!VerifyBounds(trank)) continue;
                        for (var df = -Range; df <= Range; df++)
                        {
                            var fileOff = file + df;
                            if (!VerifyBounds(fileOff)) continue;
                            var tpos = IndexOf(trank, fileOff);
                            var pieceAt = PieceOn(tpos);
                            if (pieceAt != null) continue;

                            list.Add(new GulperEelActive(this, tpos));
                        }
                    }
                }
                //query for AI in here
            };

            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }

        public SkillsDelegate Skills { get; set; }
        int IPieceWithSkill.TimeToCooldown { get; set; }
    }
}