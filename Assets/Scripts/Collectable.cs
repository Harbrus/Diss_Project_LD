﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.CoinCollected(this.gameObject);
            gameObject.SetActive(false);
        }
    }
}
