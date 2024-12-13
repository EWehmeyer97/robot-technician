using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputAnimationHands : MonoBehaviour
{
    [SerializeField] private Animator handActor;

    [Space]

    [SerializeField] private AnimationAccess[] animations;

    private void Update()
    {
        foreach (var animation in animations)
            handActor.SetFloat(animation.animationName, animation.action.action.ReadValue<float>());
    }

    [Serializable]
    public class AnimationAccess
    {
        public string animationName;
        public InputActionProperty action;
    }
}
