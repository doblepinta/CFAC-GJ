using UnityEngine;

public class BossWaypoints : MonoBehaviour
{
    public static Transform[] route;

    public void Awake()
    {
        route = new Transform[transform.childCount];

        for (int i = 0; i < route.Length; i++)
        {
            route[i] = transform.GetChild(i);
            Debug.Log(route[i]);
        }
    }

}