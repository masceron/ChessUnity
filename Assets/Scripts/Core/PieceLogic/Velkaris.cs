using System;
using System.Collections.Generic;
using Board.Action;
using Board.Interaction;
using Action = Board.Action.Action;

namespace Core.PieceLogic
{
    public class Velkaris: PieceLogic
    {
        private const int Range = 2;

        private static readonly int[][] Adjacent = {
            new[] {-1, -1},
            new[] {-1, 0},
            new[] {-1, 1},
            new[] {0, 1},
            new[] {1, 1},
            new[] {1, 0},
            new[] {1, -1},
            new[] {0, -1},
        };
        
        private static readonly int[][] Next = {
            new[] {-2, -2},
            new[] {-2, 0},
            new[] {-2, 2},
            new[] {0, 2},
            new[] {2, 2},
            new[] {2, 0},
            new[] {2, -2},
            new[] {0, -2},
        };

        private static readonly int[][] KnightSlides =
        {
            new[] {-2, -1},
            new[] {-2, 1},
            new[] {-1, 2},
            new[] {1, 2},
            new[] {2, 1},
            new[] {2, -1},
            new[] {1, -2},
            new[] {-1, -2},
        };

        public Velkaris(PieceData p) : base(p) {}

        private static bool[] StraightSlide(List<Action> list, int from, GameState gameState, int range)
        {
            var block = new bool[8];
            var board = gameState.MainBoard;
            var rank = from / InteractionManager.MaxFile;
            var file = from % InteractionManager.MaxFile;
            var maxFile = InteractionManager.MaxFile;
            var maxRank = InteractionManager.MaxRank;

            for (var i = 0; i < 8; i++)
            {
                var toRank = rank + Adjacent[i][0];
                var toFile = file + Adjacent[i][1];
                var to = toRank * maxFile + toFile;
                
                if (toRank < 0 || toRank >= maxRank || toFile < 0 || toFile >= maxFile || !gameState.ActiveBoard[to]) continue;
                
                if (board[to] == null)
                {
                    if (range > 0)
                    {
                        list.Add(new NormalMove(from, to, InteractionManager.PieceManager, InteractionManager.MaxFile));
                    }

                    block[i] = false;
                    
                    var to2Rank = rank + Next[i][0];
                    var to2File = file + Next[i][1];
                    var to2 = to2Rank * maxFile + to2File;
                    
                    if (to2Rank < 0 || to2Rank >= maxRank || to2File < 0 || to2File >= maxFile || !gameState.ActiveBoard[to2]) continue;
                    if (board[to2] == null)
                    {
                        if (range > 1)
                        {
                            list.Add(new NormalMove(from, to2, InteractionManager.PieceManager, InteractionManager.MaxFile));
                        }
                    }
                    else if (board[to2].Color != gameState.OurSide)
                    {
                        list.Add(new NormalCapture((ushort)from, (ushort)to2));
                    }
                }
                else
                {
                    block[i] = true;
                    if (board[to].Color != gameState.OurSide)
                    {
                        list.Add(new NormalCapture((ushort)from, (ushort)to));
                    }
                }
            }
            return block;
        }

        private static void KnightSlide(List<Action> list, int from, GameState gameState, bool[] block, int range)
        {
            var board = gameState.MainBoard;
            var maxFile = InteractionManager.MaxFile;
            var maxRank = InteractionManager.MaxRank;
            var rank = from / InteractionManager.MaxFile;
            var file = from % InteractionManager.MaxFile;
            
            for (var i = 0; i < 8; i++)
            {
                var toRank = rank + KnightSlides[i][0];
                var toFile = file + KnightSlides[i][1];
                var to = toRank * maxFile + toFile;
                
                if (toRank < 0 || toRank >= maxRank || toFile < 0 || toFile >= maxFile || !gameState.ActiveBoard[to]) continue;
                
                if (block[i] && block[i < 7 ? i + 1 : 0]) continue;
                
                if (board[to] == null)
                {
                    if (range > 1)
                    {
                        list.Add(new NormalMove(from, to, InteractionManager.PieceManager, InteractionManager.MaxFile));
                    }
                }
                else
                {
                    if (board[to].Color != gameState.OurSide)
                    {
                        list.Add(new NormalCapture((ushort)from, (ushort)to));
                    }
                }
            }
        }

        private static readonly Effect Marked = new(EffectType.VelkarisMarked, -1);

        private static void Kill(List<Action> list, int from, GameState gameState)
        {
            for (var i = 0; i < InteractionManager.BoardSize; i++)
            {
                var p = gameState.MainBoard[i];
                if (p == null) continue;
                
                
                if (p.Effects.Contains(Marked) && p.Color != gameState.OurSide)
                {
                    list.Add(new VelkarisKill(from, i));
                }
            }
        }

        public override List<Action> MoveToMake(int from)
        {
            var gameState = InteractionManager.GameState;
            var piece = gameState.MainBoard[from];
            var list = new List<Action>();
            
            var range = Math.Max(Range - (Piece.Effects.Contains(GameState.SlowOne) ? 1 : 0), 0);
            
            var blocker = StraightSlide(list, from, gameState, range);
            KnightSlide(list, from, gameState, blocker, range);
            
            if (piece.SkillCooldown == -1)
            {
                Kill(list, from, gameState);
            }

            return list;
        }
    }
}