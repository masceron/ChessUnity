using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class MethaneCasingExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private MethaneCasingExecute()
        {
        }

        public MethaneCasingExecute(PieceLogic target) : base(null, target)
        {
        }

        protected override void ModifyGameState()
        {
            var pieceOn = GetTarget() as PieceLogic;
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(3, pieceOn)));
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(BoardUtils.RankOf(GetTargetPos()),
                         BoardUtils.FileOf(GetTargetPos()), 1))
            {
                var ind = BoardUtils.IndexOf(rankOff, fileOff);
                if (TileManager.Ins.IsTileEmpty(BoardUtils.IndexOf(rankOff, fileOff))) continue;
                if (BoardUtils.IndexOf(rankOff, fileOff) == GetTargetPos()) continue;
                if (BoardUtils.PieceOn(ind) == null && BoardUtils.GetFormation(ind) == null)
                    TileManager.Ins.DestroyTile(rankOff, fileOff);
            }
        }
    }
}