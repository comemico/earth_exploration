using UnityEngine;
using UnityEngine.UI;

public class ExitManager : MonoBehaviour
{
    public Button push_Yes;
    public Button push_No;

    PopController popCtrl;

    void Start()
    {
        popCtrl = GetComponent<PopController>();
        AddListener();
    }

    void AddListener()
    {
        push_Yes.onClick.AddListener(EndGame);
        push_No.onClick.AddListener(() => popCtrl.ClosePanel(false));
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
        Application.Quit();//�Q�[���v���C�I��
#endif
    }

}
