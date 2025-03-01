using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Vector3 initPos;

    public  Transform target;

    // Start is called before the first frame update
    void Start()
    {
        initPos = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position - initPos;
    }
}
