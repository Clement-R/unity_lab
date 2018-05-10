using UnityEngine;

namespace ObjectPooling {

    public class PooledObject : MonoBehaviour {

        public ObjectPool Pool { get; set; }

        public void ReturnToPool() {
            if (Pool) {
                Pool.AddObject(this);
            } else {
                Debug.Log(Pool);
                Destroy(gameObject);
            }
        }
    }
}