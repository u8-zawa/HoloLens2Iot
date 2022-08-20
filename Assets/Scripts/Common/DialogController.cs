using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Events;

public class DialogController : MonoBehaviour
{
    // ダイアログのプレハブ (Assets/Prefabs/Dialog)
    [SerializeField]
    private GameObject dialogPrefab;

    // ダイアログのタイトル部分の文字列
    [SerializeField]
    private string title = "title";

    // ダイアログの説明部分の文字列
    [SerializeField]
    private string message = "message";

    // ダイアログのボタンの種類
    [SerializeField]
    private DialogButtonType buttons = DialogButtonType.Yes | DialogButtonType.No;

    // 近くにダイアログを表示させるかどうか（最適化された配置になる）
    [SerializeField]
    private bool placeForNearInteraction = true;

    // Closeを押したときにやりたいイベント
    [SerializeField]
    private UnityEvent CloseButtonEvent = new UnityEvent();

    // Confirmを押したときにやりたいイベント
    [SerializeField]
    private UnityEvent ConfirmButtonEvent = new UnityEvent();

    // Cancelを押したときにやりたいイベント
    [SerializeField]
    private UnityEvent CancelButtonEvent = new UnityEvent();

    // Acceptを押したときにやりたいイベント
    [SerializeField]
    private UnityEvent AcceptButtonEvent = new UnityEvent();

    // Yesを押したときにやりたいイベント
    [SerializeField]
    private UnityEvent YesButtonEvent = new UnityEvent();

    // Noを押したときにやりたいイベント
    [SerializeField]
    private UnityEvent NoButtonEvent = new UnityEvent();

    // OKを押したときにやりたいイベント
    [SerializeField]
    private UnityEvent OKButtonEvent = new UnityEvent();

    // ダイアログを開く
    public void OpenDialog()
    {
        Debug.Log("OpenDialog");

        // NoneのときはCloseに書き換える
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

    // ダイアログを閉じた時のイベント
    private void OnClosedDialogEvent(DialogResult dialogResult)
    {
        var result = dialogResult.Result;
        Debug.Log($"OnClosedDialogEvent: {result}");

        // イベントが設定されていたら実行する
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
