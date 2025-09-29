using UnityEngine;

public class JumpPadTrigger : MonoBehaviour
{
    private enum Dir { Up, down};
    [SerializeField] private Dir dir;
    [SerializeField] private Animator targetAnim;

    [SerializeField] private GameObject effect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            effect.SetActive(true);
            if (dir == Dir.Up)
            {
                targetAnim.SetTrigger("Up");
            }
            else
                targetAnim.SetTrigger("Down");
        }
    }
}
