using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportManager : MonoBehaviour
{
    #region Singleton

    public static TeleportManager instance;
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

    public IEnumerator TransferPosCoroutine
    (Status _component, Vector2 _dir, Vector3 _pos, float _delay, float _innerDelay, Color _color)
    {
        Vector3 Way = _pos - _component.entity.MovePoint.transform.position;

        if (_component.isPlayer)
        {
            _component.TransferPositionInvincible(_delay * 2 + _innerDelay);
            InputManager.instance.StopUIInput(true);
            InputManager.instance.StopPlayerInput(true);
            MenuManager.instance.SetFadeColor(_color);
            yield return MenuManager.instance.FadeCoroutine(1, _delay);
        }
        else
        {
            _component.TransferPositionInvincible(_innerDelay);
        }
        _component.transform.position = _pos;
        _component.entity.MovePoint.transform.position = _pos;



        if (_component.isPlayer)
        {
            CameraManager.instance.StrictMovement(Way, _component.entity.MovePoint.transform.position);
        }


        if (_dir != Vector2.zero)
            _component.entity.SetAnimDir(_dir);

        yield return new WaitForSeconds(_innerDelay);


        if (_component.isPlayer)
        {
            PlayerManager.instance.FriendlyResetWhenTransferposition();
            yield return MenuManager.instance.FadeCoroutine(0, _delay);
            InputManager.instance.StopPlayerInput(false);
            InputManager.instance.StopUIInput(false);
        }
    }

    public void InstantTransferPos
    (Status _component, Vector3 _pos, bool _keepX, bool _keepY)
    {
        Vector3 Way = _pos - _component.entity.MovePoint.transform.position;

        Way.x = _keepX ? 0 : Way.x;
        Way.y = _keepY ? 0 : Way.y;
        if (_component.isPlayer)
        {
            PlayerManager.instance.FriendlyInstantTransfer(Way);
            _component.TransferPositionInvincible(0.1f);
        }
        _component.transform.position += Way;
        _component.entity.MovePoint.transform.position += Way;


        if (_component.isPlayer)
        {
            CameraManager.instance.StrictMovement(Way, _component.entity.MovePoint.transform.position);
        }
    }

    public IEnumerator LoadSceneCoroutine
    (string _dSN, Status _component, Vector2 _dir, Vector3 _pos, float _delay, float _innerDelay, Color _color)
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

        if (_dir != Vector2.zero)
            _component.entity.SetAnimDir(_dir);

        CameraManager.instance.StrictMovement(_pos - _component.entity.MovePoint.transform.position, _component.entity.MovePoint.transform.position);

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
