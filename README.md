# AutoMemo
이 프로그램은 사용자가 기억해야 할 말을 하면 자동으로 메모해줍니다.   
말이 끝나고 나서 최소 2~3초 뒤에 메모되며, 기억하지 않아도 되는 말은 메모하지 않습니다.   
이는 "해야 돼", "제출", "필요해" 등의 키워드를 인식하는 방법으로 작동합니다.   
   
업로드한 AutoMemo.apk를 다운로드받고 실행하면 설치됩니다.   
[<AutoMemo.apk 다운로드>](https://github.com/Ryansmg/AutoMemo/raw/master/AutoMemo.apk)
> 주의: 위 링크를 클릭하면 파일이 바로 다운로드됩니다.   
> 안드로이드 전용 앱입니다.   
> 주변이 시끄러우면 목소리 인식에 실패할 수 있습니다.   
   
아래는 코드 작동 원리에 대한 간략한 설명입니다.

*****

### AudioManager.cs
오디오 입력의 데시벨을 계산합니다. 이는 RecordAudio.cs에서 목소리 감지에 사용됩니다.
### RecordAudio.cs
목소리를 감지하고 녹음한 뒤, 목소리를 SavWav.cs를 사용해 wav 파일로 변환합니다. 
### GoogleRequest.cs
API 키와 wav 파일을 포함한 POST 요청을 구글의 오디오-텍스트 변환 API에 보내고, 응답을 UpdateScreen.cs에 전달합니다.
### UpdateScreen.cs
상태 메시지를 포함한 UI를 업데이트하며, 응답이 키워드들을 포함할 시 메모를 생성합니다. 
### MoveCamera.cs
카메라의 움직임을 조절합니다.

*****

#### AudioManager.cs
Calculates a decibel of the audio input. The decibel is used in RecordAudio.cs to detect voices.
#### RecordAudio.cs
Detects and records your voice, and creates a wav file (based on the recorded AudioClip) using SavWav.cs.
#### GoogleRequest.cs
Sends a POST request (including the API key & the wav file) to the Google Speech-To-Text API, and passes the response to UpdateScreen.cs.
#### UpdateScreen.cs
Updates the UI including the status text.
It creates a textSquare if the response contains certain keywords.
#### MoveCamera.cs
Moves the Main Camera.
