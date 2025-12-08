using Game.Action.Internal;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using UnityEngine;
using Game.Managers;
using Game.Piece.PieceLogic;
using System.Linq; // <-- thêm để dùng LINQ
using Game.AI;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TemperantiaSwap : Action, IPendingAble, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
        private static int allyIndex = -1;
        private static int enemyIndex = -1; // -1 nếu chưa chọn enemy
        public TemperantiaSwap(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
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
            BoardViewer.Ins.ExecuteAction(this);
            allyIndex = -1;
            enemyIndex = -1;

        }

        public void CompleteActionForAI()
        {
            // Resolve maker and side
            var makerPiece = PieceOn(Maker);
            if (makerPiece == null) return;
            var side = makerPiece.Color;

            // Gather ally and enemy pieces
            var allies = FindPiece<PieceLogic>(side);
            var enemies = FindPiece<PieceLogic>(!side);

            if (allies == null || allies.Count == 0) return;
            if (enemies == null || enemies.Count == 0) return;

            // Count buffs on each piece
            int CountBuffs(PieceLogic p) => p.Effects.Count(e => e.Category == EffectCategory.Buff);

            // Find minimum buff count among allies
            var minBuff = allies.Min(CountBuffs);
            var candidatesA = allies.Where(p => CountBuffs(p) == minBuff).ToList();
            // Find maximum buff count among enemies
            var maxBuff = enemies.Max(CountBuffs);
            var candidatesB = enemies.Where(p => CountBuffs(p) == maxBuff).ToList();

            if (candidatesA.Count == 0 || candidatesB.Count == 0) return;

            // Choose selection (random if multiple)
            PieceLogic chosenAlly = candidatesA.Count == 1 ? candidatesA[0] : candidatesA[Random.Range(0, candidatesA.Count)];
            PieceLogic chosenEnemy = candidatesB.Count == 1 ? candidatesB[0] : candidatesB[Random.Range(0, candidatesB.Count)];
            allyIndex = chosenAlly.Pos;
            enemyIndex = chosenEnemy.Pos;

            // Execute effect now
            BoardViewer.Ins.ExecuteAction(this);
            // Reset static indices
            allyIndex = -1;
            enemyIndex = -1;
        }
    }
}