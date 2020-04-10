using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.SlideView
{
    public partial class SlideView : ContentView
    {
        public enum OrientationMode
        {
            Horizontal,
            //Vertical // TODO if needed
        }

        public enum SlideViewPanePriority
        {
            Pane0,
            Pane1
        }

        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
            nameof(Orientation), typeof(OrientationMode), typeof(SlideView), OrientationMode.Horizontal, propertyChanged: OnOrientationPropertyChanged);

        public static readonly BindableProperty PanePriorityProperty = BindableProperty.Create(
            nameof(PanePriority), typeof(SlideViewPanePriority), typeof(SlideView), SlideViewPanePriority.Pane0, propertyChanged: OnPanePriorityPropertyChanged);

        public static readonly BindableProperty Pane0Property = BindableProperty.Create(
            nameof(Pane0), typeof(View), typeof(SlideView), null, propertyChanged: OnPane0PropertyChanged);

        public static readonly BindableProperty Pane1Property = BindableProperty.Create(
            nameof(Pane1), typeof(View), typeof(SlideView), null, propertyChanged: OnPane1PropertyChanged);

        public static readonly BindableProperty AnimateTransitionProperty = BindableProperty.Create(
            nameof(AnimateTransition), typeof(bool), typeof(SlideView), true);

        public OrientationMode Orientation
        {
            get => (OrientationMode)GetValue(SlideView.OrientationProperty);
            set => SetValue(SlideView.OrientationProperty, value);
        }

        public SlideViewPanePriority PanePriority
        {
            get => (SlideViewPanePriority)GetValue(SlideView.PanePriorityProperty);
            set => SetValue(SlideView.PanePriorityProperty, value);
        }

        public View Pane0
        {
            get => (View)GetValue(SlideView.Pane0Property);
            set => SetValue(SlideView.Pane0Property, value);
        }

        public View Pane1
        {
            get => (View)GetValue(SlideView.Pane1Property);
            set => SetValue(SlideView.Pane1Property, value);
        }

        public bool AnimateTransition
        {
            get => (bool)GetValue(SlideView.AnimateTransitionProperty);
            set => SetValue(SlideView.AnimateTransitionProperty, value);
        }

        private static void OnPane0PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue)
                ((SlideView)bindable).AssignPane((View)newValue, SlideViewPanePriority.Pane0);
        }

        private static void OnPane1PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue)
                ((SlideView)bindable).AssignPane((View)newValue, SlideViewPanePriority.Pane1);
        }

        private static void OnOrientationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue)
                ((SlideView)bindable).DoLayout(true);
        }

        private static void OnPanePriorityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue)
                ((SlideView)bindable).DoLayout(false);
        }

        private bool isLayouting;

        public SlideView()
        {
            InitializeComponent();

            this.dualPaneGrid.SizeChanged += this.GridSizeChanged;
        }

        private void AssignPane(View pane, SlideViewPanePriority destination)
        {
            for (var i = 0; i < this.dualPaneGrid.Children.Count; i++) // don't enumerate as collection can get changed
            {
                var child = this.dualPaneGrid.Children[i];
                var column = Grid.GetColumn(child);
                if (destination == SlideViewPanePriority.Pane0 && column == 0 ||
                    destination == SlideViewPanePriority.Pane1 && column == 1)
                {
                    this.dualPaneGrid.Children.Remove(child);
                    i--;
                }
            }

            this.dualPaneGrid.Children.Add(pane, destination == SlideViewPanePriority.Pane0 ? 0 : 1, 0);

            this.DoLayout(true);
        }

        private async void DoLayout(bool ignoreAnimation)
        {
            if (!this.isLayouting &&
                (this.Pane0 != null && this.Pane1 != null &&
                this.dualPaneGrid.Width >= 0 && this.dualPaneGrid.Height >= 0))
            {
                this.isLayouting = true;

                var columnWidth = this.dualPaneGrid.Width / 2;
                var pane0TranslationX = this.PanePriority == SlideViewPanePriority.Pane0 ? 0 : -columnWidth * 0.2;
                var pane1TranslationX = this.PanePriority == SlideViewPanePriority.Pane1 ? -columnWidth : 0;

                if (!ignoreAnimation && this.AnimateTransition)
                {
                    this.Pane0.IsVisible = true;
                    this.Pane1.IsVisible = true;

                    var pane0TranslationTask = this.Pane0.TranslateTo(pane0TranslationX, this.dualPaneGrid.TranslationY, 250, Easing.CubicOut);
                    var pane1TranslationTask = this.Pane1.TranslateTo(pane1TranslationX, this.dualPaneGrid.TranslationY, 250, Easing.CubicInOut);
                    await Task.WhenAll(pane0TranslationTask, pane1TranslationTask);
                }
                else
                {
                    this.Pane0.TranslationX = pane0TranslationX;
                    this.Pane1.TranslationX = pane1TranslationX;
                }

                this.Pane0.IsVisible = this.PanePriority == SlideViewPanePriority.Pane0;
                this.Pane1.IsVisible = this.PanePriority == SlideViewPanePriority.Pane1;

                this.isLayouting = false;
            }
        }

        private void GridSizeChanged(object sender, System.EventArgs e)
        {
            if (!this.isLayouting)
                this.DoLayout(true);
        }
    }
}
