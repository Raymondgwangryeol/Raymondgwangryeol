## await와 async
_C# 5.0부터 추가_


</br>
</br>

#### 비동기 프로그래밍이란?
음 내가 밥 먹는다는 행위를 할 때, 그 안에서 밥도 떠먹으면서 동시에 핸드폰도 보는 그리고 동시에 물도 마시는.. 그런 프로그래밍
</br>
**동기 프로그래밍**은
</br>
식사할 때 무조건 핸드폰은 밥 다 먹고 나서 함. 물은 무조건 핸드폰 다 하고 나서 마심(???). 이런 프로그래밍을 말함. 
</br>
이걸 **멀티 쓰레딩**으로 치면
</br>
**비동기식 멀티 쓰레딩**은 코스 요리가 3파트로 나눠져 있다고 치면, 한 파트당 나오는 요리들을 한 상에 다 차려주는 스타일?
</br>
**동기식**은 한 파트에 나오는 요리들을 한 접시씩 손님에게 주는 스타일. 근데 손님이 해당 요리를 다 먹어야 다음 식사를 줌.
</br>
</br>
**멀티 쓰레딩이라고 다 비동기식인 건 아님!!**
</br>
쓰레딩은 프로그램에서 작업을 처리하는 주체(쓰레드)에 관한 개념이고
</br>
비동기/동기는 작업에 대한 요청과 응답을 처리하는 방식에 관한 개념임.
</br>

(참고 url: https://kangworld.tistory.com/24)
</br></br>

#### 그래서 await이 뭐예요?
await은 **async 함수 안에서만 사용할 수 있는 문법**으로, 작업의 흐름을 제어하는 키워드다.
</br>
비동기 프로그래밍을 지원해주는(일종의 보조 역할을 수행) 역할을 한다.
</br>
일반적으로 Task 또는 Task<T> 객체와 함께 사용된다.
</br>
UI 프로그램에서 await 키워드가 달린 Task를 만나면, 해당 Task가 끝날 때 까지 바깥쪽 비동기 메서드의 작동을 일시 중단 시킴.
</br>
여기서 중요한 건 비동기식이기 때문에, UI쓰레드가 정지되지 않고 메시지 루프를 계속 돌 수 있음. (await 키워드를 만나면 컴파일러가 필요한 코드를 자동으로 추가한다고 함)
</br>
작업이 다 끝나면, 작업 결과를 반환함. (실행되어질 함수 자체에 await가 붙는 경우) 동기식으로 계속 진행 됨.
</br>
만약 이미 완료된 작업을 나타내는 객체에 await가 붙으면, 바깥쪽 메서드를 중단하지 않고 작업 결과를 즉시 반환함. (Task 객체에 await가 붙는 경우)
</br>
awaitable 작업이 끝난 후에는 await 이후의 나머지 코드를 실행하도록 대기 작업으로 등록하고, async 메서드의 호출자에게 Task를 반환함.
</br>
</br>

**[예제]**
</br>

```cs
public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Run();  //UI Thread에서 실행
                    // Worker Thread에서 돌리고 싶으면 Task.Run()이나 TaskTest 등 Task 속성을 쓰면 됨.
        }
        private async void Run()
        {
            // 비동기로 Worker Thread에서 도는 task1
            // Task.Run(): .NET Framework 4.5+
            var task1 = Task.Run(() => LongCalcAsync(10));

            // task1이 끝나길 기다렸다가 끝나면 결과치를 sum에 할당
            int sum = await task1;

            // UI Thread 에서 실행
            // Control.Invoke 혹은 Control.BeginInvok 필요없음
            this.label1.Text = "Sum = " + sum;
            this.button1.Enabled = true;
        }

        private int LongCalcAsync(int times)
        {
            int result = 0;
            for (int i = 0; i < times; i++)
            {
                result += i; //1+2+3+4+5+6+7+8+9
                Task.Delay(1000); //밀리초 기준(1초)
            }
            return result;
        }
    }
```

</br></br>

### UI Thread에서 돌리는 Task
await를 사용하면 대부분 Background Worker Thread에서 실행되는데, await 썼다고 무조건 task가 Worker Thread에 가는 건 아님.
아래처럼 별도의 쓰레드를 생성하지 않고 UI Thread위에서 돌릴 수 있음

```cs
private void button1_Click(object sender, EventArgs e)
{
     Run();  //UI Thread에서 실행
}

private async void Run()
{    
    int sum = await LongCalc2(10);
    this.label1.Text = "Sum = " + sum;
    this.button1.Enabled = true;
}

private async Task<int> LongCalc2(int times)
{
    //UI Thread에서 실행
    Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
    int result = 0;
    for (int i = 0; i < times; i++)
    {
        result += i;                
        await Task.Delay(1000);
    }
    return result;
}
```
</br></br>

### 콘솔 프로그램에서의 await
윈폼 같은 UI프로그램에서는, await가 실행되기 전에 당시 실행중이던 쓰레드들을 캡처해서 SynchronizationContext에 저장함. 이후 await가 끝나면 여기서 아 원래 이 쓰레드 실행하기로 했지~ 이러면서 찾아서 실행함.
</br>
하지만, 콘솔 프로그램이나 윈도우즈 서비스 프로그램의 경우, SynchronizationContext가 디폴트로 null이어서, await 이후 문장들 실행 시 Thread Pool에서 직접 가져와 실행함.
</br>
(스레드 풀(Tread Pool)=> 동시에 여러 작업을 효율적으로 실행 및 관리하기 위해 서버에서 만드는 스레드의 모음)
