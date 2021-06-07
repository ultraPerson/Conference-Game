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
    bool affCheck = false;
    GameObject profPic;
    [SerializeField] GameObject affPic;
    GameObject name;

    [SerializeField] string profileDestination;

    Texture2D _photo;
    // Start is called before the first frame update
    void Start()
    {

        profPic = transform.GetChild(0).gameObject;
        affPic = transform.GetChild(3).gameObject;
        name = transform.GetChild(4).gameObject;

        if(!string.IsNullOrEmpty(profileDestination))
        {
            isProfile = true;
            profileSet = false;
        } else 
        {
            isProfile = false;
        }

        if(isProfile && !profileSet && profPic.GetComponent<Image>().sprite.name == "avatar_placeholder")
        {   
            
            GetPic(profileDestination);
        }

        if(affPic.GetComponent<Image>().sprite.name == "Crop")
        {
            affPic.SetActive(false);
            name.GetComponent<RectTransform>().sizeDelta = new Vector2(.6f, name.GetComponent<RectTransform>().sizeDelta.y);
            name.GetComponent<RectTransform>().localPosition = new Vector3(0.186f, name.transform.localPosition.y, name.transform.localPosition.z);
            affCheck = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        

        
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
        
        profPic.GetComponent<Image>().sprite = Sprite.Create(pic, new Rect(0,0, pic.width, pic.height), new Vector2(0.5f, 0.5f));
        //profPic.GetComponent<SpriteRenderer>().size = new Vector2(100,150);
        //profPic.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
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
