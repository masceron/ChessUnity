using Game.Piece.PieceLogic;
using UnityEngine;

namespace UX.UI.Ingame
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EffectBar: MonoBehaviour
    {
        [SerializeField] private GameObject pieceStatusEffect;
        [SerializeField] private GameObject effectUI;
        

        public void Disable() {
            pieceStatusEffect.SetActive(false);
        }

        public void SetUpStatusEffects(PieceLogic piece)
        {
            pieceStatusEffect.SetActive(true);
            var already = pieceStatusEffect.transform.childCount;
            var needed = piece.Effects.Count;
            if (already < needed)
            {
                for (var i = 1; i <= needed - already; i++)
                {
                    Instantiate(effectUI, pieceStatusEffect.transform, true);
                }
            }
            else if (already > needed)
            {
                var index = already - 1;
                for (var i = 1; i <= already - needed; i++)
                {
                    Destroy(pieceStatusEffect.transform.GetChild(index).gameObject);
                    index--;
                }
            }

            for (var i = 0; i < needed; i++)
            {
                pieceStatusEffect.transform.GetChild(i).GetComponent<EffectUI>().Set(piece.Effects[i]);
            }
        }
    }
}