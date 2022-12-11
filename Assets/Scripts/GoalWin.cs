using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.updateScore((int)gameManager.instance.currTime * 100);

            gameManager.instance.youWin();
        }
    }
}
