using System;
using System.Collections.Generic;
using System.Linq;
using Board.Action;
using Board.Interaction;
using Core.Effect;
using Core.General;
using Action = Board.Action.Action;

namespace Core.Piece
{
    public class Velkaris: PieceLogic
    {
        public PieceLogic Marked;
        public sbyte SkillCooldown;

        public Velkaris(PieceType type, Color color, ushort pos, List<Effect.Effect> effects) : base(type, color, pos, effects, 2)
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

        private bool[] StraightSlide(List<Action> list, GameState gameState, int effectiveRange)
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
                    if (effectiveRange > 0)
                    {
                        list.Add(new NormalMove(Pos, Pos, to, InteractionManager.PieceManager));
                    }

                    block[i] = false;
                    
                    var to2Rank = rank + Next[i][0];
                    var to2File = file + Next[i][1];
                    var to2 = to2Rank * maxFile + to2File;
                    
                    if (to2Rank < 0 || to2Rank >= maxRank || to2File < 0 || to2File >= maxFile || !gameState.ActiveBoard[to2]) continue;
                    if (board[to2] == null)
                    {
                        if (effectiveRange > 1)
                        {
                            list.Add(new NormalMove(Pos, Pos, to2, InteractionManager.PieceManager));
                        }
                    }
                    else if (board[to2].Color != gameState.OurSide)
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

        private void KnightSlide(List<Action> list, GameState gameState, bool[] block, int effectiveRange)
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
                    if (effectiveRange > 1)
                    {
                        list.Add(new NormalMove(Pos, Pos, to, InteractionManager.PieceManager));
                    }
                }
                else
                {
                    if (board[to].Color != gameState.OurSide)
                    {
                        list.Add(new NormalCapture(Pos, Pos, (ushort)to));
                    }
                }
            }
        }

        public override void PassTurn()
        {
            
        }

        public override List<Action> MoveToMake()
        {
            var gameState = InteractionManager.GameState;
            var list = new List<Action>();
            
            var effectiveRange = Math.Max(Range - (Effects.OfType<Slow>().Any() ? 1 : 0), 0);
            
            var blocker = StraightSlide(list, gameState, effectiveRange);
            KnightSlide(list, gameState, blocker, effectiveRange);
            
            if (SkillCooldown == -1 && Marked != null)
            {
                list.Add(new VelkarisKill(Pos, Pos, Marked.Pos));
            }

            return list;
        }
    }
}