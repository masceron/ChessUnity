using Game.Action.Internal;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using UnityEngine;
using Game.Managers;
using Game.Piece.PieceLogic;
using UnityEngine.InputSystem;

// <-- thêm để dùng LINQ

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TemperantiaSwap : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
        public int allyIndex = -1;
        public int enemyIndex = -1; // -1 nếu chưa chọn enemy
        public TemperantiaSwap(int maker, int allyIndex, int enemyIndex) : base(maker)
        {
            Maker = (ushort)maker;
            this.allyIndex = allyIndex;
            this.enemyIndex = enemyIndex;
        }

        // Chọn 1 quân đồng minh và 1 quân địch, hoán đổi đồng minh với kẻ địch
        protected override void ModifyGameState()
        {
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
            var ally = PieceOn(allyIndex);
            var enemy = PieceOn(enemyIndex);
            if (ally == null || enemy == null) return;
        
            // Copy danh sách buff và debuff trước khi bị xóa
            var allyDebuffs = ally.Effects.FindAll(e => e.Category == EffectCategory.Debuff);
            var enemyBuffs = enemy.Effects.FindAll(e => e.Category == EffectCategory.Buff);
        
            // Xóa debuff khỏi Ally và buff khỏi Enemy
            foreach (var effect in allyDebuffs)
            {
                ActionManager.EnqueueAction(new RemoveEffect(effect));
            }
            foreach (var effect in enemyBuffs)
            {
                ActionManager.EnqueueAction(new RemoveEffect(effect));
            }
        
            // Thêm debuff vào Enemy (resolve Piece before applying)
            foreach (var effect in allyDebuffs)
            {
                effect.Piece = enemy;
                ActionManager.EnqueueAction(new ApplyEffect(effect, PieceOn(Maker)));
            }
        
            // Thêm buff vào Ally
            foreach (var effect in enemyBuffs)
            {
                effect.Piece = ally;
                ActionManager.EnqueueAction(new ApplyEffect(effect, PieceOn(Maker)));
            }
        }
    }
}