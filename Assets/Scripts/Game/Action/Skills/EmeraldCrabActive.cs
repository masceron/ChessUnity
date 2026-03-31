using Game.Action.Internal;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using System.Collections.Generic;
using ZLinq;
using static Game.Common.BoardUtils;

// <-- thêm để dùng LINQ

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class EmeraldCrabActive : Action, ISkills
    {

        [MemoryPackConstructor]
        private EmeraldCrabActive()
        {
        }

        private readonly List<int> _selectedTarget;
        private readonly int _duration;

        public EmeraldCrabActive(PieceLogic maker, List<int> listTarget, int duration) : base(maker)
        {
            _selectedTarget = listTarget;
            _duration = duration;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            foreach (var piece in _selectedTarget.Select(PieceOn))
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Ethereal(_duration, piece)));
            }
        }
    }
}