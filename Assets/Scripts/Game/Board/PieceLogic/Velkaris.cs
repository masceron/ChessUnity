using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.General;
using Game.Board.Interaction;

namespace Game.Board.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Velkaris: PieceLogic
    {
        public PieceLogic Marked;
        public sbyte SkillCooldown;

        public Velkaris(Color color, ushort pos) : base(color, pos, 2, 2, PieceRank.Commander)
        {
            Marked = null;
            SkillCooldown = 0;
            Rank = PieceRank.Commander;
        }
        
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

        private bool[] StraightSlide(List<Action.Action> list, GameState gameState)
        {
            var block = new bool[8];
            var board = gameState.MainBoard;
            var rank = Pos / InteractionManager.MaxFile;
            var file = Pos % InteractionManager.MaxFile;
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
                    if (MoveRange > 0)
                    {
                        list.Add(new NormalMove(Pos, Pos, to));
                    }

                    block[i] = false;
                    
                    var to2Rank = rank + Next[i][0];
                    var to2File = file + Next[i][1];
                    var to2 = to2Rank * maxFile + to2File;
                    
                    if (to2Rank < 0 || to2Rank >= maxRank || to2File < 0 || to2File >= maxFile || !gameState.ActiveBoard[to2]) continue;
                    if (board[to2] == null)
                    {
                        if (MoveRange > 1)
                        {
                            list.Add(new NormalMove(Pos, Pos, to2));
                        }
                    }
                    else if (board[to2].Color != gameState.OurSide && AttackRange > 1)
                    {
                        list.Add(new NormalCapture(Pos, Pos, (ushort)to2));
                    } 
                }
                else
                {
                    block[i] = true;
                    if (board[to].Color != gameState.OurSide)
                    {
                        list.Add(new NormalCapture(Pos, Pos, (ushort)to));
                    }
                }
            }
            return block;
        }

        private void KnightSlide(List<Action.Action> list, GameState gameState, bool[] block)
        {
            var board = gameState.MainBoard;
            var maxFile = InteractionManager.MaxFile;
            var maxRank = InteractionManager.MaxRank;
            var rank = Pos / InteractionManager.MaxFile;
            var file = Pos % InteractionManager.MaxFile;
            
            for (var i = 0; i < 8; i++)
            {
                var toRank = rank + KnightSlides[i][0];
                var toFile = file + KnightSlides[i][1];
                var to = toRank * maxFile + toFile;
                
                if (toRank < 0 || toRank >= maxRank || toFile < 0 || toFile >= maxFile || !gameState.ActiveBoard[to]) continue;
                
                if (block[i] && block[i < 7 ? i + 1 : 0]) continue;
                
                if (board[to] == null)
                {
                    if (MoveRange > 1)
                    {
                        list.Add(new NormalMove(Pos, Pos, to));
                    }
                }
                else
                {
                    if (board[to].Color != gameState.OurSide && AttackRange > 1)
                    {
                        list.Add(new NormalCapture(Pos, Pos, (ushort)to));
                    }
                }
            }
        }

        public override List<Action.Action> MoveToMake()
        {
            var gameState = InteractionManager.GameState;
            var list = new List<Action.Action>();
            
            var blocker = StraightSlide(list, gameState);
            KnightSlide(list, gameState, blocker);
            
            if (SkillCooldown == -1 && Marked != null)
            {
                list.Add(new VelkarisKill(Pos, Pos, Marked.Pos));
            }

            return list;
        }
    }
}