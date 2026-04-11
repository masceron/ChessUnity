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
    public partial class OliveRidleyTurtleActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private OliveRidleyTurtleActive()
        {
        }

        public OliveRidleyTurtleActive(PieceLogic maker, int to) : base(maker, to)
        {
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var caller = GetMakerAsPiece();
            var target = GetTargetAsPiece();
            if (caller == null) return;

            if (!VerifyIndex(target.Pos) || !IsActive(target.Pos)) return;
            if (target != null) return;

            ActionManager.EnqueueAction(new SpawnPiece(
                new PieceConfig("piece_olive_ridley_eggs", caller.Color, target.Pos)
            ));

            ActionManager.EnqueueAction(new CooldownSkill(caller));
        }
    }
}

