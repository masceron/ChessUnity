using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Game.Data.Pieces;
using Game.Piece;
using TMPro;
using UnityEngine;

namespace UX.UI.Followers
{
    public enum Filters
    {
        Commander, Champion, Elite, Common, Swarm, Summoned, Construct, Skill, Relic
    }
    
    public class PieceList: MonoBehaviour
    {
        [SerializeField] private TMP_InputField searchBar;
        [SerializeField] private UDictionary<Filters, GameObject> filters;
        [SerializeField] private Transform list;

        private Dictionary<PieceType, PieceObject> piecesData;

        public void Load(Dictionary<PieceType, PieceObject> p)
        {
            piecesData = p;
        }
        
        public void Search(string start)
        {
            if (start.Length < 3) return;
            start = start.ToLower();
            var pieces = piecesData.Values.Where(p => p.pieceName.Contains(start));

            foreach (var r in pieces)
            {
                Debug.Log(r.pieceName);
            }
        }
    }

    
}