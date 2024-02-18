using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eddy.Utilities
{
	public class LookAtCameraPosition : MonoBehaviour
	{
		Transform targetToLook;

		void Awake()
		{
			targetToLook = Camera.main.transform;
		}

		// Update is called once per frame
		void Update()
		{
			transform.LookAt(targetToLook);
		}
	}
}
