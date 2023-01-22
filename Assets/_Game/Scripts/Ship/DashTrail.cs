using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrail : MonoBehaviour
{
    Queue<GameObject> pool = new Queue<GameObject>();
    //Dictionary<int, GameObject> active = new Dictionary<int, GameObject>();
    [SerializeField] Sprite trailSprite;
    [SerializeField] Material trailMat;
    [SerializeField] Color backGround;
    private GameObject Pop()
    {
        if (pool.Count == 0) 
        {
            pool.Enqueue(CreateGo());
        }
        // active.Add(go.GetInstanceID(),go);
        return pool.Dequeue();
    }
    private GameObject CreateGo() 
    {
        GameObject go = Instantiate(new GameObject(),new Vector3(-1000,-1000,0),Quaternion.identity);
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        Material mat = new Material(trailMat);
        sr.sortingOrder = -1;
        sr.sprite = trailSprite;
        sr.sharedMaterial = sr.material = mat;
        go.name = "TrailImage";
        return go;
    }
    public void Play(Transform toFollow, float duration, int trailAmount)
    {
        StartCoroutine(GenerateTrail(toFollow,duration,trailAmount));
    }
    private IEnumerator GenerateTrail(Transform toFollow,float duration,int trailAmount)
    {
        List<GameObject> activeImages = new List<GameObject>();
        List<SpriteRenderer> activeRenderers = new List<SpriteRenderer>();
        List<float> lifeTimes = new List<float>();
        float timer = 0;
        float instantiateTimer = 0;
        bool colBool = true;
        float totalLifeTime = 0.1f;
        while(timer < duration) 
        {
            if(instantiateTimer == 0) 
            {
                GameObject go = Pop();
                go.transform.position = toFollow.position;
                go.transform.rotation = toFollow.rotation;
                activeImages.Add(go);
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.material.SetColor("_Color",Color.white);
                colBool = !colBool;
                activeRenderers.Add(sr);
                lifeTimes.Add(0);
            }
            for(int i = 0; i < activeImages.Count; i++)
            {
                lifeTimes[i] += Time.deltaTime;
                activeRenderers[i].material.SetFloat("_LifeTime",Mathf.Clamp01(lifeTimes[i]/totalLifeTime));
                activeRenderers[i].material.SetColor("_Color",Color.Lerp(Color.white,backGround, Mathf.Clamp01(lifeTimes[i]/ totalLifeTime)));
            }
            timer += Time.deltaTime;
            instantiateTimer = (instantiateTimer > duration/trailAmount)? 0: instantiateTimer + Time.deltaTime;  
            yield return new WaitForEndOfFrame();
        }
        for(int i = 0; i < activeImages.Count; i++) 
        {
            activeImages[i].transform.position = new Vector2(-1000,-1000);
            activeRenderers[i].color = Color.white;
            pool.Enqueue(activeImages[i]);
        }
    }
}
