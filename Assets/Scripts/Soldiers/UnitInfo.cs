using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/UnitInfo", fileName = "New Unit Info")]
public class UnitInfo : ScriptableObject
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float damage = 20f;
}
