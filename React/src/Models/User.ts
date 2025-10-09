import type { User } from '@stream-io/video-react-sdk';

export type AppUser = User & {
  userId: string;
  name: string;
  image: string;
};