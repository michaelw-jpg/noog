import { useCallback, useEffect, useState } from "react";
import { useCall, useCallStateHooks } from "@stream-io/video-react-sdk";
import { LoadingIndicator } from "../Loading/LoadingIndicator";
import { useAudioRecording } from "../RecordingStream";
import './ConvertSendAudio.css';

// Interface för recording data
interface RecordingInfo {
  url: string;
  startTime: Date;
  endTime?: Date;
  duration?: number;
}

export const CustomCallRecordButton = () => {
  const call = useCall();
  const { useIsCallRecordingInProgress } = useCallStateHooks();
  const isCallRecordingInProgress = useIsCallRecordingInProgress();
  const [isAwaitingResponse, setIsAwaitingResponse] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [recordingInfo, setRecordingInfo] = useState<RecordingInfo | null>(null);
  const [isFetchingRecording, setIsFetchingRecording] = useState(false);

  const {
    isRecording: isDirectAudioRecording,
    startRecording: startDirectAudioRecording,
    stopRecording: stopDirectAudioRecording,
    error: directAudioError
  } = useAudioRecording();

  useEffect(() => {
    if (!call) return;

    const eventHandlers = [
      call.on("call.recording_started", () => {
        setIsAwaitingResponse(false);
        setError(null);
        console.log("Recording started");
      }),
      call.on("call.recording_stopped", () => {
        setIsAwaitingResponse(false);
        setError(null);
        console.log("Recording stopped, fetching recording data...");
        fetchRecordingData();
      }),
      call.on("call.recording_failed", (e: unknown) => {
        setIsAwaitingResponse(false);
        setError("Recording failed. Please try again.");
        console.error("Recording failed:", e);
      }),
    ];

    return () => {
      eventHandlers.forEach((unsubscribe) => unsubscribe());
    };
  }, [call]);

  const fetchRecordingData = useCallback(async () => {
    if (!call) return;

    setIsFetchingRecording(true);
    setError(null);

    try {
      console.log("Fetching recording data from Stream...");

      const maxAttempts = 15; // Öka till 15 försök
      let attempts = 0;

      const tryFetchRecording = async () => {
        try {
          const recordings = await call.queryRecordings();
          console.log("Raw recordings response:", recordings);
          console.log("Number of recordings:", recordings.recordings.length);

          if (recordings.recordings.length > 0) {
            const latestRecording = recordings.recordings[0];

            // Logga hela recording objektet för debugging
            console.log("Full recording object:", JSON.stringify(latestRecording, null, 2));

            // Stream's recording har en 'url' property direkt
            if (latestRecording.url) {
              console.log("🎉 Recording URL found:", latestRecording.url);

              const recordingData: RecordingInfo = {
                url: latestRecording.url,
                startTime: new Date(latestRecording.start_time),
                endTime: latestRecording.end_time ? new Date(latestRecording.end_time) : undefined,
                // Använd duration från recording om den finns, annars beräkna från start och end time
                duration: (latestRecording as any).duration ||
                  (latestRecording.end_time && latestRecording.start_time
                    ? (new Date(latestRecording.end_time).getTime() - new Date(latestRecording.start_time).getTime()) / 1000
                    : undefined)
              };

              setRecordingInfo(recordingData);
              setIsFetchingRecording(false);
              return true;
            } else {
              console.warn("Recording found but no URL:", latestRecording);

              // Om ingen URL, försök bygga en baserat på call ID
              if (call.id) {
                const fallbackUrl = `https://api.stream-io-video.com/calls/${call.type}/${call.id}/recordings`;
                console.log("Trying fallback URL:", fallbackUrl);

                const recordingData: RecordingInfo = {
                  url: fallbackUrl,
                  startTime: new Date(latestRecording.start_time),
                  endTime: latestRecording.end_time ? new Date(latestRecording.end_time) : undefined,
                  duration: (latestRecording as any).duration ||
                    (latestRecording.end_time && latestRecording.start_time
                      ? (new Date(latestRecording.end_time).getTime() - new Date(latestRecording.start_time).getTime()) / 1000
                      : undefined)
                };

                setRecordingInfo(recordingData);
                setIsFetchingRecording(false);
                return true;
              }
            }
          } else {
            console.log(`No recordings found yet, attempt ${attempts + 1}/${maxAttempts}`);
          }
        } catch (error) {
          console.error("Error fetching recording data:", error);
        }

        attempts++;
        if (attempts < maxAttempts) {
          console.log(`Retrying in 4 seconds... (${attempts}/${maxAttempts})`);
          setTimeout(tryFetchRecording, 4000);
        } else {
          setError("Recording URL not available after multiple attempts. The recording may still be processing.");
          setIsFetchingRecording(false);
        }
      };

      // Starta första försöket efter 2 sekunder
      setTimeout(tryFetchRecording, 2000);

    } catch (error) {
      console.error("Error in fetchRecordingData:", error);
      setError("Failed to fetch recording data");
      setIsFetchingRecording(false);
    }
  }, [call]);

  const toggleVideoRecording = useCallback(async () => {
    if (!call) {
      setError("No active call found");
      return;
    }

    try {
      setIsAwaitingResponse(true);
      setError(null);

      if (isCallRecordingInProgress) {
        console.log("Stopping recording...");
        await call.stopRecording();
      } else {
        console.log("Starting recording...");
        await call.startRecording();
        setRecordingInfo(null); // Reset previous recording
      }
    } catch (e: unknown) {
      console.error(`Failed to ${isCallRecordingInProgress ? 'stop' : 'start'} recording`, e);
      setError(`Failed to ${isCallRecordingInProgress ? 'stop' : 'start'} recording: ${e instanceof Error ? e.message : String(e)}`);
      setIsAwaitingResponse(false);
    }
  }, [call, isCallRecordingInProgress]);

  const toggleDirectAudioRecording = useCallback(async () => {
    if (isDirectAudioRecording) {
      stopDirectAudioRecording();
    } else {
      try {
        await startDirectAudioRecording();
        setError(null);
      } catch (err) {
        setError("Failed to start audio recording");
      }
    }
  }, [isDirectAudioRecording, startDirectAudioRecording, stopDirectAudioRecording]);

  const saveVideoLocally = useCallback(async () => {
    if (!recordingInfo?.url) {
      setError("No recording URL available");
      return;
    }

    try {
      console.log("Downloading from URL:", recordingInfo.url);

      const response = await fetch(recordingInfo.url);
      if (!response.ok) {
        throw new Error(`Failed to fetch recording: ${response.status} ${response.statusText}`);
      }

      const blob = await response.blob();
      console.log("Downloaded blob size:", blob.size, "bytes");

      if (blob.size === 0) {
        throw new Error("Downloaded file is empty");
      }

      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.style.display = 'none';
      a.href = url;
      a.download = `video-call-recording-${new Date().getTime()}.mp4`;

      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);

      console.log("Video saved locally - Size:", (blob.size / (1024 * 1024)).toFixed(2), "MB");
    } catch (error: unknown) {
      console.error("Error saving video:", error);
      setError(`Failed to save video: ${error instanceof Error ? error.message : String(error)}`);
    }
  }, [recordingInfo]);

  const copyRecordingUrl = useCallback(async () => {
    if (!recordingInfo?.url) {
      setError("No recording URL available");
      return;
    }

    try {
      await navigator.clipboard.writeText(recordingInfo.url);
      alert("Recording URL copied to clipboard!");
    } catch (error: unknown) {
      console.error("Failed to copy URL:", error);
      // Fallback för äldre webbläsare
      const textArea = document.createElement('textarea');
      textArea.value = recordingInfo.url;
      document.body.appendChild(textArea);
      textArea.select();
      document.execCommand('copy');
      document.body.removeChild(textArea);
      alert("Recording URL copied to clipboard!");
    }
  }, [recordingInfo]);

  const sendViaEmail = useCallback(() => {
    if (!recordingInfo?.url) {
      setError("No recording URL available");
      return;
    }

    const subject = encodeURIComponent("Video Call Recording");
    const body = encodeURIComponent(`Here is the recording from our video call:\n\n${recordingInfo.url}\n\nYou can download it from this link.\n\nRecording started: ${recordingInfo.startTime.toLocaleString()}`);
    const mailtoLink = `mailto:?subject=${subject}&body=${body}`;

    window.open(mailtoLink);
  }, [recordingInfo]);

  const showRecordingInfo = useCallback(() => {
    if (!recordingInfo) return null;

    return (
      <div className="recording-info">
        <h5>Recording Information:</h5>
        <div className="info-grid">
          <div className="info-item">
            <strong>Start Time:</strong> {recordingInfo.startTime.toLocaleString()}
          </div>
          {recordingInfo.endTime && (
            <div className="info-item">
              <strong>End Time:</strong> {recordingInfo.endTime.toLocaleString()}
            </div>
          )}
          {recordingInfo.duration && (
            <div className="info-item">
              <strong>Duration:</strong> {recordingInfo.duration} seconds
            </div>
          )}
          <div className="info-item">
            <strong>Status:</strong> Ready for download
          </div>
        </div>
      </div>
    );
  }, [recordingInfo]);

  useEffect(() => {
    if (directAudioError) {
      setError(directAudioError);
    }
  }, [directAudioError]);

  if (isAwaitingResponse) {
    return (
      <div className="recording-section">
        <LoadingIndicator />
        <div className="recording-status">
          {isCallRecordingInProgress ? 'Stopping recording...' : 'Starting recording...'}
        </div>
      </div>
    );
  }

  return (
    <div className="recording-section">
      {error && <div className="error-message">{error}</div>}

      <div className="recording-controls">
        <div className="recording-option">
          <h4>🎥 Video Recording (Stream)</h4>
          <button
            onClick={toggleVideoRecording}
            className={`record-button ${isCallRecordingInProgress ? 'recording' : ''}`}
            disabled={isDirectAudioRecording || isFetchingRecording}
          >
            {isCallRecordingInProgress ? '⏹️ Stop Recording' : '⏺️ Start Recording'}
          </button>
          <div className="recording-status">
            {isCallRecordingInProgress
              ? '🔴 Recording in progress...'
              : isFetchingRecording
              ? '🔄 Processing recording...'
              : '✅ Ready to record'
            }
          </div>
        </div>

        <div className="recording-option">
          <h4>🎵 Audio Only (Small Files)</h4>
          <button
            onClick={toggleDirectAudioRecording}
            className={`record-button audio-only ${isDirectAudioRecording ? 'recording' : ''}`}
            disabled={isCallRecordingInProgress}
          >
            {isDirectAudioRecording ? '⏹️ Stop Audio' : '⏺️ Start Audio'}
          </button>
          <div className="recording-status">
            {isDirectAudioRecording ? '🔴 Audio recording...' : '✅ Audio ready'}
          </div>
          <div className="size-info">
            <small>Audio files are 90% smaller than video files</small>
          </div>
        </div>
      </div>

      {isFetchingRecording && (
        <div className="fetching-recording">
          <LoadingIndicator />
          <div className="recording-status">
            🕒 Processing recording... This may take a few moments.
          </div>
        </div>
      )}

      {recordingInfo && (
        <div className="download-options">
          <h4>🎉 Recording Available!</h4>

          <div className="url-display">
            <label>Recording URL:</label>
            <div className="url-container">
              <input
                type="text"
                value={recordingInfo.url}
                readOnly
                className="url-input"
                onClick={(e) => e.currentTarget.select()}
              />
              <button onClick={copyRecordingUrl} className="copy-btn">
                📋 Copy URL
              </button>
            </div>
          </div>

          <div className="download-buttons">
            <button
              onClick={saveVideoLocally}
              className="download-btn video-btn"
            >
              💾 Download Video
            </button>
            <button
              onClick={sendViaEmail}
              className="download-btn email-btn"
            >
              📧 Share via Email
            </button>
          </div>

          {showRecordingInfo()}

          <div className="success-message">
            <small>✅ Your recording is ready! You can download it or share the URL.</small>
          </div>
        </div>
      )}
    </div>
  );
};