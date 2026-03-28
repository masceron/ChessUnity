using System;
using Game.Action.Skills;
using Game.AI;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumilitasPending : PendingAction, IDisposable, IAIAction, ISkills
    {
        private static int _firstTarget;
        public static int SecondTarget;

        private bool _isExecuting;

        public HumilitasPending(int maker, int to) : base(maker, to)
        {
            _isExecuting = false;
        }

        public void CompleteActionForAI()
        {
            // var listPieces = new List<PieceLogic>();

            // foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Maker), FileOf(Maker), 5))
            // {
            //     var idx = IndexOf(rank, file);
            //     var pOn = PieceOn(idx);
            //     if (pOn != null && pOn.Color != GetMaker().Color)
            //     {
            //         if (pOn.Effects != null && pOn.Effects.Any(e => e.EffectName == "effect_extremophile")) continue;
            //         listPieces.Add(pOn);
            //     }
            // }
            // // neu khong co quan nao
            // if (listPieces.Count == 0) return;
            // // neu co dung mot quan
            // if (listPieces.Count == 1)
            // {
            //     ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, listPieces[0])));
            //     SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
            //     return;
            // }
            // // neu co nhieu quan           
            // listPieces.Sort((a, b) =>
            //     b.GetValueForAI()
            //         .CompareTo(a.GetValueForAI()));

            // var selectedPieces = new List<PieceLogic>();

            // int topValue = listPieces[0].GetValueForAI();
            // var topGroup = listPieces.Where(p =>
            //     p.GetValueForAI() == topValue).ToList();

            // if (topGroup.Count >= 2)
            // {
            //     int idx1 = UnityEngine.Random.Range(0, topGroup.Count);
            //     int idx2;
            //     do { idx2 = UnityEngine.Random.Range(0, topGroup.Count); }
            //     while (idx2 == idx1);

            //     selectedPieces.Add(topGroup[idx1]);
            //     selectedPieces.Add(topGroup[idx2]);
            // }
            // else
            // {
            //     selectedPieces.Add(listPieces[0]);

            //     if (listPieces.Count > 1)
            //     {
            //         int secondValue = listPieces[1].GetValueForAI();
            //         var secondGroup = listPieces.Where(p =>
            //             p.GetValueForAI() == secondValue).ToList();
            //         if (secondGroup.Count == 0) return;
            //         int idx = UnityEngine.Random.Range(0, secondGroup.Count);
            //         selectedPieces.Add(secondGroup[idx]);
            //     }
            // }

            // foreach (var piece in selectedPieces)
            // {
            //     UnityEngine.Debug.Log("Taunting piece: " + piece.Type);
            //     ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, piece)));
            // }

            // SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }

        public void Dispose()
        {
            if (!_isExecuting) return;
            _firstTarget = -1;
            SecondTarget = -1;
            BoardViewer.SelectingFunction = 0;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker();
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -20;
            return 0;
        }

        protected override void CompleteAction()
        {
            Debug.Log("Executing HumilitasPending" + _firstTarget);
            if (_firstTarget == 0)
            {
                _firstTarget = BoardViewer.HoveringPos;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                var listPieces = SkillRangeHelper.GetActiveEnemyPieceInRadius(GetFrom(), 5);
                foreach (var piece in listPieces)
                {
                    if (piece == _firstTarget) continue;
                    var newAction = new HumilitasPending(GetFrom(), piece);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(piece);
                }

                return;
            }

            SecondTarget = BoardViewer.HoveringPos;
            CommitResult(new HumilitasActive(GetFrom(), _firstTarget, SecondTarget));
            _isExecuting = true;
        }
    }
}