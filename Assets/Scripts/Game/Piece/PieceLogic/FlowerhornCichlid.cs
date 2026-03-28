using System;
using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class FlowerhornCichlid : Commons.PieceLogic, IPieceWithSkill
    {
        public FlowerhornCichlid(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            Skills = (list, isPlayer, _) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    if (Color)
                        for (var rank = RankOf(Pos) + 1; rank <= Math.Min(MaxLength - 1, RankOf(Pos) + 3); ++rank)
                        {
                            if (!IsActive(IndexOf(rank, FileOf(Pos)))) return;
                            var target = IndexOf(rank, FileOf(Pos));
                            var pieceOn = PieceOn(target);
                            if (pieceOn != null)
                            {
                                if (pieceOn.Color != Color) list.Add(new FlowerhornCichlidActive(Pos, target));
                                return;
                            }

                            list.Add(new FlowerhornCichlidActive(Pos, target));
                        }
                    else
                        for (var rank = RankOf(Pos) - 1; rank >= Math.Max(0, RankOf(Pos) - 3); --rank)
                        {
                            if (!IsActive(IndexOf(rank, FileOf(Pos)))) return;
                            var target = IndexOf(rank, FileOf(Pos));
                            var pieceOn = PieceOn(target);
                            if (pieceOn != null)
                            {
                                if (pieceOn.Color != Color) list.Add(new FlowerhornCichlidActive(Pos, target));
                                return;
                            }

                            list.Add(new FlowerhornCichlidActive(Pos, target));
                        }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}