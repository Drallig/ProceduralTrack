Procedural Racetrack Generation - Computing Project 2019 - COM3051-N
Luke Gillard - Q5088329

To open the project, navigate to Assets/Scenes, and open the "AlternateTerrain" Unity Scene
In the scene, many parameters can be altered to produce different procedural results

In the Hierarchy, on the "MeshGenerator" object:-
The script "Mesh Generator" has several components, "Mesh Size X" and "Mesh Size Y" and "Tri Size" are not advised to be altered for best results
The "Scale" and "Bump Freq" numbers can be altered to change the result of the terrain generation, however too much deviation can produce unrealistic and bad results

The "NodeGrid" Object:-
The script "Node Grid", the "Gride Size" is a 1/10th scale of the eventual corner placements. this can be adjusted, but at default provides good/accurate track lengths
The "Corner Num" changes the number of corners randomly placed on the grid, a good amount is mid teens however as low as 9 and as high as 20 may provide accurate track generations
The "New Bool" does nothing, and it is advised the "Node Positions" array is not altered manually

The "Track Mesh" Object:- 
The "Track Generator" Script, the "Half Track Width" value is the value of half the width of the track. An accurate value would range from 5 - 8(multiplied by 2, 10 - 16)
The "New Ob" GameObject should not be altered

In order to construct a new generation, the scene must be played, and to reset the generation for a new one, the scene must be stopped and started.

Navigate the scene through the Scene window in unity once the simulation is running to inspect the simulation