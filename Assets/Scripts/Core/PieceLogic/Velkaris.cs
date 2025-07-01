using System.Collections.Generic;
using Board.Action;
using Board.Interaction;

namespace Core.PieceLogic
{
    public class Velkaris: IPieceLogic
    {
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
        private static bool[] StraightSlide(List<Action> list, int from, GameState gameState)
        {
            var block = new bool[8];
            var board = gameState.Position.main_board;
            var rank = from / InteractionManager.maxFile;
            var file = from % InteractionManager.maxFile;
            var maxFile = InteractionManager.maxFile;
            var maxRank = InteractionManager.maxRank;

            for (var i = 0; i < 8; i++)
            {
                var toRank = rank + Adjacent[i][0];
                var toFile = file + Adjacent[i][1];
                var to = toRank * maxFile + toFile;
                
                if (toRank < 0 || toRank >= maxRank || toFile < 0 || toFile >= maxFile || gameState.Position.active_board[to] == 0) continue;
                
                if (board[to] == null)
                {
                    list.Add(new NormalMove(from, to, InteractionManager.pieceManager, InteractionManager.maxFile));
                    block[i] = false;
                    
                    var to2Rank = rank + Next[i][0];
                    var to2File = file + Next[i][1];
                    var to2 = to2Rank * maxFile + to2File;
                    
                    if (to2Rank < 0 || to2Rank >= maxRank || to2File < 0 || to2File >= maxFile || gameState.Position.active_board[to2] == 0) continue;
                    if (board[to2] == null)
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

        private static void KnightSlide(List<Action> list, int from, GameState gameState, bool[] block)
        {
            var board = gameState.Position.main_board;
            var maxFile = InteractionManager.maxFile;
            var maxRank = InteractionManager.maxRank;
            var rank = from / InteractionManager.maxFile;
            var file = from % InteractionManager.maxFile;
            
            for (var i = 0; i < 8; i++)
            {
                var toRank = rank + KnightSlides[i][0];
                var toFile = file + KnightSlides[i][1];
                var to = toRank * maxFile + toFile;
                
                if (toRank < 0 || toRank >= maxRank || toFile < 0 || toFile >= maxFile || gameState.Position.active_board[to] == 0) continue;
                
                if (block[i] && block[i < 7 ? i + 1 : 0]) continue;
                
                if (board[to] == null)
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

        private static void Kill(List<Action> list, int from, GameState gameState)
        {
            for (var i = 0; i < InteractionManager.boardSize; i++)
            {
                var p = gameState.Position.main_board[i];
                if (p == null) continue;
                if (p.Effects.Contains(Effect.VelkarisMarked) && p.Color != gameState.OurSide)
                {
                    list.Add(new VelkarisKill(from, i));
                }
            }
        }
        public List<Action> MoveToMake(int from)
        {
            var gameState = InteractionManager.gameState;
            var piece = gameState.Position.main_board[from];
            var list = new List<Action>();
            KnightSlide(list, from, gameState, StraightSlide(list, from, gameState));
            if (piece.SkillCooldown == 0)
            {
                Kill(list, from, gameState);
            }
            
            return list;
        }
    }
}