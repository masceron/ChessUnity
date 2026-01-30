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
    public class MarineIguanaActive : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }
        public static int FirstTarget;
        public static int SecondTarget;

        public MarineIguanaActive(int maker, int firstTarget, int secondTarget) : base(maker)
        {
            Maker = (ushort)maker;
            FirstTarget = firstTarget;
            SecondTarget = secondTarget;
        }
        protected override void ModifyGameState()
        {
            UnityEngine.Debug.Log("Executing MarineIguanaActive");
            var first = PieceOn(FirstTarget);
            var second = PieceOn(SecondTarget);
            if (first != null)
                ActionManager.EnqueueAction(new MarinelKill(Maker, FirstTarget, SecondTarget));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

    }
}