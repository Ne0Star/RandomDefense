
using UnityEngine;

public class HelpController : MonoBehaviour
{
    [SerializeField] private Transform blocker;
    public void Close()
    {
        blocker.gameObject.SetActive(false);
    }
    public void Open()
    {
        blocker.gameObject.SetActive(true);
    }

}
