using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [System.Serializable]
    public enum Direction { Forward = 1, Backward = -1 }

    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private Side _side;
    [SerializeField, Min(1)] private int soldiersAmount = 3;
    private Queue<Tuple<SplineComputer, Direction>> _roads;
    [SerializeField, Min(0f)] private float _speed = 5f;
    [SerializeField] private float distanceFromPrimary = 1f;
    [Zenject.Inject] private Zenject.DiContainer container;
    private Soldier[] soldiers;
    private PrimaryFollowerBehavior primaryFollower = null;
    private SecondaryFollowerBehavior[] secondaryFollowers;

    private void Awake()
    {
        soldiers = new Soldier[soldiersAmount];
        secondaryFollowers = new SecondaryFollowerBehavior[soldiersAmount - 1];
    }

    public void MakeSoldiers()
    {
        InitializeSoldier(0);
        var f = soldiers[0].GetComponent<TestPathFollower>();
        primaryFollower = f.MakePrimary(_roads, _speed);

        for (int i = 1; i < soldiers.Length; i++)
        {
            InitializeSoldier(i);
            f = soldiers[i].GetComponent<TestPathFollower>();
            secondaryFollowers[i - 1] =
                f.MakeSecondary(primaryFollower, distanceFromPrimary * i);
        }
    }

    private void InitializeSoldier(int i)
    {
        soldiers[i] = container.InstantiatePrefab(soldierPrefab, transform.position, transform.rotation, transform).GetComponent<Soldier>();
        soldiers[i].side = _side;
        soldiers[i].ChangeItems();
        soldiers[i].AddRenderPriority(soldiers.Length - i);
    }

    private void OnValidate()
    {
        if (primaryFollower == null)
            return;

        primaryFollower.SetSpeed(_speed);

        for (int i = 0; i < secondaryFollowers.Length; i++)
        {
            secondaryFollowers[i].SetDistance(distanceFromPrimary * (i + 1));
        }
    }

    public void SetRoads(Queue<Tuple<SplineComputer, Direction>> roads)
    {
        _roads = roads;
    }

    public void SetSide(Side side)
    {
        _side = side;
    }
}
