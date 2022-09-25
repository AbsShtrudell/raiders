using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private Side _side;
    [Zenject.Inject] private Zenject.DiContainer container;
    private Soldier[] soldiers;

    private void Awake()
    {
        soldiers = new Soldier[3];
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            var pos = transform.position;
            pos.x -= i;

            soldiers[i] = container.InstantiatePrefab(soldierPrefab, pos, Quaternion.identity, transform).GetComponent<Soldier>();
            soldiers[i].side = _side;
            soldiers[i].ChangeItems();

        }
    }
}
