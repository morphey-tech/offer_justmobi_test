﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core.Dialog.Controller
{
  public sealed class DialogController
  {
    private const string OPEN_ANIMATION_ID = "Open";
    private const string CLOSE_ANIMATION_ID = "Close";
    
    public IDialog? DialogInstance { get; private set; }

    public bool Opened { get; set; }
    public bool Hiding { get; set; }

    private DOTweenAnimation? _doTweenAnimation;

    public void SetDialog(GameObject dialogController)
    {
      DialogInstance = dialogController.GetComponent<IDialog>();
      _doTweenAnimation = dialogController.GetComponent<DOTweenAnimation>();
    }

    public async UniTask ShowAsync(string offerId)
    {
      await DialogInstance!.Configure(offerId);
      if (_doTweenAnimation != null)
      {
        await WaitAnimation(OPEN_ANIMATION_ID);
      }
      DialogInstance!.Show();
      Opened = true;
    }

    public async UniTask HideAsync()
    {
      Hiding = true;
      if (_doTweenAnimation != null)
      {
        await WaitAnimation(CLOSE_ANIMATION_ID);
      }
      DialogInstance!.Hide();
      Opened = false;
    }

    private async UniTask WaitAnimation(string animationId)
    {
        _doTweenAnimation!.DOPlayById(animationId);
        await UniTask.Delay(TimeSpan.FromSeconds(_doTweenAnimation.duration));
        await UniTask.Yield();
    }
  }
}