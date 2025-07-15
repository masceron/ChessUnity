using System.Collections.Generic;
using Game.Board.Effects;
using Game.Board.General;
using Game.Board.Piece;
using UnityEngine;

namespace Game.Board.Assets
{
    public class AssetManager : MonoBehaviour
    {
        [SerializeField] private PieceObject[] pieceData;
        [SerializeField] private EffectObject[] effectData;
        public Dictionary<PieceType, PieceObject> PieceData;
        public Dictionary<EffectType, EffectObject> EffectData;

        public void Init()
        {
            PieceData = new Dictionary<PieceType, PieceObject>();
            foreach (var piece in pieceData)
            {
                PieceData.Add(piece.type, piece);
            }

            EffectData = new Dictionary<EffectType, EffectObject>();
            foreach (var effect in effectData)
            {
                EffectData.Add(effect.type, effect);
            }
        }
    }
}
