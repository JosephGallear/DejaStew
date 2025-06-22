using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player player;
    private Animator animator;

    [SerializeField] private Vector3 startRotation = Vector3.zero;

    private void Awake()
    {
        transform.localRotation = Quaternion.Euler(startRotation);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
