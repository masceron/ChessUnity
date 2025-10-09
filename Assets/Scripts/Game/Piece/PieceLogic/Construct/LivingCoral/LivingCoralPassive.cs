using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Effects;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Construct.LivingCoral
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LivingCoralPassive : Effect, IEndTurnEffect
    {
        private List<PieceLogic> inRange = new();
        private readonly List<PieceLogic> evasionBuff = new();

        private readonly (int, int)[] rangeSpawn = new (int, int)[4]
        {
            (1, 0), (0, -1),
            (-1, 0), (0, 1)
        };
        public EndTurnEffectType EndTurnEffectType { get; }
        private const int Interval = 3;
        private int turnCounter = 0;
        
        public LivingCoralPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.LivingCoralPassive)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
            BuffEvasionInRange();
        }
        
        private void BuffEvasionInRange()
        {
            var newInRange = new List<PieceLogic>(); 
            
            for (var rankOff = -1; rankOff <= 1; rankOff++)
            {
                var rank = RankOf(Piece.Pos) + rankOff;
                if (!VerifyBounds(rank)) continue;
                
                for (var fileOff = -1; fileOff <= 1; fileOff++)
                {
                    if (rankOff == 0 && fileOff == 0) continue;
                    var file = FileOf(Piece.Pos) + fileOff;
                    if (!VerifyBounds(file)) continue;

                    var p = PieceOn(IndexOf(rank, file));

                    if (p != null && p.Color == Piece.Color)
                    {
                        newInRange.Add(p);
                    }
                }
            }

            foreach (var pieceEntered in newInRange.Except(inRange))
            {
                if (pieceEntered.Effects.Any(e => e.EffectName == EffectName.Evasion)) continue;
                
                evasionBuff.Add(pieceEntered);
                ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, 25, pieceEntered)));
            }

            foreach (var pieceExited in inRange.Except(newInRange))
            {
                if (!evasionBuff.Contains(pieceExited)) continue;
                evasionBuff.Remove(pieceExited);

                var effect = pieceExited.Effects.Find(e => e.GetType() == typeof(Evasion));
                if (effect == null) continue;
                
                ActionManager.EnqueueAction(new RemoveEffect(effect));
            }

            inRange = newInRange;
        }

        private void SummonClownFish()
        {
            var random = new System.Random();
            var emptySpots = rangeSpawn
                .Select(offset => IndexOf(RankOf(Piece.Pos) + offset.Item1, FileOf(Piece.Pos) + offset.Item2))
                .Where(index => PieceOn((ushort)index) == null)
                .ToList();

            if (emptySpots.Count > 0)
            {
                ushort indexToSpawn = (ushort)emptySpots[random.Next(emptySpots.Count)];
                ActionManager.EnqueueAction(
                    new SpawnPiece(new PieceConfig(PieceType.ClownFish, Piece.Color, indexToSpawn))
                );
            }
        }
        
        public void OnCallEnd(Action.Action action)
        {
            turnCounter++;
            if (turnCounter % Interval == 0)
            {
                SummonClownFish();
            }
            
            if (action is { DoesMoveChangePos: true } &&
                (action.Maker == Piece.Pos || ColorOfPiece(action.Maker) == Piece.Color))
            {
                BuffEvasionInRange();
            }
        }
    }
}