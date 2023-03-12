# Avoiding-Motion-Sickness-in-VR
This game was developed in conjunction with my master's thesis at the University of Applied Sciences in Kempten, Germany. 
It's main focus is centered around the topic of Motion Sickness and the important question of how to avoid the occurence of this malady. 

Currently, Motion Sickness is one of the main obstacles that stand in the way of widespread adoption of virtual reality and its technology. 
While there are well established gameplay features that are used in modern VR games to reduce motion sickness (e.g., FOV reduction),
it is not uncommon to see gameplay elements copied from regular monitor-based games that (in VR) facilitate the development of motion sickness symptoms. 

The game allows for meaningful comparisions between different methods and design choices that can be made to reduce or outright avoid motion sickness entirely during a single game session.
To this extent, targeted provokation of motion sickness symptoms in specific situations is also a big part of the game. 

In order to prevent motion sickness a number of generalized methods have been implemented into the game. Some examples for these methods are: 
- __FOV reduction:__ 
Reducing the user's field of view based on their current in-game locomotion using shaders.
- __Rest Frames:__
Displaying a stationary, independent scene in the user's periphery or giving them a static reference in their field of view (e.g., a hat that is visible at the top of their screen).
- __Less provocative input methods:__
Users are able to use an omnidirectional VR treadmill (more specifically [Cyberith's](https://www.cyberith.com/) Virtualizer) to traverse the virtual world in a more natural way that is closer to real movement.
- __Alternative locomotion modes:__
Users can enable teleportation and rotation snapping as those are preferable compared to continous locomotion. Additionally, removing acceleration from movement reduces the perceptible conflict between the real and virtual movements. 

The experience itself is split into multiple levels, with the player starting in a main menu level that allows configuration of a wide variety of settings.
Each of the levels has its own purpose and follows an overarching theme. 
Broadly speaking, the focus of these levels can be put into three different categories: 

- __Locomotion and input devices:__
These levels contain gameplay elements that evaluate the user's ability to move confidently and effectively using a given input device or locomotion method (e.g., moving platforms and obstacles that have to be avoided).

- __Passive and artifical movement:__
Levels in this category limit the control a user has over their own movement to a varying degree. Two examples for this are a rollercoaster and a vehicle on a racetrack. 
While the rollercoaster completely removes the control of the user, the vehicle allows for direct control over the majority of movements, though there are still situations that lead to passive movement (e.g., crashing the vehicle).

- __Design choices and gameplay features:__
Essentially a collection of smaller features and design elements that should be taken into account during development of a VR game. 
A few examples of such features are alternative ways of traversing the game world (e.g., climbing and falling), the effect of certain visual patterns that can induce vection and the importance of high framerates & error-free tracking.
