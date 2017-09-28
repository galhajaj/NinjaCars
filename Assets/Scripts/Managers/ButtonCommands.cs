using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonCommands : MonoBehaviour 
{
    public void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SaveGame()
    {
        DataManager.Instance.SaveDataToFile();
    }

    public void LoadGame()
    {
        DataManager.Instance.LoadDataFromSaveFile();
        Inventory.Instance.FillChipsListFromDataManager();
    }
}
