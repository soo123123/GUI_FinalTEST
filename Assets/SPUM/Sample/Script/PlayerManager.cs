using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_6000_0_OR_NEWER
using UnityEngine.InputSystem;
#endif
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public PlayerObj _prefabObj; //플레이어 오브젝트 원형 프리팹
    public List<SPUM_Prefabs> _savedUnitList = new List<SPUM_Prefabs>(); //캐릭터 프리팹 목록
    public Vector2 _startPos;
    public Vector2 _addPos;
    public int _columnNum;
    public int UnitMaxCount = 20;
    public Transform _playerPool;
    public List<PlayerObj> _playerList = new List<PlayerObj>(); //실제 씬에 생성된 플레이어 오브젝트 목록
    public PlayerObj _nowObj; //현재 선택된 캐릭터
    public Transform _playerObjCircle; //현재 선택된 캐릭터 위치를 표시하는 원형 마커
    public Transform _goalObjCircle; //이동 목표 지점을 표시하는 마커
    
    //스크린샷 저장 기능
    public Camera _camera;
    Texture2D imageSave;
    public GameObject _bg;
    // end

    //선택한 캐릭터 애니메이션 목록 UI로 보여주기 위한 요소
    public RectTransform CommandPanel;
    public Button AnimationButton;
    public Transform AnimationPanelParent;
    public GameObject AnimationPanel;
    // end
    public enum ScreenShotSize
    {
        HD,
        FHD,
        UHD
    }
    public ScreenShotSize _screenShotSize = ScreenShotSize.HD;

    //씬 시작 시 캐릭터들을 자동으로 불러오는 초기화 함수
    void Start()
    {
        #if UNITY_6000_0_OR_NEWER
        CheckInputSystemUIModule();
        #endif
        if(_savedUnitList.Count.Equals(0) || _playerList.Count.Equals(0))
            GetPlayerList();
    }
    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;
        #if UNITY_6000_0_OR_NEWER
        if(Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
        #else
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        #endif
            
            if(hit.collider != null)
            {
                bool isHitPlayer = hit.collider.CompareTag("Player");
                CommandPanel.gameObject.SetActive(isHitPlayer);
               
                if(isHitPlayer)
                {
                    _nowObj = hit.collider.GetComponent<PlayerObj>();
                    CreateAnimationPanel(_nowObj);
                }
                else
                {
                    //Set move Player object to this point
                    if(_nowObj!=null)
                    {
                        Vector2 goalPos = hit.point;
                        _goalObjCircle.transform.position = hit.point;
                        _nowObj.SetMovePos(goalPos);
                    }
                }
            }
        }

        if(_nowObj!=null)
        {
            _playerObjCircle.transform.position = _nowObj.transform.position;
        }
    }
    public void CreateAnimationPanel(PlayerObj Unit)
    {
        foreach (Transform item in AnimationPanelParent.transform)
        {
            Destroy(item.gameObject);
        }
        var Info = Unit._prefabs.StateAnimationPairs;
        foreach (var StateName in Info.Keys)
        {
            var Panel = Instantiate(AnimationPanel, AnimationPanelParent);
            string StateNameformat = $"{StateName} State";
            Panel.GetComponentInChildren<Text>().text = StateNameformat;
            var ParentTranform = Panel.GetComponentInChildren<ContentSizeFitter>().transform;
            foreach (var clip in Info[StateName])
            {
                var Button = Instantiate(AnimationButton, ParentTranform);
                Button.GetComponentInChildren<Text>().text = clip.name;
                Button.onClick.AddListener(()=> {
                    if(Enum.TryParse(StateName, true, out PlayerState State))
                    {
                        Unit.isAction = true;
                        int index = Info[StateName].FindIndex(x => x == clip);
                        Debug.Log(State + ":" + index);
                        Unit._prefabs._anim.Rebind();
                        Unit.SetStateAnimationIndex(State, index);
                        Unit.PlayStateAnimation(State);
                    }
                });
            }
        }
    }
    public void ClearPlayerList()
    {
        List<GameObject> tList = new List<GameObject>();
        for(var i =0; i < _playerPool.transform.childCount; i++)
        {
            GameObject tOBjj = _playerPool.transform.GetChild(i).gameObject;
            tList.Add(tOBjj);
        }
        foreach(var obj in tList)
        {
            DestroyImmediate(obj);
        }

        //net Edited. 2022.01.18
        _savedUnitList.Clear();
        _playerList.Clear();
    }
    public void GetPlayerList()
    {
        ClearPlayerList();

        var saveArray = Resources.LoadAll<SPUM_Prefabs>("");
        foreach (var unit in saveArray)
        {
            if (unit.ImageElement.Count > 0)
            {
                _savedUnitList.Add(unit);
                unit.PopulateAnimationLists();
            }
        }

        
        float numXStart = _startPos.x;
        float numYStart = _startPos.y;

        float numX = _addPos.x;
        float numY = _addPos.y;
        float ttV = 0;

        int sColumnNum = _columnNum;

        for(var i = 0 ; i < UnitMaxCount;i++)
        {
            if(i > _savedUnitList.Count-1) continue;
            if(i > sColumnNum-1)
            {
                numYStart -= 1f;
                numXStart -= numX * _columnNum;
                sColumnNum += _columnNum;
                ttV += numY;
            }
            
            GameObject ttObj = Instantiate(_prefabObj.gameObject) as GameObject;
            ttObj.transform.SetParent(_playerPool);
            ttObj.transform.localScale = new Vector3(1,1,1);
            

            var tObj = Instantiate(_savedUnitList[i]);
            tObj.transform.SetParent(ttObj.transform);
            tObj.transform.localScale = new Vector3(1,1,1);
            tObj.transform.localPosition = Vector3.zero;

            ttObj.name = _savedUnitList[i].name;

            PlayerObj tObjST = ttObj.GetComponent<PlayerObj>();

            tObjST._prefabs = tObj;

            ttObj.transform.localPosition = new Vector3(numXStart + numX * i,numYStart+ttV,0);
            _playerList.Add(tObjST);
        }
    }
    public void SetAlignUnits()
    {
        float numXStart = _startPos.x;
        float numYStart = _startPos.y;

        float numX = _addPos.x;
        float numY = _addPos.y;
        float ttV = 0;

        int sColumnNum = _columnNum;
        _playerList = _playerList.Where(s => s != null).ToList();
        for(var i = 0 ; i < _playerList.Count-1;i++)
        {
            if(i > sColumnNum-1)
            {
                numYStart -= 1f;
                numXStart -= numX * _columnNum;
                sColumnNum += _columnNum;
                ttV += numY;
            }
            
            GameObject ttObj = _playerList[i].gameObject;

            ttObj.transform.localPosition = new Vector3(numXStart + numX * i,numYStart+ttV,0);
        }
    }
    //스크린샷 찍기
    public void SetScreenShot()
    {
        
        _bg.SetActive(false);
        Vector2 _nowSize = new Vector2(Screen.currentResolution.width,Screen.currentResolution.height);
        switch(_screenShotSize)
        {
            case ScreenShotSize.HD:
            Screen.SetResolution(1280, 720, false);
            break;

            case ScreenShotSize.FHD:
            Screen.SetResolution(1920, 1080, false);
            break;

            case ScreenShotSize.UHD:
            Screen.SetResolution(3840, 2160, false);
            break;
        }
        int tX = _camera.scaledPixelWidth;
		int tY = _camera.scaledPixelHeight;

		RenderTexture tempRT = new RenderTexture(tX, tY, 24, RenderTextureFormat.ARGB32)
		{
			antiAliasing = 4
		};
	
		_camera.targetTexture = tempRT;
		RenderTexture.active = tempRT;
		_camera.Render();

		imageSave = new Texture2D(tX, tY, TextureFormat.ARGB32, false, true);
		
		float tXPos = tX*0.5f - imageSave.width*0.5f;
		float tYPos = tY*0.5f - imageSave.height*0.5f;

		imageSave.ReadPixels(new Rect(tXPos, tYPos, imageSave.width, imageSave.height), 0, 0);
		imageSave.Apply();

        byte[] bytes = imageSave.EncodeToPNG();
        string tName = string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}", System.DateTime.Now);
        string filename = string.Format("{0}/SPUM/ScreenShots/{1}.png",Application.dataPath,tName);
        System.IO.File.WriteAllBytes(filename, bytes);

		RenderTexture.active = null;
		_camera.targetTexture = null;

		DestroyImmediate(tempRT);
        DestroyImmediate(imageSave);

        Screen.SetResolution((int)_nowSize.x, (int)_nowSize.y, false);
        Debug.Log("Screenshot Saved : " + filename);
        _bg.SetActive(true);
    }
    #if UNITY_6000_0_OR_NEWER
    void CheckInputSystemUIModule()
    {
        if (EventSystem.current == null)
        {
            Debug.LogWarning("[SPUM] EventSystem not found in scene");
            return;
        }

        var currentModule = EventSystem.current.currentInputModule;
        
        // StandaloneInputModule Remove and InputSystemUIInputModule Add
        if (currentModule == null || currentModule.GetType().Name != "InputSystemUIInputModule")
        {
            // StandaloneInputModule Remove
            var standaloneModule = EventSystem.current.GetComponent<StandaloneInputModule>();
            if (standaloneModule != null)
            {
                DestroyImmediate(standaloneModule);
                Debug.Log("[SPUM] StandaloneInputModule removed");
            }

            // Other InputModules Remove
            if (currentModule != null && currentModule != standaloneModule)
            {
                DestroyImmediate(currentModule);
            }

            // InputSystemUIInputModule Add
            try
            {
                var inputSystemModule = EventSystem.current.gameObject.AddComponent(System.Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem"));
                Debug.Log("[SPUM] InputSystemUIInputModule added to EventSystem for Unity 6+ compatibility");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[SPUM] Failed to add InputSystemUIInputModule: {e.Message}");
                
                // 실패 시 StandaloneInputModule 복원
                EventSystem.current.gameObject.AddComponent<StandaloneInputModule>();
                Debug.Log("[SPUM] StandaloneInputModule restored as fallback");
            }
        }
        else
        {
            Debug.Log("[SPUM] InputSystemUIInputModule already configured");
        }
    }
    #endif
}
