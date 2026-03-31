using Game.Action.Captures;
using Game.Action.Quiets;
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

        /// <param name="maker">Vị trí quân Adhesive.</param>
        /// <param name="target">Vị trí ô target.</param>
        /// <param name="adhesivePiece">Logic của quân Adhesive.</param>
        /// <param name="targetPiece">Logic Piece trên ô (null nếu không có).</param>
        /// <param name="targetFormation">Formation trên ô (không được null).</param>
        public AdhesiveCapturePending(
            PieceLogic maker,
            PieceLogic targetPiece,
            Formation targeFormation)
            : base(maker)
        {
        }

        protected override void CompleteAction()
        {
            var ui = BoardViewer.Ins.GetOrInstantiateUI<AdhesiveChoiceUI>(IngameSubmenus.AdhesiveChoiceUI);
            ui.Load(this);
        }

        /// <summary>Người chơi chọn bám vào Piece.</summary>
        public void ChoosePiece()
        {
            if (GetTargetAsPiece() is not { CurrentState: StateType.None }) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Attached(GetTargetAsPiece(), _adhesivePiece)));
            CommitResult(new MoveToAdhesive(GetMakerAsPiece(), GetTargetAsPiece(), attachToFormation: false));
        }

        /// <summary>Người chơi chọn bám vào Formation.</summary>
        public void ChooseFormation()
        {
            var targetFormation = GetTargetAsFormation();
            targetFormation.SetState(StateType.Attached);
            var adhesive = _adhesivePiece;
            targetFormation.OnRemoveFormation += formation =>
            {
                formation.ClearState();
                Attached.SpawnAdhesiveAround(formation.Pos, null, adhesive);
            };
            CommitResult(new MoveToAdhesive(GetMakerAsPiece(), targetFormation, attachToFormation: true));
        }
    }
}