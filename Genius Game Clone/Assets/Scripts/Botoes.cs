using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botoes : MonoBehaviour
{
    public GameController gameController;
    public int idBotoes;

    void OnMouseDown()
    {
        if (gameController.gameState == GameState.RESPONDER)
        {
            gameController.StartCoroutine("responder", idBotoes);
        }
    }
}
