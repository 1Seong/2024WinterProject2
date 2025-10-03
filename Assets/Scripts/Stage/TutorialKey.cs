using UnityEngine;

public class TutorialKey : MonoBehaviour
{
    private enum moveKey { shift, left, right, up, down };

    [SerializeField] private Sprite keyDownSprite;
    [SerializeField] private moveKey key;

    private Sprite defaultSprite;
    private SpriteRenderer spriter;

    private bool _isPressed;
    private bool isPressed
    {
        get => _isPressed;
        set
        {
            _isPressed = value;
            if (value)
            {
                GetComponent<SpriteRenderer>().sprite = keyDownSprite;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = defaultSprite;
            }
        }
    }

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        defaultSprite = spriter.sprite;
    }

    private void Update()
    {
        switch(key)
        {
            case moveKey.shift:
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                    isPressed = true;
                else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                    isPressed = false;
                break;

            case moveKey.left:
                if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                    isPressed = true;
                else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
                    isPressed = false;
                break;

            case moveKey.right:
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    isPressed = true;
                else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
                    isPressed = false;
                break;

            case moveKey.up:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                    isPressed = true;
                else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                    isPressed = false;
                break;

            case moveKey.down:
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                    isPressed = true;
                else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                    isPressed = false;
                break;
        }
    }
}
