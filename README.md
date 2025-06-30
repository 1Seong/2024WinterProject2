# 2024WinterProject2 🎮

## 📌 프로젝트 개요
SQUARE ERAUQS는 Unity 기반으로 개발 중인 퍼즐 장르의 게임입니다.
플레이어는 두 개의 네모를 각자의 색에 맞는 문을 찾아서 이동하여 탈출하는 것이 목적입니다.  

---

## 🛠️ 개발 환경
- **엔진**: Unity 6000.0.27f1
- **언어**: C#
- **플랫폼**: PC

---

## 🎮 핵심 기능
- ✅ 스프링, 점프, 아이템 등 인터랙션 시스템
- ✅ 에피소드별 스테이지 기반 구조 및 UI 연결
- ✅ 배경 애니메이션, 문 개폐 트리거 등 시각 효과 적용
- ✅ 멀티 스테이지 전환 시스템

---

## 🗂️ 폴더 구조
<pre> ````markdown ## 📁 폴더 구조 ``` Assets/ ├── Data/ # 게임 설정 데이터, 스테이지 정보 등 ├── Materials/ # Unity 머티리얼 파일 (표면, 광택 등) ├── Prefabs/ # 프리팹 객체들 ├── ProBuilder Data/ # ProBuilder로 생성한 메시 데이터 ├── Scenes/ # Unity 씬들 (Stage1, Stage2 등) ├── Scripts/ # C# 스크립트 (Player, GameManager 등) ├── Settings/ # 입력 설정, 프로젝트 설정 등 ├── Sprites/ # 2D 이미지 리소스 ├── TextMesh Pro/ # 텍스트 렌더링 관련 리소스 ├── TutorialInfo/ # 튜토리얼 관련 정보 및 리소스 ProjectSettings/ Packages/ ``` ```` </pre>

---

## ▶️ 실행 방법
1. 레포지토리를 클론합니다.
   ```bash
   git clone https://github.com/1Seong/2024WinterProject2.git
2. Unity Hub 또는 Unity Editor에서 해당 폴더를 열고, Scenes/MainScene.unity를 실행합니다.
3. Play 버튼을 눌러 테스트할 수 있습니다.

---

## 👥 팀원 소개
| 이름           | 역할                           |
| -------------- | -----------------              |
| 최성원         | 팀장, 메인 개발                 | 
| 김동우         | 메인 기획, 문서 작성            |
| 김영준         | 메인 개발, UI/UX               |
| 박민용         | 서브 기획, 서브 개발, 문서 작성 |
| 엄태형         | 스테이지 디자인, 아이템 디자인  |
| 이규빈         | 서브 기획, 서브 개발, 사운드    |

---

## 🚧 진행 상황 / TODO
[x] 기본 플레이어 컨트롤 구현

[x] 1~5 에피소드 구현

[x] 아이템 동작 구현

[ ] UI 제작

[ ] 사운드 및 배경음악 삽입

[ ] 아트 제작

---

## 💬 참고 사항
.gitignore는 Unity 프로젝트에 적합하게 설정되어 있습니다.

빌드는 Build/ 폴더에 저장하도록 구성할 예정입니다.

향후 GitHub Projects를 통해 작업 상태를 추적할 계획입니다.

---

## 📄 라이선스
