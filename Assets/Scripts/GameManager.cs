using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Making a Singleton of this class
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    bool is2PlayerMode = false;

    public bool twoPlayerMode
    {
        get
        {
            return is2PlayerMode;
        }
        set
        {
            is2PlayerMode = value;
        }
    }
}
