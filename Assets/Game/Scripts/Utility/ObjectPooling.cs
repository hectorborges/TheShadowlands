using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour
{
    public GameObject[] pooledObject;
    public int pooledAmount;
    public float waitTime = 0.001f;
    public bool willGrow = true;

    public List<GameObject> pooledObjects;
    public bool collectColliders;

    [HideInInspector] public List<Collider> pooledObjectsColliders;

    void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < pooledAmount; i++)
        {
            StartCoroutine(InstantiatePool(waitTime));
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            for (int i = 0; i < pooledObject.Length; i++)
            {
                GameObject obj = (GameObject)Instantiate(pooledObject[i]);
                obj.transform.parent = transform;
                pooledObjects.Add(obj);
                return obj;
            }
        }

        return null;
    }

    IEnumerator InstantiatePool(float waitTime)
    {
        for (int i = 0; i < pooledObject.Length; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject[i]);
            obj.transform.SetParent(transform);

            if (collectColliders)
            {
                Collider col = obj.GetComponent<Collider>();
                pooledObjectsColliders.Add(col);
                col.enabled = false;
            }

            obj.SetActive(false);
            pooledObjects.Add(obj);
        }

        yield return new WaitForSeconds(waitTime);
    }
}
