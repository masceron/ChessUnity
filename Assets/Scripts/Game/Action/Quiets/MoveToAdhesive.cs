using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Quiets
{
    /// <summary>
    ///     Quân Adhesive thực hiện "bám" vào một Piece hoặc Formation ở ô Target.<br/>
    ///     Quân Adhesive biến mất khỏi bàn cờ (pos = -9999) và host chuyển sang <see cref="Game.Effects.States.StateType.Attached"/>.<br/><br/>
    ///     - Bám Piece: gọi <see cref="PieceManager.MoveToParasitic"/> — bay lên đỉnh Piece và parent vào đó.<br/>
    ///     - Bám Formation: gọi <see cref="PieceManager.MoveToAdhesiveFormation"/> — bay lên đỉnh ô Formation,
    ///       thu nhỏ, nhưng không parent (Formation không phải MonoBehaviour).
    /// </summary>
    public class MoveToAdhesive : Action
    {
        private readonly bool _attachToFormation;

        /// <param name="maker">Vị trí quân Adhesive.</param>
        /// <param name="target">Vị trí ô target (Piece hoặc Formation).</param>
        /// <param name="attachToFormation">true = bám vào Formation; false = bám vào Piece.</param>
        public MoveToAdhesive(PieceLogic maker, Entity target, bool attachToFormation) : base(maker, target)
        {
            _attachToFormation = attachToFormation;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            var adhesive = GetMaker() as PieceLogic;

            if (_attachToFormation)
            {
                // Bám vào Formation — bay đến đỉnh tile Formation, thu nhỏ, không parent
                PieceManager.Ins.MoveToAdhesiveFormation(GetFrom(), GetTargetPos());
            }
            else
            {
                // Bám vào Piece — bay lên đỉnh Piece rồi parent vào (như Parasite)
                var hostLogic = GetTarget();
                PieceManager.Ins.MoveToParasitic(GetFrom(), GetTargetPos(), hostLogic as PieceLogic);
            }

            // Xóa quân Adhesive khỏi PieceBoard
            if (adhesive == null) return;
            BoardUtils.PieceBoard()[GetFrom()] = null;
            adhesive.Pos = -9999;
        }
    }
}