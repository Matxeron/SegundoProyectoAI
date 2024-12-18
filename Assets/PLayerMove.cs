using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerMove : MonoBehaviour
{
    //player move, mueve al jugador
    public float speed = 10;
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
   
        if (x != 0 || z != 0 )
        {
            transform.position += new Vector3(x, 0, z).normalized * speed * Time.deltaTime;
        }
  
    }
}
