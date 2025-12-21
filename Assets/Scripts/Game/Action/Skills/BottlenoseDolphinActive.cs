using System.Collections.Generic;
using Game.Action.Internal;
using Game.AI;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BottlenoseDolphinActive: Action, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        public BottlenoseDolphinActive(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            if (PieceOn(Target).Color != PieceOn(Maker).Color) 
            {

                ActionManager.EnqueueAction(new ApplyEffect(new Silenced(PieceOn(Target))));
            } 
            else
            {
                SetCooldown(Target, 0);
            }
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            var A = new List<(int pos, int deltaCooldown)>();
            var B = new List<(int pos, int deltaCooldown)>();
            for (int i = 0; i < BoardSize; ++i)
            {
                var piece = PieceOn(i);
                if (piece == null) continue;
                if (piece is not IPieceWithSkill) continue;
                
                var deltaCooldown = ((IPieceWithSkill)PieceOn(i)).TimeToCooldown - piece.SkillCooldown;
                if (piece.Color == PieceOn(Maker).Color)
                    A.Add( (i, deltaCooldown) );
                else 
                    B.Add( (i, deltaCooldown) );
            }

            if (A.Count > 0 && B.Count > 0)
            {
                int type = UnityEngine.Random.Range(0, 1);
                if (type == 0)
                {
                    int idx = UnityEngine.Random.Range(0, A.Count - 1);
                    SetCooldown(PieceOn(A[idx].pos).Pos, 0);
                }
                else
                {
                    int idx = UnityEngine.Random.Range(0, B.Count - 1);
                    ActionManager.EnqueueAction(new ApplyEffect(new Silenced(PieceOn(B[idx].pos))));
                }
            }
            else if (A.Count > 0)
            {
                int idx = UnityEngine.Random.Range(0, A.Count - 1);
                SetCooldown(PieceOn(A[idx].pos).Pos, 0);
            }
            else if (B.Count > 0)
            {
                int idx = UnityEngine.Random.Range(0, B.Count - 1);
                ActionManager.EnqueueAction(new ApplyEffect(new Silenced(PieceOn(B[idx].pos))));
            }
            
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}
