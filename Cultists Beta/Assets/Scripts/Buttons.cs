using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void SkipTurn()
    {
        GameIntel.Instance.SwitchingTurns();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void cheating()
    {
        GameIntel.Instance.cheat();
    }

    public void ShipWhole()
    {
        GameIntel.Instance.NewTurn();
    }
}
