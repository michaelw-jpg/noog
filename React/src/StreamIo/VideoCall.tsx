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

export default function VideoCall() {
  const [client, setClient] = useState<StreamVideoClient | null>(null);
  const [call, setCall] = useState<any>(null);
  const { user, token, callId, apiKey, error, isLoading } = useUser();

  useEffect(() => {

    if (!user || !token || !callId || !apiKey) {
      console.error("No user, token, callId or apiKey available:", { user, token, callId, apiKey });
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
  }, [user, token, callId, apiKey, isLoading]);

  if (isLoading) 
    return <p>Loading user...</p>;
  if (error) 
    return <p>Error loading call: {error.message}</p>
  if (!client || !call) 
    return <p>Initializing call...</p>;

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
