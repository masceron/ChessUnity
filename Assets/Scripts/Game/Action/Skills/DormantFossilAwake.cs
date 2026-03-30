using Game.Action.Internal;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class DormantFossilAwake : Action, ISkills
    {
        [MemoryPackInclude] private PieceConfig _target;

        [MemoryPackConstructor]
        private DormantFossilAwake()
        {
        }

        public DormantFossilAwake(PieceLogic maker, PieceConfig t) : base(maker)
        {
            _target = t;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new DestroyPiece(GetMakerAsPiece()));
            ActionManager.EnqueueAction(new SpawnPiece(_target));
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}