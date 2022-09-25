using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private Side _side;
    [Zenject.Inject] private Zenject.DiContainer container;
    private Soldier squadLeader;
    private Soldier[] soldiers;

    private void Awake()
    {
        soldiers = new Soldier[2];
    }

    private void Start()
    {
        squadLeader = container.InstantiatePrefab(soldierPrefab, transform.position, transform.rotation, transform).GetComponent<Soldier>();
        squadLeader.transform.localScale = Vector3.one;
        squadLeader.side = _side;
        squadLeader.ChangeItems();

        for (int i = 0; i < soldiers.Length; i++)
        {
            var pos = transform.position;
            pos.x -= (i + 1) * transform.localScale.x;

            soldiers[i] = container.InstantiatePrefab(soldierPrefab, pos, transform.rotation, null).GetComponent<Soldier>();
            soldiers[i].transform.localScale = transform.localScale;
            soldiers[i].side = _side;
            soldiers[i].ChangeItems();

        }
    }
}
