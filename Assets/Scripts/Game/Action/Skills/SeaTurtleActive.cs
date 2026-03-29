using Game.Action.Internal;
using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;


namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SeaTurtleActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private SeaTurtleActive()
        {
        }

        public SeaTurtleActive(int maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new SeaTurtleCountdown(2, GetMaker() as PieceLogic)));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker() as PieceLogic).TimeToCooldown);
        }
    }
}