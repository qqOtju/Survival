using UnityEngine;
using VInspector;

namespace Tools.MyGridLayout
{
    public class HealthLayout: AbstractGridLayout
    {
        [SerializeField] private int _columnsCount;
        [SerializeField] private Vector2 _spacing;
        [SerializeField] private Vector2 _horizontalPadding;
        [SerializeField] private Vector2 _verticalPadding;
        
        private Vector2 Spacing => _spacing / 1000;
        private Vector2 HorizontalPadding => _horizontalPadding / 1000;
        private Vector2 VerticalPadding => _verticalPadding / 1000;
        public void Align(int columnsCount) =>
            Align(1,columnsCount, Spacing, VerticalPadding, HorizontalPadding);
        
        [Button]
        public override void Align() =>
            Align(1,_columnsCount, Spacing, VerticalPadding, HorizontalPadding);
    }
}