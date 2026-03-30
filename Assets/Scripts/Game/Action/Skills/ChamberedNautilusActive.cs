using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class ChamberedNautilusActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private ChamberedNautilusActive()
        {
        }

        public ChamberedNautilusActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            if (GetMaker() is not PieceLogic maker) return 0;
            return pieceAI.Color != maker.Color ? -5 : 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(1, GetTarget() as PieceLogic), GetMaker() as PieceLogic));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}