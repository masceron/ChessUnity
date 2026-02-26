using System;
using System.Collections.Generic;
using Game.Effects;
using Game.Effects.Buffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using Random = UnityEngine.Random;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SpinsterWrassePending : PendingAction, IDisposable
    {
        private static PieceLogic _firstTarget;
        private static PieceLogic _secondTarget;

        public SpinsterWrassePending(int maker, int to) : base(maker)
        {
            Maker = maker;
            Target = to;
        }

        public void Dispose()
        {
            ResetTargets();
            BoardViewer.SelectingFunction = 0;
        }

        // protected override void ModifyGameState()
        // {
        //     ApplyEffect(FirstTarget, SecondTarget);
        //     SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        //     
        //     ResetTargets();
        // }

        protected override void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);

            if (_firstTarget == null || _firstTarget.Color == hovering.Color)
            {
                _firstTarget = hovering;
                TileManager.Ins.UnmarkAll();
                TileManager.Ins.Select(_firstTarget.Pos);
                TileManager.Ins.MarkPieceInRange(Maker, _firstTarget.Color, 5);
                return;
            }

            _secondTarget = hovering;
            TileManager.Ins.UnmarkAll();
            var buff = new SpinsterWrasseBuff(Maker, _firstTarget.Pos, _secondTarget.Pos);
            CommitResult(buff);

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
            _firstTarget = null;
            _secondTarget = null;
        }

        public void CompleteActionForAI()
        {
            var listA = GetPiecesInRadius(RankOf(Maker), FileOf(Maker), 5,
                p => p != null && p.Color == PieceOn(Maker).Color);

            if (listA.Count == 0) return;
            listA.Sort((a, b) =>
            {
                int buffCountA = 0, buffCountB = 0;
                foreach (var effect in a.Effects)
                    if (effect.Category == EffectCategory.Debuff)
                        buffCountA++;

                foreach (var effect in b.Effects)
                    if (effect.Category == EffectCategory.Debuff)
                        buffCountB++;

                return buffCountA.CompareTo(buffCountB);
            });

            var listB = new List<PieceLogic>();
            for (var i = 0; i < BoardSize; ++i)
            {
                var piece = PieceOn(i);
                if (piece == null || piece.Color != PieceOn(Maker).Color || piece.Effects.Any(e =>
                        e.EffectName == "effect_extremophiles" || e.EffectName == "effect_Adaptation"))
                    continue;

                listB.Add(piece);
            }

            if (listB.Count == 0) return;

            listB.Sort((a, b) => b.GetValueForAI().CompareTo(a.GetValueForAI()));

            var idxA = Random.Range(0, listA.Count - 1);
            var idxB = Random.Range(0, listB.Count - 1);
            ApplyEffect(PieceOn(idxA), PieceOn(idxB));
        }


        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }
    }
}