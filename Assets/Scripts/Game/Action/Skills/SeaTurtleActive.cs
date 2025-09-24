using Game.Action.Internal;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Action;



namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaTurtleActive: Action, ISkills
    {
        public SeaTurtleActive(int maker) : base(maker)
        {
            Target = (ushort)maker;
        }
        protected override void Animate()
        {
            
        }
        protected override void ModifyGameState()
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Shield(PieceOn(Maker))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

    }

}