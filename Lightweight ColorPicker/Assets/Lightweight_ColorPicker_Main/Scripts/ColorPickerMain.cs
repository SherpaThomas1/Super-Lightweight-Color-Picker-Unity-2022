using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPickerMain : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    private RectTransform imageSize;

    [SerializeField]
    private Texture2D imageTexture;
    [SerializeField]
    private Color pickedColor;

    private Vector3 imagePos;
    private float[] mousePos = new float[2], varPos = new float[2];
    private int[] normPos = new int[2];

    public Transform cursor;
    public AudioSource pickerSound;

    //sets which object or objects you want to change colors
    private int ButtonINT;
    //color change gameobject and ui
    [SerializeField]
    private Material changeColorMat;
    public Image PanelImage;
    public Button uiButton;

    private void Awake()
    {
       imageSize = GetComponent<RectTransform>();
       imagePos = imageSize.position;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        varPos[0] = eventData.position.x;
        varPos[1] = eventData.position.y;

        ChangeColor(varPos);
    }
    public void OnDrag(PointerEventData eventData)
    {
        varPos[0] = eventData.position.x;
        varPos[1] = eventData.position.y;

        ChangeColor(varPos);
    }
    public void SetActive(int ButtonInt)
    {
        gameObject.SetActive(true);
        ButtonINT = ButtonInt;
        pickerSound.Play();
    }
    public void Deactivate()
    {
        pickerSound.Play();
        StartCoroutine(ButtonCountDown());
    }    
    private void ChangeColor(float[] mousePosition)
    {
        //setting image position
        imagePos = imageSize.position;

        //setting mouse position - image position for pixel extraction
        mousePos[0] = mousePosition[0] - imagePos.x;
        mousePos[1] = mousePosition[1] - imagePos.y;

        if (mousePos[0] >= 0 && mousePos[0] <= imageSize.rect.width && mousePos[1] >= 0 && mousePos[1] <= imageSize.rect.height)
        {
            //set cursor postion
            cursor.position = new Vector3(mousePosition[0], mousePosition[1], 0);

            //nomalize mouse position
            normPos[0] = (int)(mousePos[0] * (imageTexture.width / imageSize.rect.width));
            normPos[1] = (int)(mousePos[1] * (imageTexture.height / imageSize.rect.height));

            //pickedColor color
            pickedColor = imageTexture.GetPixel(normPos[0], normPos[1]);

            //Set Color
            switch (ButtonINT)
            {
                case 0:
                    changeColorMat.color = pickedColor;
                    break;
                case 1:
                    {
                        var alphaColor = pickedColor;
                        //change alpha if wanted
                        alphaColor.a = PanelImage.color.a;
                        PanelImage.color = alphaColor;
                        break;
                    }

                case 2:
                    {
                        ColorBlock cb = uiButton.colors;
                        cb.normalColor = pickedColor;
                        cb.selectedColor = pickedColor;

                        uiButton.colors = cb;
                        break;
                    }
            }
        }        
    }
    IEnumerator ButtonCountDown()
    {
        yield return new WaitForSeconds(.2f);
        gameObject.SetActive(false);
    }
}
