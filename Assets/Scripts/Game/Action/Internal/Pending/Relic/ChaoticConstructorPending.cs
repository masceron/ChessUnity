using Game.Action.Quiets;
using Game.Action.Relics;
using Game.Common;
using Game.Effects;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class ChaoticConstructorPending : Action, IPendingAble, System.IDisposable, IRelicAction, IEndTurnEffect
    {
        private ChaoticConstructor chaoticConstructor;
        private int TurnToActive = 1;
        private int currentTurn = 1;
        private List<int> storedPos;

        public ChaoticConstructorPending(int maker, ChaoticConstructor cc) : base(maker)
        {
            Maker = (ushort)maker;
            chaoticConstructor = cc;
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
            storedPos = new List<int>();
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action lastMainAction)
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

                Dispose();
            }
            currentTurn--;
        }

        private static readonly System.Random Rng = new System.Random();
        private static void Shuffle<T>(IList<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = Rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        public void CompleteAction()
        {
            TileManager.Ins.UnmarkAll();
            for (int i = 0; i < BoardSize; ++i) 
            { 
                var p = PieceOn(i);
                if (p == null || p.PieceRank != Game.Piece.PieceRank.Construct) continue;

                storedPos.Add(p.Pos);
                ActionManager.ExecuteImmediately(new KillPiece(p.Pos));
            }
        }

        

        public void Dispose()
        {
            BoardViewer.SelectingFunction = 0;
            chaoticConstructor = null;
        }


        protected override void ModifyGameState()
        {
            

        }
    }
}

