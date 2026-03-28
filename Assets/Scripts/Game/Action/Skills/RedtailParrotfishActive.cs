using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class RedtailParrotfishActive : Action, ISkills
    {
        [MemoryPackInclude] private int formationPos;

        [MemoryPackInclude] private int moveTo;

        [MemoryPackConstructor]
        private RedtailParrotfishActive()
        {
        }

        public RedtailParrotfishActive(int maker, int formationPos, int moveTo) : base(maker)
        {
            this.formationPos = formationPos;
            this.moveTo = moveTo;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            MoveFormation(formationPos, moveTo);
            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}