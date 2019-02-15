# Parallaxing

## Summary

Uses maths to move items in a parallaxing manner while the camera moves

## To Use

Attach the ParallaxCamera script to the camera that will be the base to determine how parallaxing items move. Then attach the ParallaxItem to each item that will move in the parallaxing manner.

For each ParallaxItem, the position of it's gameObject can be used for the natural position of the item in game time, or the natural position can be manually inserted.

## Manual Updating

The camera will automatically trigger the items to move at Update. However, if you wish to manually trigger the parallaxing movement, then use the line
```
ParallaxCamera.Notify("Camera Moved");
```
The automatic parallaxing can be switched off in the camera's settings.

## Logarithmic Z Distances

If you have items that you wish to make move like they are far away from the camera, then the ParallaxCamera has a "Logarithmic Z Distances" option. When ticked, the z value in all the ParallaxItem's natural position value will be treated as an exponent, rather than a definitive value. So, for example, a Z value of 2 in an ParallaxItem's natural position will mean that the item will be treated as if it were 10<sup>2</sup> = 100 units away.