using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetPhoto : MonoBehaviour
{

    public bool isProfile = false;
    bool profileSet;

    [SerializeField] string profileDestination;

    Texture2D _photo;
    // Start is called before the first frame update
    void Start()
    {
        if(!string.IsNullOrEmpty(profileDestination))
        {
            isProfile = true;
            profileSet = false;
        } else 
        {
            isProfile = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if(isProfile && !profileSet)
        {   
            
            GetPic(profileDestination);
        }

        
    }

    async void GetPic(string url)
    {
        

        _photo = await GetRemoteTexture(url);
        SetPic(_photo);
        profileSet = true;
        //Dispose();
        return;

    }

    void SetPic(Texture2D pic)
    {
        GameObject thisPic = transform.GetChild(0).gameObject;
        thisPic.GetComponent<Image>().sprite = Sprite.Create(pic, new Rect(0,0, pic.width, pic.height), new Vector2(0.5f, 0.5f));
        //thisPic.GetComponent<SpriteRenderer>().size = new Vector2(100,150);
        //thisPic.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    void OnDestroy () => Dispose();

    public void Dispose () => Object.Destroy(_photo);

    public static async Task<Texture2D> GetRemoteTexture ( string url )
{
    using( UnityWebRequest www = UnityWebRequestTexture.GetTexture(url) )
    {
        Debug.Log("GetRemoteTexture");
        // begin request:
        var asyncOp = www.SendWebRequest();

        // await until it's done: 
        while( asyncOp.isDone==false )
            await Task.Delay( 1000/30 );//30 hertz
        
        // read results:
        if( www.isNetworkError || www.isHttpError )
        {
            // log error:
            #if DEBUG
            Debug.Log( $"{www.error}, URL:{www.url}" );
            #endif
            
            // nothing to return on error:
            return null;
        }
        else
        {
            
            // return valid results:
            return DownloadHandlerTexture.GetContent(www);
        }
    }
}


}
