// services/streamClient.ts
import { StreamVideoClient } from '@stream-io/video-react-sdk';
import type { AppUser } from '../Models/User';

const apiKey = import.meta.env.VITE_STREAM_API_KEY || 'd732uqnqb77a'; // t.ex. via .env

export function createStreamClient(user: AppUser, token: string): StreamVideoClient {
  if (!user.id) throw new Error('User must have an ID');
  if (!token) throw new Error('Missing Stream token');

  const client = new StreamVideoClient({
    apiKey,
    user,
    token,
  });

  return client;
}

function loadUser (user: AppUser) {

}