using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Events;

public class DialogController : MonoBehaviour
{
    // �_�C�A���O�̃v���n�u (Assets/Prefabs/Dialog)
    [SerializeField]
    private GameObject dialogPrefab;

    // �_�C�A���O�̃^�C�g�������̕�����
    [SerializeField]
    private string title = "title";

    // �_�C�A���O�̐��������̕�����
    [SerializeField]
    private string message = "message";

    // �_�C�A���O�̃{�^���̎��
    [SerializeField]
    private DialogButtonType buttons = DialogButtonType.Yes | DialogButtonType.No;

    // �߂��Ƀ_�C�A���O��\�������邩�ǂ����i�œK�����ꂽ�z�u�ɂȂ�j
    [SerializeField]
    private bool placeForNearInteraction = true;

    // Close���������Ƃ��ɂ�肽���C�x���g
    [SerializeField]
    private UnityEvent CloseButtonEvent = new UnityEvent();

    // Confirm���������Ƃ��ɂ�肽���C�x���g
    [SerializeField]
    private UnityEvent ConfirmButtonEvent = new UnityEvent();

    // Cancel���������Ƃ��ɂ�肽���C�x���g
    [SerializeField]
    private UnityEvent CancelButtonEvent = new UnityEvent();

    // Accept���������Ƃ��ɂ�肽���C�x���g
    [SerializeField]
    private UnityEvent AcceptButtonEvent = new UnityEvent();

    // Yes���������Ƃ��ɂ�肽���C�x���g
    [SerializeField]
    private UnityEvent YesButtonEvent = new UnityEvent();

    // No���������Ƃ��ɂ�肽���C�x���g
    [SerializeField]
    private UnityEvent NoButtonEvent = new UnityEvent();

    // OK���������Ƃ��ɂ�肽���C�x���g
    [SerializeField]
    private UnityEvent OKButtonEvent = new UnityEvent();

    // �_�C�A���O���J��
    public void OpenDialog()
    {
        Debug.Log("OpenDialog");

        // None�̂Ƃ���Close�ɏ���������
        if (buttons == DialogButtonType.None)
        {
            buttons = DialogButtonType.Close;
        }

        Dialog myDialog = Dialog.Open(dialogPrefab, buttons, title, message, placeForNearInteraction);
        if (myDialog != null)
        {
            myDialog.OnClosed += OnClosedDialogEvent;
        }
    }

    // �_�C�A���O��������̃C�x���g
    private void OnClosedDialogEvent(DialogResult dialogResult)
    {
        var result = dialogResult.Result;
        Debug.Log($"OnClosedDialogEvent: {result}");

        // �C�x���g���ݒ肳��Ă�������s����
        switch (result)
        {
            case DialogButtonType.Close:
                if (CloseButtonEvent.GetPersistentEventCount() > 0)
                {
                    Debug.Log("CloseButtonEvent");
                    CloseButtonEvent.Invoke();
                }
                break;
            case DialogButtonType.Confirm:
                if (ConfirmButtonEvent.GetPersistentEventCount() > 0)
                {
                    Debug.Log("ConfirmButtonEvent");
                    ConfirmButtonEvent.Invoke();
                }
                break;
            case DialogButtonType.Cancel:
                if (CancelButtonEvent.GetPersistentEventCount() > 0)
                {
                    Debug.Log("CancelButtonEvent");
                    CancelButtonEvent.Invoke();
                }
                break;
            case DialogButtonType.Accept:
                if (AcceptButtonEvent.GetPersistentEventCount() > 0)
                {
                    Debug.Log("AcceptButtonEvent");
                    AcceptButtonEvent.Invoke();
                }
                break;
            case DialogButtonType.Yes:
                if (YesButtonEvent.GetPersistentEventCount() > 0)
                {
                    Debug.Log("YesButtonEvent");
                    YesButtonEvent.Invoke();
                }
                break;
            case DialogButtonType.No:
                if (NoButtonEvent.GetPersistentEventCount() > 0)
                {
                    Debug.Log("NoButtonEvent");
                    NoButtonEvent.Invoke();
                }
                break;
            case DialogButtonType.OK:
                if (OKButtonEvent.GetPersistentEventCount() > 0)
                {
                    Debug.Log("OKButtonEvent");
                    OKButtonEvent.Invoke();
                }
                break;
        }
    }
}
