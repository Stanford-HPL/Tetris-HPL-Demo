using System;
using System.Collections;
using System.Collections.Generic;
using METAVIZ;
using UnityEngine;

public class ImplementVPI : MonoBehaviour
{
    private TargetDistractorStimulus _targetDistractorStimulus;
    
    public static Action OnGameEnd;
    
    private void Awake()
    {
        _targetDistractorStimulus = GetComponent<TargetDistractorStimulus>();
        OnGameEnd += GameOver;
    }

    private void Start()
    {
        _targetDistractorStimulus.Observe(interactedWith: false, shouldRespond: false);
    }

    public void BlockPlaced()
    {
        if (!Playfield.IsRowFull((int)MathF.Round(transform.position.y + 1f)))
        {
            _targetDistractorStimulus.Observe(interactedWith: true, shouldRespond: false);
        }
    }
    
    private void OnDestroy()
    {
        if (Common.IsRunning)
        {
            _targetDistractorStimulus.Observe(interactedWith: true, shouldRespond: true);
        }
        OnGameEnd -= GameOver;
    }

    private void GameOver()
    {
        _targetDistractorStimulus.Observe(interactedWith: false, shouldRespond: true);
        print("GameOver");
    }

    public void GameEnd()
    {
        OnGameEnd?.Invoke();
    }
}
