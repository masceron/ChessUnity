using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    public class BlueDragonActive : Action, ISkills
    {
        public BlueDragonActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Poison(1, PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        
    }
}