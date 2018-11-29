using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShieldAttackSimulator : MonoBehaviour {

    public bool Simulation;
    public bool Interation;
    public Transform cameraTransform;
    public float radius;
    public float shotCooldown;
    private float timer;
    private List<Vector4> projectiles;
    public ShieldController controller;
    public float projectileLife;
    public GameObject cube;
    private Vector3 lastpos;

	// Use this for initialization
	void Start () {
        projectiles = new List<Vector4>();
	}
	
	// Update is called once per frame
	void Update () {

        cameraTransform.RotateAround(transform.position, Vector3.up, Time.deltaTime * 10);
        cameraTransform.LookAt(transform.position);
        if(Simulation){
        timer += Time.deltaTime;
        while (timer > shotCooldown)
        {
           timer -= shotCooldown;
        //    var point = Random.onUnitSphere * radius;
        //    projectiles.Add(new Vector4(point.x, point.y, point.z, 0));
        //    Debug.Log("New Projectile");
           Instantiate(cube, this.transform.position + Random.Range(5, 10) * Vector3.up + Random.Range(-5, 5) * Vector3.right + Random.Range(-5, 5) * Vector3.forward, Quaternion.identity).SetActive(true);
        }
        }

        if(Interation && Input.GetMouseButton(0) && Vector3.Distance(Input.mousePosition, lastpos) > Screen.width/100)
        {
            lastpos = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)){
                OnProjectileHit(hit.point);
            }
        }

        //    for (int i = 0; i < Mathf.Min(controller.points.Length, projectiles.Count); )
        // {
        //     Vector4 projectile = controller.points[i];
        //     projectile.w += (Time.deltaTime / projectileLife);
        //     if(projectile.w < 1f){
        //         ++i;
        //     }else{
        //         pojec
        //     }
        //     controller.points[i] = projectile;
        // }

        projectiles = projectiles
            .Select(projectile => new Vector4(projectile.x, projectile.y, projectile.z, projectile.w + (Time.deltaTime / projectileLife)))
            .Where(projectile => projectile.w <= 1).ToList();

        // projectiles.ToArray().CopyTo(controller.points, 0);
        for (int i = 0; i < controller.points.Length; i++)
        {
            if(i < projectiles.Count)
            controller.points[i] = projectiles[i];
            else
            {
                controller.points[i] = Vector4.zero;
            }
        }
	}

    void OnProjectileHit(Vector3 worldSpaceImpact)
    {
        worldSpaceImpact = this.transform.worldToLocalMatrix * worldSpaceImpact;
        Debug.Log(worldSpaceImpact);
        projectiles.Add(new Vector4(worldSpaceImpact.x, worldSpaceImpact.y, worldSpaceImpact.z, .5f));
    }


    private void OnCollisionEnter(Collision other) {
        foreach (var item in other.contacts)
        {
            OnProjectileHit(item.point);
            Destroy(other.gameObject);
        }
    }


}