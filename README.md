# Xamarin.Forms.SlideView
*A sliding view component for Xamarin.Forms*

![SlideView](Doc/SlideView.gif)

## Usage

Import the SlideView assembly:
```xaml
xmlns:slideview="clr-namespace:Xamarin.Forms.SlideView;assembly=Xamarin.Forms.SlideView"
```

Place it in your view, and define contents for Pane0 (first pane), and Pane1 (pane that slides in):
```xaml
<slideview:SlideView x:Name="mySlideView">
    <slideview:SlideView.Pane0>
        <!-- Your controls... -->
    </slideview:SlideView.Pane0>
    
    <slideview:SlideView.Pane1>
        <!-- Your controls... -->
    </slideview:SlideView.Pane1>
</slideview:SlideView>
```

To toggle between the panes, set the PanePriority property on your SlideView to SlideView.SlideViewPanePriority.Pane0 or SlideView.SlideViewPanePriority.Pane1:
```c#
using Xamarin.Forms.SlideView;

// ...

this.mySlideView.PanePriority = SlideView.SlideViewPanePriority.Pane0; // displays first pane
this.mySlideView.PanePriority = SlideView.SlideViewPanePriority.Pane1; // displays second pane
```

See [SlideView.xaml.cs](Xamarin.Forms.SlideView/SlideView.xaml.cs) for all bindable properties.
