# AutoMemo
이 프로그램은 사용자가 기억해야 할 말을 하면 자동으로 메모해줍니다.   
말이 끝나고 나서 최소 2~3초 뒤에 메모되며, 기억하지 않아도 되는 말은 메모하지 않습니다.   
이는 "해야 돼", "제출", "필요해" 등의 키워드를 인식하는 방법으로 작동합니다.   
   
업로드한 AutoMemo.apk를 다운로드받고 실행하면 설치됩니다.   
[<AutoMemo.apk 다운로드>](https://github.com/Ryansmg/AutoMemo/raw/master/AutoMemo.apk)
> 주의: 위 링크를 클릭하면 파일이 바로 다운로드됩니다.   
> 안드로이드 전용 앱입니다.   
> 주변이 시끄러우면 목소리 인식에 실패할 가능성이 높습니다.   
   
아래는 코드 작동 원리에 대한 간략한 설명입니다.

*****

### AudioManager.cs
Calculates a decibel of the audio input.
The decibel is used in RecordAudio.cs to detect voices.
### RecordAudio.cs
Detects and records your voice, and creates a wav file (based on the recorded AudioClip) using SavWav.cs.
### GoogleRequest.cs
Sends a POST request (including the API key & the wav file) to the Google Speech-To-Text API, and passes the response to UpdateScreen.cs.
### UpdateScreen.cs
Updates the UI including the status text.
It creates a textSquare if the response contains certain keywords.
### MoveCamera.cs
Moves the Main Camera.
