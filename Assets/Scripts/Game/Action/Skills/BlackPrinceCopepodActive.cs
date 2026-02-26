using Game.Action.Internal;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

// <-- thêm để dùng LINQ

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class BlackPrinceCopepodActive : Action, ISkills
    {
        public int firstIndex = -1;
        public int secondIndex = -1;
        public int thirdIndex = -1;

        [MemoryPackConstructor]
        private BlackPrinceCopepodActive()
        {
        }

        public BlackPrinceCopepodActive(int maker, int firstIndex, int secondIndex, int thirdIndex) : base(maker)
        {
            Maker = maker;
            this.firstIndex = firstIndex;
            this.secondIndex = secondIndex;
            this.thirdIndex = thirdIndex;
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
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
            foreach (var effect in allyDebuffs) ActionManager.EnqueueAction(new RemoveEffect(effect));
            foreach (var effect in enemyBuffs) ActionManager.EnqueueAction(new RemoveEffect(effect));

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