import "@stream-io/video-react-sdk/dist/css/styles.css";
import { useEffect, useState } from 'react';
import {
  StreamVideo,
  StreamVideoClient,
  StreamCall,
  StreamTheme,
  SpeakerLayout,
  CallControls,
} from '@stream-io/video-react-sdk';
import { useUser } from '../Service/useUser';

const apiKey = 'mmhfdzb5evj2';
const callId = '18dwiUvvQsvvQ4ff92xgi';

export default function VideoCall() {
  const [client, setClient] = useState<StreamVideoClient | null>(null);
  const [call, setCall] = useState<any>(null);
  const { user, token, error, isLoading } = useUser();

  useEffect(() => {

    if (!user || !token) {
      console.error("No user or token available:", { user, token });
      return;
    }

    const streamClient = new StreamVideoClient({ apiKey, user, token });
    const streamCall = streamClient.call('default', callId);

    streamCall.join({ create: true })
      .then(() => {
        console.log("✅ Joined call:", callId);
        setCall(streamCall);
        setClient(streamClient);

        // Lyssna på recording-events
        streamCall.on('call.recording_started', () => {
          console.log("🎥 Recording started");
        });

        streamCall.on('call.recording_stopped', async () => {
          console.log("🛑 Recording stopped");

          // ✅ Nytt sätt: hämta recordings via API
          const response = await streamCall.queryRecordings();

          if (response.recordings && response.recordings.length > 0) {
            const latest = response.recordings[response.recordings.length - 1];
            console.log("✅ Recording URL:", latest.url);
            alert(`Recording available at: ${latest.url}`);

          } else {
            console.warn("No recordings found for this call.");
          }
        });
      })
      .catch(err => console.error("❌ Failed to join call:", err));

    return () => {
      streamClient.disconnectUser();
    };
  }, [user, token, isLoading]);

  if (isLoading) return <p>Loading user...</p>;
  if (error) {
    console.log("Error in video call:", error)
  } ;
  if (!client || !call) return <p>Initializing call...</p>;

  return (
    <StreamVideo client={client}>
      <StreamTheme>
        <StreamCall call={call}>
          <SpeakerLayout />
          <CallControls />
        </StreamCall>
      </StreamTheme>
    </StreamVideo>
  );
}
