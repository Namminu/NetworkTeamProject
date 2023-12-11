using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowWhereAmI : MonoBehaviour
{
    public Canvas Arrow;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("deleteImage", 2.5f);
    }
    private void deleteImage()
    {
        Destroy( Arrow );
    }
}
