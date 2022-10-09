using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    [System.Serializable]
    public class NodeData
    {
        [SerializeField]
        public BuildingType type = BuildingType.Simple;
        [SerializeField]
        public Side side = Side.Vikings;
        [SerializeField]
        public Vector2 position;

        public NodeData(Vector2 position) => this.position = position;
    }
}