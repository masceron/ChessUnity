using Data.UI.UIObject3D.Scripts;
using Game.Data.Pieces;
using Game.Managers;
using UnityEngine;

namespace UX.UI.Ingame
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CapturedUI: MonoBehaviour
    {
        public PieceConfig PieceInfo;

        public void Load(PieceConfig config)
        {
            PieceInfo = config;
            var info = AssetManager.Ins.PieceData[config.Type];
            transform.GetChild(0).GetComponent<UIObject3D>().ObjectPrefab = info.prefab.transform;
        }
    }
}