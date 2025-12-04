using Game.Action.Skills;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.AI.Consider
{
    public static class PenaltyAction
    {
        public static int PenaltyMoveToCapture(PieceLogic maker, int currentValue)
        {
            currentValue -= PenaltyPieceRank(maker.PieceRank);
            return currentValue;

            int PenaltyPieceRank(PieceRank rank)
            {
                return rank switch
                {
                    PieceRank.Swarm => 10,
                    PieceRank.Common => 20,
                    PieceRank.Summoned => 30,
                    PieceRank.Elite => 40,
                    PieceRank.Champion => 70,
                    PieceRank.Commander => 800,
                    _ => 0
                };
            }
        }

        public static int PenaltyMoveToSkill(ISkills enemySkill, int currentValue)
        {
            currentValue += enemySkill.AIPenaltyValue;
            return currentValue;
        }
    }
}
