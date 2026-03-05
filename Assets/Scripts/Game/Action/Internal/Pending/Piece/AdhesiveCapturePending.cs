using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using UX.UI.Ingame;
using UX.UI.Ingame.AdhesiveChoice;

namespace Game.Action.Internal.Pending.Piece
{
    /// <summary>
    ///     PendingAction đặt trong MoveList thay cho capture action khi ô target có Formation (StateType.None).<br/>
    ///     Implement <see cref="ICaptures"/> để được hiển thị trong tab Attack của <see cref="PieceActions"/>.<br/><br/>
    ///     Khi người chơi chọn ô:
    ///     <list type="bullet">
    ///         <item>Cả Piece lẫn Formation: <see cref="AdhesiveChoiceUI"/> hiện 2 nút lựa chọn.</item>
    ///         <item>Chỉ có Formation: <see cref="AdhesiveChoiceUI"/> hiện 1 nút (Formation).</item>
    ///     </list>
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AdhesiveCapturePending : PendingAction, ICaptures
    {
        private readonly PieceLogic _adhesivePiece;
        private readonly PieceLogic _targetPiece;     // null nếu ô không có Piece
        private readonly Formation  _targetFormation; // không null (luôn có Formation)

        /// <param name="maker">Vị trí quân Adhesive.</param>
        /// <param name="target">Vị trí ô target.</param>
        /// <param name="adhesivePiece">Logic của quân Adhesive.</param>
        /// <param name="targetPiece">Logic Piece trên ô (null nếu không có).</param>
        /// <param name="targetFormation">Formation trên ô (không được null).</param>
        public AdhesiveCapturePending(
            int        maker,
            int        target,
            PieceLogic adhesivePiece,
            PieceLogic targetPiece,
            Formation  targetFormation)
            : base(maker)
        {
            Target           = target;
            _adhesivePiece   = adhesivePiece;
            _targetPiece     = targetPiece;
            _targetFormation = targetFormation;
        }

        protected override void CompleteAction()
        {
            var ui = BoardViewer.Ins.GetOrInstantiateUI<AdhesiveChoiceUI>(IngameSubmenus.AdhesiveChoiceUI);
            ui.Load(this);
        }

        /// <summary>Người chơi chọn bám vào Piece.</summary>
        public void ChoosePiece()
        {
            if (_targetPiece == null || _targetPiece.CurrentState != StateType.None) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Attached(_targetPiece, _adhesivePiece)));
            CommitResult(new MoveToAdhesive(Maker, Target, attachToFormation: false));
        }

        /// <summary>Người chơi chọn bám vào Formation.</summary>
        public void ChooseFormation()
        {
            _targetFormation.SetState(StateType.Attached);
            var adhesive   = _adhesivePiece;
            var formation  = _targetFormation;
            formation.OnRemoveFormation += (formation) =>
            {
                formation.ClearState();
                Attached.SpawnAdhesiveAround(formation.Pos, null, adhesive);
            };
            CommitResult(new MoveToAdhesive(Maker, Target, attachToFormation: true));
        }
    }
}
