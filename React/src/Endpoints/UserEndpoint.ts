import { useEffect, useState } from "react";
import { apilink } from "../Api_Links";
import {type ApiUserResponse} from '../Models/ApiUserResponse';


export function useUserData() {
  const [data, setData] = useState<ApiUserResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await fetch(apilink);
        if (!response.ok) throw new Error("Failed to fetch user data");

        const json = await response.json();
        setData(json);
      } catch (err: any) {
        console.error("Error fetching user data:", err);
        setError(err);
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, []);

  return { data, loading, error };
}