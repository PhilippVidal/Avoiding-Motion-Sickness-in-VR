# Avoiding motion sickness in interactive VR environments:
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

<br />
<br />

![Tunneling](https://user-images.githubusercontent.com/56507722/224543769-3b35e843-169b-48ba-bdd5-d7ba4e07eb99.png)
![Caging](https://user-images.githubusercontent.com/56507722/224543778-144b003e-6423-42ed-a2ac-249a48eefde7.png)
![Schutzhelm1](https://user-images.githubusercontent.com/56507722/224543894-80725fe4-ad77-4fa6-9f07-26648690dd7d.png)
![Schutzhelm2](https://user-images.githubusercontent.com/56507722/224543716-63dfde27-80ee-48f6-afc9-d89b6fda8762.png)
![MenuRaum](https://user-images.githubusercontent.com/56507722/224543666-6b008f83-73f2-4275-a887-20a7c3048015.png)
![Klettern1](https://user-images.githubusercontent.com/56507722/224543673-3671f494-af6e-462d-8894-46ad56522459.png)
![Klettern2](https://user-images.githubusercontent.com/56507722/224543679-2f76a317-5362-4ad0-896c-b44d10b0887a.png)
![RollercoasterRaum2](https://user-images.githubusercontent.com/56507722/224543695-400966b7-8e40-4ae2-bb1a-0646263d6e5b.png)
![RotatingPlatformsRaum](https://user-images.githubusercontent.com/56507722/224543700-203ff285-499a-443e-beaf-8538cdaa3e3b.png)
![ShootingPlatformRaum](https://user-images.githubusercontent.com/56507722/224543728-0c54a524-7a05-45c5-9075-e484b5e311af.png)
![ShootingPlatformRaum2](https://user-images.githubusercontent.com/56507722/224543821-d0dea572-1a2c-4be5-b95f-a60834ed94d0.png)
![SphereDodgingRaum1](https://user-images.githubusercontent.com/56507722/224543735-fc2bfa55-b500-4bb1-8feb-7f739fac62cd.png)
![SphereDodgingRaum2](https://user-images.githubusercontent.com/56507722/224543737-121f40ec-8b0d-48f6-b624-7743dbfcfcec.png)
![BasicLocomotionRaum](https://user-images.githubusercontent.com/56507722/224543927-037e788f-8e14-4972-a5de-2bf469f2b5e4.png)
![BasicLocomotionRaum2](https://user-images.githubusercontent.com/56507722/224543930-035992c3-7d3f-4a66-bbb8-9ec4f0f0e2aa.png)
![DesignEffectsRaum](https://user-images.githubusercontent.com/56507722/224543800-06bac054-43d0-4de3-9cbb-6c51a47decb7.png)
![DesignEffectsTreppen](https://user-images.githubusercontent.com/56507722/224543809-b102e824-f517-4c21-b79c-bd21b7a174c8.png)
![DesignEffectsVectionTunnel](https://user-images.githubusercontent.com/56507722/224543811-dd2392a4-a9c3-4a53-bbe6-d28afe33fe3f.png)
![Virtualizer](https://user-images.githubusercontent.com/56507722/224543829-a8c39ece-5bf5-4886-93a3-d54286a1e63a.jpg)


