import { useCall, useCallStateHooks } from '@stream-io/video-react-sdk';
import { useState } from 'react';

export function CallUI() {
  const call = useCall();
  const { useMicrophoneState, useCameraState } = useCallStateHooks();

  const micState = useMicrophoneState();
  const camState = useCameraState();
  const [isLeaving, setIsLeaving] = useState(false);

  const toggleMic = async () => {
    if (!micState.microphone) return;
    if (micState.microphone.enabled) await micState.microphone.disable();
    else await micState.microphone.enable();
  };

  const toggleCam = async () => {
    if (!camState.camera) return;
    if (camState.camera.enabled) await camState.camera.disable();
    else await camState.camera.enable();
  };

  const leaveCall = async () => {
    if (!call) return;
    setIsLeaving(true);
    await call.leave();
  };

  //TODO: add furtur styling and logic to UI

  return (
    <div className="flex flex-col items-center justify-center gap-4 p-6 bg-gray-900 text-white h-screen item-align-bottom">
      <h2 className="text-xl font-bold mb-2">Custom Call UI</h2>

      {(
        <div className="flex gap-4">
          <button
            onClick={toggleMic}
            className={`px-4 py-2 rounded-md ${
              micState.microphone?.enabled ? 'bg-green-600' : 'bg-red-600'
            }`}
          >
            {micState.microphone?.enabled ? 'Mute Mic' : 'Unmute Mic'}
          </button>

          <button
            onClick={toggleCam}
            className={`px-4 py-2 rounded-md ${
              camState.camera?.enabled ? 'bg-green-600' : 'bg-red-600'
            }`}
          >
            {camState.camera?.enabled ? 'Camera On' : 'Camera Off'}
          </button>

          <button
            onClick={leaveCall}
            disabled={isLeaving}
            className="px-4 py-2 bg-gray-700 hover:bg-red-700 rounded-md"
          >
            Leave Call
          </button>
        </div>
      )}
    </div>
  );
}
