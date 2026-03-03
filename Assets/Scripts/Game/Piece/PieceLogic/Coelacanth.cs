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
    public class Coelacanth : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Target = 1;
        private const int Range = 4;
        private const int Duration = 1;
        public Coelacanth(PieceConfig cfg) : base(cfg, FrontDefenderMoves.Quiets, None.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new FreeMovement(this)));
            //ActionManager.EnqueueAction(new Regenera
            SetStat(SkillStat.Target, Target);
            SetStat(SkillStat.Range, Range);
            SetStat(SkillStat.Duration, Duration);

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
                            if (targetPiece == null) continue;
                            list.Add(new WhiteMonkfishActive(Pos, IndexOf(x, y), GetStat(SkillStat.Duration)));
                        }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}