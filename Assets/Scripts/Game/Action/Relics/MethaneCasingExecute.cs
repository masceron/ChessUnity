using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
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

        public MethaneCasingExecute(int maker) : base(maker)
        {
        }

        protected override void ModifyGameState()
        {
            var pieceOn = GetMaker() as PieceLogic;
            ActionManager.EnqueueAction(new Purify(GetFrom(), GetFrom()));
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(3, pieceOn)));
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(BoardUtils.RankOf(pieceOn.Pos),
                         BoardUtils.FileOf(pieceOn.Pos), 1))
            {
                var ind = BoardUtils.IndexOf(rankOff, fileOff);
                if (TileManager.Ins.IsTileEmpty(BoardUtils.IndexOf(rankOff, fileOff))) continue;
                if (BoardUtils.IndexOf(rankOff, fileOff) == pieceOn.Pos) continue;
                if (BoardUtils.PieceOn(ind) == null && BoardUtils.GetFormation(ind) == null)
                    TileManager.Ins.DestroyTile(rankOff, fileOff);
            }
        }
    }
}