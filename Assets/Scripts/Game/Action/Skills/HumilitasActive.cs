using Game.Piece.PieceLogic;
using Game.Action;
using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Effects.Buffs;
using Game.Effects.Others;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumilitasActive: Action, ISkills
    {
        public HumilitasActive(int maker) : base(maker)
        {
            Target = (ushort)maker;
        }
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new SeaTurtleCountdown(2, PieceOn(Maker))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

    }
}
