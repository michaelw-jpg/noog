import { useState, useEffect } from "react";
import type { User } from "@stream-io/video-react-sdk";
import { fetchUser } from "../Endpoints/UserEndpoint";

interface UseUserReturn {
  user: User | null;
  token: string | null;
  callId: string | null;
  apiKey: string | null;
  error: Error | null;
  isLoading: boolean;
}

export function useUser(): UseUserReturn {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [callId, setCallId] = useState<string | null>(null);
  const [apiKey, setApiKey] = useState<string | null>(null);
  const [error, setError] = useState<Error | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {

    const getUser = async () => {
      setIsLoading(true);
      setError(null);

      try {
        const userFromBackend = await fetchUser();

        const streamUser: User = {
          id: userFromBackend.id,
          name: userFromBackend.name || "Unknown",
          image: userFromBackend.image,
          type: "authenticated",
        };

        setUser(streamUser);
        setToken(userFromBackend.token);
        setCallId(userFromBackend.callId);
        setApiKey(userFromBackend.apiKey);

        console.log("useUser state (from backend):", { 
          user: streamUser,
          callId: userFromBackend.callId
        });
        
      } catch (err: any) {
        console.error("Error fetching user:", err);
        setError(err);
      } finally {
        setIsLoading(false);
      }
    };

    getUser();
  }, []);

  return { user, token, callId, apiKey, error, isLoading };
}