using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Frog, Swap } // 아이템 종류
    public ItemType itemType;          // 현재 아이템의 타입
    public GameObject frogHatPrefab;   // 개구리 모자 프리팹

    private bool isConsumed = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} collided with {other.name}");
        // 플레이어가 아이템에 닿았는지 확인
        if (isConsumed || !other.CompareTag("Player"))
            return;

        PlayerJump player = other.GetComponent<PlayerJump>();
        if (player != null)
        {
            ApplyEffect(player); // 아이템 효과 적용
            Destroy(gameObject); // 아이템 삭제
            isConsumed = true;
        }
    }

    private void ApplyEffect(PlayerJump player)
    {
        // 아이템 타입별로 효과 처리
        switch (itemType)
        {
            case ItemType.Frog:
                ApplyFrogEffect(player);
                break;

            case ItemType.Swap:
                ApplySwapEffect();
                break;
        }
    }

    private void ApplyFrogEffect(PlayerJump player)
    {
        // 점프 유닛 증가
        player.jumpUnit = 2;
        Debug.Log("Frog Item applied: Jump unit set to " + player.jumpUnit);

        // 개구리 모자 착용
        if (frogHatPrefab != null)
        {
            GameObject frogHat = Instantiate(frogHatPrefab, player.transform);
            frogHat.transform.localPosition = new Vector3(0, 1.5f, 0); // 머리 위치에 모자 배치
            Debug.Log("Frog hat instantiated for player");
        }

        Debug.Log("Frog Item applied: Jump unit increased!");
    }

    private void ApplySwapEffect()
    {
        // 플레이어 간 위치 교환
        GameObject player1 = GameManager.instance.player1;
        GameObject player2 = GameManager.instance.player2;

        if (player1 != null && player2 != null)
        {
            Vector3 tempPosition = player1.transform.position;
            player1.transform.position = player2.transform.position;
            player2.transform.position = tempPosition;

            Debug.Log("Swap Item applied: Players swapped positions!");
        }
    }
}
