using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject HeroKnight;

    void Update()
    {
        if (HeroKnight != null)
        {
            Vector3 position = transform.position;
            position.x = HeroKnight.transform.position.x;
            transform.position = position;
        }
    }
}
