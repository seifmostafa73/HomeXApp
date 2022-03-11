# HomeXApp

 MQTT Smart home App made with unity.

## Features

Voice commands 
Google account auth
Location triggered events 
In-app chatting system (removed from active secenes but can still be found in the project folders)
Low delay hardware communication (MQTT)
Dark/bright themes (all palletes are adjustable via a theme controller)
Devices QR scanning 
Favourites / quick Access sub menu
Dynamic room, devices icons and names
Dynamic toast windows


## APIs and technology used

* Unity
* C#
* PHP
* M2Mqtt
* Https
* Firebase real time Database (non SQL)
* SQL database
* Rest API
* JSON serialization


## Known Bugs
please note that this Project was built for learning purposes so it's not contantly updated , as a reuslt some api's might be old.

* the TTS and STT scripts stopped working for some reason and only accepts korean langague [although I rechecked the script and it should be working fine.]

* when you add ad device to the room you need to restart the app in order for it to show in the favourties and update the blank home page [a simple solutuion is to re-call the function after you add the device]
* Secarios sometime cause a connection problem with the free MQTT server , which is acceptable , however the used version of M2MQTT lakes the proper handling , this can be resolved by simply using a try-catch statment in the libarary's Send funtion

## ScreenShots

<img src="https://user-images.githubusercontent.com/55613060/157886244-875e97e7-b760-45ba-bd89-6f689cb0f137.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886247-c5907149-b05c-4261-88de-ee0f2d9472c8.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886250-fa171e3d-bd6d-4bfc-9be1-780a9086056c.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886252-7aed1e01-d5d3-4225-bde1-53a6190da0c2.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886255-21f41032-7708-4914-a689-8af3853879f0.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886257-c9bb0d4c-5493-4bcf-9206-a68f73453abc.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886259-75a9c9e1-e961-4b6f-a54f-bd797eabc5af.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886265-35681697-0a52-4d8d-9217-6450defff7c4.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886268-04a2b412-8c6e-4c7a-8227-bab75e2ca75c.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886270-3e060f4b-c81c-4f2c-a1fe-cae6fbf9e61f.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886277-3c7d922a-c6e8-4505-8ad5-151f48c51037.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886280-a09126fe-32f7-4d60-a9b8-a492be626b80.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886287-97c904d1-0595-42e0-bb86-1e752f3233f4.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886296-1fab85ef-5896-4331-ba83-f069aeb14204.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886300-59ed2589-da5a-4f78-bc6a-6cb90ac4f51b.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886306-f106c087-fcd5-431c-b12e-17a7fb02b890.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886308-1b9e7c45-c6f9-4dee-b2ad-c77ea4d7a6bd.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886312-a0d90840-7c83-470f-8ab5-564f5653db8f.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886315-dc5d6ceb-5618-44f6-b82c-89ba57c32dc2.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886318-36b8b0bb-bb1f-48ea-9d4f-55166e2865ce.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886324-e4e98208-e726-489d-9ff2-7cb9ccbf402c.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886328-5a1b2d1c-f79c-4cb0-90a6-563d91af8b21.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886329-c6949bec-7282-455a-9c02-3aa5e835467c.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157886333-920df834-2d6a-4239-b474-045511144d2e.jpeg" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157888081-9bb73ac0-b4e5-4112-83fd-2d2a3b94ce5b.png" width="360" height="800"/>
<img src="https://user-images.githubusercontent.com/55613060/157889788-6c709606-e9df-4cd0-b372-a669160c43c3.png" width="360" height="800"/>
