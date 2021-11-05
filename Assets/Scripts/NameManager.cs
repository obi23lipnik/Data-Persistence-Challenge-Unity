using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameManager : MonoBehaviour
{
    public static NameManager Instance;
    public string playerName;
    [SerializeField] private InputField nameInput;

    private void Awake()
    {
        // Destroy when trying to instantiate repeatedly
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ProceedToMain()
    {
        playerName = nameInput.text;
        SceneManager.LoadScene(1);
    }
}
