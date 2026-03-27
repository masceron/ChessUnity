using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

// <-- thêm để dùng LINQ

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class BlackPrinceCopepodActive : Action, ISkills
    {
        public int firstIndex = -1;
        public int secondIndex = -1;
        public int thirdIndex = -1;

        [MemoryPackConstructor]
        private BlackPrinceCopepodActive()
        {
        }

        public BlackPrinceCopepodActive(int maker, int firstIndex, int secondIndex, int thirdIndex) : base(maker)
        {
            this.firstIndex = firstIndex;
            this.secondIndex = secondIndex;
            this.thirdIndex = thirdIndex;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new NormalMove(Maker, firstIndex));

            if (PieceOn(secondIndex) != null)
                ActionManager.EnqueueAction(new ApplyEffect(new Illusion(PieceOn(secondIndex))));
            else
            {
                ActionManager.EnqueueAction(new SpawnPiece(new Piece.PieceConfig("piece_illusion_piece", PieceOn(Maker).Color, secondIndex)));
            }

            if (PieceOn(thirdIndex) != null)
                ActionManager.EnqueueAction(new ApplyEffect(new Illusion(PieceOn(thirdIndex))));
            else
            {
                ActionManager.EnqueueAction(new SpawnPiece(new Piece.PieceConfig("piece_illusion_piece", PieceOn(Maker).Color, thirdIndex)));
            }
        }
    }
}