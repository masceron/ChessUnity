using System.Collections.Generic;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.AI.Consider
{
    [CreateAssetMenu(menuName = "AI/Considerations/Control")]
    public class ControlConsiderationSO : ConsiderationSO
    {
        private const int ThreatenedTilePenalty = 10;

        public override float Score(Action.Action action, List<Action.Action> allyActions,
            List<Action.Action> enemyActions, int weight, PieceLogic maker)
        {
            if (action is not IQuiets) return 0f;

            var value = weight + TileManager.Ins.GetTileValue(action.Target);
            foreach (var ea in enemyActions)
            {
                //if (ea.Target == action.Target && ea is ICaptures or ISkills) value -= threatenedTilePenalty;
                if (ea.Target == action.Target && ea is ICaptures)
                    value = PenaltyAction.PenaltyMoveToCapture(maker, value);

                if (ea.Target == action.Target && ea is ISkills skills)
                    value = PenaltyAction.PenaltyMoveToSkill(skills, maker, value);
            }

            return value;
        }
    }
}