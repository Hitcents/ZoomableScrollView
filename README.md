# ZoomableScrollView
Test repo for a Xamarin.Forms zooming scroll view

## What we are looking for:
- A `ScrollView` with `MinimumZoom` and `MaximumZoom`
- It can default its starting zoom level to 1
- Should function exactly like the photos app when you look at a photo and can zoom in/out
- Should have parity on iOS/Android

Example:
```xml
<?xml version="1.0" encoding="UTF-8"?>
<ContentPage 
  xmlns="http://xamarin.com/schemas/2014/forms" 
  xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
  xmlns:local="clr-namespace:ZoomableScrollView" 
  x:Class="ZoomableScrollView.BoxScrollPage">
  <local:ZoomScrollView Orientation="Both" MaximumZoom="2" MinimumZoom=".5">
    <BoxView WidthRequest="200" HeightRequest="200" BackgroundColor="Red" HorizontalOptions="Center" VerticalOptions="Center"/>
  </local:ZoomScrollView>  
</ContentPage>
```

## Where it is right now

iOS
- Content larger than screen - just needs to center as you zoom out
- Content smaller than screen - does not seem to have the correct `ContentSize` at all, odd things happen

Android
- Both examples seem about the same
- When content is zoomed out, the ScrollView's `ContentSize` does not seem to be updated with the smaller scaled size. You can scroll and see empty space around the edges.
- When zooming, it does not center the Transform at the location of your pinch. You can compare to iOS (which works), when you zoom in/out at the top left of the screen.
