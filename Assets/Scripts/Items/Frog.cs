using Unity.VisualScripting;
using UnityEngine;

public class Frog : Consumable
{
    public int frogJumpUnit = 2;
    public GameObject frogHatPrefab;

    protected override void Awake()
    {
        base.Awake();
        PlayerTriggerEvent += FrogActivate;
    }

    private void FrogActivate(Collider other)
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

