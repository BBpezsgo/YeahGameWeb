/**
 * @param {import('./p2p').default} instance
 */
export function Create(instance) {
	return {
		create: function (canJoin, onMessage) { instance.Create((desc) => canJoin(JSON.stringify(desc)), onMessage) },
		join: function (offer, canJoin, onMessage) { instance.Join(JSON.parse(offer), (desc) => canJoin(JSON.stringify(desc)), onMessage) },
		send: function (message) { instance.Send(message) },
		answer: function (answer) { return instance.GotAnswer(JSON.parse(answer)) },

		get_connectionState: function () {
            switch (instance.peerConn.connectionState) {
                case 'closed': return 1
                case 'connecting': return 2
                case 'connected': return 3
                case 'new': return 4
                case 'disconnected': return 5
                case 'failed': return 6
                default: return 0
            }
        },
		get_iceConnectionState: function () {
            switch (instance.peerConn.iceConnectionState) {
                case 'checking': return 1
                case 'connected': return 2
                case 'completed': return 3
                case 'disconnected': return 4
                case 'closed': return 5
                case 'failed': return 6
                case 'new': return 7
                default: return 0
            }
        },
		get_iceGatheringState: function () {
            switch (instance.peerConn.iceGatheringState) {
                case 'complete': return 1
                case 'gathering': return 2
                case 'new': return 3
                default: return 0
            }
        },
		get_dataChannel_readyState: function () {
            if (!instance.DataChannel) return -1
			switch (instance.DataChannel.readyState) {
				case 'closed': return 1
				case 'connecting': return 2
				case 'open': return 3
				case 'closing': return 4
				default: return 0
			}
		},
	}
}
