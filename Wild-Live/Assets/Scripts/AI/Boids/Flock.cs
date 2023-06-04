using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Flock : MonoBehaviour
{
    #region Variables
    private NavMeshAgent agent = null;

    [SerializeField]
    private GameObject target = null;

    [field: SerializeField]
    public Vector2 AveragePosition { get; set; } = Vector2.zero;

    [field: SerializeField]
    public List<Boid> Boids { get; set; } = null;

    [SerializeField]
    private GameObject boidPrefab = null;

    [SerializeField]
    private int preyCount = 0;

    [field: SerializeField]
    public float Speed { get; set; } = 2f;

    [field: SerializeField]
    public float CenterStrength { get; set; } = 0f;

    [field: SerializeField]
    public float LocalBoidDistance { get; set; } = 0f;

    [field: SerializeField]
    public float AvoidStrength { get; set; } = 0f;

    [field: SerializeField]
    public float AvoidDistance { get; set; } = 0f;

    [field: SerializeField]
    public float AlignStrength { get; set; } = 0f;

    [field: SerializeField]
    public float AlignDistance { get; set; } = 0f;
    #endregion

    private void Awake()
    {
        Boids = new List<Boid>();
        agent = GetComponent<NavMeshAgent>();

        for (int i = 0; i < preyCount; i++)
        {
            var rngX = Random.Range(-5, 5);
            var rngY = Random.Range(-5, 5);
            var pos = new Vector3(rngX, transform.position.y, rngY);

            var temp = Instantiate(boidPrefab, transform);
            temp.transform.localPosition = pos;
            Boids.Add(temp.GetComponent<Boid>());
        }

    }

    private void Update()
    {
        agent.destination = target.transform.position;

        Vector2 positionSum = transform.position;
        int count = 0;

        foreach (var boid in Boids)
        {
            float distance = Vector2.Distance(boid.transform.position, transform.position);
            if (distance <= LocalBoidDistance)
            {
                positionSum += (Vector2)boid.transform.position;
                count++;
            }
        }
        if (count == 0) return;

        AveragePosition = positionSum / count;
    }
}
