using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Relic;
using Game.Managers;
using Game.Piece;
using System.Collections.Generic;
using ZLinq;


namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChaoticConstructorSpawn : Effect, IEndTurnEffect
    {
        private int _currentTurn = 1;
        private readonly List<int> _storedPos;

        public ChaoticConstructorSpawn(sbyte strength, List<int> storedPos) : base(1, strength, null, "effect_chaotic_constructor_spawn")
        {
            _storedPos = storedPos;
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        private static readonly System.Random Rng = new System.Random();
        private static void Shuffle<T>(IList<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = Rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (_currentTurn == 0)
            {
                Shuffle(_storedPos);

                foreach (var pos in _storedPos)
                {
                    var constructPieces = (from piece in AssetManager.Ins.PieceData.Values where piece.rank == PieceRank.Construct select piece.key).ToList();

                    var idx = UnityEngine.Random.Range(0, constructPieces.Count);

                    var rd = UnityEngine.Random.Range(0, 101);
                    var color = rd > 50;
                    var cfg = new PieceConfig(constructPieces[idx], color, (ushort)pos);
                    ActionManager.ExecuteImmediately(new SpawnPiece(cfg));
                }

            }
            _currentTurn--;
        }
    }
}