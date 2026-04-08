using Game.Action.Internal;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class TemperantiaSwap : Action, ISkills
    {
        private readonly int _allyPiece;
        private readonly int _enemyPiece;

        [MemoryPackConstructor]
        private TemperantiaSwap()
        {
        }

        public TemperantiaSwap(PieceLogic maker, PieceLogic firstPiece, PieceLogic secondPiece) : base(maker)
        {
            _allyPiece = firstPiece.ID;
            _enemyPiece = secondPiece.ID;
            
            Debug.Log($"Choosing {firstPiece.Type} and {secondPiece.Type}");
        }
        
        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        // Chọn 1 quân đồng minh và 1 quân địch, hoán đổi đồng minh với kẻ địch
        protected override void ModifyGameState()
        {
            var ally = GetEntityByID(_allyPiece) as PieceLogic;
            var enemy = GetEntityByID(_enemyPiece) as PieceLogic;
            if (ally == null || enemy == null) return;

            // Copy danh sách buff và debuff trước khi bị xóa
            var allyDebuffs = ally.Effects.FindAll(e => e.Category == EffectCategory.Debuff);
            var enemyBuffs = enemy.Effects.FindAll(e => e.Category == EffectCategory.Buff);

            // Xóa debuff khỏi Ally và buff khỏi Enemy
            foreach (var effect in allyDebuffs) ActionManager.EnqueueAction(new RemoveEffect(effect));
            foreach (var effect in enemyBuffs) ActionManager.EnqueueAction(new RemoveEffect(effect));

            // Thêm debuff vào Enemy (resolve Piece before applying)
            foreach (var effect in allyDebuffs)
            {
                effect.Piece = enemy;
                ActionManager.EnqueueAction(new ApplyEffect(effect, GetMakerAsPiece()));
            }

            // Thêm buff vào Ally
            foreach (var effect in enemyBuffs)
            {
                effect.Piece = ally;
                ActionManager.EnqueueAction(new ApplyEffect(effect, GetMakerAsPiece()));
            }
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}