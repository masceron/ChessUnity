using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Tile;
using Game.Effects.Debuffs;
using Game.Action.Internal;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FlyingGurnardActive: Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return -40;
        }
        public FlyingGurnardActive(int maker) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)maker;
        }
        protected override void Animate()
        {
        }
        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var push = PieceOn(Maker).Color ? 1 : -1;
            
            var frontRank = rank + push;
            if (VerifyBounds(frontRank))
            {
                var frontIndex = IndexOf(frontRank, file);
                if (VerifyIndex(frontIndex))
                {
                    var pOn = PieceOn(frontIndex);
                    if (pOn != null && pOn.Color != PieceOn(Maker).Color)
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Pacified(3, pOn)));
                    }
                    SiltCloud siltCloud = new SiltCloud(PieceOn(Maker).Color);
                    siltCloud.SetDuration(3);
                    FormationManager.Ins.SetFormation(frontIndex, siltCloud);
                }
            }
            
            var backRank = rank - push;
            if (VerifyBounds(backRank))
            {
                var backIndex = IndexOf(backRank, file);
                if (VerifyIndex(backIndex))
                {
                    var pOn = PieceOn(backIndex);
                    if (pOn != null && pOn.Color != PieceOn(Maker).Color)
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Pacified(3, pOn)));
                    }
                    SiltCloud siltCloud = new SiltCloud(PieceOn(Maker).Color);
                    siltCloud.SetDuration(3);
                    FormationManager.Ins.SetFormation(backIndex, siltCloud);
                }
            }
            
            var leftFile = file - 1;
            if (VerifyBounds(leftFile))
            {
                var leftIndex = IndexOf(rank, leftFile);
                if (VerifyIndex(leftIndex))
                {
                    var pOn = PieceOn(leftIndex);
                    if (pOn != null && pOn.Color != PieceOn(Maker).Color)
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Pacified(3, pOn)));
                    }
                    SiltCloud siltCloud = new SiltCloud(PieceOn(Maker).Color);
                    siltCloud.SetDuration(3);
                    FormationManager.Ins.SetFormation(leftIndex, siltCloud);
                }
            }
            
            var rightFile = file + 1;
            if (VerifyBounds(rightFile))
            {
                var rightIndex = IndexOf(rank, rightFile);
                if (VerifyIndex(rightIndex))
                {
                    var pOn = PieceOn(rightIndex);
                    if (pOn != null && pOn.Color != PieceOn(Maker).Color)
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Pacified(3, pOn)));
                    }
                    SiltCloud siltCloud = new SiltCloud(PieceOn(Maker).Color);
                    siltCloud.SetDuration(3);
                    FormationManager.Ins.SetFormation(rightIndex, siltCloud);
                }
            }
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}   