using MemoryPack;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class RedtailParrotfishActive: Action, ISkills
    {
        [MemoryPackConstructor]
        private RedtailParrotfishActive() { }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }
        [MemoryPackInclude]
        private int formationPos;
        [MemoryPackInclude]
        private int moveTo;
        public RedtailParrotfishActive(int maker, int formationPos, int moveTo) : base(maker)
        {
            Maker = maker;
            this.formationPos = formationPos;
            this.moveTo = moveTo;
        }
        protected override void ModifyGameState()
        {
            MoveFormation(formationPos, moveTo);
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}