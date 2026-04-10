using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class FlyingGurnardActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private FlyingGurnardActive()
        {
        }

        public FlyingGurnardActive(PieceLogic maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return -40;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(GetFrom());
            var color = GetMakerAsPiece().Color;
            var push = color ? 1 : -1;

            var frontRank = rank + push;
            if (VerifyBounds(frontRank))
            {
                var frontIndex = IndexOf(frontRank, file);
                if (VerifyIndex(frontIndex))
                {
                    var pOn = PieceOn(frontIndex);
                    if (pOn != null && pOn.Color != color)
                        ActionManager.EnqueueAction(new ApplyEffect(new Pacified(3, pOn)));
                    var siltCloud = new SiltCloud(color);
                    siltCloud.SetDuration(3);
                    SetFormation(frontIndex, siltCloud);
                }
            }

            var backRank = rank - push;
            if (VerifyBounds(backRank))
            {
                var backIndex = IndexOf(backRank, file);
                if (VerifyIndex(backIndex))
                {
                    var pOn = PieceOn(backIndex);
                    if (pOn != null && pOn.Color != color)
                        ActionManager.EnqueueAction(new ApplyEffect(new Pacified(3, pOn)));
                    var siltCloud = new SiltCloud(color);
                    siltCloud.SetDuration(3);
                    SetFormation(backIndex, siltCloud);
                }
            }

            var leftFile = file - 1;
            if (VerifyBounds(leftFile))
            {
                var leftIndex = IndexOf(rank, leftFile);
                if (VerifyIndex(leftIndex))
                {
                    var pOn = PieceOn(leftIndex);
                    if (pOn != null && pOn.Color != color)
                        ActionManager.EnqueueAction(new ApplyEffect(new Pacified(3, pOn)));
                    var siltCloud = new SiltCloud(color);
                    siltCloud.SetDuration(3);
                    SetFormation(leftIndex, siltCloud);
                }
            }

            var rightFile = file + 1;
            if (VerifyBounds(rightFile))
            {
                var rightIndex = IndexOf(rank, rightFile);
                if (VerifyIndex(rightIndex))
                {
                    var pOn = PieceOn(rightIndex);
                    if (pOn != null && pOn.Color != color)
                        ActionManager.EnqueueAction(new ApplyEffect(new Pacified(3, pOn)));
                    var siltCloud = new SiltCloud(color);
                    siltCloud.SetDuration(3);
                    SetFormation(rightIndex, siltCloud);
                }
            }

            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}