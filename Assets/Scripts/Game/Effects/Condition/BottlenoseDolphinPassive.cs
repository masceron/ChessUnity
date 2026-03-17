using System;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;

namespace Game.Effects.Condition
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BottlenoseDolphinPassive : Diurnal, IStartTurnTrigger
    {
        private const int EvasionProbability = 25;
        // Biến này để lưu state ván trước để biết lúc nào chuyển đoạn Ngày <-> Đêm
        private bool _wasActive;

        public BottlenoseDolphinPassive(PieceLogic piece) : base(-1, 1, piece, "effect_bottlenose_dolphin_passive")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
            _wasActive = IsActive; // Gắn giá trị khởi tạo
        }

        public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Buff;

        public StartTurnEffectType StartTurnEffectType { get; }

        public void OnCallStart(Action.Action lastMainAction)
        {
            // Nếu trạng thái IsActive thay đổi (Đêm -> Ngày hoặc Ngày -> Đêm)
            if (IsActive != _wasActive)
            {
                if (IsActive) 
                {
                    // Chuyển sang Ngày (Active):
                    // + Evasion 25%
                    var existingEvasion = Piece.Effects.OfType<Evasion>().FirstOrDefault();
                    if (existingEvasion != null)
                        existingEvasion.Strength += EvasionProbability;
                    else
                        ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, EvasionProbability, Piece)));
                        
                    // + QuickReflex
                    if (!Piece.Effects.OfType<QuickReflex>().Any())
                        ActionManager.EnqueueAction(new ApplyEffect(new QuickReflex(Piece)));
                }
                else 
                {
                    // Chuyển sang Đêm (Inactive): 
                    // - Bỏ QuickReflex
                    var quickReflex = Piece.Effects.OfType<QuickReflex>().FirstOrDefault();
                    if (quickReflex != null) ActionManager.EnqueueAction(new RemoveEffect(quickReflex));

                    // - Giảm/Bỏ Evasion
                    var evasionEffect = Piece.Effects.OfType<Evasion>().FirstOrDefault();
                    if (evasionEffect != null)
                    {
                        if (evasionEffect.Strength <= EvasionProbability)
                            ActionManager.EnqueueAction(new RemoveEffect(evasionEffect));
                        else
                            evasionEffect.Strength -= EvasionProbability;
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