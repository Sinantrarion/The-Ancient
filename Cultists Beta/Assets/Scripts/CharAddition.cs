using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharAddition : MonoBehaviour
{
    public List<GameObject> AllPossibleUnits;
    public List<bool> IfTaken;
    public List<GameObject> Places;
    public List<GameObject> Units;
    public List<GameObject> UnitsForTest;

    public List<int> Empty;
    public List<string> Tooltip;
    public List<Sprite> ImageForTooltip;

    public GameObject CharacterPicture;
    public GameObject Texter;
    public Text Texting;

    public Button Commence;

    private void Start()
    {
        foreach(GameObject unit in AllPossibleUnits)
        {
            IfTaken.Add(false);
        }
        CharacterPicture.SetActive(false);
    }

    public void AddCharacter(int number)
    {
        if (IfTaken[number] == false)
        {
            if (Units.Count < 3)
            {
                IfTaken[number] = true;
                Units.Add(AllPossibleUnits[number]);
                Empty.Add(number);
                GameObject Hero = Instantiate(AllPossibleUnits[number], Places[Units.Count - 1].transform.position, Quaternion.identity);
                UnitsForTest.Add(Hero);
            }
        }
    }

    public void RemoveCharacter()
    {
        if (Units.Count > 0)
        {
            IfTaken[Empty[Empty.Count - 1]] = false;
            Empty.RemoveAt(Empty.Count - 1);
            Destroy(UnitsForTest[UnitsForTest.Count - 1]);
            UnitsForTest.RemoveAt(UnitsForTest.Count - 1);
            Units.RemoveAt(Units.Count - 1);
        }
    }

    public void Update()
    {
        if(Units.Count == 3)
        {
            Commence.interactable = true;
        }
        else
        {
            Commence.interactable = false;
        }
    }

    public void TooltipOn(int CharacterNumber)
    {
        CharacterPicture.SetActive(true);
        Texting.GetComponent<Text>().text = Tooltip[CharacterNumber];
        CharacterPicture.GetComponent<Image>().sprite = ImageForTooltip[CharacterNumber];
    }

    public void TooltipOff()
    {
        CharacterPicture.SetActive(false);
        Texting.GetComponent<Text>().text = "";

    }

    public void NextLevel()
    {
        PassData.Instance.Send(Units);
        SceneManager.LoadScene(2);
    }
}
