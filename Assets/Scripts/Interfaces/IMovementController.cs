using UnityEngine;
using System.Collections;

public interface IMovementController {

	void SetMovement (Vector3 vector);

	void SetRotation (Vector3 vector);

	void SetMovementAdditive (Vector3 vector);

	void SetRotationAdditive (Vector3 vevtor);
}
