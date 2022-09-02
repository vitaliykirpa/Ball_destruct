using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour

{  static private Slingshot S;
   [Header("Set in Inscription")] 
   public GameObject prefabProjectitle;
   public float velocityMult = 8f;

   [Header("Set dynamically")]
   public GameObject       launchPoint;
   public Vector3 launchPos;
   public GameObject projectile;
   public bool aimingMode;
   private Rigidbody projectileRigidbody;
   
   static public Vector3 LAUNCH_POS{
      get{
         if(S==null)return Vector3.zero;
         return S.launchPos;
      }
   }

   void Awake() {
      S=this;
      Transform launchPointTrans =transform.Find("Louchpoint");
      launchPoint = launchPointTrans.gameObject;
      launchPoint.SetActive(false);
      launchPos = launchPointTrans.position;
   }
 void OnMouseEnter()
 {
    print("Slingshot onmouseenter");
    launchPoint.SetActive(true);
 } 

 void  OnMouseExit() {
    print("Slingshot onmouseexit");
    launchPoint.SetActive(false);
 }
 void OnMouseDown()
 {
   aimingMode = true;
   projectile=Instantiate(prefabProjectitle) as GameObject;
   projectile.transform.position=launchPos;
   projectile.GetComponent<Rigidbody>().isKinematic = true;
   projectileRigidbody=projectile.GetComponent<Rigidbody>();
   projectileRigidbody.isKinematic=true;
 }
 void Update()
 {
   if(!aimingMode) return;
   Vector3 mousePos2D = Input.mousePosition;
   mousePos2D.z = -Camera.main.transform.position.z;
   Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
   Vector3 mouseDelta = mousePos3D-launchPos;
   float maxMagnitude = this.GetComponent<SphereCollider>().radius;
   if(mouseDelta.magnitude >maxMagnitude)
   {
      mouseDelta.Normalize();
      mouseDelta *= maxMagnitude;
   }

   Vector3 projPos=launchPos+mouseDelta;
   projectile.transform.position = projPos;
   if(Input.GetMouseButtonUp(0))
   {
      aimingMode = false;
      projectileRigidbody.isKinematic = false;
      projectileRigidbody.velocity = -mouseDelta*velocityMult;
      FollowCam.POI= projectile;
      projectile = null;
      MissionDemolision.ShotFriend();
      ProjectileLine.S.poi = projectile;

   }
}
}
