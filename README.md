# AutoMemo
이 프로그램은 사용자가 뭔가 필요하다는 말을 하면 자동으로 메모해줍니다.
이는 "해야 돼", "필요해" 등의 키워드를 인식하는 방법으로 작동합니다.
아래는 코드 작동 원리에 대한 간략한 설명입니다.

## GoogleRequest.cs
Sends a POST request to the google Speech-To-Text API, and
sends the response to UpdateScreen.cs
## AudioManager.cs
Calculates a decibel of the audio input.
The decibel is used in RecordAudio.cs to detect voices.
## RecordAudio.cs
Detects and records your voice, and creates a wav file using SavWav.cs.
## UpdateScreen.cs
It updates the UI.
It creates a textSquare if the response contains certain keywords.
### MoveCamera.cs
It moves the Camera. That's all.
