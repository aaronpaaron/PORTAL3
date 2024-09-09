using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCam : MonoBehaviour
{
    [SerializeField] public Transform portal;
    [SerializeField] public Transform otherPortal;

    public Transform playerCamera;
    public Camera portalCamera;
    public MeshRenderer portalRenderer; // Tämä on portaalin mesh, jossa on RenderTexture

    void Start()
    {
        // Oletetaan, että kameran RenderTexture on asetettu jo oikein editorissa.
        if (portalCamera.targetTexture == null)
        {
            Debug.LogWarning("RenderTexture on asetettava PortalCameraan.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Lasketaan portaaliin liittyvä rotaatio
        float angle = Quaternion.Angle(portal.rotation, otherPortal.rotation);
        Quaternion angleToQuaternion = Quaternion.AngleAxis(angle, Vector3.up);

        // Lasketaan suunta, johon portaalikamera tulisi katsoa suhteessa pelaajakameraan
        Vector3 dir = angleToQuaternion * -playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, -dir.y, dir.z), Vector3.up);

        // Siirretään portaalikamera oikeaan paikkaan
        Vector3 offset = playerCamera.position - otherPortal.position;
        transform.position = portal.position + offset;

        // Laske kameran näkymä portaalin "ikkunana"
        CalculateViewThroughPortal();
    }

    void CalculateViewThroughPortal()
    {
        // Tarkistetaan portaalin suunta pelaajan kameraan nähden
        Plane portalPlane = new Plane(-portal.forward, portal.position);

        // Jos pelaaja katsoo portaalia päin
        if (portalPlane.GetSide(playerCamera.position))
        {
            // Laske offset ja suhteuta näkymä portaaliin
            Vector3 localPos = portal.InverseTransformPoint(playerCamera.position);
            Quaternion localRot = Quaternion.Inverse(portal.rotation) * playerCamera.rotation;

            // Päivitä portaalikamera renderöimään oikeasta paikasta
            Vector3 newPos = otherPortal.TransformPoint(localPos);
            Quaternion newRot = otherPortal.rotation * localRot;

            portalCamera.transform.SetPositionAndRotation(newPos, newRot);
        }
    }
}
