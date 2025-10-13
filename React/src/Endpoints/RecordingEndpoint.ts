import { apilink } from "../Api_Links";

export async function SendRecordingUrl(url: string) {
  try {
    const response = await fetch(`${apilink}/recording`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ recordingUrl: url }),
    });

    if (!response.ok) {
      throw new Error(`Failed to send recording URL: ${response.status}`);
    }

    console.log("✅ Recording URL sent successfully");
  } catch (error) {
    console.error("❌ Error sending recording URL:", error);
  }
}
