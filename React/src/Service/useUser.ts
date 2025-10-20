import { useState, useEffect } from "react";
import type { User } from "@stream-io/video-react-sdk";
import { fetchUser } from "../Endpoints/UserEndpoint";

interface UseUserReturn {
  user: User | null;
  token: string | null;
  error: Error | null;
  isLoading: boolean;
}

export function useUser(): UseUserReturn {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [error, setError] = useState<Error | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {



    const getUser = async () => {
      setIsLoading(true);
      setError(null);

      try {
        const userFromBackend = await fetchUser();

        if (userFromBackend && userFromBackend.id) {
          const streamUser: User = {
            id: userFromBackend.id,
            name: userFromBackend.name || "Unknown",
            image: userFromBackend.image,
            type: "authenticated",
          };

          setUser(streamUser);
          setToken(userFromBackend.token || "static-token-if-needed");
          console.log("useUser state (from backend):", { user: streamUser });
        } else {
          throw new Error('Invalid user data received');
        }
      } catch (err: any) {
        console.error("Error fetching user:", err);
        setError(err);

        // Fallback till dummy user
        const dummyUser: User = {
          id: "Plant_Innovation",
          name: "Oliver",
          image: "https://getstream.io/random_svg/?id=oliver&name=Oliver",
          type: "authenticated",
        };
        setUser(dummyUser);
        const dummyToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL3Byb250by5nZXRzdHJlYW0uaW8iLCJzdWIiOiJ1c2VyL1BsYW50X0lubm92YXRpb24iLCJ1c2VyX2lkIjoiUGxhbnRfSW5ub3ZhdGlvbiIsInZhbGlkaXR5X2luX3NlY29uZHMiOjYwNDgwMCwiaWF0IjoxNzU5OTI2MzE4LCJleHAiOjE3NjA1MzExMTh9.9YB-UTbCMW7m-bKLrX57CWmtZoPbAfF4LIeMdBJIsNA";
        setToken(dummyToken);
        console.log(" ❌ using dummy user due to no user from backend")
      } finally {
        setIsLoading(false);
      }
    };

    getUser();
  }, []);

  return { user, token, error, isLoading };
}