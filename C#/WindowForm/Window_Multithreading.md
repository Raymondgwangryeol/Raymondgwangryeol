## 윈도우 멀티쓰레딩과 Invoke()
</br>
</br>
윈폼에서 멀티쓰레딩을 할 때, UI 컨트롤을 갱신해야 한다면, 무조건 UI Thread위에서 해야한다.
</br>
이런 문제를 해결하기 위해서, 쓰레드 함수에서 UI 컨트롤 접근 시 항상 Control클래스의 InvokeRequired 속성을 체크해야한다.
</br>
  **+)InvokeRequired?**
  </br>
    System.Windows.Forms에 정의되어 있으며, 멀티쓰레딩일 때 호출 스레드와 다른 스레드에 있다면 true, 아니면 false
    </br>
만약 InvokeRequired 속성이 true라면, Invoke(동기 호출)이나 BeginInvoke(비동기호출)을 사용해서 UI Thread로 메서드 호출을 보내야 함.
</br>
</br>

**Invoke와 BeginInvoke?**
</br>
원래 속해있는(?) 메인 쓰레드를 현재 쓰레드(외부 쓰레드)에서 호출하는 함수.
</br>
안에 실행하려고 하는 메소드의 대리자를 넣으면 됨.
</br>
Invoke는 동기식으로 해결되고 BeginInvoke는 비동기식으로 해결 됨.
</br></br>


```cs
delegate void ShowDelegate(int percent); //퍼센트를 넣으면 progressBar에 반영되는 그런 코드인 듯

private void ShowProgress(int pct)
{
   if (InvokeRequired) // 내가 다른 쓰레드에 있으면 실행
   {
      ShowDelegate del = new ShowDelegate(ShowProgress); 
      //또는 ShowDelegate del = p => ShowProgress(p);
      Invoke(del, pct);
   }
   else
   {
      progressBar1.Value = pct;
   }
```
