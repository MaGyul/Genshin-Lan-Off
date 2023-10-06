[![MSBuild](https://github.com/MaGyul/Genshin-Lan-Off/actions/workflows/msbuild.yml/badge.svg)](https://github.com/MaGyul/Genshin-Lan-Off/actions/workflows/msbuild.yml)

# Genshin Lan Off
> 원신 랜선 뽑기는 인터넷이 연결되는 시간도 오래걸리는 단점을 방화벽을 이용해서 보완을 한 프로그램
> MS 스토어에선 원신이라는 글자가 들어간 제품 이름을 사용할 수 없어 [덕후 게임 인터넷 유틸](https://www.microsoft.com/store/apps/9N0MSQV2FT5C)로 올라가 있습니다.


## 파일관련
- 방화벽을 이용한 프로그램이다 만큼 방화벽이랑 설정 저장을 아주 편하게 하기 위해 레지스트리를 건들다 보니
- 백신 프로그램에서 trojan 종류의 멀웨어로 감지될수도 있습니다.
- 의심 스럽다면 소스코드가 공개되어 있으니 직접확인 후 사용하시면 되겠습니다.


## 사용방법
1. [Git](https://github.com/MaGyul/Genshin-Lan-Off.git)를 클론을 해서 직접 빌드를 합니다.
2. bin 폴더안 Debug 혹은 Release에 있는 Genshin Lan Off.exe를 실행을 합니다.
3. 트레이를 보시면 `원신 랜뽑`으로 프로그램이 실행돼 있을겁니다.
4. 클릭 혹은 우클릭으로 메뉴를 띄운다음 설정을 클릭합니다.
5. `랜뽑 실행 단축키`에서 변경을 누른 후 설정할 키를 누르면 입력이 됩니다.
6. `방화벽 규칙 이름`에 이름을 적은 후 적용을 누르시면 됩니다.
### 아웃바이트 규칙이 되어있을 경우
1. 적용을 누른 후 바로 설정 창이 닫힙니다.
2. 혹여나 아웃바이트 규칙이 아니거나 연결 차단이 아니거나 차단 종류가 프로그램에 GenshinImpact.exe로 안되있을 경우 오류 대화 상자가 뜰겁니다.
3. 2번 상황을 넘겼을 경우 아무런 대화 상자도 없이 설정 창이 닫힙니다.
### 방화벽 규칙이 없을경우
1. `방화벽 규칙 찾기 실패` 대화 상자가 뜰겁니다. 여기서 `예(Y)`를 누릅니다.
2. `폴더 찾아보기`가 뜨며 원신 게임이 있는 폴더를 선택합니다. `ex) C:\Program Files\Genshin Impact\Genshin Impact Game`
3. `방화벽 규칙을 생성했습니다.`라는 메시지가 뜨며 `확인`을 누르시면 설정 창이 닫힙니다.
#### 모든 설정 완료
- 트레이를 클릭 혹은 우클릭으로 메뉴를 띄우면 상태, 방화벽, 단축키, 알림 3개가 보이게 될겁니다.
- 단축키에 있는데로 단축키를 눌러 방화벽을 활성화/비활성화 할 수 있습니다.
- 활성화/비활성화는 윈도우 알림으로 알려줍니다.

## 주의상황
- 프로그램 종료시 방화벽이 자동으로 비활성화 되겠금 설정되어 있지만 비활성화가 안되고 꺼질수도 있으니 종료시 비활성화후 종료하시길 바랍니다.

## 의존성
- .Net Framework 4.6.2 이상 버전을 필요로합니다.


## 문제발생
- [이슈](https://github.com/MaGyul/Genshin-Lan-Off/issues)또는 Pull Request넣어주세요


## 면책조항
- 이 프로그램 및 소스코드를 사용하는 불이익은 사용자 본인에게있습니다.  
- 저작권자와 기여자는 이 소스코드를 '있는 그대로' 제공하며 어떠한 보증도 하지않습니다.


## 라이센스
- 이 소스코드는 [MIT LICENSE](LICENSE)를 따릅니다.


## 연락처
- 자세한 내용은 `mail@magyul.kr`로 문의하시기 바랍니다.
