using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public float parallaxFactor = 0.05f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 offset = new Vector3(mousePosition.x, mousePosition.y, 0) * parallaxFactor;
        transform.position = startPosition + offset;
    }
}
