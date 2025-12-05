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
                if (piece == null || piece.Color != PieceOn(Maker).Color) continue;
            }
        }
    }
}
