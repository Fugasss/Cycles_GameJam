using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndWindow : MonoBehaviour, IWindow
{
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
