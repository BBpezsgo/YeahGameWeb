export function Create() {
	return {
		window_innerWidth_get: function () { return window.innerWidth },
		window_innerHeight_get: function () { return window.innerHeight },
		
		window_outerWidth_get: function () { return window.outerWidth },
		window_outerHeight_get: function () { return window.outerHeight },

		window_location_get: function () { return window.location.href },

		prompt: function (message, _default ) { return prompt(message, _default) },
	}
}