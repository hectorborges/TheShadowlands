using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
	IEnumerator Start ()
    {
        yield return new WaitForSeconds(1);
        GetComponent<NavMeshSurface>().BuildNavMesh();
	}
}
