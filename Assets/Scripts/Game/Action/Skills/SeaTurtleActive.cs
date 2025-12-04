using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;


namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaTurtleActive: Action, ISkills
    {
        public int AIPenaltyValue => 0;
        public SeaTurtleActive(int maker) : base(maker)
        {
            Target = (ushort)maker;
        }
        protected override void Animate()
        {
            
        }
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new SeaTurtleCountdown(2, PieceOn(Maker))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

    }

}