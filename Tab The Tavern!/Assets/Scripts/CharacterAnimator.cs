using System.Collections;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;

    Coroutine animationLoopCoroutine;

    [Header("All Animation State")]
    [SerializeField]
    string[] states =
    {
        "Balance",
        "hips",
        "Skip",
        "Slide",
        "Snap"
    };

    [Header("First Fixed Index")]
    [SerializeField] int firstIndex = 0;

    int lastIndex = -1;
    float animationSpeed = 0.9f;

    private void Start()
    {
        animator.speed = 0f;
    }

    IEnumerator AnimationLoop()
    {
        Play(firstIndex);
        lastIndex = firstIndex;

        yield return WaitForCurrentAnimation();

        while(true)
        {
            int next = GetRandomIndex();
            Play(next);
            lastIndex = next;

            yield return WaitForCurrentAnimation();
        }
    }

    void Play(int index)
    {
        animator.Play(states[index]);
    }

    int GetRandomIndex()
    {
        int index;
        do
        {
            index = Random.Range(0, states.Length);
        }
        while (index == lastIndex);

        return index;
    }

    IEnumerator WaitForCurrentAnimation()
    {
        yield return null;
        var info = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(info.length);
    }

    public void StartGame()
    {
        animator.speed = animationSpeed;
        animationLoopCoroutine = StartCoroutine(AnimationLoop());
    }

    public void EndGame()
    {
        if (animationLoopCoroutine != null)
        {
            StopCoroutine(animationLoopCoroutine);
            animationLoopCoroutine = null;
        }
        animator.Play(states[firstIndex], 0, 0f);
        animator.speed = 0f;
        animator.Update(0f);
        
    }

}
