---
id: mediaplayer
title: MediaPlayer
layout: docs
category: APIs
permalink: Docs/APIs/MediaPlayer.md
next: alertios
previous: actionsheetios
---

MediaPlayer is used to play media file. It provides the API, play, pause, stop, etc. And you can add event listeners. It is singleton object for the device. In current version, it supprots only audio files on Tizen TV plateform.

```javascript
MediaPlayer.init(song.url)
MediaPlayer.addEventListener('prepared', (event) => {
  MediaPlayer.play()
})
MediaPlayer.addEventListener('started', (event) => {
  console.log('audio played.')
})
```
 
### Tizen TV
 
MEDIA PLAYER EVENTS: You can add the following events by the function MediaPlayer.addEventListener.  

 - idle
 - preparing
 - prepared
 - started
 - paused
 - updateplayinfo
 - seeking
 - seeked
 - playbackcomplete
 - playbackinterrupted
 - erroroccurred
 - exceptionhappened

### Props

- [`url`](#url)
- [`duration`](#duration)
- [`CONSTANTS`](#CONSTANTS)

### Methods

- [`addEventListener`](#addEventListener)
- [`removeEventListener`](#removeEventListener)
- [`init`](#init)
- [`play`](#play)
- [`pause`](#pause)
- [`seekTo`](#seekTo)
- [`stop`](#stop)
- [`position`](#position)
- [`destroy`](#destroy)


---

# Reference

## Props

### `url`
The current media file url.


### `duration`
The current media file duration

### `CONSTANTS`

- MEDIA_PLAYER_EVENTS: events supported by the MediaPlaye
- EVENT_RESULTS: the results passed into the MediaPlayer event callback

---
## Methods

### `addEventListener()`
   Add a handler to MediaPlayer events by listening to the supported event type
   and providing the handler.
   
- `param eventName`: EventName
    @see MEDIA_PLAYER_EVENTS
   
- `param handler`: Function
  The callback of the event, and there are results passed into the MediaPlayer event callback:
    - `updateplayinfo`: the results objects including position, duration props.
    - `idle`: the results objects including EVENT_RESULT.
    - `preparing`: the results objects including EVENT_RESULT.
    - `prepared`: the results objects including EVENT_RESULT.
    - `started`: the results objects including EVENT_RESULT.
    - `paused`: the results objects including EVENT_RESULT.
    - `seeking`: the results objects including EVENT_RESULT.
    - `seeked`: the results objects including EVENT_RESULT.
    - `playbackcomplete`: the results objects including EVENT_RESULT.
    - `playbackinterrupted`: the results objects including EVENT_RESULT.
    - `erroroccurred`: the results objects including EVENT_RESULT.
    - `exceptionhappened`: the results objects including EVENT_RESULT.
   
- `return` remove: () => void
---
### `removeEventListener()`
Remove a handler by passing the `change` event type and the handler

---

### `init()`

Initialize the media player.  
  `param` {`string` | `number`} source 
    The source to play.   
    The source can be a string: http://www.samsung.com/a.mp3 or require('')

---

### `play()`
Plays the media


---

### `pause()`
Pauses the media


---

### `seekTo()`
`param` {`number`} time  
Seek the media to a given position

---

### `stop()`
Stops the media

---

### `position()`
Gets the media current position  
`returns` {`number`} position


---

### `destroy()`
Destroys the media player

