﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxAutodestroy : MonoBehaviour {
    AudioSource sfx;

    void Start() {
        sfx = GetComponent<AudioSource>();
    }

    void Update() {
        if (sfx.clip != null) {
            if (!sfx.isPlaying && sfx.clip.loadState == AudioDataLoadState.Loaded) {
                Destroy(this.gameObject);
            }
        }
    }
}