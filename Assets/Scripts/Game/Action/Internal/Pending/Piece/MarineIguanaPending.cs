using Game.Action.Internal;
using UX.UI.Ingame;
using Game.Action.Internal.Pending;
using Game.Managers;
using static Game.Common.BoardUtils;
using Game.Common;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using Game.AI;


namespace Game.Action.Internal.Pending.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineIguanaPending: PendingAction, System.IDisposable, IInternal, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return pieceAI.Color != PieceOn(Maker).Color ? -50 : 0;
        }
        public static int FirstTarget;
        public static int SecondTarget;
        private bool isExecuting;
        public MarineIguanaPending(int maker, int to) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
            isExecuting = false;
        }

        // protected override void ModifyGameState()
        // {
        //     if (FirstTarget == null) return;
        //     if (SecondTarget == null) return;
        //     ActionManager.EnqueueAction(new MarinelKill(Maker, FirstTarget.Pos, SecondTarget.Pos));
        //     isExecuting = false;
        //     Dispose();
        //     SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        // }
        public override void CompleteAction()
        {
            var hovering = PieceOn(BoardViewer.HoveringPos);
            if (FirstTarget == 0) 
            {
                FirstTarget = BoardViewer.HoveringPos;
                TileManager.Ins.UnmarkAll();
                BoardViewer.ListOf.Clear();
                var Target = SkillRangeHelper.GetActiveAllyPieceInRadius(FirstTarget, 2);
                foreach (var target in Target)
                {
                    var newAction = new MarineIguanaPending(Maker, target);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(target);
                }
                return;
            }
            SecondTarget = BoardViewer.HoveringPos; 
            BoardViewer.Ins.ExecuteAction(new MarineIguanaActive(Maker, FirstTarget, SecondTarget));
            isExecuting = true;
        }
        public void Dispose()
        {
            if (!isExecuting) return;
            FirstTarget = 0;
            SecondTarget = 0;
            BoardViewer.SelectingFunction = 0; 
        }

        public void CompleteActionForAI()
        {
            UnityEngine.Debug.Log("CompleteActionForAI");
            // var listActions = new List<Action>();
            // BluffingMoves.Captures(listActions, Maker, isPlayer: true);
            // if (listActions.Count > 1)
            // {
            //     listActions = listActions.Distinct(new ActionComparer()).ToList();
            // }
            // var captureTargets = listActions.OfType<ICaptures>()
            // .Select(c => ((Action)c).Target)
            // .ToList();
            // if (captureTargets.Count == 0) return;
            // int firstTarget = captureTargets[0];
            // int secondTarget = -1;
            // int MaxValue = int.MinValue;
            // foreach (var target in captureTargets)
            // {
            //     int maxSubValue = int.MinValue;
            //     int secondSubTarget = -1;
            //     foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(target), FileOf(target), 2))
            //     {
            //         var index = IndexOf(rankOff, fileOff);
            //         var piece = PieceOn(index);
            //         if (piece == null || piece.Color != PieceOn(Target).Color || piece == PieceOn(Target)) continue;
            //         var value = piece.GetValueForAI();
            //         if (value > maxSubValue)
            //         {
            //             maxSubValue = value;
            //             secondSubTarget = index;
            //             UnityEngine.Debug.Log("maxSubValue: " + maxSubValue + " secondSubTarget: " + secondSubTarget);
            //             UnityEngine.Debug.Log("piece: " + piece.Type);
            //         }
            //     }
            //     if (secondSubTarget == -1) continue;
            //     if (PieceOn(target).GetValueForAI() + maxSubValue > MaxValue)
            //     {
            //         MaxValue = PieceOn(target).GetValueForAI() + maxSubValue;
            //         firstTarget = target;
            //         secondTarget = secondSubTarget;
            //     }
            // }
            // UnityEngine.Debug.Log("firstTarget: " + firstTarget + " secondTarget: " + secondTarget);
            // if (secondTarget == -1) return;
            // ActionManager.EnqueueAction(new MarinelKill(Maker, firstTarget, secondTarget));
        }
    }
}