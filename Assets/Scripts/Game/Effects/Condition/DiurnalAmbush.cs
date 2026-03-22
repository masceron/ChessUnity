using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;

namespace Game.Effects.Condition
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DiurnalAmbush : Diurnal, IStartTurnTrigger
    {
        private bool _wasActive;

        public DiurnalAmbush(PieceLogic piece) : base(-1, 1, piece, "effect_diurnal_ambush")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
            _wasActive = IsActive; // Khởi tạo trạng thái ban đầu
        }

        public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Buff;

        public StartTurnEffectType StartTurnEffectType { get; }

        public void OnCallStart(Action.Action lastMainAction)
        {
            if (IsActive != _wasActive)
            {
                if (IsActive)
                {
                    // Chuyển sang Ngày (Active) -> Nhận trait Ambush
                    if (!Piece.Effects.OfType<Ambush>().Any())
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Ambush(Piece)));
                    }
                }
                else
                {
                    // Chuyển sang Đêm (Inactive) -> Mất trait Ambush
                    var ambushEffect = Piece.Effects.OfType<Ambush>().FirstOrDefault();
                    if (ambushEffect != null)
                    {
                        ActionManager.EnqueueAction(new RemoveEffect(ambushEffect));
                    }
                }

                _wasActive = IsActive;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;
        }
    }
}