using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using TMPro;
using System;
using System.Linq;

namespace Week9
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textDisplay;
        [SerializeField] TMP_Dropdown searchDropdown;
        int imagesDownloaded;
        int totalPokemon;

        [SerializeField] Image imgPrefab;
        [SerializeField] Transform storeImages;
        List<Image> listOfImages = new();

        private void Start()
        {
            searchDropdown.onValueChanged.AddListener(SearchType);
            StartCoroutine(ReadTypeURL($"https://pokeapi.co/api/v2/type/normal"));
        }

        void SearchType(int n)
        {
            StartCoroutine(ReadTypeURL($"https://pokeapi.co/api/v2/type/{searchDropdown.options[n].text.ToLower()}"));
        }

        IEnumerator ReadTypeURL(string url)
        {
            foreach (Image image in listOfImages)
                Destroy(image.gameObject);
            listOfImages.Clear();

            WWW www = new WWW(url);
            yield return www;
            if (www.error == null)
            {
                JSONNode json = JSON.Parse(www.text);
                imagesDownloaded = 0;
                totalPokemon = json["pokemon"].Count;

                for (int i = 0; i < json["pokemon"].Count; i++)
                {
                    listOfImages.Add(Instantiate(imgPrefab, storeImages.transform));
                    listOfImages[^1].name = json["pokemon"][i][0]["name"];
                    StartCoroutine(ReadPokemonURL(json["pokemon"][i][0]["url"], i));
                }
            }
            else
            {
                Debug.LogError($"ERROR: {www.error}");
            }
        }

        IEnumerator ReadPokemonURL(string url, int index)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.error == null)
            {
                JSONNode json = JSON.Parse(www.text);
                StartCoroutine(DownloadImage(json["sprites"]["other"]["home"]["front_default"], index));
            }
            else
            {
                Debug.LogError($"ERROR: {www.error}");
            }
        }

        IEnumerator DownloadImage(string imageURL, int index)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageURL);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"{index}: {imageURL}");
                Debug.LogError(request.error);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                listOfImages[index].color = Color.white;
                listOfImages[index].sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

                imagesDownloaded++;
                textDisplay.text = $"Images: {imagesDownloaded}/{totalPokemon}";
            }
        }
    }
}