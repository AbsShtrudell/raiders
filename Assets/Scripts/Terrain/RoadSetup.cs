using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    [ExecuteInEditMode]
    public class RoadSetup : MonoBehaviour
    {
        [SerializeField] private BuildingsGraphEditor editor;
        private RoadsDrawer roadsDrawer;

        private void OnEnable()
        {
            roadsDrawer = GetComponent<RoadsDrawer>();
        }

        public void Setup()
        {
            roadsDrawer.Roads = new List<Dreamteck.Splines.SplineComputer>();

            foreach (var road in editor.roads)
            {
                roadsDrawer.Roads.Add(road.PathCreator);
            }
        }
    }
}
