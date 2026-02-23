using UnityEngine;

public class CoinGenerate : MonoBehaviour
{
    [SerializeField] GameObject generateObj;
    [SerializeField] Vector3 directionGenerate = Vector3.right;
    [SerializeField] int countGenerate = 5;                   
    [SerializeField] float spacing = 1.5f;                      

    private void Start()
    {
        Generator();
    }

    public void Generator()
    {
        Vector3 dir = directionGenerate.normalized; 

        for (int i = 0; i < countGenerate; i++)
        {
            Vector3 pos = transform.position + dir * spacing * i;
            Instantiate(generateObj, pos, Quaternion.identity, transform);
        }
    }
}