using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class GoldenBasslet : Commons.PieceLogic, IPieceWithSkill
    {
        private int timeToCooldown;
        private const int BlindedDuration = 2;

        public GoldenBasslet(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets, RookMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Shield(this)));
            SetStat(SkillStat.Duration, BlindedDuration);
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    switch (Color)
                    {
                        case false:
                            foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, 1))
                            {
                                var targetPos = IndexOf(rankOff, fileOff);
                                if (!IsActive(targetPos)) continue;
                                var targetPiece = PieceOn(targetPos);
                                // if (targetPiece == null || targetPiece.Color == Color) continue;
                                list.Add(new GoldenBassletActive(Pos, targetPos));
                            }
                            break;
                        case true:
                            foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, 1))
                            {
                                var targetPos = IndexOf(rankOff, fileOff);
                                if (!IsActive(targetPos)) continue;
                                var targetPiece = PieceOn(targetPos);
                                // if (targetPiece == null || targetPiece.Color == Color) continue;
                                list.Add(new GoldenBassletActive(Pos, targetPos));
                            }
                            break;
                    }
                }
                else
                {
                    //query for AI in here
                    if (excludeEmptyTile)
                    {
                        
                    }
                    else
                    {
                        
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}