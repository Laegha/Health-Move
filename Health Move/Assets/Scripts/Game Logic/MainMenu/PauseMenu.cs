using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject canvasPrefab;

    public GameObject calibrationScreen;

    static PauseMenu instance;
    public static PauseMenu pauseMenu { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if(canvas == null)
            canvas = Instantiate(canvasPrefab).GetComponent<Canvas>();

        transform.SetParent(canvas.transform);        
    }

    private void OnEnable()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        foreach(Button b in buttons)
            if(b.transform.parent != transform)
                b.enabled = false;
    }

    private void OnDisable()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button b in buttons)
            if (b.transform.parent != transform)
                b.enabled = true;
    }
}