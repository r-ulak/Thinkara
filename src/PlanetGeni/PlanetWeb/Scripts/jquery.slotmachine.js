!function (t, i, e, s) { function n(t, i) { var e, s, n = this, o = t, r = arguments, l = i; this.running = !1, this.onpause = function () { }, this.onresume = function () { }, this.cancel = function () { this.running = !1, clearTimeout(s) }, this.pause = function () { this.running && (i -= (new Date).getTime() - e, this.cancel(), this.onpause()) }, this.resume = function () { this.running || (this.running = !0, e = (new Date).getTime(), s = setTimeout(function () { o.apply(n, Array.prototype.slice.call(r, 2, r.length)) }, i), this.onresume()) }, this.reset = function () { this.cancel(), this.running = !0, i = l, s = setTimeout(function () { o.apply(n, Array.prototype.slice.call(r, 2, r.length)) }, l) }, this.add = function (t) { this.pause(), i += t, this.resume() }, this.resume() } function o(i, e) { this.element = i, this.settings = t.extend({}, a, e), this.defaults = a, this.name = l, this.$slot = t(i), this.$tiles = this.$slot.children(), this.$container = null, this._minTop = null, this._maxTop = null, this._$fakeFirstTile = null, this._$fakeLastTile = null, this._timer = null, this._oncompleteStack = [this.settings.complete], this._spinsLeft = null, this.futureActive = null, this.isRunning = !1, this.isStopping = !1, this.active = this.settings.active, this.$slot.css("overflow", "hidden"), (this.settings.active < 0 || this.settings.active >= this.$tiles.length) && (this.settings.active = 0, this.active = 0), this.$container = this.$tiles.wrapAll('<div class="slotMachineContainer" />').parent(), this._maxTop = -this.$container.height(), this._$fakeFirstTile = this.$tiles.last().clone(), this._$fakeLastTile = this.$tiles.first().clone(), this.$container.prepend(this._$fakeFirstTile), this.$container.append(this._$fakeLastTile), this._minTop = -this._$fakeFirstTile.outerHeight(), this._direction = { selected: "down" === this.settings.direction ? "down" : "up", up: { initial: this.getTileOffset(this.active), first: 0, last: this.getTileOffset(this.$tiles.length), to: this._maxTop, firstToLast: this.getTileOffset(this.$tiles.length), lastToFirst: 0 }, down: { initial: this.getTileOffset(this.active), first: this.getTileOffset(this.$tiles.length), last: 0, to: this._minTop, firstToLast: this.getTileOffset(this.$tiles.length), lastToFirst: 0 }, get: function (t) { return this[this.selected][t] } }, this.$container.css("margin-top", this._direction.get("initial")), this.settings.auto !== !1 && (this.settings.auto === !0 ? this.shuffle() : this.auto()) } function r(i, e) { var s; return t.data(i[0], "plugin_" + l) ? s = t.data(i[0], "plugin_" + l) : (s = new o(i, e), t.data(i[0], "plugin_" + l, s)), s } var l = "slotMachine", a = { active: 0, delay: 200, auto: !1, spins: 5, randomize: null, complete: null, stopHidden: !0, direction: "up" }, h = "slotMachineBlurFast", u = "slotMachineBlurMedium", c = "slotMachineBlurSlow", f = "slotMachineGradient", p = f; t(e).ready(function () { var i = '<svg version="1.1" xmlns="http://www.w3.org/2000/svg" width="0" height="0"><filter id="slotMachineBlurFilterFast"><feGaussianBlur stdDeviation="5" /></filter></svg>#slotMachineBlurFilterFast', e = '<svg version="1.1" xmlns="http://www.w3.org/2000/svg" width="0" height="0"><filter id="slotMachineBlurFilterMedium"><feGaussianBlur stdDeviation="3" /></filter></svg>#slotMachineBlurFilterMedium', s = '<svg version="1.1" xmlns="http://www.w3.org/2000/svg" width="0" height="0"><filter id="slotMachineBlurFilterSlow"><feGaussianBlur stdDeviation="1" /></filter></svg>#slotMachineBlurFilterSlow', n = '<svg version="1.1" xmlns="http://www.w3.org/2000/svg" width="0" height="0"><mask id="slotMachineFadeMask" maskUnits="objectBoundingBox" maskContentUnits="objectBoundingBox"><linearGradient id="slotMachineFadeGradient" gradientUnits="objectBoundingBox" x="0" y="0"><stop stop-color="white" stop-opacity="0" offset="0"></stop><stop stop-color="white" stop-opacity="1" offset="0.25"></stop><stop stop-color="white" stop-opacity="1" offset="0.75"></stop><stop stop-color="white" stop-opacity="0" offset="1"></stop></linearGradient><rect x="0" y="-1" width="1" height="1" transform="rotate(90)" fill="url(#slotMachineFadeGradient)"></rect></mask></svg>#slotMachineFadeMask'; t("body").append("<style>." + h + '{-webkit-filter: blur(5px);-moz-filter: blur(5px);-o-filter: blur(5px);-ms-filter: blur(5px);filter: blur(5px);filter: url("data:image/svg+xml;utf8,' + i + '");filter:progid:DXImageTransform.Microsoft.Blur(PixelRadius="5")}.' + u + '{-webkit-filter: blur(3px);-moz-filter: blur(3px);-o-filter: blur(3px);-ms-filter: blur(3px);filter: blur(3px);filter: url("data:image/svg+xml;utf8,' + e + '");filter:progid:DXImageTransform.Microsoft.Blur(PixelRadius="3")}.' + c + '{-webkit-filter: blur(1px);-moz-filter: blur(1px);-o-filter: blur(1px);-ms-filter: blur(1px);filter: blur(1px);filter: url("data:image/svg+xml;utf8,' + s + '");filter:progid:DXImageTransform.Microsoft.Blur(PixelRadius="1")}.' + f + '{-webkit-mask-image: -webkit-gradient(linear, left top, left bottom, color-stop(0%, rgba(0,0,0,0)), color-stop(25%, rgba(0,0,0,1)), color-stop(75%, rgba(0,0,0,1)), color-stop(100%, rgba(0,0,0,0)) );mask: url("data:image/svg+xml;utf8,' + n + '");}</style>') }), "function" != typeof t.easing.easeOutBounce && t.extend(t.easing, { easeOutBounce: function (t, i, e, s, n) { return (i /= n) < 1 / 2.75 ? 7.5625 * s * i * i + e : 2 / 2.75 > i ? s * (7.5625 * (i -= 1.5 / 2.75) * i + .75) + e : 2.5 / 2.75 > i ? s * (7.5625 * (i -= 2.25 / 2.75) * i + .9375) + e : s * (7.5625 * (i -= 2.625 / 2.75) * i + .984375) + e } }), o.prototype.getTileOffset = function (t) { for (var i = 0, e = 0; t > e; e++) i += this.$tiles.eq(e).outerHeight(); return -i + this._minTop }, o.prototype.getVisibleTile = function () { var t = this.$tiles.first().height(), i = parseInt(this.$container.css("margin-top").replace(/px/, ""), 10); return Math.abs(Math.round(i / t)) - 1 }, o.prototype.setRandomize = function (t) { this.settings.randomize = "number" == typeof t ? function () { return t } : t }, o.prototype.getRandom = function (t) { var i, e = t || !1; do i = Math.floor(Math.random() * this.$tiles.length); while (e && i === this.active && i >= 0); return i }, o.prototype.getCustom = function () { var t; if (null !== this.settings.randomize && "function" == typeof this.settings.randomize) { var i = this.settings.randomize.apply(this, [this.active]); (0 > i || i >= this.$tiles.length) && (i = 0), t = i } else t = this.getRandom(); return t }, o.prototype._getPrev = function () { var t = this.active - 1 < 0 ? this.$tiles.length - 1 : this.active - 1; return t }, o.prototype._getNext = function () { var t = this.active + 1 < this.$tiles.length ? this.active + 1 : 0; return t }, o.prototype.getPrev = function () { return "up" === this._direction.selected ? this._getPrev() : this._getNext() }, o.prototype.getNext = function () { return "up" === this._direction.selected ? this._getNext() : this._getPrev() }, o.prototype.setDirection = function (t) { return this.isRunning ? void 0 : (this._direction.selected = "down" === t ? "down" : "up", this._direction[this._direction.selected]) }, o.prototype._setAnimationFX = function (t, i) { var e = this; setTimeout(function () { e.$tiles.removeClass([h, u, c].join(" ")).addClass(t), i !== !0 || t === p ? e.$slot.add(e.$tiles).removeClass(f) : e.$slot.add(e.$tiles).addClass(f) }, this.settings.delay / 4) }, o.prototype._resetPosition = function () { this.$container.css("margin-top", this._direction.get("initial")) }, o.prototype.isVisible = function () { var e = this.$slot.offset().top > t(i).scrollTop() + t(i).height(), s = t(i).scrollTop() > this.$slot.height() + this.$slot.offset().top; return !e && !s }, o.prototype.prev = function () { return this.futureActive = this.getPrev(), this.isRunning = !0, this.stop(!1), this.futureActive }, o.prototype.next = function () { return this.futureActive = this.getNext(), this.isRunning = !0, this.stop(!1), this.futureActive }, o.prototype.shuffle = function (t, i) { var e = this; i !== s && (this._oncompleteStack[1] = i), this.isRunning = !0; var n = this.settings.delay; if (null === this.futureActive) { var o = this.getCustom(); this.futureActive = o } if ("number" == typeof t) switch (t) { case 1: case 2: this._setAnimationFX(c, !0); break; case 3: case 4: this._setAnimationFX(u, !0), n /= 1.5; break; default: this._setAnimationFX(h, !0), n /= 2 } else this._setAnimationFX(h, !0), n /= 2; return this.isVisible() || this.settings.stopHidden !== !0 ? this.$container.animate({ marginTop: this._direction.get("to") }, n, "linear", function () { e.$container.css("margin-top", e._direction.get("first")), 0 >= t - 1 ? e.stop() : e.shuffle(t - 1) }) : (t = 0, e.stop()), this.futureActive }, o.prototype.stop = function (t) { if (this.isRunning) { if (this.isStopping) return this.futureActive; var i = this; this.$container.clearQueue().stop(!0, !1), this._setAnimationFX(c, t === s ? !0 : t), this.isRunning = !0, this.isStopping = !0, this.active = this.getVisibleTile(), this.futureActive > this.active ? 0 === this.active && this.futureActive === this.$tiles.length - 1 && this.$container.css("margin-top", this._direction.get("firstToLast")) : this.active === this.$tiles.length - 1 && 0 === this.futureActive && this.$container.css("margin-top", this._direction.get("lastToFirst")), this.active = this.futureActive; var e = 3 * this.settings.delay; return this.$container.animate({ marginTop: this.getTileOffset(this.active) }, e, "easeOutBounce", function () { i.isStopping = !1, i.isRunning = !1, i.futureActive = null, "function" == typeof i._oncompleteStack[0] && i._oncompleteStack[0].apply(i, [i.active]), "function" == typeof i._oncompleteStack[1] && i._oncompleteStack[1].apply(i, [i.active]) }), setTimeout(function () { i._setAnimationFX(p, !1) }, e / 1.75), this.active } }, o.prototype.auto = function () { var t = this; this._timer = new n(function () { "function" != typeof t.settings.randomize && (t.futureActive = t.getNext()), t.isRunning = !0, t.isVisible() || t.settings.stopHidden !== !0 ? t.shuffle(t.settings.spins, function () { t._timer.reset() }) : setTimeout(function () { t._timer.reset() }, 500) }, this.settings.auto) }, t.fn[l] = function (i) { if (1 === this.length) return r(this, i); var e = this; return t.map(e, function (t, s) { var n = e.eq(s); return r(n, i) }) } }(jQuery, window, document);