export default class Peer2Peer {
    constructor() {
        const RTCPeerConnection = window.RTCPeerConnection || webkitRTCPeerConnection || mozRTCPeerConnection
        this.peerConn = new RTCPeerConnection({
            iceServers: [
                {
                    urls: [ 'stun:stun.l.google.com:19302' ]
                }
            ]
        })

        /** @type {null | (answer: RTCSessionDescriptionInit) => void} */
        this.GotAnswer = null
        /** @type {null | RTCDataChannel} */
        this.DataChannel = null

        console.log('Call Create(), or Join("some offer")')

        this.peerConn.onconnectionstatechange = (/** @type {Event} */ e) => { console.log('[PeerConnection]: State:', this.peerConn.connectionState) }
        this.peerConn.onicecandidateerror = (/** @type {RTCPeerConnectionIceErrorEvent} */ e) => { console.error('[PeerConnection]: ICE candidate error:', e) }
        this.peerConn.oniceconnectionstatechange = (/** @type {Event} */ e) => { console.log('[PeerConnection]: ICE connection state:', this.peerConn.iceConnectionState) }
        this.peerConn.onicegatheringstatechange = (/** @type {Event} */ e) => { console.log('[PeerConnection]: ICE gathering state:', this.peerConn.iceGatheringState) }
        this.peerConn.onnegotiationneeded = (/** @type {Event} */ e) => { console.log('[PeerConnection]: Negotiation needed:', e) }
        this.peerConn.onsignalingstatechange = (/** @type {Event} */ e) => { console.log('[PeerConnection]: Signaling state:', this.peerConn.signalingState) }
    }
    
    /**
     * @param {(desc: RTCSessionDescription) => void} canJoin
     * @param {(message: string) => void} onMessage
     */
    Create(canJoin, onMessage) {
        console.log('[PeerConnection]: Creating ...')

        this.DataChannel = this.peerConn.createDataChannel('bruh')
        this.DataChannel.onopen = (/** @type {RTCDataChannelEvent} */ e) => {
            console.log('Say things with this.DataChannel.send("hi")')
        }
        this.DataChannel.onmessage = (/** @type {MessageEvent} */ e) => {
            console.log('[DataChannel]: Message:', e.data)
            onMessage(e.data + '')
        }
        this.DataChannel.onclose = (/** @type {Event} */ e) => {
            console.log('[DataChannel]: Closed')
            this.DataChannel = null
        }
        this.DataChannel.onclosing = (/** @type {Event} */ e) => { console.log('[DataChannel]: Closing ...') }
        this.DataChannel.onerror = (/** @type {RTCErrorEvent} */ e) => { console.error('[DataChannel] Error:', e.error) }
        this.DataChannel.onbufferedamountlow = (/** @type {Event} */ e) => { console.warn(`[DataChannel]: Buffered amount is low (bufferedAmount: ${this.DataChannel.bufferedAmount}, bufferedAmountLowThreshold: ${this.DataChannel.bufferedAmountLowThreshold})`) }

        this.peerConn.createOffer({ })
            .then((desc) => this.peerConn.setLocalDescription(desc))
            .then(() => { })
            .catch((error) => console.error('[PeerConnection]: Offer creation failed:', error))

        this.peerConn.onicecandidate = (/** @type {RTCPeerConnectionIceEvent} */ e) => {
            if (e.candidate == null) {
                console.log('[PeerConnection]: Now can join')
                // console.log('Get joiners to call: ', 'join(', JSON.stringify(this.peerConn.localDescription), ')')
                canJoin(this.peerConn.localDescription)
            }
        }
        this.GotAnswer = (answer) => {
            console.log('[PeerConnection]: Initializing ...')
            this.peerConn.setRemoteDescription(new RTCSessionDescription(answer))
        }
    }

    /**
     * @param {RTCSessionDescriptionInit} offer
     * @param {(desc: RTCSessionDescription) => void} canJoin
     * @param {(message: string) => void} onMessage
     */
    Join(offer, canJoin, onMessage) {
        console.log('[PeerConnection]: Joining ...')
        
        this.peerConn.ondatachannel = (e) => {
            this.DataChannel = e.channel
            this.DataChannel.onopen = (e) => {
                console.log('Say things with this.DataChannel.send("hi")')
            }
            this.DataChannel.onmessage = (e) => {
                console.log('[DataChannel]: Message:', e.data)
                onMessage(e.data + '')
            }
            this.DataChannel.onclose = (e) => {
                console.log('[DataChannel]: Closed')
                this.DataChannel = null
            }
            this.DataChannel.onclosing = (/** @type {Event} */ e) => { console.log('[DataChannel]: Closing ...') }
            this.DataChannel.onerror = (/** @type {RTCErrorEvent} */ e) => { console.error('[DataChannel] Error:', e.error) }
            this.DataChannel.onbufferedamountlow = (/** @type {Event} */ e) => { console.warn(`[DataChannel]: Buffered amount is low (bufferedAmount: ${this.DataChannel.bufferedAmount}, bufferedAmountLowThreshold: ${this.DataChannel.bufferedAmountLowThreshold})`) }
        }

        this.peerConn.onicecandidate = (e) => {
            if (e.candidate == null) {
                console.log('[PeerConnection]: Ice canditate')
                // console.log('Get the creator to call: GotAnswer(', JSON.stringify(this.peerConn.localDescription), ')')
                canJoin(this.peerConn.localDescription)
            }
        }

        this.peerConn.setRemoteDescription(new RTCSessionDescription(offer))
        this.peerConn.createAnswer({ })
            .then((answerDesc) => this.peerConn.setLocalDescription(answerDesc))
            .catch((error) => console.warn('[PeerConnection]: Couldn\'t create answer:', error))
    }

    /**
     * @param {string | Blob | ArrayBuffer | ArrayBufferView} message
     */
    Send(message) {
        if (!this.DataChannel) {
            console.error('Disconnected')
            return
        }
        this.DataChannel.send(message)
    }
}
