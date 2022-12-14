using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{

    public GameObject LoadingScreen;
    public Slider Slider;

    [SerializeField] string s;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadAsync(s));
        }

    }

   IEnumerator LoadAsync (string s)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(s);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            Slider.value = progress;

            yield return null;
        }
    }
}
