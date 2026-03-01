using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class RedCopepod : Commons.PieceLogic, IPieceWithSkill
    {
        public RedCopepod(PieceConfig cfg) : base(cfg, KingMoves.Quiets, None.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Sanity(-1, this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    for (var x = rank - 2; x <= rank + 2; ++x)
                    for (var y = file - 2; y <= file + 2; ++y)
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