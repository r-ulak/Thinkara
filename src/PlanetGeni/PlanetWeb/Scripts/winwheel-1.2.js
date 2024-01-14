function initializeRoulet() { surface = document.getElementById(canvasId), surface.getContext && (wheel = new Image, wheel.onload = initialDraw, wheel.src = wheelImageName) } function initialDraw() { var e = surface.getContext("2d"); e.drawImage(wheel, 0, 0) } function startSpin(e, n) { var t = void 0; power = n, t = Math.floor(prizes[e].startAngle + Math.random() * (prizes[e].endAngle - prizes[e].startAngle)), "undefined" != typeof t && "reset" == wheelState && power && (t = 360 + pointerAngle - t, targetAngle = 2160 * power + t, randomLastThreshold = Math.floor(90 + 90 * Math.random()), wheelState = "spinning", doSpin()) } function doSpin() { var e = surface.getContext("2d"); if (e.save(), e.translate(.5 * wheel.width, .5 * wheel.height), e.rotate(DegToRad(currentAngle)), e.translate(.5 * -wheel.width, .5 * -wheel.height), e.drawImage(wheel, 0, 0), e.restore(), currentAngle += angle, targetAngle > currentAngle) { var n = targetAngle - currentAngle; angle = n > 6480 ? 55 : n > 5e3 ? 45 : n > 4e3 ? 30 : n > 2500 ? 25 : n > 1800 ? 15 : n > 900 ? 11.25 : n > 400 ? 7.5 : n > 220 ? 3.8 : n > randomLastThreshold ? 1.9 : 1, spinTimer = setTimeout("doSpin()", theSpeed) } else if (wheelState = "stopped", doPrizeDetection && prizes) { var t = Math.floor(currentAngle / 360), r = currentAngle - 360 * t, a = Math.floor(pointerAngle - r); for (0 > a && (a = 360 - Math.abs(a)), x = 0; x < prizes.length; x++) if (a >= prizes[x].startAngle && a <= prizes[x].endAngle) { rouleteComlete(prizes[x].name); break } } } function DegToRad(e) { return .017453292519943295 * e } function resetWheel() { clearTimeout(spinTimer), angle = 0, targetAngle = 0, currentAngle = 0, power = 0, wheelState = "reset", initialDraw() } var canvasId = "myDrawingCanvas", wheelImageName = "Content/Images/prizewheel.png", spinButtonImgOn = "spin_on.png", spinButtonImgOff = "spin_off.png", theSpeed = 20, pointerAngle = 0, doPrizeDetection = !0, spinMode = "determinedPrize", prizes = new Array; prizes[0] = { name: "Prize 0", startAngle: 0, endAngle: 44 }, prizes[1] = { name: "Prize 1", startAngle: 45, endAngle: 89 }, prizes[2] = { name: "Prize 2", startAngle: 90, endAngle: 134 }, prizes[3] = { name: "Prize 3", startAngle: 135, endAngle: 179 }, prizes[4] = { name: "Prize 4", startAngle: 180, endAngle: 224 }, prizes[5] = { name: "Prize 5", startAngle: 225, endAngle: 269 }, prizes[6] = { name: "Prize 6", startAngle: 270, endAngle: 314 }, prizes[7] = { name: "Prize 7", startAngle: 315, endAngle: 360 }; var surface, wheel, angle = 0, targetAngle = 0, currentAngle = 0, power = 0, randomLastThreshold = 150, spinTimer, wheelState = "reset";