using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Utility : MonoBehaviour
{
    public static float CheckDistance(Vector3 point1, Vector3 point2)
    {
        Vector3 heading;
        float distance;
        Vector3 direction;
        float distanceSquared;

        heading.x = point1.x - point2.x;
        heading.y = point1.y - point2.y;
        heading.z = point1.z - point2.z;

        distanceSquared = heading.x * heading.x + heading.y * heading.y + heading.z * heading.z;
        distance = Mathf.Sqrt(distanceSquared);

        direction.x = heading.x / distance;
        direction.y = heading.y / distance;
        direction.z = heading.z / distance;
        return distance;
    }

    public static void SetTransparency(Image p_image, float p_transparency)
    {
        if (p_image != null)
        {
            Color __alpha = p_image.color;
            __alpha.a = p_transparency;
            p_image.color = __alpha;
        }
    }
}