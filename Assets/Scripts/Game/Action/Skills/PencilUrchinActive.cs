using MemoryPack;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class PencilUrchinActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private PencilUrchinActive() { }

        public PencilUrchinActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }

        protected override void ModifyGameState()
        {
            SetFormation(Target, new UrchinField(false, PieceOn(Maker).Color));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}