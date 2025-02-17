using Unity.VisualScripting;
using UnityEngine;

public class Frog : ItemBehavior
{
    public int frogJumpUnit = 2;
    public GameObject frogHatPrefab;

    void Start()
    {
        isConsumable = true;
    }
    new void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        Debug.Log("child triggered!");
        FrogActivate(other);
        base.OnTriggerEnter(other);
    }

    private void FrogActivate(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerJump player = other.GetComponent<PlayerJump>();
            player.jumpUnit = frogJumpUnit;
            Debug.Log("Frog Item applied: Jump unit set to " + player.jumpUnit);

            // ������ ���� ����
            if (frogHatPrefab != null)
            {
                GameObject frogHat = Instantiate(frogHatPrefab, player.transform);
                frogHat.transform.localPosition = new Vector3(0, 1.5f, 0); // �Ӹ� ��ġ�� ���� ��ġ
                Debug.Log("Frog hat instantiated for player");
            }

            Debug.Log("Frog Item applied: Jump unit increased!");
        }
    }
}

