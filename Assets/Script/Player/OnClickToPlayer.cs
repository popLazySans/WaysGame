using UnityEngine;
using UnityEngine.InputSystem;

public class OnClickToPlayer : MonoBehaviour
{
    public GameObject Character_Camera;
    private bool isZoomToCharacter = false;
    private PlayerMove playerMove;
    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
    }
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == GetComponent<Collider>())
                {
                    ZoomCharacter();
                }
            }
        }
    }

    private void ZoomCharacter()
    {
        isZoomToCharacter = !isZoomToCharacter;
        Character_Camera.SetActive(isZoomToCharacter);
    }
}