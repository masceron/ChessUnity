using Game.Action.Internal;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class ThalassosResurrect : Action, ISkills
    {
        [MemoryPackInclude] private string _typeTo;

        [MemoryPackConstructor]
        private ThalassosResurrect()
        {
        }

        public ThalassosResurrect(PieceLogic maker, int to, string typeTo) : base(maker, to)
        {
            _typeTo = typeTo;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var caller = GetMakerAsPiece();
            ActionManager.EnqueueAction(new Resurrect(caller, new PieceConfig(_typeTo, caller.Color, GetTargetPos())));
            ActionManager.EnqueueAction(new CooldownSkill(caller));
        }
    }
}