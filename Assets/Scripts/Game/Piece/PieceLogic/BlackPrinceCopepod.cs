using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Effects.Buffs;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using UnityEngine;

namespace Game.Piece.PieceLogic
{
    public class BlackPrinceCopepod : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range2 = 4;
        public BlackPrinceCopepod(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Sanity(-1, this)));
            ActionManager.EnqueueAction(new ApplyEffect(new HardenedShield(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.EnqueueAction(new ApplyEffect(new BlackPrinceCopepodPassive(this)));

            SetStat(SkillStat.Range, Range2, 2);

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    for (var x = rank - GetStat(SkillStat.Range, 2); x <= rank + GetStat(SkillStat.Range, 2); ++x)
                        for (var y = file - GetStat(SkillStat.Range, 2); y <= file + GetStat(SkillStat.Range, 2); ++y)
                        {
                            if (!VerifyBounds(x) || !VerifyBounds(y)) continue;
                            var targetPiece = PieceOn(IndexOf(x, y));
                            if (targetPiece != null) continue;

                            Debug.Log($"Adding skill from {Pos} to {IndexOf(x, y)}");
                            list.Add(new BlackPrinceCopepodPending(Pos, IndexOf(x, y)));
                        }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}