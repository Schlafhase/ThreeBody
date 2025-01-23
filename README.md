# Three-Body Fractal
Every Pixel of the image is assigned a configuration of three bodies (each with position, velocity and mass). This configuration is calculated as follows: The configuration at pixel 0, 0 is a stable configuration where all bodies are placed at regular intervals on a circle with a radius of 100. The velocities are vectors with a magnitude of 10 pi, pointing tangentially away from the circle. For each pixel, the configuration is changed by shifting the position of the first body by the coordinate of the pixel.

Then a simulation is run for each pixel, calculating the gravitational forces between the bodies and adjusting the velocities and positions accordingly. How long and how accurately this simulation is simulated can be configured.

To get from the simulation to the colour of the pixel, I measure the distance of each body to its starting position. Since I then have three distances, I can generate a colour from them using the RGB colour format by simply putting each of the distances into one of the colour channels.

To try out the program, click [here](https://threebody.azurewebsites.net/sandbox?lang=en). You can click on the image to see the simulation at the position of the image.

I have tried other methods for calculating the colours as well. However, these are much more expensive to calculate and can therefore only be executed in a desktop application. You can download this application [here](https://github.com/Schlafhase/ThreeBody/releases/tag/v1.0.0).

## Example
Here is a pretty configuration I have found:
| Body | X | Y | X Velocity | Y Velocity | Mass |
| --- | --- | --- | --- | --- | --- |
| 0 | 0 | 100 | 15 | 0 | 100 |
| 1 | 0 | -100 | -7 | 7 | 100 |
| 2 | 0 | 0 | -8 | -8 | 100 |

Rendering the fractal using that configuration with 20 complete timesteps and 0.01 timesteps each iterations results in this:
![Result](https://github.com/Schlafhase/ThreeBody/blob/master/ThreeBodySandbox/wwwroot/fractal-distance-hd.png)
