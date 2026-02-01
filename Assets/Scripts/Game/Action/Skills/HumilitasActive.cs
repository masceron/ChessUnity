using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece;
using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic.Commons;
using Game.Action.Internal.Pending.Piece;
using Game.Effects.Debuffs;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumilitasActive : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }
        public static int FirstTarget;
        public static int SecondTarget;

        public HumilitasActive(int maker, int firstTarget, int secondTarget) : base(maker)
        {
            Maker = (ushort)maker;
            FirstTarget = firstTarget;
            SecondTarget = secondTarget;
        }
        protected override void ModifyGameState()
        {
            UnityEngine.Debug.Log("Executing HumilitasActive");
            var first = PieceOn(FirstTarget);
            var second = PieceOn(SecondTarget);
            if (first != null)
                ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, first), PieceOn(Maker)));
            if (second != null)
                ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, second), PieceOn(Maker)));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

    }
}