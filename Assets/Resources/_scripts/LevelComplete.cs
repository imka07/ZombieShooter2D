using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    public int currentLevel;

    public void OnLevelComplete()
    {
        gameManager.instance.CompleteLevel(currentLevel);
    }
}
