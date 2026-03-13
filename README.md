# 💣 Hoseo Arcade (Full-Cycle Multiplayer Action)

![Unity](https://img.shields.io/badge/Unity-2021.3.16f1-black?style=for-the-badge&logo=unity)
![Photon](https://img.shields.io/badge/Photon-PUN2-blue?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Completed-success?style=for-the-badge)

**Hoseo Arcade**는 Photon Unity Networking 2(PUN2)를 기반으로 제작된 **실시간 멀티플레이어 아케이드 게임**입니다. 
닉네임 설정부터 로비 매칭, 인게임 전투, 그리고 최종 결과 창으로 이어지는 게임의 전체 라이프사이클을 완벽하게 구현한 완성형 프로젝트입니다.

---

## 📺 Gameplay Demo
이미지를 클릭하면 실제 플레이 영상(YouTube)으로 연결됩니다.

[<img src="https://github.com/user-attachments/assets/4bbe7aa7-acb6-4acd-9edc-7c19f7505d8b" width="688" alt="Hoseo Arcade Gameplay" />](https://www.youtube.com/watch?v=11pZ_LK8rhM)

---

## ✨ 핵심 구현 기능

### 1. 완벽한 게임 루프 (Full Game Loop)
- **Title & Lobby**: 유저 닉네임 입력 및 서버/로비 접속 프로세스 구축.
- **Matchmaking**: `JoinRandomOrCreateRoom` 기능을 통한 실시간 방 생성 및 참가 시스템.
- **Result System**: 게임 종료 후 승리 조건 판정 및 결과 화면(Score) 출력 루프 완성.

### 2. 실시간 네트워크 동기화 (Networking)
- **Object Sync**: `PhotonView`, `TransformView`, `AnimatorView`를 활용한 위치 및 애니메이션 동기화.
- **RPC Battle System**: 
  - `[PunRPC]`를 이용한 폭탄(Bomb) 설치 및 폭발 판정 공유.
  - 네트워크 상의 모든 플레이어에게 실시간으로 데미지와 점수 데이터를 브로드캐스팅.
- **Lag Compensation**: 네트워크 지연 시간(Lag)을 고려한 부드러운 캐릭터 움직임 보정.

### 3. 아케이드 액션 메카닉 (Mechanics)
- **Bomb Controller**: 일정 시간 후 폭발하며 주변 오브젝트와 플레이어에게 물리적/데이터적 타격을 주는 로직.
- **Dynamic UI**: 실시간 점수판(Leaderboard) 및 플레이어 상태를 표시하는 HUD 시스템.

---

## 📂 프로젝트 구조 (Core Assets)
```text
Assets/
├── 📁 Scripts/
│   ├── NetworkManager.cs    # 서버 연결, 로비 및 룸 매칭 핵심 로직
│   ├── PlayerController.cs   # 플레이어 이동, 애니메이션 및 RPC 통신
│   ├── BombController.cs     # 폭탄 생성, 타이머 및 폭발 범위 판정
│   └── GameMgr.cs            # 전체 게임 상태(시작/종료) 및 점수 관리
├── 📁 Prefabs/               # 네트워크로 생성되는 플레이어 및 폭탄 에셋
└── 📁 Scenes/                # Title -> Lobby -> GameMain 전체 씬 구성
