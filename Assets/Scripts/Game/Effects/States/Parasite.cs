using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Parasite</b><br/>
    ///     Khi quân này thực hiện Capture vào quân địch có State <see cref="StateType.None"/>:<br/>
    ///     - Quân này tạm thời biến mất khỏi bàn cờ (di chuyển sang bên cạnh bàn cờ).<br/>
    ///     - Quân địch bị áp State <see cref="StateType.Infested"/>.<br/>
    ///     - Khi quân Infested đó chết: quân Parasite này di chuyển về vị trí ngẫu nhiên
    ///       còn trống xung quanh vị trí đó.<br/>
    ///     Quân Parasite chỉ thực hiện hành động này lên các quân có State là <see cref="StateType.None"/>.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Parasite : StateEffect, IBeforePieceActionTrigger
    {
        public override StateType StateType => StateType.Parasite;

        public BeforeActionPriority Priority => BeforeActionPriority.Declaration;

        public Parasite(PieceLogic piece) : base(-1, 0, piece, "effect_parasite")
        {
        }

        /// <summary>
        ///     Nếu action là Capture vào quân địch có State None: chặn capture thông thường,
        ///     thay bằng ParasiteCapture (quân biến mất, áp Infested lên mục tiêu).
        /// </summary>
        public void OnCallBeforePieceAction(Action.Action action)
        {
           var targetPiece = action.GetTargetAsPiece();

           if (action is not ICaptures 
                || action.GetMakerAsPiece() != Piece 
                || targetPiece == null
                || targetPiece.CurrentState != StateType.None) return;

           action.Result = ResultFlag.Infest;

           ApplySkill(action);    

           ActionManager.EnqueueAction(new ApplyEffect(new Infested(targetPiece, Piece)));
           ActionManager.EnqueueAction(new MoveToParasitic(Piece, targetPiece));
        }

        protected virtual void ApplySkill(Action.Action action)
        {}
    }
}
