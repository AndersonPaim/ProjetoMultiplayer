using UnityEngine;
using System.Collections;
using Mirror.Examples.Pong;


namespace Mirror.Examples.Pong
{
    public class TraumaInducer : NetworkBehaviour
    {
        [Tooltip("Seconds to wait before trigerring the explosion particles and the trauma effect")]
        public float Delay = 0;
        [Tooltip("Maximum stress the effect can inflict upon objects Range([0,1])")]
        public float MaximumStress = 3f;
        [Tooltip("Maximum distance in which objects are affected by this TraumaInducer")]
        public float Range = 45;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            //ShakeCamera();
        }

        public void SetRange(float range)
        {
            Range = range;
        }

        public void Shake()
        {
            ShakeCamera();
        }

        private void ShakeCamera()
        {
            /* Find all gameobjects in the scene and loop through them until we find all the nearvy stress receivers */
            var targets = UnityEngine.Object.FindObjectsOfType<GameObject>();
            for(int i = 0; i < targets.Length; ++i)
            {
                var receiver = targets[i].GetComponent<Player>();

                if(receiver == null) continue;
                float distance = Vector3.Distance(transform.position, targets[i].transform.position);
                /* Apply stress to the object, adjusted for the distance */
                if(distance > Range) continue;
                float distance01 = Mathf.Clamp01(distance / Range);
                float stress = (1 - Mathf.Pow(distance01, 2)) * MaximumStress;

                Camera c = _camera;

                if(c.GetComponent<StressReceiver>() != null)
                {
                    c.GetComponent<StressReceiver>().InduceStress(stress);
                }
            }
        }
    }
}