# Intel Realsense Hand Toolkit for Unity

![realsense.jpg](https://avatars3.githubusercontent.com/u/14095512?v=3&s=200)

Intel Realsense Hand Toolkit for Unity is toolkit for developing on hand tracking feature in Unity application with Intel Realsense camera easier.

## Latest Unity3D Package

[Download Unity Package](https://github.com/ReiiYuki/Intel-Realsense-Hand-Toolkit-Unity/releases/download/1.1.0/Intel-Realsense-Hand-Toolkit-v1.1.0.unitypackage)

## Requirement

```
Unity3D
Intel Realsense SDK
Intel Realsense Full Hand Tracking and Gesture Recognition SDK
F200/SR300/R200 Camera Driver
```

## Initialization

### Setup

1. Connect depth camera by USB 3.0+

2. Drag `DepthCameraManager` Prefabs in `Assets/IRToolkit/Prefabs` to Scene

3. Write Script !

### Configuration

#### Hand Position Manager

`Sign` is number that will be multiply with x-axis of hand's position. User can adjust it to 1 or -1.
* 1 is worked for Head Mounted Display Device
* -1 is worked for Normal Display Device

#### Gestural Manager

`Is Enable All Gesture` is boolean for enable all of gesture in system.

If `Is Enable All Gesture` is false, system will enable all of gesture those mention in `Gesture Actions`.

[See gesture information](https://software.intel.com/sites/landingpage/realsense/camera-sdk/v1.1/documentation/html/index.html?doc_hand_handling_gestures.html)

## Usage

### Hand Positioning

#### Initialize

```Cs
void Start()
{
  GameObject.Find("DepthCameraManager").GetComponent<HandPositionManager>().AddSubscriber(gameObject);
}
```

#### Receive Hand Position

```Cs
void OnLeftHandChange(Vector3 handPosition)
{
  // Do your work when left hand position is updated
}

void OnRightHandChange(Vector3 handPosition)
{
  // Do your work when right hand position is updated
}
```

#### Notify Hand State Change

```cs
void OnLeftHandAppear()
{
  // Do your work when left hand is appeared
}

void OnLeftHandDisappear()
{
  // Do your work when left hand is disappeared
}

void OnRightHandAppear()
{
  // Do your work when right hand is appeared
}

void OnRightHandDisappear()
{
  // Do your work when right hand is disappeared
}
```

### Hand Gesture

#### Initialize
```cs
void Start()
{
  GameObject.Find("DepthCameraManager").GetComponent<GesturalManager>().AddSubscriber(gameObject);
}
```

#### Method Pattern

* void On+`GestureName`()
* void On+`HandSide`+Hand+`GestureName`()

#### Receive Gesture  

```cs
void OnGesture(GestureData data){
  // Do your work
  // data.name is gesture name
}

void OnThumbUp(){
  // Do your work here when current gesture is thumb up
}

void OnLeftHandThumbUp(){
  // Do your work here when left hand gesture is thumb up
}

void OnRightVSign(){
  //Do your work here when right hand gesture is v sign
}
```

## MIT License

```
MIT License

Copyright (c) 2017 Voraton Lertrattanapaisal

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

## Reference

* [Intel® RealSense™ SDK 2016 R2 Documentation](https://software.intel.com/sites/landingpage/realsense/camera-sdk/v1.1/documentation/html/)
