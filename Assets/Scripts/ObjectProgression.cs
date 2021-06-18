using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProgression : MonoBehaviour
{
    // singleton for now
    // when we finalize all of the stats, we'll make this a static class so we don't need to make an object of it

    public static ObjectProgression Instance;

    public float startSpeed = 2;
    public float timeBetweenSpawns = 2;
    public float paintBallSpeed = 5;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public float GetCurrentSpawnTimeGap()
    {
        // this value will change in the future based on the time the player has played or based on the score
        return timeBetweenSpawns;
    }

}
