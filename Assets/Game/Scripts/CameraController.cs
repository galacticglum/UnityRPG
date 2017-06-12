using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float targetHeight = 1;
    [SerializeField]
    private float distance = 12;
    [SerializeField]
    private float collisionOffset = 0.1f;
    [SerializeField]
    private float maximumZoom = 20;
    [SerializeField]
    private float minimumZoom = 0.6f;
    [SerializeField]
    private float horizontalSensitivity = 200;
    [SerializeField]
    private float verticalSensitivity = 200;
    [SerializeField]
    private float verticalMaximum = 80;
    [SerializeField]
    private float verticalMinimum = -80;
    [SerializeField]
    private float zoomSpeed = 40;
    [SerializeField]
    private float rotationDampening = 3;
    [SerializeField]
    private float zoomDampening = 5;
    [SerializeField]
    private LayerMask collisionLayerMask = -1;
    [SerializeField]
    private bool lockToRearOfTarget;
    [SerializeField]
    private bool allowMouseInputX = true;
    [SerializeField]
    private bool allowMouseInputY = true;

    private float xRotation;
    private float yRotation;
    private float currentDistance;
    private float desiredDistance;
    private float correctDistance;
    private bool doRotateBehind;
    private bool isToggleMove;
    private float toggleMoveCooldown = 0.5f;
    private float currentCooldown;

	// Use this for initialization
	private void Start ()
	{
	    xRotation = transform.eulerAngles.x;
	    yRotation = transform.eulerAngles.y;
	    currentDistance = distance;
	    desiredDistance = distance;
	    correctDistance = distance;

	    if (lockToRearOfTarget)
	    {
	        doRotateBehind = true;
	    }

	    Rigidbody rigidBody = GetComponent<Rigidbody>();
        if (rigidBody != null)
        {
            rigidBody.freezeRotation = true;
        }
	}
	
	// Update is called once per frame
	private void Update ()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
	}

    private void LateUpdate()
    {
        if (target == null) return;
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else if (currentCooldown <= 0)
        {
            currentCooldown = 0;
        }

        if (Input.GetAxis("Toggle Move") != 0 && currentCooldown == 0)
        {
            currentCooldown = toggleMoveCooldown;
            isToggleMove = !isToggleMove;
        }

        if (isToggleMove && Input.GetAxis("Vertical") != 0)
        {
            isToggleMove = false;
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (allowMouseInputX)
            {
                xRotation += Input.GetAxis("Mouse X") * horizontalSensitivity * 0.02f;
            }
            else
            {
                RotateBehindTarget();
            }

            if (allowMouseInputY)
            {
                yRotation -= Input.GetAxis("Mouse Y") * verticalSensitivity * 0.02f;
            }

            if (!lockToRearOfTarget)
            {
                doRotateBehind = false;
            }
        }
        else if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || doRotateBehind || isToggleMove)
        {
            RotateBehindTarget();
        }

        yRotation = ClampAngle(yRotation, verticalMinimum, verticalMaximum);
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(desiredDistance, minimumZoom, maximumZoom);
        correctDistance = desiredDistance;

        Vector3 targetOffset = new Vector3(0, -targetHeight, 0);
        Quaternion rotation = Quaternion.Euler(yRotation, xRotation, 0);
        Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance + targetOffset);
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + targetHeight, target.position.z);

        bool hasCorrected = false;
        RaycastHit hit;
        if (Physics.Linecast(targetPosition, position, out hit, collisionLayerMask))
        {
            correctDistance = Vector3.Distance(targetPosition, hit.point) - collisionOffset;
            hasCorrected = true;
        }

        currentDistance = !hasCorrected || correctDistance > currentDistance ? 
            Mathf.Lerp(currentDistance, correctDistance, Time.deltaTime * zoomDampening) : correctDistance;
        correctDistance = Mathf.Clamp(currentDistance, minimumZoom, maximumZoom);
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
        transform.rotation = rotation;
    }

    private void RotateBehindTarget()
    {
        float targetAngle = target.eulerAngles.y;
        float currentAngle = transform.eulerAngles.y;
        xRotation = Mathf.LerpAngle(currentAngle, targetAngle, rotationDampening * Time.deltaTime);

        if (targetAngle == currentAngle)
        {
            if (!lockToRearOfTarget)
            {
                doRotateBehind = false;
            }
        }
        else
        {
            doRotateBehind = true;
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }

        if (angle > 360)
        {
            angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }
}
