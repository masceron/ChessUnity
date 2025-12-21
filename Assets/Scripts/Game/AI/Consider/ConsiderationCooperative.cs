using System.Collections.Generic;
using Game.Action.Quiets;
using UnityEngine;
using Game.Piece.PieceLogic.Commons;
using Game.Common;
using Game.Managers;
using Game.Action.Captures;
using Game.Action.Skills;

namespace Game.AI.Consider
{
    [CreateAssetMenu(menuName = "AI/Considerations/Cooperative")]
    public class CooperativeConsiderationSO : ConsiderationSO
    {
        private const int threatenedTilePenalty = 10;
        public override float Score(Action.Action action, List<Action.Action> allyActions, List<Action.Action> enemyActions, int weight, PieceLogic maker)
        {
            if (action == null || action is not IQuiets) return 0f;

            float scaleValue = 1f;
            
            var (targetRank, targetFile) = BoardUtils.RankFileOf(action.Target);

            // đếm số Ally Piece ở quanh action.Target, mỗi Ally Piece xung quanh giúp tăng scaleValue lên 0.25f
            for (int r = targetRank - 1; r <= targetRank + 1; r++)
            {
                for (int f = targetFile - 1; f <= targetFile + 1; f++)
                {
                    if (r == targetRank && f == targetFile)
                        continue;

                    if (BoardUtils.VerifyBounds(r) && BoardUtils.VerifyBounds(f))
                    {
                        var piece = BoardUtils.PieceOn(BoardUtils.IndexOf(r, f));
                        if (piece != null && piece.Color == maker.Color)
                        {
                            scaleValue += 0.25f;
                        }
                    }
                }
            }

            int value = Mathf.FloorToInt(scaleValue * weight + TileManager.Ins.GetTileValue(action.Target));
            foreach (var ea in enemyActions)
            {
                //if (ea.Target == action.Target && ea is ICaptures or ISkills) value -= threatenedTilePenalty;
                if (ea.Target == action.Target && ea is ICaptures)
                {
                    value = PenaltyAction.PenaltyMoveToCapture(maker, value);
                }

                if (ea.Target == action.Target && ea is ISkills skills)
                {
                    value = PenaltyAction.PenaltyMoveToSkill(skills, maker, value);
                }
            }
            return value;
        }
    }
}
