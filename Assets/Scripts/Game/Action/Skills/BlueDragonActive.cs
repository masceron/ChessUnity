using Game.Action.Internal;
using Game.AI;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    public class BlueDragonActive : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? -15 : 0;
        }

        public BlueDragonActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, PieceOn(Target)), PieceOn(Maker)));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}