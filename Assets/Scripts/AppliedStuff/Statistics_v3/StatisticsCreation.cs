using GreatDevResh.Data.LocalHelpers;
using GreatDevResh.Data.Statistics_v3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GreatDevResh.AppliedStuff.Statistics_v3
{
    public class StatisticsCreation : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _titleIF;
        [SerializeField] private Toggle _titleToggle;
        [SerializeField] private StatisticsShower _statisticsShower;
        
        private int _mod; // group, activity or regime
        private int _type; // series, quantity, sum or average percent
        private int _period; // in total, for last year, etc.

        public void CleanValues()
        {
            _mod = 0;
            _type = 0;
            _period = 0;
            _titleIF.SetTextWithoutNotify("");
            SwitchTitle(false);
        }

        public void SetMod(int mod)
        {
            _mod = mod;
            _titleToggle.gameObject.SetActive(_mod == 0);
        }

        public void SetType(int type) => _type = type;
        public void SetPeriod(int period) => _period = period;
        
        public void SwitchTitle(bool isOff)
        {
            _titleIF.gameObject.SetActive(!isOff);
            _titleToggle.SetIsOnWithoutNotify(isOff);
        }

        public void CreateLine()
        {
            if (!CheckIfCorrectLine())
                return;
            StatisticsLine line;
            switch (_type)
            {
                case 1:
                    line = new StatisticsMaxSeries();
                    break;
                case 16:
                    line = new StatisticsNumber();
                    break;
                case 17:
                    line = new StatisticsPercent();
                    break;
                default:
                    line = new StatisticsColorBlocks();
                    break;
            }

            line.type = _type;
            line.period = _period;
            PrecalculatedData.AddLine(!_titleToggle.isOn ? Helpers.ProcessTitle(_titleIF.text) : "", _mod, line);
            _statisticsShower.UpdateList();
        }

        private bool CheckIfCorrectLine()
        {
            if (!_titleToggle.isOn && Helpers.ProcessTitle(_titleIF.text) == "")
                return false;
            // maybe more conditions
            return true;
        }
    }
}