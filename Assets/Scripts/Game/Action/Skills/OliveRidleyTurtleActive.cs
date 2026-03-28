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

        public OliveRidleyTurtleActive(int maker, int to) : base(maker)
        {
            Target = to;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var caller = GetMaker();
            if (caller == null) return;

            if (!VerifyIndex(Target) || !IsActive(Target)) return;
            if (GetTarget() != null) return;

            ActionManager.EnqueueAction(new SpawnPiece(
                new PieceConfig("piece_olive_ridley_eggs", caller.Color, Target)
            ));

            SetCooldown(GetMaker(), ((IPieceWithSkill)caller).TimeToCooldown);
        }
    }
}

