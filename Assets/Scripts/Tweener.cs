using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    // private Tween activeTween;
    private List<Tween> activeTweens;

    // Start is called before the first frame update
    void Start()
    {
        activeTweens = new List<Tween>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = activeTweens.Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(activeTweens[i].Target.localPosition, activeTweens[i].EndPos) > 0.0000001f)
            {
                activeTweens[i].Target.localPosition = Vector3.Lerp(activeTweens[i].StartPos, activeTweens[i].EndPos, (Time.time - activeTweens[i].StartTime) / activeTweens[i].Duration);
            }
            else
            {
                activeTweens[i].Target.localPosition = activeTweens[i].EndPos;
                activeTweens.RemoveAt(i);
            }
        }
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (!TweenExists(targetObject))
        {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TweenExists(Transform target)
    {
        return activeTweens.Exists(x => x.Target == target);
    }

    public void RemoveTween(Transform targetObject)
    {
        for (int i = activeTweens.Count - 1; i >= 0; i--)
        {
            if (activeTweens[i].Target == targetObject)
            {
                activeTweens.RemoveAt(i);
            }
        }
    }
}
