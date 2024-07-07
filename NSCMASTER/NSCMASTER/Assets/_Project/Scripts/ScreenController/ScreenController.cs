using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenController : MonoBehaviour
{
    public static void LoadScreen(string name)
    {
        SceneManager.LoadScene(name);
    }
}
