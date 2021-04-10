using System.Collections;
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

        Vector3 to = waypoint.To;
        Vector3 from = waypoint.From;
        
        
        Vector3 dir = (to - from).normalized;
        Rigidbody rig = obj.GetComponent<Rigidbody>();


        obj.transform.LookAt(transform.position + dir);
        StartCoroutine(LaunchDeer(rig, dir));
    }

    private IEnumerator LaunchDeer(Rigidbody rig, Vector3 dir)
    {
        yield return new WaitForSeconds(2f);
        rig.velocity = dir * (crashSpeed * Random.Range(0.75f, 1f));
        yield return new WaitForSeconds(5f);
        Destroy(rig.gameObject);
    }
}