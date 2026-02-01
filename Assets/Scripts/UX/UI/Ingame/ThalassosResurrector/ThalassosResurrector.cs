using System.Linq;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece;
using PrimeTween;
using UnityEngine;
using ZLinq;

namespace UX.UI.Ingame.ThalassosResurrector
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ThalassosResurrector: MonoBehaviour
    {
        [SerializeField] private GameObject selector;
        [SerializeField] private GameObject item;
        private int caller;
        private int to;
        public void Load(int c, int t)
        {
            caller = c;
            to = t;
            var gameState = MatchManager.Ins.GameState;
            var pieceCaller = BoardUtils.PieceOn(c);
            var collection = !pieceCaller.Color ? gameState.WhiteCaptured : gameState.BlackCaptured;
            var pieceInfos = AssetManager.Ins.PieceData;
            var candidates = collection.Where(e => pieceInfos[e.Type].rank is PieceRank.Common or PieceRank.Swarm).Distinct().ToList();
            
            var already = selector.transform.childCount;
            var needed = candidates.Count;
            
            if (already < needed)
            {
                for (var i = 1; i <= needed - already; i++)
                {
                    Instantiate(item, selector.transform, true);
                }
            }
            else if (already > needed)
            {
                var index = already - 1;
                for (var i = 1; i <= already - needed; i++)
                {
                    Destroy(selector.transform.GetChild(index).gameObject);
                    index--;
                }
            }

            for (var i = 0; i < needed; i++)
            {
                selector.transform.GetChild(i).GetComponent<ResurrectorItem>().Load(candidates[i].Type);
            }
        }
        
        private void OnEnable()
        {
            var rect = (RectTransform)transform.GetChild(0);
            rect.anchoredPosition = new Vector2(-50, 0);
            Tween.UIAnchoredPosition(rect, Vector3.zero, 0.3f);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            ((RectTransform)transform.GetChild(0)).anchoredPosition = new Vector2(-50, 0);
        }

        public void Choose(string type)
        {
            MatchManager.Ins.InputProcessor.ExecuteAction(new ThalassosResurrect(caller, to, type));
            Disable();
        }
    }
}