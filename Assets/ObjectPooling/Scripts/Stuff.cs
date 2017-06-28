using System;
using UnityEngine;

namespace ObjectPooling {
    public class Stuff : PooledObject {
        [System.NonSerialized]
        ObjectPool poolInstanceForPrefab;

        void Awake() {
        }

        /*
        void OnTriggerEnter(Collider enteredCollider) {
            if (enteredCollider.CompareTag("Kill Zone")) {
                ReturnToPool();
            }
        }
        */

        public T GetPooledInstance<T>() where T : PooledObject {
            if (!poolInstanceForPrefab) {
                poolInstanceForPrefab = ObjectPool.GetPool(this);
            }
            return (T)poolInstanceForPrefab.GetObject();
        }
    }
}

