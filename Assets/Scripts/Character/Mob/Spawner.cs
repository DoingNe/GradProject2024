using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector2.one);

        Gizmos.DrawWireCube(Vector2.zero, Vector2.one);
    }
}
