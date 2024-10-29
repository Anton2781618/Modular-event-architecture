using RopeToolkit.Example;
using UnityEngine;

public class RaycastService : MonoBehaviour
{
    [SerializeField] private RopeSpawner ropeSpawner;
    public Renderer debugTransform;
    RaycastHit hit = new RaycastHit();
    public LayerMask Layers = default;


    public void Update()
    {
        Raycaster();
        if(Input.GetKeyDown(KeyCode.R))
        {
            ropeSpawner.Detach();
        }
    }

    public Collider GetHitCollider() => hit.collider != null && CheckLayers() ? hit.collider : null;

    private bool CheckLayers()
    {
        if((Layers.value & (1 << hit.collider.gameObject.layer)) == 0) return false;

        return true;

    }

    private void Raycaster()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 50f, Layers))
        {
            if(!debugTransform.gameObject.activeSelf) debugTransform.gameObject.SetActive(true);

            debugTransform.transform.position = hit.point;
            
            if (Vector3.Distance(transform.position, hit.transform.position ) < 3)
            {
                debugTransform.material.color = Color.green;

                if(Input.GetKeyUp(KeyCode.E))
                {
                    if(!ropeSpawner.FerstTarget)
                    {
                        ropeSpawner.FerstTarget = hit.collider.GetComponent<Rigidbody>();
                    }
                    else
                    {
                        ropeSpawner.LastTarget = hit.collider.GetComponent<Rigidbody>();

                        ropeSpawner.Attach();
                    }
                }

            }
            else
            {
                debugTransform.material.color = Color.red;
            }
        }
        else
        {
            if(debugTransform.gameObject.activeSelf) debugTransform.gameObject.SetActive(false);
        }
    }
}
