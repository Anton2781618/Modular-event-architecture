using DynamicMeshCutter;
using UnityEngine;


namespace ModularEventArchitecture
{
    public class Sword : Weapon
    {
        // [SerializeField] private bool сanСut = false; 
        [SerializeField] private int damage = 60; 
        [SerializeField] private LayerMask triggerMask;
        // [SerializeField] private PlaneBehaviour planeBehaviour;
        public GameObject BloodAttach;
        public GameObject[] BloodFX;
        public Vector3 direction;
        //оглушеающий удар
        public bool Stun = false;
        public bool hit = false;

        private void OnTriggerStay(Collider other)
        {
            if (!hit || (triggerMask.value & (1 << other.gameObject.layer)) == 0) return;
            
            if(other.transform.gameObject != transform.gameObject)
            {
                NPC npc = other.transform.root.GetComponent<NPC>();

                if(npc.GetModule<IStatus>() == null) 
                {
                    Debug.LogError("У объекта нет модуля здоровья");
                    
                    return;
                }

                if(npc.GetModule<IStatus>().IsDead() == false)
                {   
                    Vector3 point =  other.ClosestPoint(transform.position);

                    Instantiate(BloodFX[Random.Range(0, BloodFX.Length)], point, Quaternion.LookRotation(point - transform.position));

                    // var nearestBone = GetNearestObject(other.transform.root, point);

                    
                    // if(nearestBone != null)
                    // {
                    //     var attachBloodInstance = Instantiate(BloodAttach);
                    //     var bloodT = attachBloodInstance.transform;
                    //     bloodT.position = other.ClosestPoint(transform.position);
                    //     bloodT.localRotation = Quaternion.identity;
                    //     bloodT.localScale = Vector3.one * Random.Range(0.75f, 1.2f);
                    //     bloodT.LookAt(point, direction);
                    //     bloodT.Rotate(90, 0, 0);
                    //     bloodT.transform.parent = nearestBone;
                    //     //Destroy(attachBloodInstance, 20);
                    // }

                    //получить направление удара
                    direction = (point - transform.position).normalized;
                    if (Stun)
                    {
                        npc.LocalEvents.Publish(LocalEventBus.События.Оглушен, new StunEvent {isStun = true, Time = 10});
                        npc.LocalEvents.Publish(LocalEventBus.События.Получить_урон, new DamageEvent {Damage = 10, HitDirection = direction});
                    }
                    else
                    {
                        npc.LocalEvents.Publish(LocalEventBus.События.Получить_урон, new DamageEvent {Damage = damage, HitDirection = direction});
                    }

                    // if (сanСut) planeBehaviour.Cut();
                }
                
                SetHitBoolOFF();
            }
        }

        public override void SetHitBoolOFF() => hit = false;

        Transform GetNearestObject(Transform hit, Vector3 hitPos)
        {
            var closestPos = 100f;
            Transform closestBone = null;
            var childs = hit.GetComponentsInChildren<Transform>();

            foreach (var child in childs)
            {
                var dist = Vector3.Distance(child.position, hitPos);
                if (dist < closestPos)
                {
                    closestPos = dist;
                    closestBone = child;
                }
            }

            var distRoot = Vector3.Distance(hit.position, hitPos);
            if (distRoot < closestPos)
            {
                closestPos = distRoot;
                closestBone = hit;
            }
            return closestBone;
        }

        public override void Attack(string animationName, bool isStun = false)
        {

            if(hit == true) return;

            _animator.SetTrigger(animationName);

            Stun = isStun;

            hit = true;
        }
    }
}