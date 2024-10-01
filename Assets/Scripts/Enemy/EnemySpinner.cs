using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpinner : MonoBehaviour
{


    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * -200f);
     


    }

}
