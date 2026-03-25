using Game.Action.Internal;
using Game.Action.Quiets;
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
    public partial class EmeraldCrabActive : Action, ISkills
    {

        [MemoryPackConstructor]
        private EmeraldCrabActive()
        {
        }

        public List<int> selectedTarget;
        private int Duration;

        public EmeraldCrabActive(int maker, List<int> listTarget, int duration) : base(maker)
        {
            Maker = maker;
            selectedTarget = listTarget;
            Duration = duration;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            foreach (var pos in selectedTarget)
            {
                var piece = PieceOn(pos);
                ActionManager.EnqueueAction(new ApplyEffect(new Ethereal(Duration, piece)));
            }
        }
    }
}