using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D Hit = Physics2D.Raycast(MousePos, Vector2.zero);

            if (Hit.collider != null)
            {
                Hit.collider.gameObject.GetComponent<Card>()?.OnCardClicked();
            }
        }
    }
}
