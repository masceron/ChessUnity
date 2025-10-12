using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BottlenoseDolphinActive: Action, ISkills
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

    }
}
