using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RendierSpawner : MonoBehaviour
{
    public float spawnTime = 10.0f;

    public float currentTime = 0.0f;

    [SerializeField] private GameObject reinDeer;
    [SerializeField] private float crashSpeed;

    [SerializeField] private List<GameObject> currentSpawned;
    [SerializeField] private List<Waypoint> currentWaypoints;

    public void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime > 0) return;
        currentTime = spawnTime;

        SpawnReindeer();
    }

    private void SpawnReindeer()
    {
        GameObject obj = Instantiate(reinDeer);
        currentSpawned.Add(obj);

        Waypoint waypoint = currentWaypoints[Random.Range(0, currentWaypoints.Count)];
        obj.transform.position = waypoint.From;

        Rigidbody rig = obj.GetComponent<Rigidbody>();
        Vector3 dir = (waypoint.To - waypoint.From).normalized;
        
        obj.transform.LookAt(transform.position + dir);
        rig.velocity = dir * crashSpeed;
    }
}