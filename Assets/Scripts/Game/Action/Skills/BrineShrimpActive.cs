using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Others;
using Game.Effects.States;
using Game.Effects.Traits;
using Game.Piece.PieceLogic;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using System.Collections.Generic;
using static Game.Common.BoardUtils;

// <-- thêm để dùng LINQ

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
        private int Duration;

        public BrineShrimpActive(int maker, int duration) : base(maker)
        {
            Maker = maker;
            Duration = duration;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Petrified(Duration, PieceOn(Maker))));
            ActionManager.EnqueueAction(new ApplyEffect(new BrineShrimpSummon(PieceOn(Maker))));
        }
    }
}