using UnityEngine;

public class JellyFish : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = Random.Range(0.90f, 1.1f);
        animator.Play("jellyfish_move_animation", 0, Random.Range(0.05f, 6.25f));
    }
}
