using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Relic;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece;
using System.Collections.Generic;
using System.Linq;
using ZLinq;


namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChaoticConstructorSpawn : Effect, IEndTurnEffect
    {
        private readonly bool color;
        private int TurnToActive = 1;
        private int currentTurn = 1;
        private List<int> storedPos;
        private ChaoticConstructorPending chaoticConstructorPending;

        public ChaoticConstructorSpawn(sbyte strength, bool color, ChaoticConstructorPending ccp, List<int> storedPos) : base(1, strength, null, "effect_chaotic_constructor_spawn")
        {
            this.color = color;
            this.storedPos = storedPos;
            chaoticConstructorPending = ccp;
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
            if (currentTurn == 0)
            {
                Shuffle(storedPos);

                foreach (var pos in storedPos)
                {
                    var constructPieces = (from piece in AssetManager.Ins.PieceData.Values where piece.rank == PieceRank.Construct select piece.key).ToList();

                    int idx = UnityEngine.Random.Range(0, constructPieces.Count);

                    bool color;
                    int rd = UnityEngine.Random.Range(0, 101);
                    if (rd <= 50) color = false;
                    else color = true;
                    var cfg = new PieceConfig(constructPieces[idx], color, (ushort)pos);
                    ActionManager.ExecuteImmediately(new SpawnPiece(cfg));
                }

                chaoticConstructorPending.Dispose();
            }
            currentTurn--;
        }
    }
}