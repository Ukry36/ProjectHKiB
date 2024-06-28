using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{
    #region Singleton

    public static SceneLoadingManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public IEnumerator LoadSceneCoroutine
    (string _dSN, Color _color, float _delay, float _innerDelay, Status _component, Vector3 _way)
    {
        InputManager.instance.StopPlayerInput(true);

        ScreenManager.instance.SetFadeColor(_color);
        yield return ScreenManager.instance.FadeCoroutine(1, _delay);
        _component.transform.position += _way;
        _component.entity.MovePoint.transform.position += _way;
        CameraManager.instance.StrictMovement(_way);

        yield return new WaitForSeconds(_innerDelay);

        AsyncOperation op = SceneManager.LoadSceneAsync(_dSN);
        SceneManager.sceneLoaded += LoadSceneEnd;
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {

            }
            else
            {
                op.allowSceneActivation = true;

            }
        }

    }

    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {
        StartCoroutine(LoadSceneEndCoroutine());

        SceneManager.sceneLoaded -= LoadSceneEnd;
    }

    private IEnumerator LoadSceneEndCoroutine()
    {
        yield return ScreenManager.instance.FadeCoroutine(0, 0.5f);
        InputManager.instance.StopPlayerInput(false);
    }
}
