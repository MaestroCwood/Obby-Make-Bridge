using UnityEngine;

public class RotatorObj : MonoBehaviour
{
    [SerializeField] float speedRotate;



    private void Update()
    {
        transform.Rotate(Vector3.up * speedRotate * Time.deltaTime);
    }
}
