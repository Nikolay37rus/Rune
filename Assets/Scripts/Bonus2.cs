using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus2 : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.Rotate(0, 40 * Time.deltaTime, 0);
    }
}
