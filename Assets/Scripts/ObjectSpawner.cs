using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public int initialObjectCount = 5;
    public GameObject objectPrefab;
    public GameObject containerPrefab;

    public Transform spawnLocation;
    public float maxDistanceFromDefaultPos = 5;

    ObjectToContain[] objectsToContain;
    ColorObject containerColorObj;

    IEnumerator spawner;

    private void Awake()
    {
        objectsToContain = new ObjectToContain[initialObjectCount];
        // spawn the objects
        for(int i = 0; i < initialObjectCount; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            objectsToContain[i] = obj.GetComponent<ObjectToContain>();             
            obj.SetActive(false);
        }

        // spawn the container
        GameObject containerObj = Instantiate(containerPrefab);
        containerColorObj = containerObj.GetComponent<ColorObject>();
       
    }
    private void Start()
    {
        EnableNewContainer();

        spawner = Spawner();
        StartCoroutine(spawner);
    }

    public IEnumerator Spawner()
    {
        while(!GameManager.Instance.isGameOver)
        {
            yield return new WaitForSeconds(ObjectProgression.Instance.GetCurrentSpawnTimeGap());
            StartObjectToContain(GetSpawnableObjectIndex());
        }
    }

    public int GetSpawnableObjectIndex()
    {
        for(int i = 0; i < objectsToContain.Length; i++)
        {
            if (!objectsToContain[i].gameObject.activeInHierarchy)
            {
                return i;
            }
        }

        Debug.Log("Not enough objects");
        return 0;
    }

    public void EnableNewContainer()
    {
        // move container to default position
        // play new container animation(quickly moves from right to left to its position)

        RandomizeColor(containerColorObj);
        containerColorObj.gameObject.SetActive(true);
    }

    void StartObjectToContain(int index)
    {
        ObjectToContain obj = objectsToContain[index];
        MakeColorObjectUnique(obj.GetColorObject());
        obj.gameObject.SetActive(true);

        // start moving the object      
        obj.SetSpeed(ObjectProgression.Instance.startSpeed);
        Vector2 dir = (containerColorObj.transform.position - obj.transform.position).normalized;
        obj.SetDirection(dir);
        obj.SetMove(true);
    }

    void MakeColorObjectUnique(ColorObject colorObject)
    {
        RandomizeColor(colorObject);

        // random height
        float height = Random.Range(-maxDistanceFromDefaultPos, maxDistanceFromDefaultPos);
        // set position
        colorObject.transform.position = spawnLocation.position + Vector3.up * height;
    }

    void RandomizeColor(ColorObject colorObject)
    {
        colorObject.SetColor(Colors.Instance.GetRandomColorIndex());
    }
}
