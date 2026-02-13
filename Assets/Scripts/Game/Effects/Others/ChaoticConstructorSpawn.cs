using Game.Action;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece;
using System.Collections.Generic;
using Game.Effects.Triggers;
using ZLinq;


namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChaoticConstructorSpawn : Effect, IStartTurnTrigger
    {
        private int _currentTurn = 1;
        private readonly List<int> _storedPos;

        public ChaoticConstructorSpawn(int strength, List<int> storedPos) : base(1, strength, null, "effect_chaotic_constructor_spawn")
        {
            _storedPos = storedPos;
            StartTurnEffectType = StartTurnEffectType.StartOfAnyTurn;
        }

        private static readonly System.Random Rng = new();
        private static void Shuffle<T>(IList<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = Rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Buff;

        public StartTurnEffectType StartTurnEffectType { get; }
        public void OnCallStart(Action.Action lastMainAction)
        {
            if (_currentTurn == 0)
            {
                Shuffle(_storedPos);

                foreach (var cfg in from pos in _storedPos let constructPieces = (from piece in AssetManager.Ins.PieceData.Values where piece.rank == PieceRank.Construct select piece.key).ToList() let idx = UnityEngine.Random.Range(0, constructPieces.Count) let rd = UnityEngine.Random.Range(0, 101) let color = rd > 50 select new PieceConfig(constructPieces[idx], color, pos))
                {
                    ActionManager.EnqueueAction(new SpawnPiece(cfg));
                }
            }
            _currentTurn--;
        }
    }
}