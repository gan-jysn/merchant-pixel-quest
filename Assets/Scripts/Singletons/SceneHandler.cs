using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : SingletonPersistent<SceneHandler> {

    [SerializeField] bool hasDelay = false;
    [SerializeField] float delay = 0f;

    //Loads Scene given Scene Name
    public void LoadScene(string sceneName) {
        if (string.IsNullOrEmpty(sceneName))
            return;

        if (!hasDelay) {
            SceneManager.LoadScene(sceneName);
        } else if (hasDelay) {
            StartCoroutine(DelayLoad(sceneName));
        }
    }

    private IEnumerator DelayLoad(string sceneName) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void CloseApplication() {
        StartCoroutine(DelayQuit());
    }

    private IEnumerator DelayQuit() {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }
}
