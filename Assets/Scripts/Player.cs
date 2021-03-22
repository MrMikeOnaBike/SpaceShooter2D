using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //take the current position and put it in the starting position near the bottom center of the screen
        //current position = new position(0, -4(ish), 0)

        transform.position = new Vector3(0f, -3.6f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
