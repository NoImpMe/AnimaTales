using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class DisplayController : MonoBehaviour
{ 
    //옵션 중 그래픽 관련 옵션 관리
        List<(int, int)> resolutionList = new List<(int, int)>() { (960, 540), (1280, 720), (1366, 768), (1600, 900), (1920, 1080), (2560, 1440) }; //해상도 리스트. 일단 16:9만 사용. 최대 8k까지 감지
        List<int> framerateList = new List<int>() { 30, 60, 120, 144 };     //프레임레이트 리스트.

        //엔진에서 마우스로 끌어서 참조하는 부분을 최소화. 스크립트에서 대부분 처리하도록 
        [SerializeField] private Transform resolutionObject;
        [SerializeField] private Transform fullScreenModeObject;
        [SerializeField] private Transform framerateObject;

         private TMP_Text resolutionText;
         private TMP_Text fullScreenModeText;
         private TMP_Text framerateText;

         private Button resolutionButtonDown;
         private Button fullScreenModeButtonDown;
         private Button framerateButtonDown;

         private Button resolutionButtonUp;
         private Button fullScreenModeButtonUp;
        private Button framerateButtonUp;

        private (int, int) resolution;     //해상도
        private int fullScreenMode;        //전체 화면
        private int framerate;             //주사율

        private OptionButtonController optionButtonController;

        private void Awake()
        {
            for (int i = resolutionList.Count - 1; 0 <= i; --i)
            {
                if (Screen.currentResolution.height < resolutionList[i].Item2)
                    resolutionList.RemoveAt(i);
                else break;
            }

            InitOptionItem(resolutionObject, out resolutionText, out resolutionButtonDown, out resolutionButtonUp, OnClickResolutionDown, OnClickResolutionUp);
            InitOptionItem(fullScreenModeObject, out fullScreenModeText, out fullScreenModeButtonDown, out fullScreenModeButtonUp, OnClickFullScreenModeDown, OnClickFullScreenModeUp);
            InitOptionItem(framerateObject, out framerateText, out framerateButtonDown, out framerateButtonUp, OnClickFramerateDown, OnClickFramerateUp);


        optionButtonController = GetComponentInParent<OptionButtonController>();    //옵션 내 공용 버튼을 위함. (적용, 닫기 버튼)
        }

        protected void OnEnable()
        {
            resolution.Item1 = PreferenceData.ResolutionWidth;
            resolution.Item2 = PreferenceData.ResolutionHeight;
            fullScreenMode = PreferenceData.FullScreenMode;
            framerate = PreferenceData.Framerate;

            UpdateResolution();
            UpdateFullScreenMode();
            UpdateFramerate();

        }

        public void OnClickApply()      //현재 적용 버튼은 그래픽 옵션에서만 사용함. 다른 기능은 즉시 반영하여 적용 버튼이 별도로 필요하지 않음
        {
            if (CheckGraphicSettingChange())    //그래픽 변경 시 정상적으로 표시가 되지 않을 수도 있으므로 15초 이내로 사용자 입력이 없으면 이전 설정으로 되돌아 올 수 있도록 안전장치를 해둔다.
            {
                Screen.SetResolution(resolution.Item1, resolution.Item2, (FullScreenMode)fullScreenMode);
                Application.targetFrameRate = framerate;
                PreferenceData.ResolutionWidth = resolution.Item1;
                    PreferenceData.ResolutionHeight = resolution.Item2;
                    PreferenceData.FullScreenMode = fullScreenMode;
                    PreferenceData.Framerate = framerate;
                    PreferenceData.ApplyGraphicOptionSetting();
                    OnEnable();    
            }
        }

        private void OnClickResolutionDown()
        {
            if (resolutionList[resolutionList.Count - 1].Item1 < resolution.Item1)
            {
                resolution.Item1 = resolutionList[resolutionList.Count - 1].Item1;
                resolution.Item2 = resolutionList[resolutionList.Count - 1].Item2;
            }
            else
            {
                for (int i = 0; i < resolutionList.Count; ++i)
                {
                    if (resolution.Item1 == resolutionList[i].Item1)
                    {
                        resolution.Item1 = resolutionList[i - 1].Item1;
                        resolution.Item2 = resolutionList[i - 1].Item2;
                        break;
                    }
                }
            }
            UpdateResolution();
        }
        private void OnClickResolutionUp()
        {
            if (resolution.Item1 < resolutionList[0].Item1)
            {
                resolution.Item1 = resolutionList[0].Item1;
                resolution.Item2 = resolutionList[0].Item2;
            }
            else
            {
                for (int i = 0; i < resolutionList.Count; ++i)
                {
                    if (resolution.Item1 == resolutionList[i].Item1)
                    {
                        resolution.Item1 = resolutionList[i + 1].Item1;
                        resolution.Item2 = resolutionList[i + 1].Item2;
                        break;
                    }
                }
            }
            UpdateResolution();
        }
        private void OnClickFullScreenModeDown()
        {
            if (fullScreenMode == 1) fullScreenMode = 0;
            else if (fullScreenMode == 3) fullScreenMode = 1;
            UpdateFullScreenMode();
        }
        private void OnClickFullScreenModeUp()
        {
            if (fullScreenMode == 0) fullScreenMode = 1;
            else if (fullScreenMode == 1) fullScreenMode = 3;
            UpdateFullScreenMode();
        }
        private void OnClickFramerateDown()
        {
            for (int i = 0; i < framerateList.Count; ++i)
            {
                if (framerate == framerateList[i])
                {
                    framerate = framerateList[i - 1];
                    break;
                }
            }
            UpdateFramerate();
        }
        private void OnClickFramerateUp()
        {
            for (int i = 0; i < framerateList.Count; ++i)
            {
                if (framerate == framerateList[i])
                {
                    framerate = framerateList[i + 1];
                    break;
                }
            }
            UpdateFramerate();
        }
        
       
        private void UpdateResolution()
        {
            resolutionText.text = resolution.Item1 + " x " + resolution.Item2;
            resolutionButtonDown.interactable = resolutionList[0].Item1 < resolution.Item1;
            resolutionButtonUp.interactable = resolution.Item1 < resolutionList[resolutionList.Count - 1].Item1;
        }
        private void UpdateFullScreenMode()
        {
            switch (fullScreenMode)
            {
                case 0:
                    fullScreenModeText.text = "전체 화면";
                    break;
                case 1:
                    fullScreenModeText.text = "전체 창모드";
                    break;
                case 3:
                    fullScreenModeText.text = "창모드";
                    break;
                default:
                    fullScreenModeText.text = "Error";
                    break;
            }
            fullScreenModeButtonDown.interactable = fullScreenMode != 0;
            fullScreenModeButtonUp.interactable = fullScreenMode != 2;
        }
        private void UpdateFramerate()
        {
            switch (framerate)
            {
                case 0:
                    framerateText.text = "무한";
                    break;
                default:
                    framerateText.text = framerate + " Hz";
                    break;
            }

            framerateButtonDown.interactable = framerate != framerateList[0];
            framerateButtonUp.interactable = framerate != framerateList[framerateList.Count - 1];
        }

        private void InitOptionItem(Transform itemObj, out TMP_Text valueText, out Button DownBtn, out Button UpBtn, UnityAction OnClickDownListener, UnityAction OnClickUpListener)
        {
            valueText = itemObj.Find("TMP_Value").GetComponent<TMP_Text>();
            DownBtn = itemObj.Find("Btn_Down").GetComponent<Button>();
            UpBtn = itemObj.Find("Btn_Up").GetComponent<Button>();

            DownBtn.onClick.AddListener(OnClickDownListener);
            UpBtn.onClick.AddListener(OnClickUpListener);
        }

        private bool CheckGraphicSettingChange()    //옵션을 변경한게 있는지 체크
        {
            return PreferenceData.ResolutionWidth != resolution.Item1 ||
            PreferenceData.ResolutionHeight != resolution.Item2 ||
            PreferenceData.FullScreenMode != fullScreenMode ||
            PreferenceData.Framerate != framerate;
        }
    }


