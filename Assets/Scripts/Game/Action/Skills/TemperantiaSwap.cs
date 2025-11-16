using Game.Action.Internal;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using UnityEngine;
using Game.Managers;
using Game.Piece.PieceLogic;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TemperantiaSwap : Action, IPendingAble, ISkills
    {
        private static int allyIndex = -1;
        private static int enemyIndex = -1; // -1 nếu chưa chọn enemy
        public TemperantiaSwap(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }


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
                ActionManager.ExecuteImmediately(new RemoveEffect(effect));
            }
            foreach (var effect in enemyBuffs)
            {
                ActionManager.ExecuteImmediately(new RemoveEffect(effect));
            }

            // Thêm debuff vào Enemy (resolve Piece before applying)
            foreach (var effect in allyDebuffs)
            {
                effect.Piece = enemy;
                ActionManager.ExecuteImmediately(new ApplyEffect(effect));
            }

            // Thêm buff vào Ally
            foreach (var effect in enemyBuffs)
            {
                effect.Piece = ally;
                ActionManager.ExecuteImmediately(new ApplyEffect(effect));
            }
        }

        public void CompleteAction()
        {
            // Nếu chưa có enemy (chỉ hành động chọn ally), set firstSelection trên commander để tương thích
            if (allyIndex == -1)
            {
                if (PieceOn(Maker) is not Temperantia) return;
                
                Debug.Log("Temperantia: Have chosen ally, now choose target Enemy");
                allyIndex = Target;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                var enemyPieces = FindPiece<PieceLogic>(!PieceOn(Maker).Color);
                foreach (var e in enemyPieces)
                {
                    BoardViewer.ListOf.Add(new TemperantiaSwap(Maker, e.Pos));
                    TileManager.Ins.MarkAsMoveable(e.Pos);
                }
                return;
            }

            enemyIndex = Target;
            Debug.Log("ExecuteAction");
            BoardViewer.Ins.ExecuteAction(this);
            allyIndex = -1;
            enemyIndex = -1;

        }
    }
}