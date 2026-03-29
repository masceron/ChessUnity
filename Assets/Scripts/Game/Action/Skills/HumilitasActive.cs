using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class HumilitasActive : Action, ISkills
    {
        public static int FirstTarget;
        public static int SecondTarget;

        [MemoryPackConstructor]
        private HumilitasActive()
        {
        }

        public HumilitasActive(int maker, int firstTarget, int secondTarget) : base(maker)
        {
            FirstTarget = firstTarget;
            SecondTarget = secondTarget;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Executing HumilitasActive");
            var first = PieceOn(FirstTarget);
            var second = PieceOn(SecondTarget);
            if (first != null)
                ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, first), GetMaker() as PieceLogic));
            if (second != null)
                ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, second), GetMaker() as PieceLogic));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}