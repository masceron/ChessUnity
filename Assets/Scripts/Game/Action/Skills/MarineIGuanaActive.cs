using Game.Action.Internal;
using Game.Action;
using UX.UI.Ingame;
using Game.Action.Internal.Pending;
using Game.Managers;
using static Game.Common.BoardUtils;
using Game.Common;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using System.Collections.Generic;
using System.Linq;
using Game.Movesets;
using Game.Action.Captures;
using Game.AI;


namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineIguanaActive: Action, IPendingAble, ISkills, System.IDisposable, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return pieceAI.Color != PieceOn(Maker).Color ? -50 : 0;
        }
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        public MarineIguanaActive(int maker, int to) : base(maker)
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
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(FirstTarget.Pos), FileOf(FirstTarget.Pos), 2))
                {
                    var index = IndexOf(rankOff, fileOff);
                    var piece = PieceOn(index);
                    if (piece == null || piece.Color != FirstTarget.Color) continue;
                    var newAction = new MarineIguanaActive(Maker, index);
                    BoardViewer.ListOf.Add(newAction);
                    TileManager.Ins.MarkAsMoveable(index);
                }
                return;
            }
            SecondTarget = hovering; 
        
            ActionManager.EnqueueAction(new MarinelKill(Maker, FirstTarget.Pos, SecondTarget.Pos));
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