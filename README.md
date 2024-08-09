# 2DURP GodRays

## Overview
**2DURP GodRays** is a post-processing shader designed to add a God Rays effect, particularly in 2D scenes. This effect simulates light scattering through a medium, creating visually striking rays of light that enhance the atmosphere of your game.

[![Watch the video](./Images%20and%20Videos/Movie_002.mp4)](./Images%20and%20Videos/Movie_002.mp4)

## How to Use

### 1. Download and Import
- Download the files from this repository and add them to your Unity project.

### 2. Add GodRays Render Feature
- Add the GodRays render feature to your 2D Renderer.
- Set the reference to the GodRays material (which can be created using the `godrays.shader` file).

### 3. Set Up the Camera
- Add the `GodRaysCamera2D` script to your main camera.

### 4. Create a Light Source
- Create an empty GameObject and add a `SpriteRenderer` component.
- Choose a sprite (it can be anything, but simple shapes like squares or circles are preferred by the developer).

### 5. Add a Global Volume
- Add a Global Volume to your scene and apply the GodRays post-processing effect.

### 6. Run the Scene
- Play the scene to see the God Rays effect in action!

## Post-Processing Settings

### Light
- **Exposure**: Controls the brightness of the rays.
- **Density**: Controls the distance between two sample rays.
- **Weight**: Controls the intensity of the rays.
- **Decay**: Controls the fading of the rays over distance.

### Performance
- **Downscale Factor**: Controls the resolution of the sample image used by the GPU. A higher value means better performance but lower quality.
- **Samples**: The number of times the ray is calculated. More samples lead to higher quality but lower performance.
- **Filter Mode**: 
  - `0`: Point Filter (Pixelated)
  - `1`: Bilinear (Smoother)
  - `2`: Trilinear (Super Smooth)

## Images and Videos



Click the link above to watch a video demonstration of the 2DURP GodRays shader in action.

## Conclusion
That's it! You've successfully added the God Rays effect to your 2D scene. Enjoy the enhanced visuals!
