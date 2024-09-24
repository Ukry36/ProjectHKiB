using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{
    #region Singleton

    public static SceneLoadingManager instance;
    private float currentDelay, currentInnerDelay;

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
    (string _dSN, Color _color, float _delay, float _innerDelay, Status _component, Vector3 _pos, Vector3 _prevPos)
    {
        currentDelay = _delay;
        currentInnerDelay = _innerDelay;

        AudioManager.instance.ChangeAreaBGMs(null, currentDelay);
        InputManager.instance.StopPlayerInput(true);
        InputManager.instance.StopUIInput(true);

        MenuManager.instance.SetFadeColor(_color);
        yield return MenuManager.instance.FadeCoroutine(1, currentDelay);
        _component.transform.position = _pos;
        _component.entity.MovePoint.transform.position = _pos;
        CameraManager.instance.StrictMovement(_pos - _prevPos, _prevPos);

        yield return new WaitForSeconds(currentInnerDelay / 2);

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
        PlayerManager.instance.FriendlyResetWhenTransferposition();
        StartCoroutine(LoadSceneEndCoroutine());

        SceneManager.sceneLoaded -= LoadSceneEnd;
    }

    private IEnumerator LoadSceneEndCoroutine()
    {
        yield return new WaitForSeconds(currentInnerDelay / 2);
        yield return MenuManager.instance.FadeCoroutine(0, currentDelay);
        InputManager.instance.StopPlayerInput(false);
        InputManager.instance.StopUIInput(false);
    }
}
