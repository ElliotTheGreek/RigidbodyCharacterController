using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour {
	/* store each animation in a generated HashID so that other scripts can easily acces and set them*/

	/* IDs for the gun animations */
	public int grappleOut, blasterOut, portalOut;

	void Awake()
	{
		grappleOut = Animator.StringToHash ("GrappleOut");
		blasterOut = Animator.StringToHash ("BlasterOut");
		portalOut = Animator.StringToHash ("PortalOut");
	}
}
