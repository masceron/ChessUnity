using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic;
using UnityEngine;
using Game.Action.Internal;
using Game.Action;
using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Game.Piece;

namespace Game.Effects.RegionalEffect
{
    public class DjinnBlessing: RegionalEffect
    {
        private int isActive;
        public DjinnBlessing() : base(RegionalEffectType.DjinnBlessing)
        {
            isActive = 1;
        }
        protected override void ApplyEffect(int currentTurn)
        {
            Debug.Log("isActive: " + isActive);
            if (isActive == 3)
            {
                PieceLogic[] board = MatchManager.Ins.GameState.PieceBoard;
                
                List<PieceLogic> validPieces = new List<PieceLogic>();
                foreach(PieceLogic piece in board) {
                    if (piece != null) {
                        validPieces.Add(piece);
                    }
                }
                
                if (validPieces.Count > 0) {
                    int randomInd = Random.Range(0, validPieces.Count);
                    if (validPieces[randomInd].PieceRank == PieceRank.Commander) return;

                    sbyte duration = (sbyte)Random.Range(1, 10);
                    sbyte strength = (sbyte)Random.Range(1, 10);
                    int roll = Random.Range(1, 101);
                    Debug.Log("roll: " + roll + " " + validPieces[randomInd].Type);
                    if (roll <= 45) {
                        ActionManager.EnqueueAction(new ApplyEffect(GetRandomBuffEffect(validPieces[randomInd], duration, strength)));
                    }
                    else if (roll <= 90) {
                        ActionManager.EnqueueAction(new ApplyEffect(GetRandomDebuffEffect(validPieces[randomInd], duration, strength)));
                    }
                    else {
                        ActionManager.EnqueueAction(new KillPiece(validPieces[randomInd].Pos));
                    }
                }
                isActive = 0;
            }
            isActive++;
        }
        private Effect GetRandomDebuffEffect(PieceLogic piece, sbyte duration, sbyte strength)
        {
            var debuffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Debuff)
                .Select(kvp => kvp.Key)
                .ToArray();
            
            var random = new System.Random();
            var selectedEffectName = debuffEffects[random.Next(debuffEffects.Length)];
            return GameState.CreateEffect(selectedEffectName, duration, strength, piece);
        }
        private Effect GetRandomBuffEffect(PieceLogic piece, sbyte duration, sbyte strength)
        {
            var buffEffects = AssetManager.Ins.EffectData
                .Where(kvp => kvp.Value.category == EffectCategory.Buff)
                .Select(kvp => kvp.Key)
                .ToArray();
            
            var random = new System.Random();
            var selectedEffectName = buffEffects[random.Next(buffEffects.Length)];
            return GameState.CreateEffect(selectedEffectName, duration, strength, piece);
        }

    }
}