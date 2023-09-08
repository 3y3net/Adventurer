using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI_3y3net
{

    public enum AvailableTargets { None, Player}

    public class StateController : MonoBehaviour
    {

        public BaseState currentState;
        public NavMeshAgent navMeshAgent;
        public Animator animator;
                
        public LayerMask sensorLayers;

        protected SphereCollider sensorTrigger = null;
        public Transform sensorPosition;
        public float sensorLimit = 20;

        public float fov = 140;
        public float viewRange = 15;
        public float smellMultiplicator = 1;
        public float noiseMultiplicator = 1;
        public float minDetectionDistance = 2f;

        public AudioSource audioStates;

        public Transform agentTransform;


        public float minDamage = 10f;
        public float maxDamage = 50f;
        public List<AudioClip> damages;
        public List<GameObject> bloodSplatts;        
        AudioSource audioSource;

        public float totalLife = 500;
        public float injuredPercent = 50;
        public float crawlPercent = 25;
        [HideInInspector]
        public int injuredState = 0;
        float startLife;
        bool isDead = false;

        public List<Rigidbody> ragDoll = new List<Rigidbody>();

        public enum EventType
        {
            Hit, Dead
        }
        public class EventInfo
        {
            public EventType messageInfo;
            public float value;
        }
        public delegate void CallbackEventHandler(EventInfo eventInfo);
        public CallbackEventHandler CallBackFunction;

        // Hashes
        private int _speedHash = Animator.StringToHash("Speed");
        private int _seekingHash = Animator.StringToHash("Seeking");
        private int _attackHash = Animator.StringToHash("Attack");
        private int _injuredState = Animator.StringToHash("InjuredState");

        //On each trigger this states needs to be updated
        [System.Serializable]
        public class TriggerDetection
        {
            public GameObject Player;            
            public GameObject Noise;            
            public GameObject Flashlight;
            public GameObject Smelled;
            public Vector3 lastViewedPlayer;
            public Vector3 lastListenedNoise;
            public Vector3 lastSmelledObject;
            public float distancePlayer;
            public float distanceNoise;
            public float distanceSmell;
            public float distanceFlashlight;
            public float distanceViewed;
            public bool hitted = false;
        }

        private float infinite = float.MaxValue;

        public TriggerDetection triggersDetected;
        public Vector3 currentTarget;
        public bool isTargetReached = false;
        public bool faceCurrentTarget = false;

        public float LevelMultiplicator = 1;
        //Sensor limits
        public float maxSmell = 3f;
        public float minSmell = 0.5f;
        public float maxListen = 3;
        public float minListen = 0.5f;
        public float maxView = 50;
        public float minView = 10;
        public float maxFOV = 220;
        public float minFOV = 90;
        public float minDetcetion = 2;
        public float maxDetcetion = 5;

        public Vector3 targetRotation;

        void Awake()
        {
            ResetSensors();
        }

        void ResetSensors()
        {
            triggersDetected.Player = null;
            triggersDetected.Noise = null;
            triggersDetected.Flashlight = null;
            triggersDetected.Smelled = null;

            triggersDetected.distancePlayer = infinite;
            triggersDetected.distanceNoise = infinite;
            triggersDetected.distanceFlashlight = infinite;
            triggersDetected.distanceSmell = infinite;
        }

        public void SetGameLevels()
        {
            //Total life
            totalLife *= LevelMultiplicator;

            //Listen capacity
            noiseMultiplicator *= LevelMultiplicator;
            noiseMultiplicator = Mathf.Clamp(noiseMultiplicator, minListen, maxListen);

            //Smell capacity
            smellMultiplicator *= LevelMultiplicator;
            smellMultiplicator = Mathf.Clamp(smellMultiplicator, minSmell, maxSmell);

            //View capacity
            viewRange *= LevelMultiplicator;
            viewRange = Mathf.Clamp(viewRange, minView, maxView);

            //Global sensor limits
            sensorLimit *= LevelMultiplicator;
            sensorLimit = Mathf.Clamp(sensorLimit, 10, 50);

            //Field of view angle
            fov *= LevelMultiplicator;
            fov = Mathf.Clamp(fov, minFOV, maxFOV);

            //Minimun detection distance
            minDetectionDistance *= LevelMultiplicator;
            minDetectionDistance = Mathf.Clamp(minDetectionDistance, minDetcetion, maxDetcetion);
        }

        private void Start()
        {            

            if (GameLogic.instance)
                LevelMultiplicator = GameLogic.instance.LevelMultiplicator;
            SetGameLevels();

            startLife = totalLife;

            if (navMeshAgent == null)            
                navMeshAgent = gameObject.GetComponentInParent<NavMeshAgent>();
                
            if (animator == null)
                animator = gameObject.GetComponentInParent<Animator>();
            if (sensorTrigger == null)
            {
                sensorTrigger = gameObject.AddComponent<SphereCollider>();
                sensorTrigger.radius = sensorLimit;
                sensorTrigger.isTrigger = true;
            }

            currentState.OnEnterState(this);
            agentTransform = navMeshAgent.gameObject.transform;

            audioSource = GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.loop = false;
                audioSource.playOnAwake = false;
            }
        }

        private void Update()
        {
            if (isDead)
                return;

            SetAnimatorParameters();

            currentState.UpdateState(this);

            bool shouldExit = currentState.ExitOnFirstTransition;
            foreach (BaseTransition bt in currentState.transitions)
            {
                if (bt.TransitionCondition(this))
                {
                    currentState.OnExitState(this);
                    bt.OnTransition(this);
                    currentState = bt.transitionState;
                    currentState.OnEnterState(this);
                    if (shouldExit)
                        break;
                }
            }            
        }

        public void SetTarget(Vector3 target)
        {
            navMeshAgent.destination = target;
            navMeshAgent.isStopped = false;
        }

        void SetAnimatorParameters()
        {
            
            if (animator != null)
            {
                injuredState = 0;
                float lifepercent = 100.0f * totalLife / startLife;

                if (lifepercent <= injuredPercent)
                    injuredState = 1;
                if (lifepercent <= crawlPercent)
                    injuredState = 2;

                animator.SetInteger(_injuredState, injuredState);
                
                if (lifepercent<=injuredPercent && currentState.speed>0.5f)
                        currentState.speed = 0.5f;

                animator.SetFloat(_speedHash, currentState.speed);//, 0.1f, Time.deltaTime);
                animator.SetInteger(_seekingHash, currentState.seeking);
                animator.SetInteger(_attackHash, currentState.attackType);
            }

            if (navMeshAgent.desiredVelocity.sqrMagnitude > Mathf.Epsilon && currentState.speed > 0.1f)
            {
                //Debug.Log("NAVMESH ROTATION");
                Quaternion lookRotation = Quaternion.LookRotation(navMeshAgent.desiredVelocity, Vector3.up);
                agentTransform.rotation = Quaternion.Slerp(agentTransform.rotation, lookRotation, 5.0f * Time.deltaTime);
            }
        }

        void OnAnimatorMove()
        {
            if (isDead)
                return;
            if(Time.deltaTime>0)            
                navMeshAgent.velocity = animator.deltaPosition / Time.deltaTime;
            else
                navMeshAgent.velocity = Vector3.zero;
            // Grab the root rotation from the animator and assign as our transform's rotation.
            if (currentState.seeking != 0)
            {
                //Debug.Log("SEEKING ROTATION");
                agentTransform.rotation = animator.rootRotation;
            }            
        }

        void OnTriggerEvent(Collider other, TriggerType type)
        {
            if (isDead)
                return;
            //float distance = Vector3.Distance(transform.transform.position, other.transform.position);
            if (type != TriggerType.Exit)
            {
                RaycastHit hitInfo;
                //Get visual angle (0,360) of collider. -1 if no visible
                float angle = ColliderIsVisible(other, out hitInfo, sensorLayers);
                float pathDistance = CalculatePathLength(other.transform.position);
                //Visual sensor testing
                //Debug.Log("Distance to " + other.gameObject.name + " = " + pathDistance);
                if (other.CompareTag("DeadPlayer"))
                {
                    triggersDetected.Player = null;
                    //Debug.Log("PLAYER DEAD");
                }
                else if (other.CompareTag("Player")) {                    
                    //VIEW PLAYER?
                    if ((angle <= fov * 0.5f || hitInfo.distance<minDetectionDistance) && angle > 0f)
                    {
                        //Debug.Log("VIEW " + hitInfo.distance + " fov:"+angle);
                        //If visible and angle in FoV
                        triggersDetected.Player = hitInfo.collider.gameObject;
                        triggersDetected.distancePlayer = hitInfo.distance;
                        triggersDetected.lastViewedPlayer = triggersDetected.Player.transform.position;                        
                    }
                    else {
                        triggersDetected.Player = null;
                    }
                }
                //VIEW FLASHLIGHT?
                else if (other.CompareTag("Flashlight"))
                {
                    //Debug.Log("FLASHLIGHT " + angle);
                    if (other.bounds.Contains(sensorPosition.position) && angle>0) {
                        //If visible, no matter angle and head is inside light cone
                        triggersDetected.Flashlight = other.gameObject;
                        triggersDetected.distanceFlashlight = hitInfo.distance;
                        //Debug.Log("View Flashlight");
                    }
                    else
                        triggersDetected.Flashlight = null;
                }
                else if(other.CompareTag("SmellObject"))
                {
                    
                    //SMELL PLAYER? 
                    if (other.GetComponent<SmellObjectProperties>().IsSmeelActive() && pathDistance < smellMultiplicator * other.GetComponent<SmellObjectProperties>().smellRange)
                    {
                        //Debug.Log("SMELL " + pathDistance);
                        triggersDetected.Smelled = other.gameObject; //MAY BE NOT VISIBLE... 
                        triggersDetected.lastSmelledObject = triggersDetected.Smelled.transform.position;
                        triggersDetected.distanceSmell = pathDistance;
                    }
                    else
                    {
                        triggersDetected.Smelled = null;
                    }
                }
                else if (other.CompareTag("NoiseObject"))
                {
                    //SMELL PLAYER? 
                    if (other.GetComponent<NoiseObjectProperties>().IsNoiseActive() && pathDistance < noiseMultiplicator * other.GetComponent<NoiseObjectProperties>().noiseRange)
                    {
                        triggersDetected.Noise = other.gameObject; //MAY BE NOT VISIBLE... 
                        triggersDetected.lastListenedNoise = triggersDetected.Noise.transform.position;
                        triggersDetected.distanceNoise = pathDistance;
                    }
                    else
                    {
                        triggersDetected.Noise = null;
                    }
                }
                /*
                if (sensorLayers == (sensorLayers | (1 << other.gameObject.layer)))
                    foreach (BaseTransition bt in currentState.transitions)
                        bt.OnTransitionTrigger(this, other, type, sensorLayers);
                 */
            }
            else //Trigger exit
            {
                if (other.CompareTag("Player"))
                {                    
                    triggersDetected.Player = null;
                    triggersDetected.Smelled = null;
                }
                else if (other.CompareTag("Flashlight"))// && (other.bounds.Contains(sensorPosition.position)))
                    triggersDetected.Flashlight = null;
            }
        }

        void OnTriggerEnter(Collider col)
        {
            OnTriggerEvent(col, TriggerType.Enter);
        }

        void OnTriggerStay(Collider col)
        {
            OnTriggerEvent(col, TriggerType.Stay);
        }

        void OnTriggerExit(Collider col)
        {
            OnTriggerEvent(col, TriggerType.Exit);
        }

        protected virtual float ColliderIsVisible(Collider other, out RaycastHit hitInfo, LayerMask layerMask)
        {
            // Let's make sure we have something to return
            hitInfo = new RaycastHit();

            // Calculate the angle between the sensor origin and the direction of the collider
            Vector3 head =  sensorPosition.position;
            Vector3 direction = other.transform.position - head;

            Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
            float angle = Vector3.Angle(directionXZ, transform.forward);

            //Debug.DrawRay(sensorPosition.position, direction.normalized * viewRange, Color.green);
            Debug.DrawRay(sensorPosition.position, directionXZ.normalized * viewRange, Color.yellow);

            // Now we need to test line of sight. Perform a ray cast from our sensor origin in the direction of the collider for distance
            // of our sensor radius scaled by the zombie's sight ability. This will return ALL hits.
            RaycastHit[] hits = Physics.RaycastAll(head, direction.normalized, viewRange, layerMask);

            // Find the closest collider that is NOT the AIs own body part. If its not the target then the target is obstructed
            float closestColliderDistance = float.MaxValue;
            Collider closestCollider = null;

            // Examine each hit
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                // Is this hit closer than any we previously have found and stored
                if (hit.distance < closestColliderDistance)
                {
                    closestColliderDistance = hit.distance;
                    closestCollider = hit.collider;
                    hitInfo = hit;
                }
            }


            // If the closest hit is the collider we are testing against, it means we have line-of-sight
            // so return true.
            if (closestCollider && closestCollider.gameObject == other.gameObject)
            {
                Debug.DrawRay(sensorPosition.position, direction.normalized * viewRange, Color.red);
                return angle;
            }
            // otherwise, something else is closer to us than the collider so line-of-sight is blocked
            return -1f;
        }

        public void HitPlayer(int value)
        {
            if (isDead)
                return;
            if (triggersDetected.Player != null)
            {
                ImpactInfo impactInfo = new ImpactInfo();
                impactInfo.damage = Random.Range(minDamage, maxDamage);
                impactInfo.hitPoint = Vector3.zero;
                impactInfo.hitNormal = Vector3.zero;

                //triggersDetected.Player.GetComponent<playercontroller>().Damage(impactInfo);                                                
            }
        }

        void Damage(ImpactInfo impactInfo)
        {
            var rot = Quaternion.FromToRotation(Vector3.up, impactInfo.hitNormal);
            Instantiate(bloodSplatts[Random.Range(0, bloodSplatts.Count)], impactInfo.hitPoint, rot, transform);

            if (isDead)
                return;
            totalLife -= impactInfo.damage / LevelMultiplicator;
            if(totalLife<=0)
                MakeDead();            

            if (audioSource)
            {
                if (!audioSource.isPlaying)
                {
                    //Debug.Log(value + " " + gameObject.name);
                    audioSource.clip = damages[Random.Range(0, damages.Count)];
                    audioSource.Play();
                }
            }

            triggersDetected.lastListenedNoise = GameLogic.instance.player.transform.position;
            triggersDetected.hitted = true;

            if (CallBackFunction != null)
            {
                EventInfo myEvent = new EventInfo();
                myEvent.messageInfo = EventType.Hit;
                myEvent.value = impactInfo.damage;
                CallBackFunction(myEvent);
            }

            //Debug.Log("Life: " + totalLife);
        }

        void Explosion(float damage)
        {
            if (isDead)
                return;
            totalLife -= damage / LevelMultiplicator;
            if (totalLife <= 0)
                MakeDead();
            //Debug.Log("Expl. Life: " + totalLife);
        }

        void MakeDead()
        {
            isDead = true;
            navMeshAgent.enabled = false;
            animator.enabled = false;
            foreach (Rigidbody rb in ragDoll)
                rb.isKinematic = false;

            if (CallBackFunction != null)
            {
                EventInfo myEvent = new EventInfo();
                myEvent.messageInfo = EventType.Dead;
                myEvent.value = 0;
                CallBackFunction(myEvent);
            }
        }

        float CalculatePathLength(Vector3 targetPosition)
        {
            // Create a path and set it based on a target position.
            NavMeshPath path = new NavMeshPath();
            if (navMeshAgent.enabled)
                navMeshAgent.CalculatePath(targetPosition, path);

            // Create an array of points which is the length of the number of corners in the path + 2.
            Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

            // The first point is the enemy's position.
            allWayPoints[0] = transform.position;

            // The last point is the target position.
            allWayPoints[allWayPoints.Length - 1] = targetPosition;

            // The points inbetween are the corners of the path.
            for (int i = 0; i < path.corners.Length; i++)
            {
                allWayPoints[i + 1] = path.corners[i];
            }

            // Create a float to store the path length that is by default 0.
            float pathLength = 0;

            // Increment the path length by an amount equal to the distance between each waypoint and the next.
            for (int i = 0; i < allWayPoints.Length - 1; i++)
            {
                pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
            }

            return pathLength;
        }
    }
}