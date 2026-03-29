using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class RustyParrotfishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private RustyParrotfishActive()
        {
        }

        public RustyParrotfishActive(int maker, int to) : base(maker, to)
        {
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            FormationManager.Ins.RemoveFormation(GetTargetPos());
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker() as PieceLogic).TimeToCooldown);
        }
    }
}