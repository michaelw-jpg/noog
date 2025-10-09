import { useState, useEffect } from "react";
import type { User } from "@stream-io/video-react-sdk";
import type { AppUser } from "../Models/User";

interface UseUserReturn {
  user: User | null;
  token: string | null;
  loading: boolean;
  error: Error | null;
}

export function useUser(receivedUser?: User): UseUserReturn {
  const [user, setUser] = useState<User | null>(receivedUser || null);
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    // Om användaren redan skickades in, hoppa över fetch
    if (receivedUser) {
      setUser(receivedUser);
      setToken("static-token-if-needed");
      setLoading(false);
      return;
    }

    const fetchUser = async () => {
      try {
        // Här kan du byta till riktig backend senare
        // const res = await fetch('/api/auth/me');
        // const data = await res.json();

        const dummyUser: User = {
          id: "Plant_Innovation",
          name: "Oliver",
          image: "https://getstream.io/random_svg/?id=oliver&name=Oliver",
          type: "guest",
        };

        const dummyToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL3Byb250by5nZXRzdHJlYW0uaW8iLCJzdWIiOiJ1c2VyL1BsYW50X0lubm92YXRpb24iLCJ1c2VyX2lkIjoiUGxhbnRfSW5ub3ZhdGlvbiIsInZhbGlkaXR5X2luX3NlY29uZHMiOjYwNDgwMCwiaWF0IjoxNzU5OTI2MzE4LCJleHAiOjE3NjA1MzExMTh9.9YB-UTbCMW7m-bKLrX57CWmtZoPbAfF4LIeMdBJIsNA";
        setUser(dummyUser);

        setToken(dummyToken);
      } catch (err: any) {
        console.error("Error fetching user:", err);
        setError(err);
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, [receivedUser]);

  console.log("useUser state:", { user });

  return { user, token, loading, error };
}
