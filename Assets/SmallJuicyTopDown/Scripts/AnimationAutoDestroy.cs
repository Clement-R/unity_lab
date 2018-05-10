using UnityEngine;
using System.Collections;
using ObjectPooling;

public class AnimationAutoDestroy : Stuff {

    void Start() {
        Invoke("ReturnToPool", this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}