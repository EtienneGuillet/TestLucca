var pattern = [
  "ArrowUp",
  "ArrowUp",
  "ArrowDown",
  "ArrowDown",
  "ArrowLeft",
  "ArrowRight",
  "ArrowLeft",
  "ArrowRight",
  "b",
  "a",
];
var current = 0;

var keyHandler = function (event) {
  if (pattern.indexOf(event.key) < 0 || event.key !== pattern[current]) {
    current = 0;
    return;
  }

  current++;

  if (pattern.length === current) {
    current = 0;
    $("#surpriseModal").modal("show");
  }
};

document.addEventListener("keydown", keyHandler, false);

function addEventListenerForEnterKey(suffix) {
  document
    .getElementById("inputChat" + suffix)
    .addEventListener("keyup", function (event) {
      if (event.key === "Enter") {
        event.preventDefault();
        document.getElementById("buttonChat" + suffix).click();
      }
    });
}

function sendMessage(senderSuffix, receiverSuffix) {
  const input = document.getElementById("inputChat" + senderSuffix);
  if (!input.value) {
    return;
  }
  const chat = $(`#${"chat" + receiverSuffix}`);
  const newMessage = document.createElement("div");

  newMessage.className = "message";
  newMessage.innerHTML = input.value;
  input.value = "";
  chat.append(newMessage);
  setTimeout(function () {
    chat.animate(
      {
        scrollTop: chat.prop("scrollHeight"),
      },
      500
    );
  }, 10);
}

addEventListenerForEnterKey("A");
addEventListenerForEnterKey("B");
