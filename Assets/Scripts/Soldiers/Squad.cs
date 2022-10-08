using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private Side _side;
    [SerializeField, Min(1)] private int soldiersAmount = 3;
    [SerializeField] private PathCreation.PathCreator pathCreator;
    [SerializeField] private float speed = 5f;
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

    private void Start()
    {
        InitializeSoldier(0);
        var f = soldiers[0].GetComponent<TestPathFollower>();
        primaryFollower = f.MakePrimary(pathCreator, 5f, 0f, PathCreation.EndOfPathInstruction.Loop);

        for (int i = 1; i < soldiers.Length; i++)
        {
            InitializeSoldier(i);
            f = soldiers[i].GetComponent<TestPathFollower>();
            secondaryFollowers[i - 1] =
                f.MakeSecondary(pathCreator, primaryFollower, 0f, PathCreation.EndOfPathInstruction.Loop, i * distanceFromPrimary);
        }
    }

    private void InitializeSoldier(int i)
    {
        soldiers[i] = container.InstantiatePrefab(soldierPrefab, transform.position, transform.rotation, transform).GetComponent<Soldier>();
        soldiers[i].transform.localScale = Vector3.one;
        soldiers[i].side = _side;
        soldiers[i].ChangeItems();
        soldiers[i].AddRenderPriority(soldiers.Length - i);
    }

    private void OnValidate()
    {
        if (primaryFollower == null)
            return;

        primaryFollower.SetSpeed(speed);

        for (int i = 0; i < secondaryFollowers.Length; i++)
        {
            secondaryFollowers[i].SetDistance(distanceFromPrimary * (i + 1));
        }
    }
}
