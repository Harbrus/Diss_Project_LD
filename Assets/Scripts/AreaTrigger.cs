using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(this.gameObject.tag == "Death")
            {
                GameManager.Instance.RespawnPlayer();
            }
            else if (this.gameObject.tag == "Node")
            {
                GameManager.Instance.NodeReached(this.gameObject);
            }
            else if (this.gameObject.tag == "End")
            {
                GameManager.Instance.LevelComplete();
            }
        }
    }
}
