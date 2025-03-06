using System.Collections.Generic;
using GreatDevResh.Data.Classes;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.AllDays
{
    public class HorizontalWeekManager : MonoBehaviour
    {
        [SerializeField] private RectTransform weekPrefab;

        [SerializeField] private MyScrollRect scrollRect;
        [SerializeField] private RectTransform viewPortTransform;
        [SerializeField] private RectTransform contentTransform;
        [SerializeField] private VerticalLayoutGroup VerticalLayoutContentGroup;
        
        private List<HorizontalWeek> weekElements;

        private Vector2 OldVelocity = Vector2.zero;
        private bool isContentPositionUpdated = false;

        private MyDate DateOfFirstWeek;
        private int dayOfFirstWeek;
        private MyDate DateOfLastWeek; // not include last week
        private int dayOfLastWeek;

        private bool sortingDefaultValue = true;
        private List<string> sortingGroupsElse;

        private const int JumpAmount = 4;

        private bool WeeksWereCreated = false;
        
        private void Update()
        {
            ActiveAndInactiveWeeks();
        }

        private void ActiveAndInactiveWeeks()
        {
            if (isContentPositionUpdated)
            {
                isContentPositionUpdated = false;
                scrollRect.velocity = OldVelocity;
            }

            float height = (weekPrefab.rect.height + VerticalLayoutContentGroup.spacing) * JumpAmount;
            while (contentTransform.localPosition.y > height &&
                (DateOfLastWeek > GeneralData.startDate || DateOfLastWeek == GeneralData.startDate && dayOfLastWeek >= GeneralData.startDay))
            {
                Canvas.ForceUpdateCanvases();
                isContentPositionUpdated = true;
                contentTransform.localPosition -= new Vector3(0, height, 0);
                scrollRect.AddToContentPosition(new Vector2(0, -height));
                
                dayOfFirstWeek -= 7 * JumpAmount;
                while (dayOfFirstWeek < 1)
                {
                    DateOfFirstWeek.Month--;
                    dayOfFirstWeek += DateOfFirstWeek.AmountOfDays();
                }
                dayOfLastWeek -= 7 * JumpAmount;
                while (dayOfLastWeek < 1)
                {
                    DateOfLastWeek.Month--;
                    dayOfLastWeek += DateOfLastWeek.AmountOfDays();
                }
                
                RecalculateNumbers();
            }
            while (contentTransform.localPosition.y < 0 && DateController.IsItWasEarlier(DateOfFirstWeek, dayOfFirstWeek))
            {
                Canvas.ForceUpdateCanvases();
                isContentPositionUpdated = true;
                contentTransform.localPosition += new Vector3(0, height, 0);
                scrollRect.AddToContentPosition(new Vector2(0, height));
                
                dayOfFirstWeek += 7 * JumpAmount;
                while (dayOfFirstWeek > DateOfFirstWeek.AmountOfDays())
                {
                    dayOfFirstWeek -= DateOfFirstWeek.AmountOfDays();
                    DateOfFirstWeek.Month++;
                }
                dayOfLastWeek += 7 * JumpAmount;
                while (dayOfLastWeek > DateOfLastWeek.AmountOfDays())
                {
                    dayOfLastWeek -= DateOfLastWeek.AmountOfDays();
                    DateOfLastWeek.Month++;
                }
                
                RecalculateNumbers();
            }

            OldVelocity = scrollRect.velocity;
        }
        
        public void RecalculateNumbers()
        {
            if (weekElements is null)
                return;
            for (int i = 0; i < weekElements.Count; ++i)
            {
                weekElements[i].UpdateDays(new MyDate(ref DateOfFirstWeek), dayOfFirstWeek - (weekElements.Count - i - 1) * 7,
                    in sortingDefaultValue, in sortingGroupsElse);
            }
        }

        public void SetNewSorting(in bool defaultValue, in List<string> notDefault)
        {
            sortingDefaultValue = defaultValue;
            sortingGroupsElse = notDefault;
            
            if (gameObject.activeInHierarchy)
                RecalculateNumbers();
        }
        
        private void Start()
        {
            int itemsToAdd = 6 +
                             Mathf.CeilToInt(viewPortTransform.rect.height / (weekPrefab.rect.height + VerticalLayoutContentGroup.spacing));

            weekElements = new List<HorizontalWeek>();

            for (int i = 0; i < itemsToAdd; ++i)
            {
                RectTransform RT = Instantiate(weekPrefab, contentTransform);
                RT.SetAsFirstSibling();
                weekElements.Add(RT.GetComponent<HorizontalWeek>());
            }

            CalculateDateOfLastWeek();
            RecalculateNumbers();

            contentTransform.localPosition = new Vector3(contentTransform.localPosition.x,
                (weekPrefab.rect.height + VerticalLayoutContentGroup.spacing) * (-itemsToAdd - 2),
                 contentTransform.localPosition.z);

            WeeksWereCreated = true;
        }
        
        private void Awake()
        {
            Info.Init();
            
            DateOfFirstWeek = new MyDate(ref Info.now);
            dayOfFirstWeek = DateController.Day;
            int dayOfWeek = DateController.DayOfWeek <= 0 ? 7 : DateController.DayOfWeek;
            dayOfFirstWeek -= dayOfWeek - 8;

            if (sortingGroupsElse is null)
                sortingGroupsElse = new List<string>();

            if (dayOfFirstWeek < 1)
            {
                DateOfFirstWeek.Month--;
                dayOfFirstWeek += DateOfFirstWeek.AmountOfDays();
            }
            if (dayOfFirstWeek > DateOfFirstWeek.AmountOfDays())
            {
                dayOfFirstWeek -= DateOfFirstWeek.AmountOfDays();
                DateOfFirstWeek.Month++;
            }

            // Init();
        }
        
        private void CalculateDateOfLastWeek()
        {
            dayOfLastWeek = dayOfFirstWeek - 7 * weekElements.Count;
            DateOfLastWeek = new MyDate(ref DateOfFirstWeek);
            while (dayOfLastWeek < 1)
            {
                DateOfLastWeek.Month--;
                dayOfLastWeek += DateOfLastWeek.AmountOfDays();
            }
        }
        
        private void OnEnable()
        {
            if (WeeksWereCreated)
                RecalculateNumbers();
        }
    }
}