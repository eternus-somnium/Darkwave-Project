AI Only Files: 

  This package contains the Revenant Prefab, the EWalker Script, and the Enemy Script. 
		

- The prefab uses the EWalker and Enemy scripts to run, so to import the enemy, just drag the prefab and the scripts into the project. 
  Go into the inspector and make sure that the Revenant prefab has the EWalker script attached. 

- Now, to make the enemy able to move, it must act as a NavMesh Agent. Go to 'Add Component' at the bottom of the inspector for the prefab. 
  Then go into 'Navigation' and select 'NavMesh Agent'. 

- Finally, You must create the NavMesh. To do this, you will need to add the 'Navigation' window to your GUI. Go to 'Window' (in the top left bar) 
  and select 'Navigation' (close to the bottom). To create the NavMesh, you will need to declare the 'state' of each object you want the Agent to 
  walk on or avoid. Click on an object (a building, a wall, etc) and select "Navigation Static." Then, for 'Navigation Area', open the drop-down
  and choose the behavior you want. (Walls and buildings would be "Not Walkable") Then choose the surface you want the Agents to be able to walk on
  and choose "Walkable." As of now, I only know of ways to make agents avoid/walk on static objects, so each object in  the NavMesh must be Navigation
  Static. (All objects must have the "Navigation Static" box checked) Then, to Bake the NavMesh, go to 'Bake' (the tab close to the top of the 'Navigation' window)
  and select any final settings (agent height, radius, etc). Click on the Walkable surface (the plane the Agent will traverse) and click the "bake" button
  at the bottom right of the 'Navigation' window. The plane (or ground) will turn blue meaning that all is done!

***********************************************************************************************************************************************************************