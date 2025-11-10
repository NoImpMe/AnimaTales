using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

    public static class PreferenceData
    {
        class Event : UnityEvent { }

        //그래픽 설정 값
        private static int resolutionWidth;       //해상도 너비
        private static int resolutionHeight;      //해상도 높이
        private static int fullScreenMode;        //전체 화면               //0: 전체화면, 1: 전체 창모드, 3: 윈도우 (2도 전체 창모드)
        private static int framerate;             //주사율
        

        //사운드 설정 값
        private static int masterVolume;
        private static int bgmVolume;
        private static int sfxVolume;
        static Event BgmVolumeChangeEvent = new Event();
        public static void AddListenerBgmVolumeChangeEvent(UnityAction callback) { BgmVolumeChangeEvent.RemoveListener(callback); BgmVolumeChangeEvent.AddListener(callback); }
        public static void RemoveListenerBgmVolumeChangeEvent(UnityAction callback) { BgmVolumeChangeEvent.RemoveListener(callback); }
        public static void InvokeBgmVolumeChangeEvent() { BgmVolumeChangeEvent.Invoke(); }
        static Event SfxVolumeChangeEvent = new Event();
        public static void AddListenerSfxVolumeChangeEvent(UnityAction callback) { SfxVolumeChangeEvent.RemoveListener(callback); SfxVolumeChangeEvent.AddListener(callback); }
        public static void RemoveListenerSfxVolumeChangeEvent(UnityAction callback) { SfxVolumeChangeEvent.RemoveListener(callback); }
        public static void InvokeSfxVolumeChangeEvent() { SfxVolumeChangeEvent.Invoke(); }
        //게임플레이 설정 값
        private static int mouseSensitivity;      //마우스 감도

        //단축키 설정 값

        #region get/set
        public static int ResolutionWidth
        {
            get { return resolutionWidth; }
            set { resolutionWidth = value; PlayerPrefs.SetInt(GetMemberName(() => resolutionWidth), value); }
        }
        public static int ResolutionHeight
        {
            get { return resolutionHeight; }
            set { resolutionHeight = value; PlayerPrefs.SetInt(GetMemberName(() => resolutionHeight), value); }
        }
        public static int FullScreenMode
        {
            get { return fullScreenMode; }
            set { fullScreenMode = value; PlayerPrefs.SetInt(GetMemberName(() => fullScreenMode), value); }
        }
        public static int Framerate
        {
            get { return framerate; }
            set { framerate = value; PlayerPrefs.SetInt(GetMemberName(() => framerate), value); }
        }
        

        public static int MasterVolume
        {
            get { return masterVolume; }
            set
            {
                masterVolume = value; PlayerPrefs.SetInt(GetMemberName(() => masterVolume), value);
                InvokeBgmVolumeChangeEvent(); InvokeSfxVolumeChangeEvent();
            }
        }
        public static int BgmVolume
        {
            get { return bgmVolume; }
            set
            {
                bgmVolume = value; PlayerPrefs.SetInt(GetMemberName(() => bgmVolume), value);
                InvokeBgmVolumeChangeEvent();
            }
        }
        public static int SfxVolume
        {
            get { return sfxVolume; }
            set
            {
                sfxVolume = value; PlayerPrefs.SetInt(GetMemberName(() => sfxVolume), value);
                InvokeSfxVolumeChangeEvent();
            }
        }
        #endregion

        static PreferenceData()     //초기화. PlayerPrefs 내 값을 변수에 할당.
        {

            resolutionWidth = PlayerPrefs.GetInt(GetMemberName(() => resolutionWidth), Screen.currentResolution.width);
            resolutionHeight = PlayerPrefs.GetInt(GetMemberName(() => resolutionHeight), Screen.currentResolution.height);
            fullScreenMode = PlayerPrefs.GetInt(GetMemberName(() => fullScreenMode), 0);

            framerate = PlayerPrefs.GetInt(GetMemberName(() => framerate), 60);
           
            masterVolume = PlayerPrefs.GetInt(GetMemberName(() => masterVolume), 100);
            bgmVolume = PlayerPrefs.GetInt(GetMemberName(() => bgmVolume), 100);
            sfxVolume = PlayerPrefs.GetInt(GetMemberName(() => sfxVolume), 100);

            mouseSensitivity = PlayerPrefs.GetInt(GetMemberName(() => mouseSensitivity), 20);
        }

        private static string GetMemberName<T>(Expression<Func<T>> memberExpression)    //변수명을 string으로 리턴해주는 함수. 변수명을 그대로 key로 쓰기 위함. 
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        public static void ApplyGraphicOptionSetting()
        {
            Screen.SetResolution(ResolutionWidth, ResolutionHeight, (FullScreenMode)FullScreenMode);
            Application.targetFrameRate = Framerate;
        }
    }
