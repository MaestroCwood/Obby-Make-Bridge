using System.Collections;
using UnityEngine;

public class GenerateCoin : MonoBehaviour
{
    [SerializeField] float delayGenerate = 5f;
    [SerializeField] int countGenerate = 5;
    [SerializeField] GameObject icoFx;
    [SerializeField] GameObject destroyFx;
 
    [SerializeField] bool isActiveGenerate = false;





    private void Start()
    {
        StartCoroutine(Generator());
    }
    IEnumerator Generator()
    {
        while(enabled)
        {
            if (isActiveGenerate)
            {
                GameObject go = Instantiate(icoFx, transform.position, Quaternion.identity);

                Animate(go);
                GameEvents.OnGenerateCoin?.Invoke(countGenerate);
                yield return new WaitForSeconds(delayGenerate);
            }

            yield return null;
        }
    }



    void Animate(GameObject go)
    {   

        Vector3 target = new Vector3(go.transform.position.x, go.transform.position.y +20, go.transform.position.z);
        float rand = Random.Range(2f, 3f);
        go.transform.LeanMove(target, rand).setOnComplete(() =>
        {
            Instantiate(destroyFx, go.transform.position, Quaternion.identity);
            Destroy(go, 0.1f);
        });
        go.transform.LeanRotateY(920f, rand).setEase(LeanTweenType.linear);
    }
}
