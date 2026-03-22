using Game.Action.Internal.Pending;
using Game.Action.Internal.Pending.Piece;

namespace UX.UI.Ingame.AdhesiveChoice
{
    /// <summary>
    ///     UI popup hiển thị 2 nút "Piece" và "Formation" khi quân Adhesive bắt vào ô vừa có Piece vừa có Formation.<br/>
    ///     Kết nối với <see cref="AdhesiveChoicePending"/> để CommitResult khi người chơi chọn.
    /// </summary>
    public class AdhesiveChoiceUI : IngamePendingMenu
    {
        protected override PendingAction PendingAction { get; set; }

        private AdhesiveCapturePending _pending;

        /// <summary>Load pending action vào UI.</summary>
        public void Load(AdhesiveCapturePending pending)
        {
            _pending    = pending;
            PendingAction = pending;
            gameObject.SetActive(true);
        }

        /// <summary>Được gọi bởi nút "Piece" trong UI.</summary>
        public void OnClickPiece()
        {
            _pending.ChoosePiece();
            gameObject.SetActive(false);
        }

        /// <summary>Được gọi bởi nút "Formation" trong UI.</summary>
        public void OnClickFormation()
        {
            _pending.ChooseFormation();
            gameObject.SetActive(false);
        }

        public void OnClickCancel()
        {
            BoardViewer.Ins.Unmark();
            gameObject.SetActive(false);
        }
    }
}
