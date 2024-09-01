using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EngineerBuilder : MonoBehaviour
{
    private EngineerActionRecorder actionRecorder;

    [BoxGroup("Object Building Settings"), LabelText("��ġ�� ������Ʈ")]
    public GameObject objectPrefab; // ��ġ�� ������Ʈ ������
    [BoxGroup("Object Building Settings"), LabelText("��ġ ������ ǥ�� ���̾�")]
    public LayerMask buildableSurfaceLayer; // ��ġ ������ ǥ�� ���̾�
    [BoxGroup("Object Building Settings"), LabelText("��ġ �ִ� �Ÿ�")]
    public float maxBuildDistance = 5f; // �÷��̾ ��ġ�� �� �ִ� �ִ� �Ÿ�

    private GameObject objectPreview; // ������Ʈ ��ġ �̸�����
    private bool isBuilding = false; // �Ǽ� ��� Ȱ��ȭ ����
    private Collider objectCollider; // ������Ʈ �������� Collider
    private Renderer[] objectRenderers; // ������Ʈ �������� ��� Renderer
    private List<GameObject> placedObjects = new List<GameObject>(); // ��ġ�� ������Ʈ ���� ����Ʈ
    private bool canPlace = false; // ��ġ���� ����

    private Transform cameraTransform; // ī�޶� Transform


    private List<GameObject> objectClones_ = new List<GameObject>();
    public List<GameObject> objectClones => objectClones_;

    private void Start()
    {
        actionRecorder = GetComponent<EngineerActionRecorder>();
        cameraTransform = Camera.main.transform; // ���� ī�޶��� Transform ��������
    }

    // �Ǽ� ��带 Ȱ��ȭ/��Ȱ��ȭ�ϴ� �Է� ó��
    public void HandleBuildingInput(bool isBuildingModeActive, bool placeObject)
    {
        if (isBuildingModeActive != isBuilding)
        {
            isBuilding = !isBuilding;

            if (isBuilding)
            {
                StartBuildingMode();
            }
            else
            {
                StopBuildingMode();
            }
        }

        if (isBuilding)
        {
            UpdateObjectPreview();
            if (placeObject && canPlace)
            {
                PlaceObject();
            }
        }
    }

    // �Ǽ� ��� ����
    private void StartBuildingMode()
    {
        if (objectPrefab == null) return;

        // ��ġ �̸����� ����
        objectPreview = Instantiate(objectPrefab);
        objectPreview.layer = 0;

        objectCollider = objectPreview.GetComponent<Collider>(); // Collider ���� ��������
        objectCollider.isTrigger = true; // �浹 ����

        objectRenderers = objectPreview.GetComponentsInChildren<Renderer>(); // ��� Renderer ���� ��������

        if (objectPreview.GetComponent<Turret>() != null)
        {
            objectPreview.GetComponent<Turret>().enabled = false;
        }

        objectPreview.GetComponent<NavMeshObstacle>().enabled = false;

        SetObjectPreviewMaterialAlpha(0.5f); // �������ϰ� ����
    }

    // �Ǽ� ��� ����
    private void StopBuildingMode()
    {
        if (objectPreview != null)
        {
            Destroy(objectPreview);
        }
    }

    // ��ġ �̸����� ������Ʈ
    private void UpdateObjectPreview()
    {
        if (objectPreview == null) return;

        // ȭ�� �߾ӿ��� ������ �߻�
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // ���̸� �׷��� �ð������� Ȯ��
        Debug.DrawRay(ray.origin, ray.direction * maxBuildDistance, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, maxBuildDistance, buildableSurfaceLayer))
        {
            // ��ǥ ��ġ�� ȸ���� ����
            objectPreview.transform.position = hit.point;

            // ������ ������Ʈ�� ȸ���� ī�޶��� �չ���� ǥ�� ���� ������ �������� ����
            Vector3 forwardDirection = cameraTransform.forward; // ī�޶� �ٶ󺸴� ����
            Vector3 upDirection = hit.normal; // ǥ���� ���� ����

            objectPreview.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(forwardDirection, upDirection), upDirection);

            // ��ġ ���� ���ο� ���� ���� ����
            UpdateObjectPreviewColor(IsPlacementValid());

            canPlace = true; // ��ġ����
        }
        else
        {
            // ���̰� ���� �ʾ��� ��, ������ �� �κп� ������ ��ġ ����
            objectPreview.transform.position = ray.origin + ray.direction * maxBuildDistance;
            objectPreview.transform.rotation = Quaternion.LookRotation(cameraTransform.forward, Vector3.up); // ī�޶� �������� ����
            UpdateObjectPreviewColor(false); // ��ġ �Ұ����� ���·� ���� ����

            canPlace = false; // ��ġ�Ұ�
        }
    }

    // ������Ʈ ��ġ
    private void PlaceObject()
    {
        if (objectPreview == null) return;

        // �浹 �˻�: ��ġ�Ϸ��� ��ġ�� �ٸ� ������Ʈ�� �ִ��� Ȯ��
        if (IsPlacementValid())
        {
            Vector3 buildPosition = objectPreview.transform.position;
            Quaternion buildRotation = objectPreview.transform.rotation;

            // ������Ʈ�� �����ϰ� ����Ʈ�� �߰�
            GameObject placedObject = Instantiate(objectPrefab, buildPosition, buildRotation);
            objectClones.Add(placedObject);

            // ��ġ �ൿ�� ���
            actionRecorder.RecordPlaceObject(objectPrefab, buildPosition, buildRotation); // ��ȭ
        }
        else
        {
            Debug.Log("Cannot place object here, another object is in the way.");
        }
    }

    // ��ġ ���� ���� Ȯ��
    private bool IsPlacementValid()
    {
        Collider[] colliders = Physics.OverlapBox(
            objectCollider.bounds.center,
            objectCollider.bounds.extents,
            objectPreview.transform.rotation,
            ~buildableSurfaceLayer); // �浹�� �� �ִ� ���̾ �����Ͽ� �˻�

        foreach (var collider in colliders)
        {
            if (collider != objectCollider) // �ڱ� �ڽ��� �ƴ� �ٸ� ������Ʈ�� �浹 ��
            {
                return false; // ��ġ �Ұ���
            }
        }

        return true; // ��ġ ����
    }

    // ������Ʈ �̸������� ���� ����
    private void SetObjectPreviewMaterialAlpha(float alpha)
    {
        foreach (Renderer renderer in objectRenderers)
        {
            foreach (Material material in renderer.materials)
            {
                material.SetFloat("_Mode", 3); // ������ ��带 Transparent�� ����
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;

                Color color = material.color;
                color.a = alpha;
                material.color = color;

                // ����� ������ ���� ���� ������ ����
                material.SetOverrideTag("RenderType", "Transparent");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
        }
    }

    // ��ġ ���� ���ο� ���� ������ ���� ������Ʈ
    private void UpdateObjectPreviewColor(bool canPlace)
    {
        Color color = canPlace ? Color.green : Color.red; // ��ġ ���� ���ο� ���� ���� ����
        color.a = 0.5f; // ������ ����

        foreach (Renderer renderer in objectRenderers)
        {
            foreach (Material material in renderer.materials)
            {
                material.color = color; // ���� ����
            }
        }
    }

    // ���� ����� �� ��ġ�� ������Ʈ ����
    public void ClearPlacedObjects()
    {
        foreach (GameObject obj in placedObjects)
        {
            Destroy(obj); // ������Ʈ ����
        }
        placedObjects.Clear(); // ����Ʈ �ʱ�ȭ
    }

    public List<GameObject> GetObjectClones()
    {
        return objectClones;
    }
}
