using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class RedCopepod : Commons.PieceLogic, IPieceWithSkill
    {
        public RedCopepod(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Sanity(-1, this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0 || FindPiece<Commons.PieceLogic>(!Color).Count == 0) return;
                if (isPlayer)
                {
                    var p = PieceOn(Pos);
                    for (var i = -2; i <= 2; ++i)
                    {
                        if (!VerifyBounds(RankOf(Pos) + i)) continue;
                        for (var j = -2; j <= 2; ++j)
                        {
                            if (!VerifyBounds(FileOf(Pos) + j)) continue;
                            if (PieceOn(Pos) != null) continue;

                            list.Add(new RedCopepodActive(Pos, IndexOf(RankOf(Pos) + i, FileOf(Pos) + j)));
                        }
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}