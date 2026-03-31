using Game.Action.Internal;
using Game.Effects.Others;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class BrineShrimpActive : Action, ISkills
    {

        [MemoryPackConstructor]
        private BrineShrimpActive()
        {
        }
        [MemoryPackInclude]
        private int _duration;

        public BrineShrimpActive(PieceLogic maker, int duration) : base(maker)
        {
            _duration = duration;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var petrified = new Petrified(_duration, GetMakerAsPiece());
            ActionManager.EnqueueAction(new ApplyEffect(petrified));
            ActionManager.EnqueueAction(new ApplyEffect(new BrineShrimpSummon(GetMakerAsPiece(), _duration)));
        }
    }
}