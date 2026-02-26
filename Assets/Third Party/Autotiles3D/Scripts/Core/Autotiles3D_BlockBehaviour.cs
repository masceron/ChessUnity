using Third_Party.Autotiles3D.Scripts.Utility;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Third_Party.Autotiles3D.Scripts.Core
{
    public class Autotiles3D_BlockBehaviour : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private int _tileID = -1;

        [HideInInspector] [SerializeField] private int _ruleID;

        [HideInInspector] [SerializeField] private string _tileName;

        [HideInInspector] [SerializeField] private string _group;

        [HideInInspector] [SerializeField] private Vector3Int _internalPosition;

        [HideInInspector] [SerializeField] private Quaternion _localRotation;

        [HideInInspector] [SerializeField] private bool _isBaked;

        public GameObject View;
        private Autotiles3D_Anchor _anchor;

        private Autotiles3D_Grid _grid;

        public string TileName => _tileName;
        public int TileID => _tileID;
        public string GroupName => _group;
        public int RuleID => _ruleID;

        public Vector3Int InternalPosition
        {
            get => _internalPosition;
            set => _internalPosition = value;
        }

        public Quaternion LocalRotation
        {
            get => _localRotation;
            set => _localRotation = value;
        }

        public Autotiles3D_Grid Grid
        {
            get
            {
                if (this == null)
                    return null;
                if (_grid == null)
                    _grid = transform.GetComponentInParent<Autotiles3D_Grid>();
                return _grid;
            }
        }

        public Autotiles3D_Anchor Anchor
        {
            get
            {
                if (this == null)
                    return null;
                if (_anchor == null)
                    _anchor = transform.GetComponentInParent<Autotiles3D_Anchor>();
                return _anchor;
            }
        }

        public bool IsBaked
        {
            get
            {
                if (Anchor == null)
                    _isBaked = false;
                else if (Anchor.BakedParent == null)
                    _isBaked = false;
                return _isBaked;
            }
            set => _isBaked = value;
        }

        public void ToggleView(bool enable)
        {
            if (View != null)
                View.SetActive(enable);
        }

        public InternalNode GetInternalNode()
        {
            if (Anchor != null)
            {
                var layer = Anchor.Layer;
                if (layer != null)
                    if (layer.ContainsKey(_internalPosition))
                        return layer.GetInternalNode(_internalPosition);
            }

            return null;
        }

        public void OnInstanceUpdate(InternalNode node)
        {
            _tileID = node.TileID;
            _group = node.TileGroupName;
            _tileName = node.TileName;
            _ruleID = node.RuleID;

            if (Mathf.Abs(node.TileID) < 5) Debug.Log("assigning empty tileiD");

            InternalPosition = node.InternalPosition;
            LocalRotation = node.LocalRotation;
        }

#if UNITY_EDITOR

        public Autotiles3D_Tile GetTile()
        {
            return Autotiles3DUtility.GetTile(_tileID, _tileName, _group);
        }


        public void UpdateTileInfo(Autotiles3D_Tile tile)
        {
            _group = tile.Group;
            _tileName = tile.Name;
            _tileID = tile.TileID;
        }

        public InternalNode Randomize(bool focusInstance = true)
        {
            var node = GetInternalNode();
            if (node != null)
                node.Randomize();
            else
                Debug.LogWarning(
                    "Couldnt retrieve Internal node. Is this block part of the correct Grid->Layer->Anchor hierarchy?");
            return node;
        }

#endif
    }
}