
using System.Collections.Generic;
using Game.Action.Internal;
using Game.AI;
using Game.Common;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    public class EpauletteSharkActive : Action, ISkills, IAIAction
    {
        public EpauletteSharkActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(Target));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        
        public void CompleteActionForAI()
        {
            List<PieceLogic> bestPieces = new List<PieceLogic>();
            PieceLogic bestPiece = null;
            int maxPoint = int.MinValue;
            
            var (rank, file) = RankFileOf(Maker);

            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null || pOn.Pos == Maker || pOn.PieceRank != PieceRank.Swarm 
                    || pOn.Color == PieceOn(Maker).Color) continue;
                
                int AIValue = pOn.GetValueForAI();
                if (AIValue > maxPoint)
                {
                    bestPieces.Clear();
                    bestPieces.Add(pOn);
                    maxPoint = AIValue;
                }
                else if (AIValue == maxPoint) bestPieces.Add(pOn);
            }

            if (bestPieces.Count == 0)
            {
                //
            }
            else if (bestPieces.Count == 1)
            {
                bestPiece = bestPieces[0];
            }
            else
            {
                bestPiece = bestPieces[UnityEngine.Random.Range(0, bestPieces.Count)];
            }

            if (bestPiece != null)
            {
                ActionManager.EnqueueAction(new KillPiece(bestPiece.Pos));
                SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
            }
        }   
    }
}