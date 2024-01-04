using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    void Update()
    {
        float x = gameObject.transform.position.x;
        float z = gameObject.transform.position.z;
        float y = 0.5f + Mathf.Sin(Time.time) * 0.4f;
        gameObject.transform.position = new Vector3(x, y, z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("PoerUp destroyed!");
            Destroy(gameObject);
        }
    }
}
