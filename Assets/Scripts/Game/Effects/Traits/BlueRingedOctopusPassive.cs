using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    
    public class BlueRingedOctopusPassive : Effect, IAfterPieceActionEffect
    {
        public BlueRingedOctopusPassive(PieceLogic piece) : base(-1, 1, piece, "effect_blue_ringed_octopus_passive")
        {

        }
        
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 50;
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            var activeBoard = ActiveBoard();
            if (action.Maker == Piece.Pos)
            {
                var pos1 = action.Maker;
                var pos2 = action.Target;

                var (first1, first2) = RankFileOf(pos1);
                var (second1, second2) = RankFileOf(pos2);

                var pushX = Math.Sign(second1 - first1);
                var pushY = Math.Sign(second2 - first2);

                var curX = first1;
                var curY = first2;

                HashSet<int> effectedPieces = new HashSet<int>();
                while(true) 
                {
                    var p = PieceOn(IndexOf(curX, curY));
                    if (p != null && p.Color != Piece.Color) 
                        effectedPieces.Add(IndexOf(curX, curY));

                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            var (rank, file) = (curX + x, curY + y);
                            if (!VerifyBounds(rank) || !VerifyBounds(file)) continue;
                            var piece = PieceOn(IndexOf(rank, file));
                            if (piece == null || piece.Color == Piece.Color) continue;

                            effectedPieces.Add(IndexOf(rank, file));
                        }
                    }
                    curX += pushX; 
                    curY += pushY;
                    
                    if (curX == second1 && curY == second2) break;
                }

                foreach (var piece in effectedPieces)
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, PieceOn(piece)), Piece));
                }
            }
            else
            {
                var initActive = new List<int>();

                for (int x = -1; x <= 1; ++x)
                {
                    for (int y = -1; y <= 1; ++y)
                    {
                        var (rank, file) = (RankOf(Piece.Pos) + x, FileOf(Piece.Pos) + y);
                        if (activeBoard[IndexOf(rank, file)])
                        {
                            activeBoard[IndexOf(rank, file)] = false;
                            initActive.Add(IndexOf(rank, file));
                        }
                    }
                }

                if (activeBoard[action.Target] == false || activeBoard[action.Maker] == false)
                {
                    foreach (var pos in initActive)
                        activeBoard[pos] = true;
                    
                    ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, PieceOn(action.Maker)), Piece));
                    return;
                }
                
                var blocker = Pathfinder.LineBlocker(
                    RankOf(action.Maker), FileOf(action.Maker), RankOf(action.Target), FileOf(action.Target)
                );
                
                if (blocker.Item1 != -1)
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, PieceOn(action.Maker)), Piece));
                }
                
                foreach (var pos in initActive)
                    activeBoard[pos] = true;
                
            }
        }
    }
}