using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class RedCopepod : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range = 2;
        public RedCopepod(PieceConfig cfg) : base(cfg, KingMoves.Quiets, None.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Sanity(-1, this)));
            SetStat(SkillStat.Range, Range);

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    for (var x = rank - Range; x <= rank + Range; ++x)
                    for (var y = file - Range; y <= file + Range; ++y)
                    {
                        if (!VerifyBounds(x) || !VerifyBounds(y)) continue;
                        var targetPiece = PieceOn(IndexOf(x, y));
                        if (targetPiece != null) continue;
                        list.Add(new RedCopepodActive(Pos, IndexOf(x, y)));
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}