using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotAnimation : MonoBehaviour
{
    [SerializeField]
    private RectTransform cursorTransform;
    [SerializeField]
    private GameObject imagePrefab;
    [SerializeField]
    private float animationDuration;
    [SerializeField]
    private AnimationCurve animationAlphaCurve;

    private Animator animator;
    public static ShotAnimation instance;

    private List<float> currentAnimations;

    private void Awake()
    {
        instance = this;
        currentAnimations = new List<float>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        //var alpha = 0f;
        //for (int i = currentAnimations.Count - 1; i >= 0 ; i--)
        //{
        //    currentAnimations[i] += Time.deltaTime;
        //    if (currentAnimations[i] >= animationDuration)
        //        currentAnimations.RemoveAt(i);
        //    else
        //        alpha += animationAlphaCurve.Evaluate(currentAnimations[i] / animationDuration);
        //}
        //var color = image.color;
        //color.a = alpha;
        //image.color = color;
    }

    public void shoot()
    {
        var newSpark = Instantiate(imagePrefab, transform);
        ((RectTransform)newSpark.transform).anchoredPosition = cursorTransform.anchoredPosition;
        //animator.SetTrigger("Kill");
        //currentAnimations.Add(0f);
    }
}
