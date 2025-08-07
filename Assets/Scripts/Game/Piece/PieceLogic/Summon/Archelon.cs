using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Data.Pieces;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Moves;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Summon
{
    public class Archelon: PieceLogic, IPieceWithSkill
    {
        public Archelon(PieceConfig cfg) : base(cfg, RookMoves.Quiets, RookMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new HardenedShield(this, 3)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new ArchelonDraw(this)));

            Skills = list =>
            {
                if (SkillCooldown != 0) return;

                var (rank, file) = RankFileOf(Pos);

                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var pOn = PieceOn(index);
                    if (pOn == null || pOn == this) continue;
                    if (pOn.Color == Color)
                    {
                        list.Add(new ArchelonShield(Pos, index));
                    }
                }
            };
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list, Pos);
            Captures(list, Pos);
            Skills(list);
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}