using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathScript : MonoBehaviour
{
    public Component[] pathChildren;
    public List<Transform> path;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        pathChildren = transform.GetComponentsInChildren<Transform>();
        path = new List<Transform>();
        foreach (Transform pathChild in pathChildren)
        {
            if (pathChild != transform)
                path.Add(pathChild);
        }

        for (int i = 0; i < path.Count; i++)
        {
            if (i > 0)
            {
                Gizmos.DrawLine(path [i].position, path [i - 1].position);
                Gizmos.DrawWireSphere(path [i].position, 0.3f);
            }
        }
    }
}
