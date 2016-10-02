using UnityEngine;
using System.Collections;

public class Symbol : MonoBehaviour
{
    public event System.Action<Symbol> FullHitSymbolEvent;
    public event System.Action<Symbol, float> HittingSymbolEvent;

    public TextMesh TextMesh;
    
    public int Index { get; private set; }

    float ActHitTime;
    bool isHitting;
    const float FullHitTime = 1f;

    private ParticleSystem Particles;

    void Awake()
    {
        Particles = GetComponentInChildren<ParticleSystem>();
        Particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHitting)
        {
            ActHitTime += Time.deltaTime;
            var t = Mathf.InverseLerp(0, FullHitTime, ActHitTime);

            if (HittingSymbolEvent != null)
                HittingSymbolEvent(this, t);

            if (ActHitTime > FullHitTime)
            {
                if (FullHitSymbolEvent != null)
                    FullHitSymbolEvent(this);

            }
        }
        else
        {
            if(!Particles.isStopped)
                Particles.Stop();
        }

    }

    public void PlayParticles()
    {
        if (!Particles.isPlaying)
        {
            Particles.Play();
        }
    }

    public void SetIndex(int index)
    {
        Index = index;
        TextMesh.text = Index.ToString();
    }

    public void SetAlpha(float alpha)
    {
        TextMesh.color = new Color(TextMesh.color.r, TextMesh.color.r, TextMesh.color.r, alpha);
    }

    void OnTriggerEnter(Collider collider)
    {
        ActHitTime = 0;
        isHitting = true;
    }

    void OnTriggerExit(Collider collider)
    {
        isHitting = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        Debug.Log(Index);
    }

    /*
    public static Color GetColorForIndex(int index)
    {
        var s = System.Convert.ToString(index % 8, 2).PadLeft(3, '0');
        Debug.Log(s);
        return new Color((float)char.GetNumericValue(s[0]), (float)char.GetNumericValue(s[1]), (float)char.GetNumericValue(s[2]));
    }
    */

}
