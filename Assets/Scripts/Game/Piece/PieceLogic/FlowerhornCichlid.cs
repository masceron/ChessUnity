
using Game.Movesets;
using Game.Effects.SpecialAbility;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using System;

namespace Game.Piece.PieceLogic
{
    public class FlowerhornCichlid : Commons.PieceLogic, IPieceWithSkill
    {
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        public FlowerhornCichlid(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) { return; }
                if (isPlayer)
                {
                    if (Color)
                    {
                        for (int rank = RankOf(Pos) + 1; rank < Math.Min(MaxLength, RankOf(Pos) + 3); ++rank)
                        {
                            if (IsActive(IndexOf(rank, FileOf(Pos))) == false) { return; }
                            int target = IndexOf(rank, FileOf(Pos));
                            Commons.PieceLogic pieceOn = PieceOn(target);
                            if (pieceOn != null)
                            {
                                if (pieceOn.Color != Color)
                                {
                                    list.Add(new FlowerhornCichlidActive(Pos, target));
                                }
                                return;
                            }
                            else
                            {
                                list.Add(new FlowerhornCichlidActive(Pos, target));
                            }
                        }
                    }
                    else
                    {
                        for (int rank = RankOf(Pos) - 1; rank >= Math.Max(0, RankOf(Pos) - 3); --rank)
                        {
                            if (IsActive(IndexOf(rank, FileOf(Pos))) == false) { return; }
                            int target = IndexOf(rank, FileOf(Pos));
                            Commons.PieceLogic pieceOn = PieceOn(target);
                            if (pieceOn != null)
                            {
                                if (pieceOn.Color != Color)
                                {
                                    list.Add(new FlowerhornCichlidActive(Pos, target));
                                }
                                return;
                            }
                            else
                            {
                                list.Add(new FlowerhornCichlidActive(Pos, target));
                            }
                        }
                    }
                    
                }
            };
        }
    }
}