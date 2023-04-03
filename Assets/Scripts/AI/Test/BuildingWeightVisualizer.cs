using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders.AI
{
    public class BuildingWeightVisualizer : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI text;

        public void ChangeText(WeightedBuilding weightedBuilding)
        {
            text.text = weightedBuilding.ToString();
        }
    }
}
