import {
  CallControls,
  CallingState,
  SpeakerLayout,
  StreamCall,
  StreamTheme,
  StreamVideo,
  StreamVideoClient,
  useCallStateHooks,
} from '@stream-io/video-react-sdk';
import type { User } from '@stream-io/video-react-sdk';
import { CustomCallRecordButton } from './ConvertAudioFile/ConvertSendAudio';
import '@stream-io/video-react-sdk/dist/css/styles.css';
import './videoCall.css';
// Remove dotenv import; not needed in frontend

//TODO: Add envoriment variables in a .env file
const apiKey = 'mmhfdzb5evj2';
const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL3Byb250by5nZXRzdHJlYW0uaW8iLCJzdWIiOiJ1c2VyL1RodW5kZXJpbmdfQXJhZ29uIiwidXNlcl9pZCI6IlRodW5kZXJpbmdfQXJhZ29uIiwidmFsaWRpdHlfaW5fc2Vjb25kcyI6NjA0ODAwLCJpYXQiOjE3NTkyMjUwNTksImV4cCI6MTc1OTgyOTg1OX0.5Lc6NcM6mIXmdkuIEgrIh0lfmqmfKXM84RYui2nzOU0';
const userId = 'Thundering_Aragon';
const callId = 'JAbludgZPR87agBGEnT62';

const user: User = {
  id: userId,
  name: 'Oliver',
  image: 'https://getstream.io/random_svg/?id=oliver&name=Oliver',
};

const client = new StreamVideoClient({ apiKey, user, token });
const call = client.call('default', callId);
call.join({ create: true });

export default function VideoCallSetup() {
  return (
    <StreamVideo client={client}>
      <StreamCall call={call}>
        <MyUILayout />
      </StreamCall>
    </StreamVideo>
  );
}

export const MyUILayout = () => {
  const { useCallCallingState } = useCallStateHooks();
  const callingState = useCallCallingState();

  if (callingState !== CallingState.JOINED) {
    return <div>Loading...</div>;
  }

  return (
     <StreamTheme>
      <div className="call-container">
        <div className="call-header">
          <h2>Video Call - {user.name}</h2>
          <button className="leave-btn" onClick={() => call.leave()}>
            Leave Call
          </button>
        </div>

        <div className="video-area">
          <SpeakerLayout participantsBarPosition='bottom' />
        </div>

        <div className="controls-area">
          <CallControls />
          <CustomCallRecordButton /> {/* LÄGG TILL INSPELNINGSKNAPPEN HÄR */}
        </div>
      </div>
    </StreamTheme>
  );
};