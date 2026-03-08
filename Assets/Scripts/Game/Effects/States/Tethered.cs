using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Tethered</b><br/>
    ///     Quân này đang được nối dây với quân <see cref="TetheredPeer"/>.<br/>
    ///     - Nhận hiệu ứng tùy theo mô tả Skill của quân cast dây nối.<br/>
    ///     - Cuối mỗi turn (<see cref="IEndTurnTrigger"/>): kiểm tra khoảng cách 2 quân.
    ///       Nếu vượt quá <c>MaxRange</c> → quân về State None (dây đứt).<br/>
    ///     - Nếu quân nối bị ăn (<see cref="IDeadTrigger"/>): đứt dây.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Tethered : StateEffect, IEndTurnTrigger, IDeadTrigger
    {
        /// <summary>Quân cờ đầu kia của dây nối.</summary>
        public PieceLogic TetheredPeer;

        /// <summary>Range tối đa (×2 = ngưỡng đứt dây). Lấy từ Skill của caster.</summary>
        public int MaxRange;

        public override StateType StateType => StateType.Tethered;

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;
        public EndTurnEffectType EndTurnEffectType => EndTurnEffectType.EndOfAnyTurn;

        public Tethered(PieceLogic piece, PieceLogic peer, int maxRange)
            : base(-1, 0, piece, "effect_tethered")
        {
            TetheredPeer = peer;
            MaxRange = maxRange;
        }

        private void BreakTether()
        {
            var symbioticEffect = TetheredPeer.Effects.Find(e => e is Symbiotic) as Symbiotic;
            if (symbioticEffect != null) symbioticEffect.IsTethered = false;
            Piece.ClearState();
        }

        /// <summary>Cuối mỗi turn: kiểm tra khoảng cách, đứt dây nếu quá xa.</summary>
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (!BoardUtils.IsAlive(Piece) || !BoardUtils.IsAlive(TetheredPeer))
            {
                if (BoardUtils.IsAlive(Piece)) BreakTether();
                return;
            }

            if (BoardUtils.Distance(Piece.Pos, TetheredPeer.Pos) > MaxRange)
            {
                BreakTether();
            }
        }

        /// <summary>Khi 1 trong 2 quân chết: đứt dây.</summary>
        public void OnCallDead(PieceLogic pieceToDie)
        {
            if (pieceToDie == TetheredPeer || pieceToDie == Piece)
            {
                BreakTether();
            }
        }
    }
}
