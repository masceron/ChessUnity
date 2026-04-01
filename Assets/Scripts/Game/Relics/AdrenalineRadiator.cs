using Game.Common;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AdrenalineRadiator : RelicLogic
    {
        public AdrenalineRadiator(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown != 0) return;
            foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
            {
                if (piece == null || piece.Color != Color) continue;
                if (BoardUtils.IsOnBlackSide(piece.Pos) == Color) continue;
                TileManager.Ins.MarkAsMoveable(piece.Pos);
                //Làm lại
                //var pending = new AdrenalineRadiatorPending(this);
                //BoardViewer.ListOf.Add(pending);
            }

            BoardViewer.Selecting = -2;
            BoardViewer.SelectingFunction = 4;
        }

        public override void ActiveForAI()
        {
        }
    }
}