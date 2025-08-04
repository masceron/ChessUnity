using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class AnomalocarisActive: Action, ISkills
    {
        public AnomalocarisActive(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(1, PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}