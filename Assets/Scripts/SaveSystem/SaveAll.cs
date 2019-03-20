using UnityEngine;
using System.Collections;

// Maybe make this static later and put update in another class idk?
public class SaveAll : MonoBehaviour
{
    public SkillTree2 skillTree;
    public bool NewLoad = false;

    // Update is called once per frame
    void Update()
    {
        // If left shift 5 was pressed save all
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5)) 
        {
            PlayerDatabase.Save();
            skillTree.Save();
        }

        // If left shift 9 was pressed load all
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha9))
        {
            NewLoad = true;
            PlayerDatabase.Load();
            skillTree.Load();
        }

        NewLoad = false;
    }
}
