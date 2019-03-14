using UnityEngine;
using System.Collections;

// Maybe make this static later and put update in another class idk?
public class SaveAll : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // If shift 5 was pressed save all
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5)) 
        {
            PlayerDatabase.Save();
        }

        // If shift 9 was pressed load all
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlayerDatabase.Load();
        }
    }
}
