using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class AnglerfishTaunt: Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -10;
            return 0;
        }

        public AnglerfishTaunt(int maker, int to) : base(maker)
        {
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Taunted(3, PieceOn(Target))));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}