export function Create() {
	return {
		clear: function () { localStorage.clear() },
		getItem: function (key) { return localStorage.getItem(key) },
		key: function (index) { return localStorage.key(index) },
		length: function () { return localStorage.length },
		removeItem: function (key) { localStorage.removeItem(key) },
		setItem: function (key, value) { localStorage.setItem(key, value) },
	}
}
