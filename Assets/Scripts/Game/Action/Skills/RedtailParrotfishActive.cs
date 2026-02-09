using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RedtailParrotfishActive: Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }
        private readonly int formationPos;
        private readonly int moveTo;
        public RedtailParrotfishActive(int maker, int formationPos, int moveTo) : base(maker)
        {
            Maker = (ushort)maker;
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