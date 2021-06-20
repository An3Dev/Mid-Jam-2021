using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance;
    public int initialObjectCount = 8;
    public GameObject objectPrefab;
    public GameObject containerPrefab;

    public Transform spawnLocation, containerLocation;
    public float maxDistanceFromDefaultPos = 5;

    ObjectToContain[] objectsToContainArray;
    ColorObject containerColorObj;
    Container container;

    IEnumerator spawner;

    int numSpawnedObjectsForThisContainer = 0;
    int numObjectsForContainerChange = 2;

    public static float minYPos, maxYPos;

    Camera cam;
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

        objectsToContainArray = new ObjectToContain[initialObjectCount];
        // spawn the objects
        for(int i = 0; i < initialObjectCount; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            objectsToContainArray[i] = obj.GetComponent<ObjectToContain>();             
            obj.SetActive(false);
        }

        // spawn the container
        GameObject containerObj = Instantiate(containerPrefab);
        container = containerObj.GetComponent<Container>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
       
    }
    private void Start()
    {
        containerColorObj = container.GetColorObject();
        SetContainerChange();

        EnableNewContainer();

        spawner = Spawner();
        StartCoroutine(spawner);

        minYPos = cam.ViewportToWorldPoint(new Vector3(0, Stats.minViewportObjectPos, 0)).y;
        maxYPos = cam.ViewportToWorldPoint(new Vector3(0, Stats.maxViewportObjectPos, 0)).y;
    }

    public IEnumerator Spawner()
    {
        while(!GameManager.Instance.isGameOver)
        {
            yield return new WaitForSeconds(Stats.GetCurrentSpawnTimeGap());
            if (GameManager.Instance.isGameOver)
            {
                yield break;
            }
            ObjectToContain obj = EnableObjectToContain(GetSpawnableObjectIndex());

            // if we have spawned the required amount of objects for the container to change color.
            if (numSpawnedObjectsForThisContainer == numObjectsForContainerChange) 
            {
                obj.SetIsContainerTrigger(true);
                numSpawnedObjectsForThisContainer = 0;
                SetContainerChange();

                // delay spawning for one second
                yield return new WaitForSeconds(Stats.containerChangeSpawnDelay);           
            } else
            {
                obj.SetIsContainerTrigger(false);
            }
        }
    }

    public void SetContainerChange()
    {
        numObjectsForContainerChange = Random.Range(Stats.minContainerColorChangePasses, Stats.maxContainerColorChangePasses);
        //container.SetNumObjectsForContainerChange(numObjectsForContainerChange);
    }

    public int GetSpawnableObjectIndex()
    {
        for(int i = 0; i < objectsToContainArray.Length; i++)
        {
            if (!objectsToContainArray[i].gameObject.activeInHierarchy)
            {
                return i;
            }
        }

        Debug.Log("Not enough objects");
        return 0;
    }

    public void EnableNewContainer()
    {
        container.transform.position = containerLocation.position;
        RandomizeColor(containerColorObj);
        containerColorObj.gameObject.SetActive(true);
    }

    public void OnGameOver()
    {
        StopCoroutine("Spawner");
        for(int i = 0; i < objectsToContainArray.Length; i++)
        {
            if (objectsToContainArray[i].gameObject.activeInHierarchy)
            {
                objectsToContainArray[i].OnGameOver();
            }
        }
    }

    ObjectToContain EnableObjectToContain(int index)
    {
        ObjectToContain obj = objectsToContainArray[index];
        MakeColorObjectUnique(obj.GetColorObject());
        obj.gameObject.SetActive(true);

        numSpawnedObjectsForThisContainer++;

        // start moving the object      
        obj.SetSpeed(Stats.GetCurrentObjectSpeed());
        Vector2 dir = Vector2.right;
        obj.SetDirection(dir);
        obj.SetMove(true);
        return obj;
    }

    void MakeColorObjectUnique(ColorObject colorObject)
    {
        RandomizeColor(colorObject);

        // random height
        float multiplier = Stats.Instance.GetMaxDistanceFromCenterMultiplier();
        float height =  Random.Range(0 - minYPos * multiplier, maxYPos * multiplier);
        // set position
        colorObject.transform.position = new Vector2(spawnLocation.position.x, height);
    }

    void RandomizeColor(ColorObject colorObject)
    {
        colorObject.SetColor(Colors.Instance.GetRandomColorIndex());
    }
}
