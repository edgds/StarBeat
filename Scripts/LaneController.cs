using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    public int rotationSpeed;
    public GameObject Sun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Sun.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
