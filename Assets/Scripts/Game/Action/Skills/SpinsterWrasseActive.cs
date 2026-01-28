using System;
using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Effects.Buffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SpinsterWrasseActive : PendingAction, IDisposable
    {
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        private bool Color;
        public SpinsterWrasseActive(int maker, int to, bool color) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
            Color = color;
        }

        // protected override void ModifyGameState()
        // {
        //     ApplyEffect(FirstTarget, SecondTarget);
        //     SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        //     
        //     ResetTargets();
        // }

        public override void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);

            if (FirstTarget == null || FirstTarget.Color == hovering.Color)
            {
                FirstTarget = hovering;
                TileManager.Ins.UnmarkAll();
                TileManager.Ins.Select(FirstTarget.Pos);
                TileManager.Ins.MarkPieceInRange(Maker, FirstTarget.Color, 5);
                return;
            } 
            
            SecondTarget = hovering;
            TileManager.Ins.UnmarkAll();

            
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
        }

        private void ApplyEffect(PieceLogic firstTarget, PieceLogic secondTarget)
        {
            ActionManager.EnqueueAction(new Purify(Maker, firstTarget.Pos));
            ActionManager.EnqueueAction(new ApplyEffect(new Adaptation(secondTarget), PieceOn(Maker)));
        }

        private static void ResetTargets()
        {
            FirstTarget = null;
            SecondTarget = null;
        }
        
        public void Dispose()
        {
            ResetTargets();
            BoardViewer.SelectingFunction = 0;
        }
        public void CompleteActionForAI()
        {
            var listA = GetPiecesInRadius(RankOf(Maker), FileOf(Maker), 5, p => p != null && p.Color == PieceOn(Maker).Color);
            
            if (listA.Count == 0) return;
            listA.Sort((a, b) =>
            {
                int buffCountA = 0, buffCountB = 0;
                foreach (var effect in a.Effects)
                {
                    if (effect.Category == Effects.EffectCategory.Debuff)
                        buffCountA++;
                }

                foreach (var effect in b.Effects)
                {
                    if (effect.Category == Effects.EffectCategory.Debuff)
                        buffCountB++;
                }

                return buffCountA.CompareTo(buffCountB);
            });

            var listB = new List<PieceLogic>();
            for (int i = 0; i < BoardSize; ++i)
            {
                var piece = PieceOn(i);
                if (piece == null || piece.Color != PieceOn(Maker).Color || piece.Effects.Any(
                        e => e.EffectName == "effect_extremophiles" || e.EffectName == "effect_Adaptation"))
                    continue;
                
                listB.Add(piece);
            }
            
            if (listB.Count == 0) return;
            
            listB.Sort((a, b) => b.GetValueForAI().CompareTo(a.GetValueForAI()));
            
            var idxA = UnityEngine.Random.Range(0, listA.Count - 1);
            var idxB = UnityEngine.Random.Range(0, listB.Count - 1);
            ApplyEffect(PieceOn(idxA), PieceOn(idxB));
            
        }


        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }
    }
}