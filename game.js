const canvas = document.getElementById("game");
const ctx = canvas.getContext("2d");

function resize() {
  canvas.width = window.innerWidth;
  canvas.height = window.innerHeight;
}
window.addEventListener("resize", resize);
resize();

const input = { left: false, right: false, jump: false };
window.addEventListener("keydown", (e) => {
  if (e.code === "KeyA") input.left = true;
  if (e.code === "KeyD") input.right = true;
  if (e.code === "Space") input.jump = true;
});
window.addEventListener("keyup", (e) => {
  if (e.code === "KeyA") input.left = false;
  if (e.code === "KeyD") input.right = false;
  if (e.code === "Space") input.jump = false;
});

function noise(x, a, b) {
  return Math.sin(x * a) * b + Math.sin(x * a * 0.37) * b * 0.6;
}

const world = {
  level: 1,
  levelSwitchPending: false,
  width1: 9000,
  width2: 9000,
  orbX: 4200,
  orbY: 0,
  endX: 7800
};

const player = {
  x: 120,
  y: 0,
  vx: 0,
  vy: 0,
  w: 34,
  h: 54,
  onGround: false
};

const camera = { x: 0, y: 0, zoom: 1 };

const particles = [];
function emitOrbBurst(x, y, count) {
  for (let i = 0; i < count; i++) {
    const a = Math.random() * Math.PI * 2;
    const s = 1 + Math.random() * 6;
    particles.push({
      x,
      y,
      vx: Math.cos(a) * s,
      vy: Math.sin(a) * s,
      life: 70 + Math.random() * 40
    });
  }
}

function getGroundY(x) {
  if (world.level === 1) {
    return canvas.height * 0.83 + noise(x * 0.01, 0.8, 18);
  }
  return canvas.height * 0.86 + noise(x * 0.01, 0.7, 20);
}

function getObstacles() {
  if (world.level === 1) {
    return [
      { x: 900, w: 160, h: 90 },
      { x: 1500, w: 140, h: 120 },
      { x: 2500, w: 420, h: 150 },
      { x: 3400, w: 360, h: 120 },
      { x: 4700, w: 700, h: 220 }
    ];
  }
  return [
    { x: 1000, w: 900, h: 120 },
    { x: 2400, w: 700, h: 120 },
    { x: 3600, w: 120, h: 100 },
    { x: 4100, w: 120, h: 170 },
    { x: 4600, w: 120, h: 230 },
    { x: 5100, w: 120, h: 300 },
    { x: 5600, w: 120, h: 360 },
    { x: 6200, w: 550, h: 80 }
  ];
}

function terrainHeightAt(x) {
  let y = getGroundY(x);
  const obs = getObstacles();
  for (const o of obs) {
    if (x > o.x && x < o.x + o.w) {
      y = Math.min(y, getGroundY(o.x) - o.h);
    }
  }
  return y;
}

function update() {
  const speed = 2.9;
  if (input.left) player.vx = -speed;
  else if (input.right) player.vx = speed;
  else player.vx *= 0.8;

  if (input.jump && player.onGround) {
    player.vy = -10.5;
    player.onGround = false;
  }

  player.vy += 0.45;
  player.x += player.vx;
  player.y += player.vy;

  const footY = player.y + player.h * 0.5;
  const floor = terrainHeightAt(player.x);
  if (footY > floor) {
    player.y = floor - player.h * 0.5;
    player.vy = 0;
    player.onGround = true;
  }

  const targetCamX = player.x - canvas.width * 0.35;
  camera.x += (targetCamX - camera.x) * 0.09;

  if (world.level === 1) {
    const dx = player.x - world.orbX;
    const dy = player.y - (getGroundY(world.orbX) - 140);
    if (!world.levelSwitchPending && Math.hypot(dx, dy) < 48) {
      world.levelSwitchPending = true;
      emitOrbBurst(world.orbX, getGroundY(world.orbX) - 140, 30000);
      document.getElementById("hint").textContent = "点击鼠标进入第二关";
    }
  } else if (player.x > world.endX) {
    document.getElementById("hint").textContent = "一切，才刚刚开始。";
    player.vx = 0;
  }

  for (let i = particles.length - 1; i >= 0; i--) {
    const p = particles[i];
    p.x += p.vx;
    p.y += p.vy;
    p.vy += 0.03;
    p.life -= 1;
    if (p.life <= 0) particles.splice(i, 1);
  }
}

window.addEventListener("mousedown", () => {
  if (world.levelSwitchPending && world.level === 1) {
    world.level = 2;
    world.levelSwitchPending = false;
    player.x = 260;
    player.y = getGroundY(player.x) - 100;
    document.getElementById("hint").textContent = "第二关：巨型雕像与悬浮";
  }
});

function drawMountains(layer, color, amp, freq, offsetY) {
  ctx.fillStyle = color;
  ctx.beginPath();
  ctx.moveTo(0, canvas.height);
  for (let sx = 0; sx <= canvas.width; sx += 8) {
    const wx = (sx + camera.x * layer) * 0.01;
    const y = offsetY + Math.sin(wx * freq) * amp + Math.sin(wx * freq * 0.44) * amp * 0.6;
    ctx.lineTo(sx, y);
  }
  ctx.lineTo(canvas.width, canvas.height);
  ctx.closePath();
  ctx.fill();
}

