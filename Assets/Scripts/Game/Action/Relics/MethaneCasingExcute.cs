using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Relics
{
    public class MethaneCasingExcute : Action, IRelicAction
    {
        public MethaneCasingExcute(int maker) : base(maker)
        {
        }

        protected override void ModifyGameState()
        {
            PieceLogic pieceOn = BoardUtils.PieceOn(Maker);
            ActionManager.EnqueueAction(new Purify(Maker, Maker));
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(3, pieceOn)));
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(BoardUtils.RankOf(pieceOn.Pos), BoardUtils.FileOf(pieceOn.Pos), 1))
            {
                int ind = BoardUtils.IndexOf(rankOff, fileOff);
                if (TileManager.Ins.IsTileEmpty(BoardUtils.IndexOf(rankOff, fileOff))){ continue; }
                if (BoardUtils.IndexOf(rankOff, fileOff) == pieceOn.Pos) { continue; }
                if (BoardUtils.PieceOn(ind) == null && FormationManager.Ins.GetFormation(ind) == null)
                {
                    TileManager.Ins.DestroyTile(rankOff, fileOff);
                }
            }
        }
    }
}