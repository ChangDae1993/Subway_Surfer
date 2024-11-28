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
      Module.socket = new WebSocket("ws://localhost:9009");

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

  ShowRanking: function () {
    // 앨범 목록 조회 (서버에서 사용자 앨범 정보 받아오기)
    fetch("/rankings")
      .then((response) => response.json())
      .then((data) => {
        console.log("랭킹을 보여주세요");
      })
      .catch((error) => {
        console.error("앨범 정보 조회 오류:", error);
      });
  },
});
