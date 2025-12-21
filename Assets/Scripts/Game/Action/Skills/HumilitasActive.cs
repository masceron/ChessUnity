using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Action.Internal.Pending;
using UX.UI.Ingame;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Common;
using Game.AI;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumilitasActive : Action, ISkills, IPendingAble, System.IDisposable, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -20;
            return 0;
        }
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        public HumilitasActive(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        public void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);
            if (FirstTarget == null)
            {
                FirstTarget = hovering;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(FirstTarget.Pos), FileOf(FirstTarget.Pos), 5))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null || piece.Color != FirstTarget.Color) continue;
                    var newAction = new HumilitasActive(Maker, index);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }
                return;
            }
            SecondTarget = hovering;

            ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, FirstTarget)));
            ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, SecondTarget)));
            BoardViewer.Ins.ExecuteAction(this);
        }
        public void Dispose()
        {
            FirstTarget = null;
            SecondTarget = null;
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