function drawGroundAndStones() {
  const start = Math.floor(camera.x) - 120;
  const end = start + canvas.width + 240;

  ctx.fillStyle = world.level === 1 ? "#f5f1ef" : "#f3d9d4";
  ctx.beginPath();
  ctx.moveTo(-50, canvas.height + 50);
  for (let x = start; x <= end; x += 6) {
    const sx = x - camera.x;
    ctx.lineTo(sx, getGroundY(x));
  }
  ctx.lineTo(canvas.width + 50, canvas.height + 50);
  ctx.closePath();
  ctx.fill();

  ctx.strokeStyle = "#1f2530";
  ctx.lineWidth = 2;
  ctx.beginPath();
  for (let x = start; x <= end; x += 6) {
    const sx = x - camera.x;
    const y = getGroundY(x);
    if (x === start) ctx.moveTo(sx, y);
    else ctx.lineTo(sx, y);
  }
  ctx.stroke();

  for (let i = 0; i < 120; i++) {
    const rx = start + i * 95 + (Math.sin(i * 17.1) * 42);
    const ry = getGroundY(rx) - 5;
    drawRock(rx - camera.x, ry, 30 + (i % 3) * 20, 16 + (i % 4) * 10);
  }
}

function drawRock(x, y, w, h) {
  ctx.save();
  ctx.translate(x, y);
  const tilt = Math.sin(x * 0.013) * 0.3;
  ctx.rotate(tilt);

  ctx.fillStyle = "#121a26";
  ctx.beginPath();
  ctx.moveTo(-w * 0.5, 0);
  ctx.lineTo(-w * 0.26, -h * 0.95);
  ctx.lineTo(w * 0.12, -h * 0.82);
  ctx.lineTo(w * 0.48, -h * 0.2);
  ctx.lineTo(w * 0.35, 0);
  ctx.closePath();
  ctx.fill();

  ctx.fillStyle = "#2f3743";
  ctx.beginPath();
  ctx.moveTo(-w * 0.25, -h * 0.65);
  ctx.lineTo(w * 0.06, -h * 0.76);
  ctx.lineTo(w * 0.35, -h * 0.25);
  ctx.lineTo(-w * 0.05, -h * 0.32);
  ctx.closePath();
  ctx.fill();
  ctx.restore();
}

function drawObstacles() {
  const obs = getObstacles();
  ctx.fillStyle = world.level === 1 ? "#15202d" : "#3d2025";
  for (const o of obs) {
    const x = o.x - camera.x;
    const y = getGroundY(o.x) - o.h;
    ctx.fillRect(x, y, o.w, o.h);
  }
}

function drawFog() {
  for (let i = 0; i < 2; i++) {
    const alpha = i === 0 ? 0.08 : 0.14;
    ctx.fillStyle = `rgba(255,255,255,${alpha})`;
    for (let n = 0; n < 12; n++) {
      const x = ((n * 220 + performance.now() * (i === 0 ? 0.02 : 0.05)) % (canvas.width + 320)) - 160;
      const y = canvas.height * (i === 0 ? 0.34 : 0.55) + Math.sin(n + performance.now() * 0.001) * 20;
      ctx.beginPath();
      ctx.ellipse(x, y, 130, i === 0 ? 34 : 52, 0, 0, Math.PI * 2);
      ctx.fill();
    }
  }
}

function drawPlayer() {
  const x = player.x - camera.x;
  const y = player.y;
  ctx.fillStyle = "#111";
  ctx.fillRect(x - player.w * 0.5, y - player.h * 0.5, player.w, player.h);
}

function drawOrbAndParticles() {
  if (world.level === 1 && !world.levelSwitchPending) {
    const x = world.orbX - camera.x;
    const y = getGroundY(world.orbX) - 140;
    const pulse = 14 + Math.sin(performance.now() * 0.01) * 3;
    const g = ctx.createRadialGradient(x, y, 2, x, y, 32);
    g.addColorStop(0, "rgba(255,255,255,1)");
    g.addColorStop(1, "rgba(255,255,255,0)");
    ctx.fillStyle = g;
    ctx.beginPath();
    ctx.arc(x, y, 32, 0, Math.PI * 2);
    ctx.fill();
    ctx.fillStyle = "#fff";
    ctx.beginPath();
    ctx.arc(x, y, pulse, 0, Math.PI * 2);
    ctx.fill();
  }

  ctx.fillStyle = "rgba(255,255,255,0.9)";
  for (const p of particles) {
    ctx.fillRect(p.x - camera.x, p.y, 2, 2);
  }
}

function render() {
  const top = world.level === 1 ? "#f6f3f2" : "#f2d7d2";
  const bottom = world.level === 1 ? "#e4b9b5" : "#d77774";
  const bg = ctx.createLinearGradient(0, 0, 0, canvas.height);
  bg.addColorStop(0, top);
  bg.addColorStop(1, bottom);
  ctx.fillStyle = bg;
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  drawMountains(0.08, "rgba(116,73,78,0.22)", 26, 0.5, canvas.height * 0.34);
  drawMountains(0.15, "rgba(100,61,67,0.26)", 32, 0.7, canvas.height * 0.42);
  drawMountains(0.22, "rgba(84,48,54,0.32)", 38, 0.9, canvas.height * 0.5);
  drawMountains(0.3, "rgba(74,37,43,0.36)", 42, 1.1, canvas.height * 0.58);
  drawMountains(0.4, "rgba(60,28,34,0.42)", 48, 1.4, canvas.height * 0.66);

  drawFog();
  drawGroundAndStones();
  drawObstacles();
  drawOrbAndParticles();
  drawPlayer();
}

function loop() {
  update();
  render();
  requestAnimationFrame(loop);
}
loop();
