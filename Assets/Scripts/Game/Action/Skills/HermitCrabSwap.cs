using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class HermitCrabSwap : Action, ISkills
    {
        [MemoryPackConstructor]
        private HermitCrabSwap()
        {
        }

        public HermitCrabSwap(PieceLogic maker, PieceLogic to) : base(maker, to)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Swap(GetFrom(), GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            // var board = PieceBoard();
            // int a = Maker;
            // int b = Target;

            // var pieceA = board[a];
            // var pieceB = board[b];

            // board[a] = pieceB;
            // if (pieceB != null) pieceB.Pos = a;
            // board[b] = pieceA;
            // if (pieceA != null) pieceA.Pos = b;
            // ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));

            Swap(GetMakerAsPiece(), GetTargetAsPiece());
            SetCooldown(GetTargetAsPiece(), ((IPieceWithSkill)GetTargetAsPiece()).TimeToCooldown);
        }
    }
}