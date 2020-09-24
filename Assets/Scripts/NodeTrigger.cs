using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.NodeReached(this.gameObject);
        }
    }
}
