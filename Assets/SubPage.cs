using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPage : MonoBehaviour
{
    [SerializeField] InpageSwiper _InPageSwiper = null;
    [SerializeField] GameObject RemotePageList = null;
    [SerializeField] GameObject FavouritePageList = null;
    public void RemoteSubpage()
    {
        if (PlayerPrefs.GetString("Remote" + 0, "") != "")
        {
            _InPageSwiper.NextButton();
            RemotePageList.SetActive(true);
            FavouritePageList.SetActive(false);
        }else{
            Toast.Instance.Show("No remotes found 😕",2f,Toast.ToastColor.Magenta);
        }
    }

    public void FavouritesSubpage()
    {
            _InPageSwiper.BackButton();
            RemotePageList.SetActive(false);
            FavouritePageList.SetActive(true);
    }
}
