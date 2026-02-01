using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Common;
using Game.AI;
using Game.Action.Skills;
using Game.Action;
using Game.AI;

namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumilitasPending : PendingAction, System.IDisposable, IAIAction, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -20;
            return 0;
        }
        public static int FirstTarget;
        public static int SecondTarget;

        private bool isExecuting;
        public HumilitasPending(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
            isExecuting = false;
        }

        // protected override void ModifyGameState()
        // {
        //     if (FirstTarget == null) return;
        //     if (SecondTarget == null) return;
        //     
        //     ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, FirstTarget), PieceOn(Maker)));
        //     ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, SecondTarget), PieceOn(Maker)));
        //     isExecuting = false;
        //     Dispose();
        //     SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        // }
        public override void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);
            UnityEngine.Debug.Log("Executing HumilitasPending" + FirstTarget);
            if (FirstTarget == 0)
            {

                FirstTarget = BoardViewer.HoveringPos;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                var listPieces = SkillRangeHelper.GetActiveEnemyPieceInRadius(Maker, 5);
                foreach (var piece in listPieces)
                {
                    if (piece == FirstTarget) continue;
                    var newAction = new HumilitasPending(Maker, piece);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(piece);
                }
                return;
            }
            SecondTarget = BoardViewer.HoveringPos;
            BoardViewer.Ins.ExecuteAction(new HumilitasActive(Maker, FirstTarget, SecondTarget));
            isExecuting = true;
        }
        public void Dispose()
        {
            if (!isExecuting) return;
            FirstTarget = -1;
            SecondTarget = -1;
            BoardViewer.SelectingFunction = 0;
        }

        public void CompleteActionForAI()
        {
            // var listPieces = new List<PieceLogic>();

            // foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Maker), FileOf(Maker), 5))
            // {
            //     var idx = IndexOf(rank, file);
            //     var pOn = PieceOn(idx);
            //     if (pOn != null && pOn.Color != PieceOn(Maker).Color)
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
            //     SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
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

            // SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}
