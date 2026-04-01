using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class RedtailParrotfishActive : Action, ISkills
    {
        [MemoryPackInclude] private int _formationPos;

        [MemoryPackInclude] private int _moveTo;

        [MemoryPackConstructor]
        private RedtailParrotfishActive()
        {
        }

        public RedtailParrotfishActive(PieceLogic maker, Formation formationPos, int moveTo) : base(maker)
        {
            _formationPos = formationPos.Pos;
            _moveTo = moveTo;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            MoveFormation(_formationPos, _moveTo);
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}