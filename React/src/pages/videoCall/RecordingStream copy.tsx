// startRecording.tsx - Förbättrad ljudinspelning
import { useCallback, useEffect, useState } from "react";

export const useAudioRecording = () => {
  const [isRecording, setIsRecording] = useState(false);
  const [mediaRecorder, setMediaRecorder] = useState<MediaRecorder | null>(null);
  const [audioChunks, setAudioChunks] = useState<Blob[]>([]);
  const [error, setError] = useState<string | null>(null);

  const startRecording = useCallback(async () => {
    try {
      // Begär högkvalitativt ljud
      const stream = await navigator.mediaDevices.getUserMedia({
        audio: {
          echoCancellation: true,
          noiseSuppression: true,
          sampleRate: 44100,
          channelCount: 1, // Mono för mindre filstorlek
        }
      });

      // Använd WebM format med Opus codec för bra komprimering
      const options = {
        mimeType: 'audio/webm;codecs=opus'
      };

      let recorder: MediaRecorder;

      try {
        recorder = new MediaRecorder(stream, options);
      } catch (e) {
        // Fallback till default om WebM inte stöds
        console.log("WebM not supported, using default format");
        recorder = new MediaRecorder(stream);
      }

      const chunks: Blob[] = [];

      recorder.ondataavailable = (e) => {
        if (e.data.size > 0) {
          chunks.push(e.data);
        }
      };

      recorder.onstop = () => {
        // Bestäm format baserat på vad som användes
        const mimeType = recorder.mimeType || 'audio/webm';
        const fileExtension = mimeType.includes('webm') ? 'webm' : 'wav';

        const audioBlob = new Blob(chunks, { type: mimeType });
        const audioUrl = URL.createObjectURL(audioBlob);

        // Spara ljudfilen automatiskt
        const a = document.createElement('a');
        a.href = audioUrl;
        a.download = `audio-only-${new Date().getTime()}.${fileExtension}`;
        a.click();

        // Rensa upp
        URL.revokeObjectURL(audioUrl);
        stream.getTracks().forEach(track => track.stop());

        console.log(`Audio saved - Size: ${(audioBlob.size / 1024).toFixed(1)} KB`);
      };

      recorder.onerror = (e) => {
        console.error("MediaRecorder error:", e);
        setError("Recording error occurred");
        stream.getTracks().forEach(track => track.stop());
      };

      setMediaRecorder(recorder);
      setAudioChunks(chunks);
      recorder.start(1000); // Sampla data varje sekund
      setIsRecording(true);
      setError(null);

    } catch (err) {
      setError("Failed to access microphone. Please check permissions.");
      console.error("Error starting audio recording:", err);
    }
  }, []);

  const stopRecording = useCallback(() => {
    if (mediaRecorder && isRecording) {
      mediaRecorder.stop();
      setIsRecording(false);
    }
  }, [mediaRecorder, isRecording]);

  return {
    isRecording,
    startRecording,
    stopRecording,
    error
  };
};