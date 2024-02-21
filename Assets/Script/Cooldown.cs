using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Cooldown
{
    public enum Progress
    {
        Ready,
        Started,
        InProgress,
        Finished
    }

    public Progress CurrentProgress = Progress.Ready;

    public float Duration = 1.0f;

    public float TimeLeft
    {
        get { return _currentduration; }
    }

    public bool IsOnCooldown
    {
        get { return _IsOnCooldown; }
    }

    private float _currentduration = 0f;
    private bool _IsOnCooldown;

    private Coroutine _coroutine;

    public void StartCooldown()
    {
        if (CurrentProgress is Progress.Started or Progress.InProgress)
            return;

        _coroutine = CoroutineHost.Instance.StartCoroutine(DoCoolDown());
    }

    public void StopCoolDown()
    {
        if (_coroutine != null)
            CoroutineHost.Instance.StopCoroutine(_coroutine);

        _currentduration = 0f;
        _IsOnCooldown = false;
        CurrentProgress = Progress.Ready;
    }

    IEnumerator DoCoolDown()
    {
        CurrentProgress = Progress.Started;
        _currentduration = Duration;
        _IsOnCooldown = true;

        while (_currentduration > 0)
        {
            _currentduration -= Time.deltaTime;
            CurrentProgress = Progress.InProgress;

            yield return null;
        }

        _currentduration = 0f;
        _IsOnCooldown = false;

        CurrentProgress = Progress.Finished;
    }
}
