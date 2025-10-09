import "@stream-io/video-react-sdk/dist/css/styles.css";
import { useEffect, useState } from 'react';
import {
  PaginatedGridLayout,
  StreamVideo,
  StreamVideoClient,
  StreamCall,
  useParticipantViewContext,
  SpeakerLayout,
  type User,
  type VideoPlaceholderProps,
  StreamTheme,
  Call,
  CallControls,
} from '@stream-io/video-react-sdk';
import { CallUI } from '../Component/CallUI';
import { useUser } from '../Service/useUser';
const apiKey = 'mmhfdzb5evj2';
const callId = '18dwiUvvQsvvQ4ff92xgi';





export default function VideoCall() {
  const [client, setClient] = useState<StreamVideoClient | null>(null);

  const [call, setCall] = useState<any>(null);

  const { user, token, loading, error } = useUser();

  useEffect(() => {
  if (loading) {
      console.log("⏳ Loading user data...");
      return;
    }
    if (!user || !token) {
      console.error("❌ No user or token available:", { user, token });
      return;
    }
    console.log("🚀 Initializing Stream client...");


  const streamClient = new StreamVideoClient({ apiKey, user, token });
  const streamCall = streamClient.call('default', callId);

  streamCall.join({ create: true })
      .then(() => {
        console.log("✅ Joined call:", callId);
        setCall(streamCall);
        setClient(streamClient);
      })
      .catch(err => {
        console.error("❌ Failed to join call:", err);
      });


  return () => {
    console.log("🔌 Disconnecting user...");
    streamClient.disconnectUser();
  };
}, [user, token, loading]);

  if (loading) {
    return <p>Loading user data...</p>;
  }

  if (error) {
    return <p>Error: {error.message}</p>;
  }

  if (!client || !call) {
    return <p>Initializing call...</p>;
  }


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

