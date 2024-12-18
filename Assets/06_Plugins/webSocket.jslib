mergeInto(LibraryManager.library, {
  Hello: function () {
    window.alert("Hello, world!");
  },

  HelloString: function (str) {
    window.alert(UTF8ToString(str));
  },

  PrintFloatArray: function (array, size) {
    for (var i = 0; i < size; i++) console.log(HEAPF32[(array >> 2) + i]);
  },

  AddNumbers: function (x, y) {
    return x + y;
  },

  StringReturnValueFunction: function () {
    var returnStr = "bla";
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  socket: null, // 전역 WebSocket 변수

  WebSocketSetting: function () {
    if (Module.socket && Module.socket.readyState === WebSocket.OPEN) {
      console.log("WebSocket 이미 연결됨");
      return;
    }

    function initializeWebSocket() {
      Module.socket = new WebSocket("ws://localhost:3000/ws");

      Module.socket.onopen = () => {
        console.log("WebSocket 연결 성공");
      };

      Module.socket.onclose = () => {
        console.error("WebSocket 연결이 끊어졌습니다. 재연결 시도 중...");
        setTimeout(initializeWebSocket, 1000); // 1초 후 재연결
      };

      Module.socket.onerror = (error) => {
        console.error("WebSocket 오류 발생:", error);
      };

      Module.socket.onmessage = (event) => {
        console.log("서버로부터 받은 메시지:", event.data);
      };
    }
    initializeWebSocket(); // WebSocket 연결 시도
  },

  SendScore: function (score) {
    if (Module.socket && Module.socket.readyState === WebSocket.OPEN) {
      Module.socket.send(score);
    } else {
      console.error("WebSocket 연결이 닫혀 있습니다.");
    }
  },

  PrintNumber: function (number) {
    if (Module.socket && Module.socket.readyState === WebSocket.OPEN) {
      Module.socket.send(UTF8ToString(number));
    } else {
      console.error("WebSocket 연결이 닫혀 있습니다.");
    }
  },

  ShowRanking: async function () {
    try {
      const response = await fetch("/rankings", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
      });
      const data = await response.json();

      if (response.ok) {
        const names = data.map((row) => row.user_id).join(","); // 배열을 문자열로 결합
        const scores = data.map((row) => row.score).join(","); // 배열을 문자열로 결합

        // 두 문자열을 하나로 결합해서 보내기
        const combined = names + "|" + scores; // 이름과 점수를 '|'로 구분해서 결합

        console.log("ShowRanking 호출됨!"); // 디버그 로그 추가
        // Unity로 데이터 전송
        globalUnityInstance.SendMessage(
          "RankingArea", // Unity 오브젝트 이름
          "SettingRank", // Unity 메서드 이름
          combined // 결합된 하나의 문자열 전달
        );
      } else {
        alert(data.error);
      }
    } catch (error) {
      console.error("랭킹 조회 중 오류 발생:", error);
    }
  },
});
