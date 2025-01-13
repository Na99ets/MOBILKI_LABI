using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private readonly Dictionary<int, bool[]> digitToSegments = new Dictionary<int, bool[]>
        {
            { 0, new[] { true, true, true, false, true, true, true } },
            { 1, new[] { false, false, true, false, false, true, false } },
            { 2, new[] { true, false, true, true, true, false, true } },
            { 3, new[] { true, false, true, true, false, true, true } },
            { 4, new[] { false, true, true, true, false, true, false } },
            { 5, new[] { true, true, false, true, false, true, true } },
            { 6, new[] { true, true, false, true, true, true, true } },
            { 7, new[] { true, false, true, false, false, true, false } },
            { 8, new[] { true, true, true, true, true, true, true } },
            { 9, new[] { true, true, true, true, false, true, true } }
        };

        private readonly List<Grid> numbersGrids;

        [Obsolete]
        public MainPage()
        {
            InitializeComponent();

            numbersGrids = new List<Grid> { H1, H2, M1, M2, S1, S2 };

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                UpdateTime();
                return true;
            });
        }

        private void UpdateTime()
        {
            var now = DateTime.Now;
            var digits = new[]
            {
                now.Hour / 10,
                now.Hour % 10,
                now.Minute / 10,
                now.Minute % 10,
                now.Second / 10,
                now.Second % 10
            };

            for (int i = 0; i < numbersGrids.Count; i++)
            {
                UpdateDigit(numbersGrids[i], digits[i]);
            }
        }

        private void UpdateDigit(Grid grid, int digit)
        {
            if (!digitToSegments.TryGetValue(digit, out var segments))
                return;

            for (int i = 0; i < grid.Children.Count; i++)
            {
                if (grid.Children[i] is Rectangle rectangle)
                {
                    rectangle.Fill = segments[i] ? Colors.Green : Colors.Transparent;
                }
            }
        }
    }
}

