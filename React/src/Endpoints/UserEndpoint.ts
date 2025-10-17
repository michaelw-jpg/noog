import type { UserResponse } from "../Models/UserResponse";
import { apilink } from "../Api_Links";

export async function fetchUser(timeoutMs = 3000): Promise<UserResponse> {
  const controller = new AbortController();
  const timeout = setTimeout(() => controller.abort(), timeoutMs);

  try {
    const response = await fetch(`${apilink}/api/user`, {
      signal: controller.signal,
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const data: UserResponse = await response.json();

    // Validate data
    if (!data.id || !data.token || !data.callId || !data.apiKey) {
      throw new Error(`Incomplete user data from backend`);
    }

    return data;
  } catch (err) {
    console.error("Error fetching user (with timeout):", err);
    throw err;
  } finally {
    clearTimeout(timeout);
  }
}
