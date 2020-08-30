using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameController gamecontroller;

    void OnMouseDown()
    {
        gamecontroller.StartRodada();
    }
}
