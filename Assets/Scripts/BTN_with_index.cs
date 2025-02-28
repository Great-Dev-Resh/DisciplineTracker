using UnityEngine;

namespace GreatDevResh
{
    public class BTN_with_index : MonoBehaviour // LEGACY
    {
        private int index;
        private int smallChange, bigChange;
        
        public delegate void VoidDelegate(int index, int value);

        private VoidDelegate what;

        public void Init(int _index, VoidDelegate _what)
        {
            index = _index;
            what = _what;
        }

        public void SetChanges(int small, int big)
        {
            smallChange = small;
            bigChange = big;
        }

        public void ChangeValue(int value) => what(index, value);
        public void SmallChange() => what(index, smallChange);
        public void BigChange() => what(index, bigChange);
    }
}