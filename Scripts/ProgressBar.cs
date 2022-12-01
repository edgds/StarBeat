using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private float time;
    private Vector2 startPos;
    private Vector2 endPos;
    public float timeToReach;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        startPos = transform.position;
        endPos = new Vector2(0.0f, transform.position.y);
    }

    void FixedUpdate()
    {
        time += Time.deltaTime/timeToReach;
        transform.position = Vector2.Lerp(startPos, endPos, time);
    }
}
