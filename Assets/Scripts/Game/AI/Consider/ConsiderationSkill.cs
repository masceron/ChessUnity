using System.Collections.Generic;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.AI.Consider
{
    [CreateAssetMenu(menuName = "AI/Considerations/Skill")]
    public class SkillConsiderationSO : ConsiderationSO
    {
        public override float Score(Action.Action action, List<Action.Action> allyActions,
            List<Action.Action> enemyActions, int weight, PieceLogic maker)
        {
            if (action is not ISkills) return 0f;

            float score = weight;
            var targetPiece = action.GetTarget();

            if (targetPiece != null) score += targetPiece.GetValueForAI();

            return score;
        }
    }
}