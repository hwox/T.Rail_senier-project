﻿

// 제 메모장입니다만


public class Todo_Memo  {

// 아이템 resource 함수 쓰지말고 미리 사용할 이미지들 다 캐싱해놓기

    // 뒤에 배경 layer나누기 -> 나눴음
    // 움직이는거나 혹시 raycast 쓸 때 layer로 체크하기


    // 씬 전환시에 데이터 그대로 유지
    // 어느범위, 어떤 걸 남겨놓을건지
  
    // 그리고 update말고 쓸 수 잇는건 코루틴사용

    // 아니 싱글톤 공부 제대로 해야겠는데


    // 플레이어 움직일 때 카메라가 따라가게

    // 소파랑 상자-> 오브젝트 풀링으로 바꾸기


    // 아씨,,,,,,,
    // 기차 안에 자꾸 local rotation으로 들어가서
    // quaternion 하면 GC 오질텐데
    // 이거 어떻게 바꾸냐

    // 아니 그리고 가구 만들고나서 그 기차 자식으로 넣어줘야하는데
    // 이것도 생각해봐야해
    // 어떻게 수정할지 일단은 이렇게 해도
}


//// -----------> 진행상황

// 아이템 -
// 일단 만들어놓긴 했는데 오브젝트풀링 한번 더 찾아보고 
// 더 올바른 방법 있는지 보기
// 그리고 스크립트를 수정해줘야ㅑ해 instance 된 부분을
// 차라리 child를 하던지해서 연결시켜주게 수정해야함
// 완료-
