using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cars : MonoBehaviour
{
    /*public Transform[] waypoints;
    public GameObject carPrefab;
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 15f;
    public int initialCarCount = 3;

    private void Start()
    {
        for (int i = 0; i < initialCarCount; i++)
        {
            SpawnCarAtRandomWaypoint();
        }

        StartCoroutine(SpawnCar());
    }

    private IEnumerator SpawnCar()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnCarAtFixedWaypoint();
        }
    }

    private void SpawnCarAtRandomWaypoint()
    {
        int randomWaypointIndex = Random.Range(0, waypoints.Length);
        Transform startWaypoint = waypoints[randomWaypointIndex];

        GameObject carGO = Instantiate(carPrefab, startWaypoint.position, Quaternion.identity);

        WaypointController mover = carGO.GetComponent<WaypointController>();
        mover.SetWaypoints(waypoints);
        mover.SetMoveSpeed(5f);
    }

    private void SpawnCarAtFixedWaypoint()
    {
        Transform startWaypoint = waypoints[0];

        GameObject carGO = Instantiate(carPrefab, startWaypoint.position, Quaternion.identity);

        WaypointController mover = carGO.GetComponent<WaypointController>();
        mover.SetWaypoints(waypoints);
        mover.SetMoveSpeed(5f);
    }*/
}
