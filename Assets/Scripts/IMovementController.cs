using UnityEngine;
using System.Collections;

public interface IMovementController {

	void SetMovementVector (Vector3 vector);

	void SetRotationVector (Vector3 vector);
}
