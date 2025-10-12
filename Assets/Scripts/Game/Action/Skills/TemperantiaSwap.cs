using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using Game.Managers;
using Game.Effects;
using System;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TemperantiaSwap : Action, IDoubleSelectionSkill
    {
        private PieceLogic Ally;
        private PieceLogic Enemy;
        public TemperantiaSwap(int maker, int ally, int enemy = -1) : base(maker, false)
        {
            Maker = (ushort)maker;
            Ally = PieceOn(ally);
            
            if (enemy != -1){
                Target = (ushort)enemy;
                Ally = PieceOn(ally);
                Enemy = PieceOn(enemy);
            }
            else{
                Target = (ushort)ally;
                Ally = PieceOn(ally);
            }
        }
        protected override void ModifyGameState()
        {
            // Lấy danh sách debuff của Ally và buff của Enemy
            var allyDebuffs = Ally.Effects.FindAll(e => e.Category == EffectCategory.Debuff);
            var enemyBuffs = Enemy.Effects.FindAll(e => e.Category == EffectCategory.Buff);

            // Xóa debuff khỏi Ally và buff khỏi Enemy
            foreach (var effect in allyDebuffs)
            {
                ActionManager.ExecuteImmediately(new RemoveEffect(effect));
            }
            foreach (var effect in enemyBuffs)
            {
                ActionManager.ExecuteImmediately(new RemoveEffect(effect));
            }

            // Thêm debuff vào Enemy
            foreach (var effect in allyDebuffs)
            {
                effect.Piece = Enemy;
                ActionManager.ExecuteImmediately(new ApplyEffect(effect));
            }

            // Thêm buff vào Ally
            foreach (var effect in enemyBuffs)
            {
                effect.Piece = Ally;
                ActionManager.ExecuteImmediately(new ApplyEffect(effect));
            }
            
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        public bool IsBothSelected(){
            return Ally != null && Enemy != null;
        }
    }
}