const timeNode = document.getElementById("time");

function renderTime() {
  const now = new Date();
  timeNode.textContent = `当前时间: ${now.toLocaleTimeString()}`;
}

renderTime();
setInterval(renderTime, 1000);
