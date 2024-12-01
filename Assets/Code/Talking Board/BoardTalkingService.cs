using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Code
{
    public class BoardTalkingService : MonoBehaviour
    {
        [SerializeField] private Game game;

        [SerializeField] private TMP_Text hintButtonOuijiEquip;
        [SerializeField] private TMP_Text textUI;
        [SerializeField] private Transform letterHighlighter;
        [SerializeField] private List<Transform> letterPositions;

        [SerializeField] private Ease ease;
        [SerializeField] private float timeSpendOnLetter;
        [SerializeField] private float timeToMoveToNewLetter;
        [SerializeField] private float moveDistanceDelta;

        public event Action BoardCanTalk;

        private string text;
        private string textToDisplay;

        private bool isNextFrame;
        private bool haveText;

        public void Update()
        {
            isNextFrame = true;

            if (Input.GetKeyDown(KeyCode.F))
            {
                StartDisplayingText(textToDisplay);
            }
        }

        public void ActivateTrigger(string textToSHow)
        {
            textToDisplay = textToSHow;
            hintButtonOuijiEquip.color = Color.yellow;
            haveText = true;
            BoardCanTalk?.Invoke();
        }

        [Button]
        public void StartDisplayingText(string _textToDisplay)
        {
            textUI.text = "";

            letterHighlighter.DOKill();
            StopAllCoroutines();
            StartCoroutine(DisplayText(_textToDisplay));
            //SetTextToDisplay();
        }

        private IEnumerator DisplayText(string _text)
        {

            if (_text.Length > 0)
            {
                for (int i = 0; i < _text.Length; i++)
                {
                    var positionToMove = GetLetterPosition(_text[i]);

                    float moveTime = timeToMoveToNewLetter;

                    letterHighlighter.DOLocalMove(positionToMove, moveTime).SetEase(ease);
                    yield return new WaitForSeconds(moveTime);

                    //while (moveTime > 0)
                    //{
                    //    //letterHighlighter.localPosition = Vector3.MoveTowards(letterHighlighter.localPosition, positionToMove, moveDistanceDelta * Time.deltaTime);
                    //    moveTime -= Time.deltaTime;
                    //    isNextFrame = false;
                    //
                    //    yield return new WaitUntil(() => isNextFrame == true);
                    //}

                    textUI.text = textUI.text + _text[i];
                    yield return new WaitForSeconds(timeSpendOnLetter);
                }
            }

            haveText = false;
            hintButtonOuijiEquip.color = Color.white;
            textToDisplay = "I have nothing to tell you";
        }

        private Vector3 GetLetterPosition(char letter)
        {
            switch (letter)
            {
                case ' ':
                    return new Vector3(0, -0.0002f, 0.0008f);

                case 'a':
                case 'A':
                    return letterPositions[0].localPosition;

                case 'b':
                case 'B':
                    return letterPositions[1].localPosition;

                case 'c':
                case 'C':
                    return letterPositions[2].localPosition;

                case 'd':
                case 'D':
                    return letterPositions[3].localPosition;

                case 'e':
                case 'E':
                    return letterPositions[4].localPosition;

                case 'f':
                case 'F':
                    return letterPositions[5].localPosition;

                case 'g':
                case 'G':
                    return letterPositions[6].localPosition;

                case 'h':
                case 'H':
                    return letterPositions[7].localPosition;

                case 'i':
                case 'I':
                    return letterPositions[8].localPosition;

                case 'j':
                case 'J':
                    return letterPositions[9].localPosition;

                case 'k':
                case 'K':
                    return letterPositions[10].localPosition;

                case 'l':
                case 'L':
                    return letterPositions[11].localPosition;

                case 'm':
                case 'M':
                    return letterPositions[12].localPosition;

                case 'n':
                case 'N':
                    return letterPositions[13].localPosition;

                case 'o':
                case 'O':
                    return letterPositions[14].localPosition;

                case 'p':
                case 'P':
                    return letterPositions[15].localPosition;

                case 'q':
                case 'Q':
                    return letterPositions[16].localPosition;

                case 'r':
                case 'R':
                    return letterPositions[17].localPosition;

                case 's':
                case 'S':
                    return letterPositions[18].localPosition;

                case 't':
                case 'T':
                    return letterPositions[19].localPosition;

                case 'u':
                case 'U':
                    return letterPositions[20].localPosition;

                case 'v':
                case 'V':
                    return letterPositions[21].localPosition;

                case 'w':
                case 'W':
                    return letterPositions[22].localPosition;

                case 'x':
                case 'X':
                    return letterPositions[23].localPosition;

                case 'y':
                case 'Y':
                    return letterPositions[24].localPosition;

                case 'z':
                case 'Z':
                    return letterPositions[25].localPosition;

                default:
                    throw new ArgumentException("Invalid input letter");
            }
        }
    }
}