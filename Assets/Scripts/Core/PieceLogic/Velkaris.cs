using System.Collections.Generic;
using Board.Action;
using Board.Interaction;

namespace Core.PieceLogic
{
    public class Velkaris: IPieceLogic
    {
        private readonly int[][] adjacent = {
            new[] {-1, -1},
            new[] {-1, 0},
            new[] {-1, 1},
            new[] {0, 1},
            new[] {1, 1},
            new[] {1, 0},
            new[] {1, -1},
            new[] {0, -1},
        };
        
        private readonly int[][] next = {
            new[] {-2, -2},
            new[] {-2, 0},
            new[] {-2, 2},
            new[] {0, 2},
            new[] {2, 2},
            new[] {2, 0},
            new[] {2, -2},
            new[] {0, -2},
        };

        private readonly int[][] knightSlides =
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
        private bool[] StraightSlide(List<IAction> list, int from, GameState gameState)
        {
            var block = new bool[8];
            var board = gameState.Position.main_board;
            var rank = from / InteractionManager.maxFile;
            var file = from % InteractionManager.maxFile;
            var maxFile = InteractionManager.maxFile;
            var maxRank = InteractionManager.maxRank;

            for (var i = 0; i < 8; i++)
            {
                var toRank = rank + adjacent[i][0];
                var toFile = file + adjacent[i][1];
                var to = toRank * maxFile + toFile;
                
                if (toRank < 0 || toRank >= maxRank || toFile < 0 || toFile >= maxFile || gameState.Position.active_board[to] == 0) continue;
                
                if (board[to].Type == PieceType.Nil)
                {
                    list.Add(new NormalMove(from, to, InteractionManager.pieceManager, InteractionManager.maxFile));
                    block[i] = false;
                    
                    var to2Rank = rank + next[i][0];
                    var to2File = file + next[i][1];
                    var to2 = to2Rank * maxFile + to2File;
                    
                    if (to2Rank < 0 || to2Rank >= maxRank || to2File < 0 || to2File >= maxFile || gameState.Position.active_board[to2] == 0) continue;
                    if (board[to2].Type == PieceType.Nil)
                    {
                        list.Add(new NormalMove(from, to2, InteractionManager.pieceManager,
                            InteractionManager.maxFile));
                    }
                    else if (board[to2].Color != gameState.OurSide)
                    {
                        list.Add(new NormalCapture(from, to2));
                    }
                }
                else
                {
                    block[i] = true;
                    if (board[to].Color != gameState.OurSide)
                    {
                        list.Add(new NormalCapture(from, to));
                    }
                }
            }
            return block;
        }

        private void KnightSlide(List<IAction> list, int from, GameState gameState, bool[] block)
        {
            var board = gameState.Position.main_board;
            var maxFile = InteractionManager.maxFile;
            var maxRank = InteractionManager.maxRank;
            var rank = from / InteractionManager.maxFile;
            var file = from % InteractionManager.maxFile;
            
            for (var i = 0; i < 8; i++)
            {
                var toRank = rank + knightSlides[i][0];
                var toFile = file + knightSlides[i][1];
                var to = toRank * maxFile + toFile;
                
                if (toRank < 0 || toRank >= maxRank || toFile < 0 || toFile >= maxFile || gameState.Position.active_board[to] == 0) continue;
                
                if (block[i] && block[i < 7 ? i + 1 : 0]) continue;
                
                if (board[to].Type == PieceType.Nil)
                {
                    list.Add(new NormalMove(from, to, InteractionManager.pieceManager, InteractionManager.maxFile));
                }
                else
                {
                    if (board[to].Color != gameState.OurSide)
                    {
                        list.Add(new NormalCapture(from, to));
                    }
                }
            }
        }
        public List<IAction> MoveToMake(int from)
        {
            var list = new List<IAction>();
            var gameState = InteractionManager.gameState;
            KnightSlide(list, from, gameState, StraightSlide(list, from, gameState));
            
            return list;
        }
    }
}