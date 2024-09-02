using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EngineerActionRecorder;

public class CloneReplayer : MonoBehaviour
{
    private EngineerBuilder Engineer;
    private Animator animator;

    private List<PlayerAction> recordedActions;
    private int currentActionIndex = 0;
    private bool isReplaying = false;
    private float replayStartTime;

    public Material[] cloneMaterials;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isReplaying)
        {
            ReplayAction();
        }
    }

    // ���÷��̸� �����ϴ� �޼���
    public void StartReplay(List<PlayerAction> actions)
    {
        if (actions == null || actions.Count == 0) return;

        recordedActions = actions;
        currentActionIndex = 0;
        isReplaying = true;
        replayStartTime = Time.time;
    }

    // �ൿ�� ����ϴ� �޼���
    private void ReplayAction()
    {
        if (currentActionIndex >= recordedActions.Count)
        {
            isReplaying = false;
            Destroy(gameObject);
            return;
        }

        // ���� �ൿ ��������
        var action = recordedActions[currentActionIndex];

        // ���� �ð��� ��� ���� �ð��� ���� ���
        float elapsedTime = Time.time - replayStartTime;

        // ���� ���� �ൿ�� Ÿ�ӽ������� ����� �ð����� �۰ų� ������ �ൿ ���
        if (action.TimeStamp <= elapsedTime)
        {
            ApplyAnimatorParameters(action.AnimatorParameters);

            switch (action.Type)
            {
                case ActionType.Move:
                    transform.position = action.Position;
                    transform.rotation = action.Rotation;
                    break;
                case ActionType.Jump:
                    transform.position = action.Position;
                    transform.rotation = action.Rotation;
                    animator.SetTrigger("Jump");
                    break;
                case ActionType.PlaceObject:
                    GameObject placedObject = Instantiate(action.Prefab, action.Position, action.Rotation);
                    placedObject.GetComponent<Collider>().isTrigger = true; // Ŭ���� ��ȯ�� ������Ʈ�� Ʈ���� 
                    changebjectPreviewColor(placedObject);  
                    Engineer.objectClones.Add(placedObject);
                    animator.SetTrigger("Place");
                    break;

               
            }

            // ���� �ൿ���� �̵�
            currentActionIndex++;
        }
    }

    private void ApplyAnimatorParameters(Dictionary<string, EngineerActionRecorder.AnimatorParameter> animatorParameters)
    {
        foreach (var parameter in animatorParameters)
        {
            switch (parameter.Value.type)
            {
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(parameter.Key, (float)parameter.Value.value);
                    break;
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(parameter.Key, (int)parameter.Value.value);
                    break;
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(parameter.Key, (bool)parameter.Value.value);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    // Trigger ���´� ���� �������� ������, ��� �� �ʿ��� ��쿡�� ���
                    break;
            }
        }
    }

    // ������Ʈ ���׸��� ����
    private void changebjectPreviewColor(GameObject placeObject)
    {
        Renderer[] objectRenderers = placeObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in objectRenderers)
        {
            renderer.materials = cloneMaterials; // ���׸��� ��ü
        }
    }

    public void SetEngineer(EngineerBuilder builder)
    {
        Engineer = builder;
    }

  
}
