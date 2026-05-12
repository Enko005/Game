using Unity.VisualScripting;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance {get; private set;}

    [SerializeField] private Sword sword;
    private void Awake() { Instance = this; }

    private void Update()
    {
        if (Player.Instance.IsAlive())
            FollowMousePosition();
    }
    public Sword GetActiveWeapon()
    {
        return sword;
    }

    private void FollowMousePosition()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetMousePosition();

        transform.rotation = Quaternion.Euler(0, mousePos.x < playerPosition.x ? 180 : 0, 0);

        if (mousePos.x < playerPosition.x)
        {
            transform.localPosition = new Vector3(-0.6f, 0.6f, 0);
        }
        else
        {
            transform.localPosition = new Vector3(0.6f, 0.6f, 0);
        }
    }
}
