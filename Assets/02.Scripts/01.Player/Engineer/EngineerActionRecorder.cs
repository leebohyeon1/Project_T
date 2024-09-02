using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;



public class EngineerActionRecorder : MonoBehaviour
{
    public enum ActionType
    {
        Move,
        Rotate,
        Jump,
        PlaceObject  // ������Ʈ ��ġ �׼� �߰�
    }

    [System.Serializable]
    public class PlayerAction
    {
        public ActionType Type;        // �ൿ�� ����
        public Vector3 Position;       // �÷��̾��� ��ġ �Ǵ� ������Ʈ ��ġ ��ġ
        public Quaternion Rotation;    // �÷��̾��� ȸ�� �Ǵ� ������Ʈ ��ġ ȸ��
        public string FunctionName;    // ȣ���� �Լ� �̸�
        public GameObject Prefab;      // ��ġ�� ������Ʈ ������
        public Dictionary<string, AnimatorParameter> AnimatorParameters; // �ִϸ����� �Ķ���� ��
        public float TimeStamp;        // �ൿ�� ��ϵ� �ð�
    }

    // �ִϸ����� �Ķ���� ����ü ����
    [System.Serializable]
    public struct AnimatorParameter
    {
        public AnimatorControllerParameterType type;
        public object value;
    }

    private Animator animator; // �÷��̾� �ִϸ��̼�

    private List<PlayerAction> actions = new List<PlayerAction>(); // ��ȭ�� �ൿ ����Ʈ
    private bool isRecording = false;  // ���� ��ȭ ������ ����
    private float startTime; // ��ȭ ���� �ð�

    void Start()
    {
        animator = GetComponent<Animator>();

        StartRecording();
    }

    void FixedUpdate()
    {
        if (isRecording)
        {
            // �̵��� �ڵ����� ���
            RecordAction(ActionType.Move, transform.position, transform.rotation);
        }
    }

    // ��ȭ�� �����ϴ� �޼���
    public void StartRecording()
    {
        isRecording = true;
        startTime = Time.time;
    }

    // ��ȭ�� �����ϴ� �޼���
    public void StopRecording()
    {
        isRecording = false;
    }

    // ��ȭ �����͸� �ʱ�ȭ�ϴ� �޼���
    public void ResetRecording()
    {
        actions.Clear();
       isRecording = false;
    }

    // �ڵ� ��ȭ �޼���
    private void RecordAction(ActionType type, Vector3 position, Quaternion rotation, string functionName = null, GameObject prefab = null)
    {
        var animatorParameters = new Dictionary<string, AnimatorParameter>();

        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            AnimatorParameter paramData = new AnimatorParameter { type = parameter.type };

            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Float:
                    paramData.value = animator.GetFloat(parameter.name);
                    break;
                case AnimatorControllerParameterType.Int:
                    paramData.value = animator.GetInteger(parameter.name);
                    break;
                case AnimatorControllerParameterType.Bool:
                    paramData.value = animator.GetBool(parameter.name);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    paramData.value = false; // Trigger�� �̺�Ʈ ����̹Ƿ� ���¸� �������� ����
                    break;
            }

            animatorParameters[parameter.name] = paramData;
        }

        var currentAction = new PlayerAction
        {
            Type = type,
            Position = position,
            Rotation = rotation,
            FunctionName = functionName,
            AnimatorParameters = animatorParameters,
            Prefab = prefab,
            TimeStamp = Time.time - startTime

        };

        actions.Add(currentAction);
    }

    // ������Ʈ ��ġ ��ȭ �޼���
    public void RecordPlaceObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        RecordAction(ActionType.PlaceObject, position, rotation, null, prefab);
    }

    public void RecordJump(Vector3 position, Quaternion rotation)
    {
        RecordAction(ActionType.Jump, position, rotation);
    }

    // ��ȭ�� �ൿ ����Ʈ�� ��ȯ�ϴ� �޼���
    public List<PlayerAction> GetRecordedActions()
    {
        return new List<PlayerAction>(actions);
    }

}
